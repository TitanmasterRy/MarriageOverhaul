using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F4: Spouse sickness ───────────────────────────────────
        private void Sickness_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableSpouseSickness)
                return;

            var info = ExtendedContent.GetSick(spouse.Name);

            // Lingering tiredness from an unattended illness.
            if (this.Data.TiredDaysRemaining > 0)
            {
                this.Data.TiredDaysRemaining--;
                this.PushDialogue(spouse, info.Tired, "sad");
            }

            // Only roll once per season, and never while already sick or having a bad day.
            if (this.Data.SpouseSickToday || this.Data.BadDayToday || this.IsHoneymoon())
                return;
            if (this.Data.LastSicknessSeason == this.CurrentSeasonIndex)
                return;

            this.Data.LastSicknessSeason = this.CurrentSeasonIndex;
            if (this.Rng.NextDouble() >= this.Config.SicknessChancePerSeason)
                return;

            // The spouse wakes up unwell and stays in bed (off-schedule).
            this.Data.SpouseSickToday = true;
            this.Data.SickCuredToday = false;
            try { spouse.ignoreScheduleToday = true; spouse.Halt(); } catch { }
            this.PushDialogue(spouse, info.Sick, "sad");
            this.forceGrumpyToday = true;
        }

        public void Sickness_OnGiftToSpouse(NPC spouse, SObject gift)
        {
            if (!this.Config.EnableSpouseSickness || !this.Data.SpouseSickToday || this.Data.SickCuredToday || gift == null)
                return;
            if (!this.IsCureItem(gift))
                return;

            this.Data.SickCuredToday = true;
            this.ChangeSpouseFriendship(80);
            this.ShowNarration(ExtendedContent.GetSick(spouse.Name).Grateful);
        }

        private bool IsCureItem(SObject gift)
        {
            string n = gift.Name ?? "";
            return n.Contains("Soup")
                || n == "Muscle Remedy" || n == "Energy Tonic"
                || n == "Tea Leaves" || n == "Green Tea" || n == "Ginger Ale";
        }

        private void Sickness_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableSpouseSickness || !this.Data.SpouseSickToday)
                return;

            if (!this.Data.SickCuredToday)
            {
                this.ChangeSpouseFriendship(-60);
                this.Data.TiredDaysRemaining = 2;
            }
            this.Data.SpouseSickToday = false;
            this.Data.SickCuredToday = false;
        }

        // ── F10: Bad days ─────────────────────────────────────────
        private void BadDay_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableBadDays)
                return;

            var info = ExtendedContent.GetBadDay(spouse.Name);

            // Recovered (after a resolved bad day) or lingering flat dialogue.
            if (this.Data.BadDayRecoveredPending)
            {
                this.Data.BadDayRecoveredPending = false;
                this.PushDialogue(spouse, info.Recovered, "happy");
            }
            else if (this.Data.FlatDaysRemaining > 0)
            {
                this.Data.FlatDaysRemaining--;
                this.PushDialogue(spouse, info.Flat, "sad");
            }

            // Don't trigger while sick, during the honeymoon, or in the favorite season.
            if (this.Data.SpouseSickToday || this.Data.BadDayToday || this.IsHoneymoon())
                return;
            var sp = ExtendedContent.GetSeasonPref(spouse.Name);
            if (sp != null && sp.Favorite == this.CurrentSeasonIndex)
                return;
            if (this.AbsoluteDay - this.Data.LastBadDay < 5)
                return;
            if (this.Rng.NextDouble() >= 0.15)
                return;

            this.Data.BadDayToday = true;
            this.Data.BadDayResolved = false;
            this.Data.LastBadDay = this.AbsoluteDay;
            int idx = this.Rng.Next(info.Openers.Count);
            this.Data.BadDayLineIndex = idx;
            this.PushDialogue(spouse, info.Openers[idx], "sad");
        }

        public void BadDay_OnGiftToSpouse(NPC spouse, SObject gift)
        {
            if (!this.Config.EnableBadDays || !this.Data.BadDayToday || this.Data.BadDayResolved || gift == null)
                return;
            // Comfort food: any edible gift counts.
            if (gift.Edibility > 0)
                this.Data.BadDayResolved = true;
        }

        private void BadDay_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableBadDays || !this.Data.BadDayToday)
                return;

            // Talking to them at all that day also counts.
            bool resolved = this.Data.BadDayResolved || this.TalkedToSpouseToday();
            if (resolved)
            {
                this.ChangeSpouseFriendship(50);
                this.Data.BadDayRecoveredPending = true;
            }
            else
            {
                this.ChangeSpouseFriendship(-25);
                this.Data.FlatDaysRemaining = 1;
            }
            this.Data.BadDayToday = false;
            this.Data.BadDayResolved = false;
        }
    }
}
