using System;
using System.Linq;
using StardewModdingAPI;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>Register the testing console commands with SMAPI.</summary>
        private void RegisterDebugCommands()
        {
            var c = this.Helper.ConsoleCommands;

            c.Add("mo_status", "Show the current Marriage Overhaul state for your spouse.", this.Cmd_Status);
            c.Add("mo_argue", "Force an argument event now (be in the farmhouse with your spouse).", this.Cmd_Argue);
            c.Add("mo_makeup", "Force the 'needs makeup gift' state on your spouse.", this.Cmd_Makeup);
            c.Add("mo_anniversary", "Flag today as your wedding anniversary.", this.Cmd_Anniversary);
            c.Add("mo_setwedding", "Set the wedding date to one year ago today (makes today an anniversary).", this.Cmd_SetWedding);
            c.Add("mo_date", "Force a location date-night invitation now.", this.Cmd_Date);
            c.Add("mo_moviedate", "Force a movie-theater date invitation now (ignores the unlock check).", this.Cmd_MovieDate);
            c.Add("mo_dateevent", "Play tonight's date cutscene now using the real selection (unique-first, then pool, then freeform).", this.Cmd_DateEvent);
            c.Add("mo_datescene", "mo_datescene <u|g|f> [index] - play a specific date scene: unique, generic, or freeform.", this.Cmd_DateScene);
            c.Add("mo_datestatus", "Show date-scene progress (unique/generic seen) for your spouse.", this.Cmd_DateStatus);
            c.Add("mo_resetdates", "Reset which date scenes your spouse has seen (replays the unique ones).", this.Cmd_ResetDates);
            c.Add("mo_listscenes", "List every date scene ID (vanilla unique + generic).", this.Cmd_ListScenes);
            c.Add("mo_playscene", "mo_playscene <sceneId> [npcName] - play any date scene by ID with any NPC, even unmarried. e.g. mo_playscene abigail_1", this.Cmd_PlayScene);
            c.Add("mo_dumpevent", "Print the generated date event script for your spouse WITHOUT running it.", this.Cmd_DumpEvent);
            c.Add("mo_datediag", "Print date-cutscene diagnostics (location/tile/portrait) without warping.", this.Cmd_DateDiag);
            c.Add("mo_tile", "Print your current location name, tile coordinates and facing (to find date spots).", this.Cmd_Tile);
            c.Add("mo_frametest", "mo_frametest <frame> - freeze your spouse on a sprite frame for a few seconds (sprite-frame finder).", this.Cmd_FrameTest);
            c.Add("mo_testwarp", "mo_testwarp <Map> [x] [y] - warp to a location/tile with NO event (isolates the warp).", this.Cmd_TestWarp);
            c.Add("mo_dateminimal", "Run a MINIMAL date event (one line) to test the event pipeline with least surface.", this.Cmd_DateMinimal);
            c.Add("mo_eventhere", "mo_eventhere <raw event script> - run a raw event in your CURRENT location (no warp).", this.Cmd_EventHere);
            c.Add("mo_jealousy", "Queue a jealousy reaction for tomorrow morning.", this.Cmd_Jealousy);
            c.Add("mo_warn", "Send the divorce warning letter (arrives tomorrow).", this.Cmd_Warn);
            c.Add("mo_hungry", "Mark the spouse as having gone hungry (grumpy line tomorrow).", this.Cmd_Hungry);
            c.Add("mo_mood", "mo_mood <Happy|Neutral|Grumpy> - set mood and show the greeting now.", this.Cmd_Mood);
            c.Add("mo_hearts", "mo_hearts <0-14> - set spouse friendship to N hearts.", this.Cmd_Hearts);
            c.Add("mo_reset", "Reset all Marriage Overhaul cooldowns.", this.Cmd_Reset);
            c.Add("mo_cheat", "Force the cheating 'ultimate punishment' now (spouse has an affair and leaves).", this.Cmd_Cheat);
        }

        private NPC RequireSpouse()
        {
            NPC spouse = this.GetSpouse();
            if (spouse == null)
                this.Monitor.Log("You aren't married (or no save is loaded).", LogLevel.Warn);
            return spouse;
        }

        private void Cmd_Status(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;

            string name = spouse.Name;
            int pts = this.GetSpousePoints();
            string scheduleStr = (this.Data.ProvideDays != null && this.Data.ProvideDays.Count == 7)
                ? string.Join(" ", this.Data.ProvideDays.Select(p => p ? "P" : "C"))
                : "(none)";

            this.Monitor.Log(
                $"\n── Marriage Overhaul status ──" +
                $"\n Spouse:            {name} ({(SpouseContent.IsVanilla(name) ? "vanilla" : "modded/generic")})" +
                $"\n Friendship:        {pts} pts ({pts / 250.0:0.0} hearts)" +
                $"\n Mood:              {this.Data.Mood}" +
                $"\n Feeding schedule:  {scheduleStr}   (today = {(this.IsProvideDayToday() ? "PROVIDE" : "COOK")})" +
                $"\n Fed yesterday:     {this.Data.FedYesterday}   Went hungry: {this.Data.WentHungry}" +
                $"\n Makeup needed:     {this.Data.MakeupNeeded}" + (this.Data.MakeupNeeded ? $" ({this.Data.MakeupCategory}, day {this.AbsoluteDay - this.Data.MakeupStartDay}/{MakeupWindowDays})" : "") +
                $"\n Argument:          last day {this.Data.LastArgumentDay} (cooldown {ArgumentCooldownDays}d), this week: {this.ArgumentHappenedThisWeek()}" +
                $"\n Warning active:    {this.Data.WarningActive}   Consecutive low days: {this.Data.ConsecutiveLowDays}/{this.Config.ConsecutiveDaysBeforeAutoDivorce}" +
                $"\n Jealousy:          pending {this.Data.PendingJealousy}, last day {this.Data.LastJealousyDay}" +
                $"\n Date night:        last day {this.Data.LastDateNightDay} (cooldown {DateCooldownDays}d), accepted tonight: {this.Data.DateAcceptedTonight}" +
                $"\n Wedding day:       {this.Data.WeddingAbsoluteDay} (today = {this.AbsoluteDay}), anniversary today: {this.Data.IsAnniversaryToday}",
                LogLevel.Info);
        }

        private void Cmd_Argue(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.Data.LastArgumentDay = this.AbsoluteDay;
            this.MarkArgumentThisWeek();
            this.StartArgument(spouse);
            this.Monitor.Log("Argument started. (If nothing appeared, make sure no menu is open.)", LogLevel.Info);
        }

        private void Cmd_Makeup(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.Makeup_Begin(spouse);
            this.Monitor.Log($"Makeup state set. Category wanted: '{this.Data.MakeupCategory}'. Hint shows next morning.", LogLevel.Info);
        }

        private void Cmd_Anniversary(string cmd, string[] args)
        {
            if (this.RequireSpouse() == null)
                return;
            this.Data.IsAnniversaryToday = true;
            this.Data.AnniversaryGiftGivenToday = false;
            this.Monitor.Log("Today is now flagged as your anniversary. Gift your spouse for +200, or forget for -100 at day's end.", LogLevel.Info);
        }

        private void Cmd_SetWedding(string cmd, string[] args)
        {
            if (this.RequireSpouse() == null)
                return;
            this.Data.WeddingAbsoluteDay = this.AbsoluteDay - 112; // exactly one year ago
            this.Data.IsAnniversaryToday = true;
            this.Data.AnniversaryGiftGivenToday = false;
            this.Data.LastAnniversaryYearProcessed = -1;
            this.Monitor.Log("Wedding date set to one year ago today; today is now an anniversary.", LogLevel.Info);
        }

        private void Cmd_Date(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.Data.LastDateNightDay = this.AbsoluteDay;
            this.PresentDateOffer(spouse, movie: false);
            this.Monitor.Log("Date offer presented. Accept, then use 'world_settime 2000' for the scene.", LogLevel.Info);
        }

        private void Cmd_MovieDate(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.Data.LastDateNightDay = this.AbsoluteDay;
            this.PresentDateOffer(spouse, movie: true);
            this.Monitor.Log("Movie date offer presented. Accept, then use 'world_settime 2000' for the scene.", LogLevel.Info);
        }

        private void Cmd_DateEvent(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.Monitor.Log($"Playing tonight's date scene for {spouse.Name} via the real selection...", LogLevel.Info);
            this.ChooseAndPlayDateScene(spouse);
        }

        private void Cmd_DateScene(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            if (args.Length < 1)
            {
                this.Monitor.Log("Usage: mo_datescene <u|g|f> [index]   (u = unique, g = generic, f = freeform)", LogLevel.Warn);
                return;
            }
            string kind = args[0].ToLowerInvariant();
            int index = args.Length > 1 && int.TryParse(args[1], out int ix) ? ix : 0;

            if (kind == "u" || kind == "unique")
            {
                var u = DateScenes.GetUnique(spouse.Name);
                if (u == null || u.Count == 0)
                {
                    this.Monitor.Log($"{spouse.Name} has no unique scenes (modded spouse).", LogLevel.Info);
                    return;
                }
                index = System.Math.Clamp(index, 0, u.Count - 1);
                this.Monitor.Log($"Playing {spouse.Name} unique scene #{index} at {u[index].Map}.", LogLevel.Info);
                this.StartDateScene(spouse, u[index]);
            }
            else if (kind == "g" || kind == "generic")
            {
                index = System.Math.Clamp(index, 0, DateScenes.GenericCount - 1);
                var scene = DateScenes.GenericPool[index];
                this.Monitor.Log($"Playing generic scene #{index} at {scene.Map}.", LogLevel.Info);
                this.StartDateScene(spouse, scene);
            }
            else if (kind == "f" || kind == "free" || kind == "freeform")
            {
                this.Monitor.Log("Playing a freeform date scene from the shared dialogue pool.", LogLevel.Info);
                this.PlayFreeformDate(spouse);
            }
            else
            {
                this.Monitor.Log("Usage: mo_datescene <u|g|f> [index]", LogLevel.Warn);
            }
        }

        private void Cmd_ListScenes(string cmd, string[] args)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("\nDate scene IDs (use: mo_playscene <id> [npcName]):");
            string lastOwner = null;
            foreach (var info in DateScenes.AllSceneInfo())
            {
                if (info.Owner != lastOwner)
                {
                    sb.Append($"\n  -- {info.Owner} --");
                    lastOwner = info.Owner;
                }
                sb.Append($"\n    {info.Id,-14} ({info.Map})");
            }
            sb.Append("\n  -- freeform --");
            sb.Append("\n    freeform       (random pool scene; needs an npc name if unmarried)");
            this.Monitor.Log(sb.ToString(), LogLevel.Info);
        }

        private void Cmd_PlayScene(string cmd, string[] args)
        {
            if (args.Length < 1)
            {
                this.Monitor.Log("Usage: mo_playscene <sceneId> [npcName]   (run mo_listscenes for ids)", LogLevel.Warn);
                return;
            }
            string id = args[0].ToLowerInvariant();

            // Resolve an explicit NPC name if given (any villager works, married or not).
            NPC npc = null;
            if (args.Length > 1)
            {
                npc = Game1.getCharacterFromName(args[1]);
                if (npc == null)
                {
                    this.Monitor.Log($"NPC '{args[1]}' not found.", LogLevel.Warn);
                    return;
                }
            }

            if (id == "freeform" || id == "f")
            {
                npc ??= this.GetSpouse();
                if (npc == null)
                {
                    this.Monitor.Log("Freeform needs an npc when you're unmarried: mo_playscene freeform Abigail", LogLevel.Warn);
                    return;
                }
                this.Monitor.Log($"Playing a freeform scene with {npc.Name}.", LogLevel.Info);
                this.PlayFreeformDate(npc);
                return;
            }

            DateScene scene = DateScenes.GetById(id);
            if (scene == null)
            {
                this.Monitor.Log($"Unknown scene id '{id}'. Run mo_listscenes for the full list.", LogLevel.Warn);
                return;
            }

            // No explicit NPC: use the scene's owner (vanilla spouse it was written for), else your spouse.
            if (npc == null && scene.Owner != null)
                npc = Game1.getCharacterFromName(scene.Owner);
            npc ??= this.GetSpouse();
            if (npc == null)
            {
                this.Monitor.Log($"Specify an NPC for this scene: mo_playscene {id} <npcName>", LogLevel.Warn);
                return;
            }

            this.Monitor.Log($"Playing scene '{scene.Id}' with {npc.Name} at {scene.Map}.", LogLevel.Info);
            this.StartDateScene(npc, scene);
        }

        private void Cmd_DateStatus(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            string name = spouse.Name;
            int uc = DateScenes.UniqueCount(name);
            var useen = this.Data.SeenUniqueDateScenes.TryGetValue(name, out var us) ? us : new System.Collections.Generic.List<int>();
            var gseen = this.Data.SeenGenericDateScenes.TryGetValue(name, out var gs) ? gs : new System.Collections.Generic.List<int>();
            this.Monitor.Log(
                $"Date scenes for {name}: unique {useen.Count}/{uc} seen, generic {gseen.Count}/{DateScenes.GenericCount} seen. " +
                $"Recent generic: [{string.Join(",", this.Data.RecentGenericDateScenes)}].",
                LogLevel.Info);
        }

        private void Cmd_ResetDates(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            string name = spouse.Name;
            this.Data.SeenUniqueDateScenes.Remove(name);
            this.Data.SeenGenericDateScenes.Remove(name);
            this.Data.RecentGenericDateScenes.Clear();
            this.Monitor.Log($"Reset seen date scenes for {name}. Unique scenes will replay.", LogLevel.Info);
        }

        private void Cmd_DumpEvent(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            var events = SpouseContent.GetDateEvents(spouse.Name);
            if (events == null || events.Count == 0)
            {
                this.Monitor.Log($"{spouse.Name} has no scripted date events.", LogLevel.Info);
                return;
            }
            var spot = SpouseContent.GetDateSpot(spouse.Name);
            for (int i = 0; i < events.Count; i++)
                this.Monitor.Log($"--- event #{i + 1} ({(i == 0 ? "unique" : "pool")}, {spot.Map} @ {spot.X},{spot.Y}) ---\n{this.BuildDateEventScript(spouse, events[i], spot)}", LogLevel.Info);
        }

        private void Cmd_DateDiag(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.LogDateLocationDiagnostics(spouse, SpouseContent.GetDateSpot(spouse.Name));
        }

        private void Cmd_Tile(string cmd, string[] args)
        {
            int dir = Game1.player.FacingDirection;
            string dirName = dir == 0 ? "up/north" : dir == 1 ? "right/east" : dir == 2 ? "down/south (ocean on beach)" : "left/west";
            this.Monitor.Log(
                $"Location: '{Game1.currentLocation?.NameOrUniqueName}'  Tile: ({Game1.player.TilePoint.X}, {Game1.player.TilePoint.Y})  Facing: {dir} ({dirName})",
                LogLevel.Info);
        }

        private void Cmd_FrameTest(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            if (args.Length < 1 || !int.TryParse(args[0], out int frame))
            {
                this.Monitor.Log("Usage: mo_frametest <frame>  (freezes your spouse on a sprite frame for a few seconds)", LogLevel.Warn);
                return;
            }
            int x = Game1.player.TilePoint.X;
            int y = Game1.player.TilePoint.Y;
            string n = spouse.Name;
            // Show the spouse frozen on the requested sprite frame for a few seconds, in an event context.
            string script = $"continue/{x} {y}/farmer {x} {y} 2 {n} {x + 1} {y} 2/skippable/showFrame {n} {frame}/pause 3500/end";
            this.Monitor.Log($"[date-debug] showing {n} on frame {frame} for ~3.5s.", LogLevel.Info);
            try
            {
                Game1.currentLocation.startEvent(new Event(script));
            }
            catch (System.Exception ex)
            {
                this.Monitor.Log($"[date-debug] frame test failed: {ex.Message}", LogLevel.Error);
            }
        }

        private void Cmd_TestWarp(string cmd, string[] args)
        {
            if (args.Length < 1)
            {
                this.Monitor.Log("Usage: mo_testwarp <Map> [x] [y]", LogLevel.Warn);
                return;
            }
            string map = args[0];
            int x = args.Length > 1 && int.TryParse(args[1], out int px) ? px : 38;
            int y = args.Length > 2 && int.TryParse(args[2], out int py) ? py : 15;
            this.Monitor.Log($"[date-debug] warping to {map} ({x},{y}) with NO event...", LogLevel.Info);
            try
            {
                Game1.warpFarmer(map, x, y, 2);
                this.Monitor.Log("[date-debug] warpFarmer call returned (warp queued). If the game survives this, the warp is fine.", LogLevel.Info);
            }
            catch (System.Exception ex)
            {
                this.Monitor.Log($"[date-debug] warp threw: {ex}", LogLevel.Error);
            }
        }

        private void Cmd_DateMinimal(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            string map = args.Length > 0 ? args[0] : "Beach";
            int x = args.Length > 1 && int.TryParse(args[1], out int px) ? px : 38;
            int y = args.Length > 2 && int.TryParse(args[2], out int py) ? py : 19;
            var spot = new DateSpot { Map = map, X = x, Y = y, Facing = 2 };
            var def = new DateEventScript
            {
                Beats = new System.Collections.Generic.List<string>
                {
                    "speak {n} \"This is just a test of our date night.\"",
                    "emote {n} 20", "pause 600"
                }
            };
            this.Monitor.Log($"[date-debug] running MINIMAL date event at {map} ({x},{y})...", LogLevel.Info);
            this.StartDateCutscene(spouse, def, spot);
        }

        private void Cmd_EventHere(string cmd, string[] args)
        {
            if (args.Length < 1)
            {
                this.Monitor.Log("Usage: mo_eventhere <raw event script>  (e.g. continue/8 8/farmer 8 8 2/skippable/pause 500/end)", LogLevel.Warn);
                return;
            }
            // SMAPI splits args on spaces; rejoin to reconstruct the full script (slashes are preserved).
            string script = string.Join(" ", args);
            this.Monitor.Log($"[date-debug] running raw event in '{Game1.currentLocation?.NameOrUniqueName}':\n{script}", LogLevel.Info);
            try
            {
                Game1.currentLocation.startEvent(new Event(script));
                this.Monitor.Log("[date-debug] startEvent returned without throwing.", LogLevel.Info);
            }
            catch (System.Exception ex)
            {
                this.Monitor.Log($"[date-debug] startEvent threw: {ex}", LogLevel.Error);
            }
        }

        private void Cmd_Jealousy(string cmd, string[] args)
        {
            if (this.RequireSpouse() == null)
                return;
            this.Data.PendingJealousy = true;
            this.Data.LastJealousyDay = this.AbsoluteDay;
            this.Monitor.Log("Jealousy queued. Sleep to see the -15 and dialogue next morning.", LogLevel.Info);
        }

        private void Cmd_Warn(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            this.Data.WarningLetterSentDay = -1000; // bypass cooldown
            this.Data.WarningActive = false;
            this.SendWarningLetter(spouse);
            this.Monitor.Log("Warning letter queued; it arrives in tomorrow's mailbox.", LogLevel.Info);
        }

        private void Cmd_Hungry(string cmd, string[] args)
        {
            if (this.RequireSpouse() == null)
                return;
            this.Data.WentHungry = true;
            this.Data.FedYesterday = false;
            this.Monitor.Log("Spouse marked hungry; grumpy hungry line shows next morning.", LogLevel.Info);
        }

        private void Cmd_Mood(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            if (args.Length < 1)
            {
                this.Monitor.Log("Usage: mo_mood <Happy|Neutral|Grumpy>", LogLevel.Warn);
                return;
            }
            string mood = char.ToUpper(args[0][0]) + args[0].Substring(1).ToLower();
            if (mood != "Happy" && mood != "Neutral" && mood != "Grumpy")
            {
                this.Monitor.Log("Mood must be Happy, Neutral, or Grumpy.", LogLevel.Warn);
                return;
            }
            this.Data.Mood = mood;
            if (mood == "Happy")
                this.PushDialogue(spouse, SpouseContent.GetHappyMood(spouse.Name));
            else if (mood == "Grumpy")
                this.PushDialogue(spouse, SpouseContent.GetGrumpyMood(spouse.Name));
            this.Monitor.Log($"Mood set to {mood}. Talk to your spouse to hear it.", LogLevel.Info);
        }

        private void Cmd_Hearts(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            if (args.Length < 1 || !int.TryParse(args[0], out int hearts))
            {
                this.Monitor.Log("Usage: mo_hearts <0-14>", LogLevel.Warn);
                return;
            }
            hearts = Math.Max(0, Math.Min(14, hearts));
            int target = this.HeartsToPoints(hearts);
            int delta = target - this.GetSpousePoints();
            this.ChangeSpouseFriendship(delta);
            this.Monitor.Log($"Spouse friendship set to {hearts} hearts ({target} pts).", LogLevel.Info);
        }

        private void Cmd_Cheat(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            NPC partner = this.PickAffairPartner(spouse.Name);
            if (partner == null)
            {
                this.Monitor.Log("No eligible single candidate found to cheat with.", LogLevel.Warn);
                return;
            }
            this.Data.CheatPartner = partner.Name;
            this.Data.PendingCheatReveal = true;
            this.Monitor.Log($"Forcing cheating reveal: {spouse.Name} with {partner.Name}.", LogLevel.Info);
            this.Cheating_OnDayStarted(spouse);
        }

        private void Cmd_Reset(string cmd, string[] args)
        {
            if (this.RequireSpouse() == null)
                return;
            this.Data.LastArgumentDay = -1000;
            this.Data.LastDateNightDay = -1000;
            this.Data.LastJealousyDay = -1000;
            this.Data.WarningLetterSentDay = -1000;
            this.Data.WarningActive = false;
            this.Data.ConsecutiveLowDays = 0;
            this.Data.ArgumentThisWeek = false;
            this.Monitor.Log("All cooldowns reset.", LogLevel.Info);
        }
    }
}
