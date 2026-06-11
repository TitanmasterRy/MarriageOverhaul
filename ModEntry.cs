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
        private bool forceGrumpyToday;

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
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
            helper.Events.Multiplayer.ModMessageReceived += this.OnModMessageReceived;

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
            this.Multiplayer_OnSaveLoaded();
        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            this.Multiplayer_OnSaving();
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.argumentTriggeredToday = false;
            this.forceGrumpyToday = false;

            // Farmhands wait for the host to send their saved data before acting (avoids placeholder-data
            // glitches on the join day, e.g. re-firing one-time milestones). Resolves next day.
            if (!this.dataReady)
                return;

            NPC spouse = this.GetSpouse();
            if (spouse == null)
                return;

            // Generate / refresh the weekly feeding schedule.
            if (this.Config.EnableFeeding)
                this.Feeding_EnsureWeeklySchedule();

            // Pull the real wedding date from vanilla friendship data when available. This self-heals
            // saves where the mod was installed after marriage (those used to record "today" as the
            // wedding date, giving everyone a wrong anniversary). Falls back to "today" only for
            // modded spouses with no vanilla wedding data.
            int realWedding = this.GetVanillaWeddingAbsoluteDay();
            if (realWedding >= 0)
            {
                if (this.Data.WeddingAbsoluteDay != realWedding)
                {
                    bool wasSet = this.Data.WeddingAbsoluteDay >= 0;
                    this.Data.WeddingAbsoluteDay = realWedding;
                    // If we just corrected a wrong stored date, let this year's real anniversary fire.
                    if (wasSet)
                        this.Data.LastAnniversaryYearProcessed = -1;
                }
            }
            else if (this.Data.WeddingAbsoluteDay < 0)
            {
                this.Data.WeddingAbsoluteDay = this.AbsoluteDay;
            }

            // Apply consequences queued from yesterday and inject morning dialogue.
            this.Cheating_OnDayStarted(spouse);
            this.Jealousy_OnDayStarted(spouse);
            this.Feeding_OnDayStarted(spouse);
            this.Anniversary_OnDayStarted(spouse);
            this.Makeup_OnDayStarted(spouse);
            this.Mood_OnDayStarted(spouse);
            this.Divorce_OnDayStarted(spouse);
            this.Extended_OnDayStarted(spouse);
        }

        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            NPC spouse = this.GetSpouse();
            if (spouse != null)
            {
                this.Feeding_OnDayEnding(spouse);
                this.Anniversary_OnDayEnding(spouse);
                this.Divorce_OnDayEnding(spouse);
                this.Cheating_OnDayEnding(spouse);
                this.Extended_OnDayEnding(spouse);

                // Record today's friendship for tomorrow's trend comparison.
                this.Data.LastFriendshipPoints = this.GetSpousePoints();
            }

            // Farmhands persist through the host: push the day's final state for saving.
            this.SendFarmhandDataToHost();
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            if (!this.dataReady)
                return;

            NPC spouse = this.GetSpouse();
            if (spouse == null)
                return;

            this.Argument_OnTimeChanged(spouse, e.NewTime);
            this.Extended_OnTimeChanged(spouse, e.NewTime);
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
                    if (!string.IsNullOrEmpty(this.Data?.PendingCheatLetterText))
                        dict["MO.Cheating"] = this.Data.PendingCheatLetterText;
                    this.Extended_InjectMail(dict);
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

        /// <summary>Push a line with a matching facial expression (angry / sad / happy / love / neutral).</summary>
        public void PushDialogue(NPC npc, string text, string emotion)
        {
            this.PushDialogue(npc, EmotionPrefix(emotion) + text);
        }

        /// <summary>The dialogue portrait token for an emotion. The game maps it to that NPC's portrait.</summary>
        private static string EmotionPrefix(string emotion)
        {
            switch (emotion)
            {
                case "angry": return "$a ";
                case "sad": return "$s ";
                case "happy": return "$h ";
                case "love": return "$l ";
                default: return ""; // neutral — no token
            }
        }

        /// <summary>Show the spouse speaking a single line now, with a matching facial expression.</summary>
        public void ShowSpouseSpeech(NPC npc, string text, string emotion)
        {
            if (npc == null || string.IsNullOrWhiteSpace(text))
                return;
            try
            {
                npc.CurrentDialogue.Push(new Dialogue(npc, null, EmotionPrefix(emotion) + text));
                Game1.drawDialogue(npc);
            }
            catch
            {
                this.ShowNarration(text);
            }
        }

        /// <summary>
        /// Queue a mod letter for tomorrow. Fixed-ID mail is only delivered once per save (the ID is
        /// recorded permanently in mailReceived), so clear that flag first to let repeatable letters resend.
        /// </summary>
        public void QueueRepeatableMail(string id)
        {
            try { Game1.player.mailReceived.Remove(id); } catch { }
            try { Game1.addMailForTomorrow(id); } catch { }
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
