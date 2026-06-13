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

        // The spouse greeting lines pushed this morning, so they can be re-shown if a marriage-dialogue
        // mod (or vanilla marriage dialogue) clears them when the player first talks. See HarmonyPatches.
        private readonly List<string> pendingSpouseLines = new List<string>();

        // Dialogue boxes waiting to be shown once the player actually has control. Forcing a dialogue open
        // during the morning transition desyncs multiplayer (other players get stuck on a black screen),
        // so immediate narration / spouse scenes are queued here and flushed when it's safe.
        private readonly Queue<System.Action> pendingDisplays = new Queue<System.Action>();

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            this.Config = helper.ReadConfig<ModConfig>();
            this.Data = new ModData();
            I18n.Init(helper.Translation);

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saving += this.OnSaving;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.DayEnding += this.OnDayEnding;
            helper.Events.GameLoop.TimeChanged += this.OnTimeChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
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
            this.pendingSpouseLines.Clear();
            this.pendingDisplays.Clear();

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
            NPC spouse = this.GetSpouse();
            if (spouse == null)
                return;

            this.Argument_OnTimeChanged(spouse, e.NewTime);
            this.Extended_OnTimeChanged(spouse, e.NewTime);
        }

        /// <summary>Flush one queued dialogue box once the player has control (never during transitions, which would desync multiplayer).</summary>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (this.pendingDisplays.Count == 0)
                return;
            if (!Context.IsPlayerFree || Game1.activeClickableMenu != null)
                return;
            System.Action show = this.pendingDisplays.Dequeue();
            try { show(); }
            catch (Exception ex) { this.Monitor.Log($"Deferred display failed: {ex.Message}", LogLevel.Trace); }
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
                // Only act once actually married. While engaged (including the wedding morning, before the
                // ceremony runs) player.spouse is already set, so without this the mod's morning dialogue
                // would fire during the wedding event and break it.
                if (!player.isMarriedOrRoommates())
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
                // With dialogue-mod compatibility on, defer the spouse's greeting instead of pushing it to
                // the dialogue stack now — a marriage-dialogue mod (e.g. Haley Ever After) would overwrite it
                // before it's seen. The checkAction patch shows it on the first talk so it always appears.
                if (this.Config != null && this.Config.EnableDialogueCompat && npc.Name == this.SpouseName)
                {
                    this.pendingSpouseLines.Add(text);
                    return;
                }
                npc.CurrentDialogue.Push(new Dialogue(npc, null, text));
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Could not push dialogue for {npc.Name}: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Whether the spouse has deferred greeting lines waiting to be shown.</summary>
        public bool HasPendingSpouseLines() => this.pendingSpouseLines.Count > 0;

        /// <summary>Return and clear the deferred spouse greeting lines.</summary>
        public List<string> TakePendingSpouseLines()
        {
            var copy = new List<string>(this.pendingSpouseLines);
            this.pendingSpouseLines.Clear();
            return copy;
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

        /// <summary>Show the spouse speaking a single line, with a matching facial expression. Deferred until the player has control (multiplayer-safe).</summary>
        public void ShowSpouseSpeech(NPC npc, string text, string emotion)
        {
            if (npc == null || string.IsNullOrWhiteSpace(text))
                return;
            string line = EmotionPrefix(emotion) + text;
            this.QueueOrShow(() =>
            {
                try
                {
                    npc.CurrentDialogue.Push(new Dialogue(npc, null, line));
                    Game1.drawDialogue(npc);
                }
                catch
                {
                    try { Game1.drawObjectDialogue(text); } catch { }
                }
            });
        }

        /// <summary>Show a dialogue now if the player has control, otherwise queue it until they do (avoids forcing dialogue during the morning transition, which desyncs multiplayer).</summary>
        private void QueueOrShow(System.Action show)
        {
            if (Context.IsPlayerFree && Game1.activeClickableMenu == null)
            {
                try { show(); }
                catch (Exception ex) { this.Monitor.Log($"Display failed: {ex.Message}", LogLevel.Trace); }
            }
            else
            {
                this.pendingDisplays.Enqueue(show);
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
            this.QueueOrShow(() => Game1.drawObjectDialogue(text));
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
