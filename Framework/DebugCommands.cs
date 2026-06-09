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
            c.Add("mo_dateevent", "Play a date cutscene immediately (real event if the spouse has one, else narration).", this.Cmd_DateEvent);
            c.Add("mo_dumpevent", "Print the generated date event script for your spouse WITHOUT running it.", this.Cmd_DumpEvent);
            c.Add("mo_datediag", "Print date-cutscene diagnostics (location/tile/portrait) without warping.", this.Cmd_DateDiag);
            c.Add("mo_tile", "Print your current location name, tile coordinates and facing (to find date spots).", this.Cmd_Tile);
            c.Add("mo_frametest", "mo_frametest <frame> - freeze your spouse on a sprite frame for a few seconds (to find the camera frame).", this.Cmd_FrameTest);
            c.Add("mo_testwarp", "mo_testwarp <Map> [x] [y] - warp to a location/tile with NO event (isolates the warp).", this.Cmd_TestWarp);
            c.Add("mo_dateminimal", "Run a MINIMAL date event (one line) to test the event pipeline with least surface.", this.Cmd_DateMinimal);
            c.Add("mo_eventhere", "mo_eventhere <raw event script> - run a raw event in your CURRENT location (no warp).", this.Cmd_EventHere);
            c.Add("mo_jealousy", "Queue a jealousy reaction for tomorrow morning.", this.Cmd_Jealousy);
            c.Add("mo_warn", "Send the divorce warning letter (arrives tomorrow).", this.Cmd_Warn);
            c.Add("mo_hungry", "Mark the spouse as having gone hungry (grumpy line tomorrow).", this.Cmd_Hungry);
            c.Add("mo_mood", "mo_mood <Happy|Neutral|Grumpy> - set mood and show the greeting now.", this.Cmd_Mood);
            c.Add("mo_hearts", "mo_hearts <0-14> - set spouse friendship to N hearts.", this.Cmd_Hearts);
            c.Add("mo_reset", "Reset all Marriage Overhaul cooldowns.", this.Cmd_Reset);
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
            var events = SpouseContent.GetDateEvents(spouse.Name);
            if (events != null && events.Count > 0)
            {
                int idx = this.PickDateEventIndex(spouse.Name, events.Count);
                this.Monitor.Log($"Playing date cutscene #{idx + 1} for {spouse.Name} at {events[idx].Map}.", LogLevel.Info);
                this.StartDateCutscene(spouse, events[idx]);
            }
            else
            {
                this.Monitor.Log($"{spouse.Name} has no scripted cutscene; showing narration scene.", LogLevel.Info);
                this.ShowNarration(SpouseContent.GetDate(spouse.Name).Scene);
            }
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
            for (int i = 0; i < events.Count; i++)
                this.Monitor.Log($"--- event #{i + 1} ({events[i].Map} @ {events[i].X},{events[i].Y}) ---\n{this.BuildDateEventScript(spouse, events[i])}", LogLevel.Info);
        }

        private void Cmd_DateDiag(string cmd, string[] args)
        {
            NPC spouse = this.RequireSpouse();
            if (spouse == null)
                return;
            var events = SpouseContent.GetDateEvents(spouse.Name);
            if (events == null || events.Count == 0)
            {
                this.Monitor.Log($"{spouse.Name} has no scripted date events (would use narration).", LogLevel.Info);
                return;
            }
            this.LogDateLocationDiagnostics(spouse, events[0]);
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
                this.Monitor.Log("Usage: mo_frametest <frame>  (try numbers like 24-30 to find Haley's camera frame)", LogLevel.Warn);
                return;
            }
            int x = Game1.player.TilePoint.X;
            int y = Game1.player.TilePoint.Y;
            string n = spouse.Name;
            // Show the spouse frozen on the requested sprite frame for a few seconds, in an event context.
            string script = $"continue/{x} {y}/farmer {x} {y} 2 {n} {x + 1} {y} 2/skippable/showFrame {n} {frame}/pause 3500/end";
            this.Monitor.Log($"[date-debug] showing {n} on frame {frame} for ~3.5s. If you see a camera, that's the frame!", LogLevel.Info);
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
            int x = args.Length > 1 && int.TryParse(args[1], out int px) ? px : 38;
            int y = args.Length > 2 && int.TryParse(args[2], out int py) ? py : 15;
            string map = args.Length > 0 ? args[0] : "Beach";
            var def = new DateEventScript
            {
                Map = map,
                X = x,
                Y = y,
                Lines = new System.Collections.Generic.List<string> { "This is just a test of our date night." }
            };
            this.Monitor.Log($"[date-debug] running MINIMAL date event at {map} ({x},{y})...", LogLevel.Info);
            this.StartDateCutscene(spouse, def);
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
