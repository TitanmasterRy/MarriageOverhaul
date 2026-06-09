using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>
        /// Morning: determine the spouse's mood from recent trends, feeding, arguments and a small
        /// random factor, then inject a mood-appropriate greeting. Neutral keeps the vanilla tone.
        /// </summary>
        private void Mood_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableMoodSystem)
                return;

            string mood = this.forceGrumpyToday ? "Grumpy" : this.ComputeMood();
            this.Data.Mood = mood;

            switch (mood)
            {
                case "Happy":
                    this.PushDialogue(spouse, SpouseContent.GetHappyMood(spouse.Name));
                    break;
                case "Grumpy":
                    this.PushDialogue(spouse, SpouseContent.GetGrumpyMood(spouse.Name));
                    break;
                // Neutral: no injection — leave the vanilla tone intact.
            }
        }

        private string ComputeMood()
        {
            int score = 0;

            // Friendship trend vs. yesterday.
            int current = this.GetSpousePoints();
            if (this.Data.LastFriendshipPoints >= 0)
            {
                if (current > this.Data.LastFriendshipPoints)
                    score++;
                else if (current < this.Data.LastFriendshipPoints)
                    score--;
            }

            // Whether they were fed yesterday.
            score += this.Data.FedYesterday ? 1 : -1;

            // A recent argument sours the mood.
            if (this.AbsoluteDay - this.Data.LastArgumentDay <= 3)
                score--;

            // Being in a makeup state weighs on them.
            if (this.Data.MakeupNeeded)
                score--;

            // Small random nudge.
            score += Game1.random.Next(-1, 2); // -1, 0, or 1

            if (score >= 2)
                return "Happy";
            if (score <= -2)
                return "Grumpy";
            return "Neutral";
        }
    }
}
