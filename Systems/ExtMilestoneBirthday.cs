using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F1: Relationship milestones (year 1 / 3 / 5) ──────────
        private void Milestone_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableMilestones || this.Data.WeddingAbsoluteDay < 0)
                return;

            if (!this.WeddingAnniversaryFallsOn(this.AbsoluteDay, out int dayYear))
                return;

            DecomposeDay(this.Data.WeddingAbsoluteDay, out _, out _, out int weddingYear);
            int milestoneYear = dayYear - weddingYear;

            if ((milestoneYear == 1 || milestoneYear == 3 || milestoneYear == 5)
                && !this.Data.MilestonesFired.Contains(milestoneYear))
            {
                this.Data.MilestonesFired.Add(milestoneYear);
                this.ShowSpouseSpeech(spouse, ExtendedContent.GetMilestone(spouse.Name, milestoneYear), "love");
                this.InsideJokes_Record(spouse);
            }
        }

        // ── F11: Player birthday ──────────────────────────────────
        private void Birthday_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableBirthdaySystem)
                return;
            // Vanilla has no player birthday, so it's configured in the mod (day 0 = disabled).
            if (this.Config.PlayerBirthdayDay <= 0 || string.IsNullOrWhiteSpace(this.Config.PlayerBirthdaySeason))
                return;
            if (!string.Equals(this.Config.PlayerBirthdaySeason, Game1.currentSeason, System.StringComparison.OrdinalIgnoreCase)
                || this.Config.PlayerBirthdayDay != Game1.dayOfMonth)
                return;
            if (this.Data.LastBirthdayYearProcessed == Game1.year)
                return;

            this.Data.LastBirthdayYearProcessed = Game1.year;

            var info = ExtendedContent.GetBirthday(spouse.Name);

            // Leave the personalized "cooked" gift in the fridge.
            Item gift = this.CreateItem(info.GiftItem);
            if (gift != null)
                this.PutInFridge(gift);

            // Sweet morning scene; add the pointed anniversary callback if one was missed in the last year.
            string line = info.Line;
            if (this.AbsoluteDay - this.Data.LastMissedAnniversaryDay < DaysPerYear)
                line += "^^" + info.PointedAddon;

            this.ShowSpouseSpeech(spouse, line, "love");
        }
    }
}
