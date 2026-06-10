using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>Morning: show the disappointed line if the anniversary was missed yesterday.</summary>
        private void Anniversary_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableAnniversary)
                return;

            if (this.Data.AnniversaryMissed)
            {
                this.Data.AnniversaryMissed = false;
                this.PushDialogue(spouse, SpouseContent.GetAnniversary(spouse.Name).Disappointed, "sad");
                this.forceGrumpyToday = true;
            }
        }

        /// <summary>End of day: resolve today's anniversary, then queue tomorrow's reminder if due.</summary>
        private void Anniversary_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableAnniversary)
                return;

            // Resolve a forgotten anniversary that was active today.
            if (this.Data.IsAnniversaryToday)
            {
                if (!this.Data.AnniversaryGiftGivenToday)
                {
                    this.ChangeSpouseFriendship(-100);
                    this.Data.AnniversaryMissed = true;
                }
                this.Data.IsAnniversaryToday = false;
                this.Data.AnniversaryGiftGivenToday = false;
            }

            // Is tomorrow the anniversary? If so, queue the reminder letter for the morning.
            int tomorrow = this.AbsoluteDay + 1;
            if (this.WeddingAnniversaryFallsOn(tomorrow, out int annivYear)
                && this.Data.LastAnniversaryYearProcessed != annivYear)
            {
                this.Data.LastAnniversaryYearProcessed = annivYear;
                this.Data.IsAnniversaryToday = true;        // it will be "today" after the night passes
                this.Data.AnniversaryGiftGivenToday = false;

                this.Data.PendingAnniversaryLetterText = SpouseContent.GetAnniversary(spouse.Name).Reminder;
                this.Helper.GameContent.InvalidateCache("Data/mail");
                Game1.addMailForTomorrow("MO.Anniversary");
            }
        }

        /// <summary>Called from the gift patch when a gift is given to the spouse on the anniversary.</summary>
        private void Anniversary_OnGiftToSpouse(NPC spouse)
        {
            if (!this.Config.EnableAnniversary || !this.Data.IsAnniversaryToday)
                return;
            if (this.Data.AnniversaryGiftGivenToday)
                return;

            this.Data.AnniversaryGiftGivenToday = true;
            this.ChangeSpouseFriendship(200);
            this.ShowNarration(SpouseContent.GetAnniversary(spouse.Name).Sweet);
        }

        /// <summary>Whether the given absolute day shares the wedding's season + day-of-month (in a later year).</summary>
        private bool WeddingAnniversaryFallsOn(int absoluteDay, out int year)
        {
            year = -1;
            if (this.Data.WeddingAbsoluteDay < 0)
                return false;

            DecomposeDay(this.Data.WeddingAbsoluteDay, out int wSeason, out int wDay, out _);
            DecomposeDay(absoluteDay, out int season, out int day, out int dayYear);

            if (season == wSeason && day == wDay && absoluteDay > this.Data.WeddingAbsoluteDay)
            {
                year = dayYear;
                return true;
            }
            return false;
        }

        private static void DecomposeDay(int totalDays, out int seasonIndex, out int dayOfMonth, out int year)
        {
            seasonIndex = (totalDays / 28) % 4;
            dayOfMonth = (totalDays % 28) + 1;
            year = (totalDays / 112) + 1;
        }
    }
}
