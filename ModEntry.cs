using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using HarmonyLib;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    /// <summary>The mod entry point. Wires up events and hosts shared state and helpers.</summary>
    public partial class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }

        public ModConfig Config { get; private set; }
        public ModData Data { get; private set; }

        private const int PointsPerHeart = 250;

        // Transient (per-day, not persisted) runtime flags.
        private bool argumentTriggeredToday;
        private bool dateSceneShownToday;
        private bool forceGrumpyToday;
        private bool pendingDateOffer;
        private bool pendingDateOfferMovie;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            this.Config = helper.ReadConfig<ModConfig>();
            this.Data = new ModData();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saving += this.OnSaving;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.DayEnding += this.OnDayEnding;
            helper.Events.GameLoop.TimeChanged += this.OnTimeChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;

            if (this.Config.EnableDebugCommands)
                this.RegisterDebugCommands();

            try
            {
                var harmony = new Harmony(this.ModManifest.UniqueID);
                HarmonyPatches.Apply(harmony, this.Monitor);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Failed to apply Harmony patches: {ex}", LogLevel.Error);
            }
        }

        // ── Event handlers ────────────────────────────────────────────

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            this.SetupGmcm();
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.Data = this.Helper.Data.ReadSaveData<ModData>("main-data") ?? new ModData();
        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            this.Helper.Data.WriteSaveData("main-data", this.Data);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.argumentTriggeredToday = false;
            this.dateSceneShownToday = false;
            this.forceGrumpyToday = false;
            this.pendingDateOffer = false;

            NPC spouse = this.GetSpouse();
            if (spouse == null)
                return;

            // Generate / refresh the weekly feeding schedule.
            if (this.Config.EnableFeeding)
                this.Feeding_EnsureWeeklySchedule();

            // First time we see this marriage: record the wedding date and start the date-night
            // cooldown so the spouse doesn't ask for a date on the wedding day itself.
            if (this.Data.WeddingAbsoluteDay < 0)
            {
                this.Data.WeddingAbsoluteDay = this.AbsoluteDay;
                this.Data.LastDateNightDay = this.AbsoluteDay;
            }

            // Apply consequences queued from yesterday and inject morning dialogue.
            this.Jealousy_OnDayStarted(spouse);
            this.Feeding_OnDayStarted(spouse);
            this.Anniversary_OnDayStarted(spouse);
            this.Makeup_OnDayStarted(spouse);
            this.Mood_OnDayStarted(spouse);
            this.DateNight_OnDayStarted(spouse);
            this.Divorce_OnDayStarted(spouse);
        }

        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            NPC spouse = this.GetSpouse();
            if (spouse == null)
                return;

            this.Feeding_OnDayEnding(spouse);
            this.Anniversary_OnDayEnding(spouse);
            this.Divorce_OnDayEnding(spouse);
            this.DateNight_OnDayEnding(spouse);

            // Record today's friendship for tomorrow's trend comparison.
            this.Data.LastFriendshipPoints = this.GetSpousePoints();
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Present a queued date-night invitation only once the player is actually in control
            // (never during the wedding cutscene, festivals, other events, or open menus).
            this.DateNight_TryPresentPending();

            // Bring the player home once a date cutscene finishes.
            this.DateEvent_WatchForEnd();
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            NPC spouse = this.GetSpouse();
            if (spouse == null)
                return;

            this.Argument_OnTimeChanged(spouse, e.NewTime);
            this.DateNight_OnTimeChanged(spouse, e.NewTime);
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/mail"))
            {
                e.Edit(asset =>
                {
                    var dict = asset.AsDictionary<string, string>().Data;
                    if (!string.IsNullOrEmpty(this.Data?.PendingWarningLetterText))
                        dict["MO.DivorceWarning"] = this.Data.PendingWarningLetterText;
                    if (!string.IsNullOrEmpty(this.Data?.PendingAnniversaryLetterText))
                        dict["MO.Anniversary"] = this.Data.PendingAnniversaryLetterText;
                });
            }
        }

        // ── Shared helpers ────────────────────────────────────────────

        /// <summary>The player's current spouse NPC, or null if unmarried / spouse not loaded.</summary>
        public NPC GetSpouse()
        {
            try
            {
                Farmer player = Game1.player;
                if (player == null || string.IsNullOrEmpty(player.spouse))
                    return null;
                return player.getSpouse();
            }
            catch
            {
                return null;
            }
        }

        public string SpouseName => Game1.player?.spouse;

        public int GetSpousePoints()
        {
            string name = this.SpouseName;
            if (name != null && Game1.player.friendshipData.ContainsKey(name))
                return Game1.player.friendshipData[name].Points;
            return 0;
        }

        /// <summary>Apply a friendship change to the current spouse (clamped by the game).</summary>
        public void ChangeSpouseFriendship(int amount)
        {
            NPC spouse = this.GetSpouse();
            if (spouse != null)
                Game1.player.changeFriendship(amount, spouse);
        }

        /// <summary>Absolute in-game day count since the start of the save.</summary>
        public int AbsoluteDay => Game1.Date.TotalDays;

        public int HeartsToPoints(int hearts) => hearts * PointsPerHeart;

        /// <summary>Push a line onto the spouse's dialogue stack so it appears when next spoken to.</summary>
        public void PushDialogue(NPC npc, string text)
        {
            if (npc == null || string.IsNullOrWhiteSpace(text))
                return;
            try
            {
                npc.CurrentDialogue.Push(new Dialogue(npc, null, text));
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Could not push dialogue for {npc.Name}: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Show a narration / cutscene line immediately (queued safely by the game).</summary>
        public void ShowNarration(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            try
            {
                Game1.drawObjectDialogue(text);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Could not show narration: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Whether the spouse is at home in the farmhouse with the player present.</summary>
        public bool SpouseIsHomeWithPlayer(NPC spouse)
        {
            try
            {
                return spouse != null
                    && Game1.currentLocation is StardewValley.Locations.FarmHouse
                    && spouse.currentLocation == Game1.currentLocation;
            }
            catch
            {
                return false;
            }
        }

        public int DayOfWeekIndex => (Game1.dayOfMonth - 1) % 7;
        public int WeekIndex => this.AbsoluteDay / 7;
    }
}
