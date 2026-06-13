using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        private static readonly string[] MilestoneSkillNames = { "Farming", "Fishing", "Foraging", "Mining", "Combat" };
        private static readonly int[] MilestoneLevels = { 5, 10 };

        /// <summary>Get the player's current level for a tracked skill, by name.</summary>
        private int GetSkillLevel(string skillName)
        {
            Farmer player = Game1.player;
            switch (skillName)
            {
                case "Farming": return player.FarmingLevel;
                case "Fishing": return player.FishingLevel;
                case "Foraging": return player.ForagingLevel;
                case "Mining": return player.MiningLevel;
                case "Combat": return player.CombatLevel;
                default: return 0;
            }
        }

        // ── Skill milestone rewards ────────────────────────────────
        private void SkillMilestone_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableSkillMilestoneRewards)
                return;

            string dialogueSkill = null;
            int dialogueLevel = 0;
            int giftCount = 0;

            foreach (string skill in MilestoneSkillNames)
            {
                int current = this.GetSkillLevel(skill);
                // First time we see a skill, baseline it to the current level so milestones you reached
                // BEFORE the marriage (or before installing) don't all fire retroactively. Only crossings
                // that happen during the marriage count.
                int previous = this.Data.SkillLevelSnapshot.TryGetValue(skill, out int p) ? p : current;

                foreach (int milestone in MilestoneLevels)
                {
                    string key = skill + milestone;
                    if (current >= milestone && previous < milestone && !this.Data.SkillMilestonesFired.Contains(key))
                    {
                        this.Data.SkillMilestonesFired.Add(key);
                        giftCount++;

                        // If multiple milestones land on the same day, only the first gets a dialogue
                        // line (to avoid spamming the player with several scenes in one morning); every
                        // milestone still earns its gift.
                        if (dialogueSkill == null)
                        {
                            dialogueSkill = skill;
                            dialogueLevel = milestone;
                        }
                    }
                }

                this.Data.SkillLevelSnapshot[skill] = current;
            }

            if (dialogueSkill == null)
                return;

            for (int i = 0; i < giftCount; i++)
            {
                Item gift = this.CreateItem(this.Config.MilestoneGiftItemId, this.Config.MilestoneGiftQuantity);
                if (gift != null)
                    this.PutInFridge(gift);
            }

            string line = ExtendedContent.GetSkillMilestone(spouse.Name, dialogueSkill, dialogueLevel);
            this.ShowSpouseSpeech(spouse, line, "love");
        }
    }
}
