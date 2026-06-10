using System.Linq;
using StardewModdingAPI;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        private const int CheatCooldownDays = 14;

        /// <summary>
        /// End of day: if the relationship has been neglected below the threshold, the spouse may
        /// begin an affair with another single marriage candidate (revealed the next morning).
        /// </summary>
        private void Cheating_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableCheating || this.Data.PendingCheatReveal)
                return;

            int threshold = this.HeartsToPoints(this.Config.CheatingThresholdHearts);
            if (this.GetSpousePoints() >= threshold)
                return;

            if (this.AbsoluteDay - this.Data.LastCheatDay < CheatCooldownDays)
                return;

            if (Game1.random.NextDouble() >= this.Config.CheatingChance)
                return;

            NPC partner = this.PickAffairPartner(spouse.Name);
            if (partner == null)
                return;

            this.Data.LastCheatDay = this.AbsoluteDay;
            this.Data.PendingCheatReveal = true;
            this.Data.CheatPartner = partner.Name;
            this.Data.PendingCheatLetterText = this.BuildCheatingLetter(spouse, partner);

            this.Helper.GameContent.InvalidateCache("Data/mail");
            this.QueueRepeatableMail("MO.Cheating");
        }

        /// <summary>Morning: deliver the heartbreak — the affair is revealed, and the spouse leaves.</summary>
        private void Cheating_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableCheating || !this.Data.PendingCheatReveal)
                return;

            this.Data.PendingCheatReveal = false;
            string partnerDisplay = this.PartnerDisplayName(this.Data.CheatPartner);

            try
            {
                Game1.addHUDMessage(new HUDMessage($"You found out {spouse.displayName} has been seeing {partnerDisplay}...", 3));
            }
            catch { /* HUD is cosmetic */ }

            this.PushDialogue(spouse,
                $"...I can't keep pretending. {partnerDisplay} and I... it just happened. You were never here. I'm sorry you had to find out like this.",
                "sad");
            this.forceGrumpyToday = true;

            // The ultimate punishment: the relationship is over.
            this.ChangeSpouseFriendship(-this.GetSpousePoints());
            this.TriggerDivorce();
        }

        /// <summary>Pick a random single marriage candidate (not the spouse, not married to anyone).</summary>
        private NPC PickAffairPartner(string spouseName)
        {
            var farmers = Game1.getAllFarmers();
            var names = SpouseContent.VanillaSpouses
                .Where(c => c != spouseName)
                .OrderBy(_ => Game1.random.Next())
                .ToList();

            foreach (string name in names)
            {
                if (farmers.Any(f => f.spouse == name))
                    continue; // already someone's spouse
                NPC npc = Game1.getCharacterFromName(name);
                if (npc != null)
                    return npc;
            }
            return null;
        }

        private string PartnerDisplayName(string internalName)
        {
            try
            {
                return Game1.getCharacterFromName(internalName)?.displayName ?? internalName;
            }
            catch
            {
                return internalName;
            }
        }

        private string BuildCheatingLetter(NPC spouse, NPC partner)
        {
            return
                "I've started this letter a dozen times and torn up every one." +
                $"^^The truth is, somewhere along the way you stopped being here. Not just gone to the fields, but gone from us. And in all that emptiness, I grew close to {partner.displayName}. Closer than I should have." +
                "^^I'm not proud of it. But I can't keep lying, to you or to myself. I'm leaving." +
                $"^^- {spouse.displayName}";
        }
    }
}
