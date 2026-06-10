using System;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>The player's current friendship hearts with the spouse (0-14 while married).</summary>
        public int GetSpouseHearts()
        {
            string name = this.SpouseName;
            if (string.IsNullOrEmpty(name))
                return 0;
            try { return Game1.player.getFriendshipHeartLevelForNPC(name); }
            catch { return this.GetSpousePoints() / PointsPerHeart; }
        }

        /// <summary>The baseline morning tone from current friendship hearts, using the configurable thresholds.</summary>
        private SpouseContent.MorningTier GetFriendshipMorningTier()
        {
            int hearts = this.GetSpouseHearts();
            if (hearts < this.Config.MorningVeryLowHeartsMax)
                return SpouseContent.MorningTier.VeryLow;
            if (hearts < this.Config.MorningHighHeartsMin)
                return SpouseContent.MorningTier.Low;
            if (hearts < this.Config.MorningVeryHighHeartsMin)
                return SpouseContent.MorningTier.High;
            return SpouseContent.MorningTier.VeryHigh;
        }

        /// <summary>
        /// Push the morning greeting. Friendship hearts set the baseline tier; the mood system shifts it
        /// one tier up (Happy) or down (Grumpy) for day-to-day variance, so the two never double-fire.
        /// The chosen line avoids repeating yesterday's.
        /// </summary>
        private void PushTieredMorningGreeting(NPC spouse, string mood)
        {
            SpouseContent.MorningTier tier = this.GetFriendshipMorningTier();

            if (mood == "Happy")
                tier = (SpouseContent.MorningTier)Math.Min((int)tier + 1, (int)SpouseContent.MorningTier.VeryHigh);
            else if (mood == "Grumpy")
                tier = (SpouseContent.MorningTier)Math.Max((int)tier - 1, (int)SpouseContent.MorningTier.VeryLow);

            string line = SpouseContent.GetMorningLine(spouse.Name, tier, Game1.random, this.Data.LastMorningLine);
            this.Data.LastMorningLine = line;
            this.PushDialogue(spouse, line, MorningTierEmotion(tier));
        }

        /// <summary>Portrait expression that matches the resulting morning tier.</summary>
        private static string MorningTierEmotion(SpouseContent.MorningTier tier)
        {
            switch (tier)
            {
                case SpouseContent.MorningTier.VeryLow: return "angry";   // cold / resentful
                case SpouseContent.MorningTier.High: return "happy";      // warm
                case SpouseContent.MorningTier.VeryHigh: return "love";   // affectionate
                default: return "neutral";                                // Low — flat
            }
        }
    }
}
