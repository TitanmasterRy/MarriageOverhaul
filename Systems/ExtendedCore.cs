using System.Collections.Generic;
using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>Morning dispatcher for all extended systems.</summary>
        private void Extended_OnDayStarted(NPC spouse)
        {
            this.Honeymoon_OnDayStarted(spouse);
            this.Preferences_OnDayStarted(spouse);
            this.Milestone_OnDayStarted(spouse);
            this.Birthday_OnDayStarted(spouse);
            this.SkillMilestone_OnDayStarted(spouse);
            this.Sickness_OnDayStarted(spouse);
            this.BadDay_OnDayStarted(spouse);
            this.Achievements_OnDayStarted(spouse);
            this.Children_OnDayStarted(spouse);
            this.InsideJokes_OnDayStarted(spouse);
            this.Visitor_OnDayStarted(spouse);
            this.Requests_OnDayStarted(spouse);
            this.Seasonal_OnDayStarted(spouse);
            this.Gossip_OnDayStarted(spouse);
            this.Dreams_OnDayStarted(spouse);
            this.Chores_OnDayStarted(spouse);
        }

        /// <summary>End-of-day dispatcher for all extended systems.</summary>
        private void Extended_OnDayEnding(NPC spouse)
        {
            this.RecordRollingFriendship();
            this.RecordMoodScore(this.ComputeDailyMoodScore());
            this.Sickness_OnDayEnding(spouse);
            this.BadDay_OnDayEnding(spouse);
            this.Seasonal_OnDayEnding(spouse);
            this.Requests_OnDayEnding(spouse);
            this.Letters_OnDayEnding(spouse);
            this.Achievements_OnDayEnding(spouse);
        }

        private void Extended_OnTimeChanged(NPC spouse, int time)
        {
            this.Visitor_OnTimeChanged(spouse, time);
        }

        /// <summary>Gift dispatcher additions for the extended systems (called from Gift_OnGiftToSpouse).</summary>
        public void Extended_OnGiftToSpouse(NPC spouse, SObject gift)
        {
            this.Sickness_OnGiftToSpouse(spouse, gift);
            this.BadDay_OnGiftToSpouse(spouse, gift);
            this.Requests_OnGiftToSpouse(spouse, gift);
        }

        /// <summary>Add the extended mail entries (love letters, request notes) to the Data/mail asset.</summary>
        public void Extended_InjectMail(IDictionary<string, string> dict)
        {
            if (!string.IsNullOrEmpty(this.Data?.PendingLoveLetterText))
                dict["MO.LoveLetter"] = this.Data.PendingLoveLetterText;
            if (!string.IsNullOrEmpty(this.Data?.PendingRequestLetterText))
                dict["MO.Request"] = this.Data.PendingRequestLetterText;
        }

        /// <summary>A simple proxy of how the relationship is going today (drives chore quality, F17).</summary>
        private int ComputeDailyMoodScore()
        {
            int score = 0;
            if (this.Data.FedYesterday) score++;
            if (this.AbsoluteDay - this.Data.LastArgumentDay <= 2) score--;
            if (this.Data.LastFriendshipPoints >= 0 && this.GetSpousePoints() > this.Data.LastFriendshipPoints) score++;
            if (this.Data.BadDayToday && !this.Data.BadDayResolved) score--;
            if (this.TalkedToSpouseToday()) score++;
            if (this.GetSpousePoints() >= this.HeartsToPoints(13)) score++;
            return score;
        }
    }
}
