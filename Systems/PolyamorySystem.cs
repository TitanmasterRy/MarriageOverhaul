using System.Collections.Generic;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>
        /// Compatibility for multi-spouse mods (e.g. Polyamory Sweet Love, Free Love). The rest of the mod
        /// operates on the single primary spouse (<see cref="GetSpouse"/>); when this is enabled, every other
        /// spouse also receives the overhaul's morning greeting so they don't feel left out. The heavier,
        /// stateful systems (feeding, anniversary, divorce, jealousy, makeup, requests, etc.) intentionally
        /// remain primary-spouse only, since the mod tracks one relationship's state.
        /// </summary>
        private void Poly_GreetOtherSpouses(NPC primary)
        {
            if (!this.Config.PolyamoryCompat)
                return;

            foreach (NPC spouse in this.GetAllSpouses())
            {
                if (spouse == null || (primary != null && spouse.Name == primary.Name))
                    continue;

                if (this.Config.FriendshipTieredMorningDialogue)
                {
                    SpouseContent.MorningTier tier = this.FriendshipMorningTierFor(spouse);
                    string line = SpouseContent.GetMorningLine(spouse.Name, tier, Game1.random, null);
                    this.PushDialogue(spouse, line, MorningTierEmotion(tier));
                }
                else
                {
                    this.PushDialogue(spouse, SpouseContent.GetRandomGeneralLine(spouse.Name, Game1.random));
                }
            }
        }

        /// <summary>Every NPC the player is currently married to (mod-agnostic — reads vanilla friendship data,
        /// so it works with any multi-spouse mod). Roommates are excluded unless the roommate option is on.</summary>
        public List<NPC> GetAllSpouses()
        {
            var list = new List<NPC>();
            try
            {
                Farmer player = Game1.player;
                if (player == null)
                    return list;

                foreach (var pair in player.friendshipData.Pairs)
                {
                    Friendship f = pair.Value;
                    if (f == null || !f.IsMarried())
                        continue;
                    if (f.IsRoommate() && !this.Config.EnableForRoommate)
                        continue;

                    NPC npc = Game1.getCharacterFromName(pair.Key);
                    if (npc != null)
                        list.Add(npc);
                }
            }
            catch { /* be defensive: never let spouse enumeration break the morning */ }
            return list;
        }

        /// <summary>The friendship-driven morning tier for a specific spouse (the standard path uses the primary spouse).</summary>
        private SpouseContent.MorningTier FriendshipMorningTierFor(NPC spouse)
        {
            int hearts;
            try { hearts = Game1.player.getFriendshipHeartLevelForNPC(spouse.Name); }
            catch { hearts = 0; }

            if (hearts < this.Config.MorningVeryLowHeartsMax)
                return SpouseContent.MorningTier.VeryLow;
            if (hearts < this.Config.MorningHighHeartsMin)
                return SpouseContent.MorningTier.Low;
            if (hearts < this.Config.MorningVeryHighHeartsMin)
                return SpouseContent.MorningTier.High;
            return SpouseContent.MorningTier.VeryHigh;
        }
    }
}
