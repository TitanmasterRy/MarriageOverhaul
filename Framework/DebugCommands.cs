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
            c.Add("mo_jealousy", "Queue a jealousy reaction for tomorrow morning.", this.Cmd_Jealousy);
            c.Add("mo_warn", "Send the divorce warning letter (arrives tomorrow).", this.Cmd_Warn);
            c.Add("mo_hungry", "Mark the spouse as having gone hungry (grumpy line tomorrow).", this.Cmd_Hungry);
            c.Add("mo_mood", "mo_mood <Happy|Neutral|Grumpy> - set mood and show the greeting now.", this.Cmd_Mood);
            c.Add("mo_hearts", "mo_hearts <0-14> - set spouse friendship to N hearts.", this.Cmd_Hearts);
            c.Add("mo_cheat", "Force the cheating 'ultimate punishment' now (spouse has an affair and leaves).", this.Cmd_Cheat);
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
                this.PushDialogue(spouse, SpouseContent.GetHappyMood(spouse.Name, Game1.random), "happy");
            else if (mood == "Grumpy")
                this.PushDialogue(spouse, SpouseContent.GetGrumpyMood(spouse.Name, Game1.random), "angry");
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
            this.Data.LastJealousyDay = -1000;
            this.Data.WarningLetterSentDay = -1000;
            this.Data.WarningActive = false;
            this.Data.ConsecutiveLowDays = 0;
            this.Data.ArgumentThisWeek = false;
            this.Data.LastCheatDay = -1000;
            this.Monitor.Log("All cooldowns reset.", LogLevel.Info);
        }
    }
}
