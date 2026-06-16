using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>
        /// Called from the gift Harmony patch when the player gives a gift to a non-spouse NPC.
        /// Rolls for whether the spouse will notice (consequence applied next morning).
        /// </summary>
        public void Jealousy_OnGiftToOther(NPC recipient, SObject gift)
        {
            if (!this.Config.EnableJealousy)
                return;

            NPC spouse = this.GetSpouse();
            if (spouse == null || recipient == null)
                return;
            if (recipient.Name == spouse.Name)
                return;

            // Only once per day.
            if (this.Data.PendingJealousy || this.Data.LastJealousyDay == this.AbsoluteDay)
                return;

            if (!this.IsJealousyTarget(spouse, recipient))
                return;

            float chance = this.Config.JealousyChance;
            try
            {
                if (gift != null && recipient.getGiftTasteForThisItem(gift) == NPC.gift_taste_love)
                    chance = this.Config.JealousyChanceLoved;
            }
            catch { /* taste lookup failed; keep base chance */ }

            if (Game1.random.NextDouble() < chance)
            {
                this.Data.PendingJealousy = true;
                this.Data.LastJealousyDay = this.AbsoluteDay;
            }
        }

        /// <summary>Whether the recipient is someone the spouse would be jealous of. A registered custom NPC
        /// may declare an explicit rival list (only those count); otherwise the default same-gender rule applies.</summary>
        private bool IsJealousyTarget(NPC spouse, NPC recipient)
        {
            try
            {
                // Custom NPCs may name specific rivals — when they do, jealousy is limited to that list.
                if (CustomNpcRegistry.TryGetRivals(spouse.Name, out var rivals))
                {
                    foreach (string r in rivals)
                        if (string.Equals(r, recipient.Name, System.StringComparison.OrdinalIgnoreCase))
                            return true;
                    return false;
                }

                // Default behavior: jealous of NPCs of the same gender as the spouse
                // (i.e. the same "type" the spouse represents as a romantic interest).
                return recipient.Gender == spouse.Gender;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>Morning: apply the queued jealousy penalty and dialogue.</summary>
        private void Jealousy_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableJealousy || !this.Data.PendingJealousy)
                return;

            this.Data.PendingJealousy = false;
            this.ChangeSpouseFriendship(-15);
            this.PushDialogue(spouse, SpouseContent.GetJealousy(spouse.Name, Game1.random), "angry");
            this.forceGrumpyToday = true;
        }
    }
}
