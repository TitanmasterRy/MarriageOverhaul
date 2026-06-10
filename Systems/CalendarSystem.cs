using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>The wedding day-of-month if the anniversary falls in the currently displayed season, else -1.</summary>
        public int AnniversaryDayThisSeason()
        {
            if (this.Data == null || this.Data.WeddingAbsoluteDay < 0)
                return -1;
            DecomposeDay(this.Data.WeddingAbsoluteDay, out int wSeason, out int wDay, out _);
            return wSeason == this.CurrentSeasonIndex ? wDay : -1;
        }

        /// <summary>The configured player birthday day-of-month if it falls in the currently displayed season, else -1.</summary>
        public int BirthdayDayThisSeason()
        {
            if (this.Config.PlayerBirthdayDay <= 0 || string.IsNullOrWhiteSpace(this.Config.PlayerBirthdaySeason))
                return -1;
            int birthSeason = Utility.getSeasonNumber(this.Config.PlayerBirthdaySeason);
            return birthSeason == this.CurrentSeasonIndex ? this.Config.PlayerBirthdayDay : -1;
        }

        /// <summary>Reflectively fetch the calendar's per-day clickable components (null on the daily-quest board).</summary>
        public List<ClickableTextureComponent> GetCalendarDayComponents(object billboard)
        {
            try
            {
                return this.Helper.Reflection
                    .GetField<List<ClickableTextureComponent>>(billboard, "calendarDays", required: false)
                    ?.GetValue();
            }
            catch
            {
                return null;
            }
        }
    }
}
