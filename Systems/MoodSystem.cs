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

            // Friendship tier sets the baseline tone; mood shifts it up/down for variance. One line, no stacking.
            if (this.Config.FriendshipTieredMorningDialogue)
            {
                this.PushTieredMorningGreeting(spouse, mood);
                return;
            }

            // Legacy behavior: a single mood-based greeting.
            switch (mood)
            {
                case "Happy":
                    this.PushDialogue(spouse, SpouseContent.GetHappyMood(spouse.Name), "happy");
                    break;
                case "Grumpy":
                    this.PushDialogue(spouse, SpouseContent.GetGrumpyMood(spouse.Name), "angry");
                    break;
                default: // Neutral — share an everyday line from the general pool.
                    this.PushDialogue(spouse, SpouseContent.GetRandomGeneralLine(Game1.random));
                    break;
            }
        }

        // Spouses who are cheered rather than dampened by rainy weather.
        private static bool SpouseLovesRain(string name)
            => name == "Sebastian" || name == "Abigail";

        private bool IsRainingNow()
        {
            try
            {
                return Game1.getFarm()?.IsRainingHere() ?? false;
            }
            catch
            {
                return false;
            }
        }

        private string ComputeMood()
        {
            // Sometimes a mood is just a mood — random, regardless of circumstances.
            if (Game1.random.NextDouble() < 0.2)
            {
                int r = Game1.random.Next(3);
                return r == 0 ? "Happy" : r == 1 ? "Grumpy" : "Neutral";
            }

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

            // Weather colors the mood — most are gloomier in the rain, a few are cheered by it.
            if (this.IsRainingNow())
                score += SpouseLovesRain(this.SpouseName) ? 1 : -1;

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
