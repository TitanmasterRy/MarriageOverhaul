using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Objects;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── Shared time / marriage helpers ────────────────────────
        private const int DaysPerYear = 112;

        private Random Rng => Game1.random;

        public int DaysMarried => this.Data.WeddingAbsoluteDay < 0 ? 0 : this.AbsoluteDay - this.Data.WeddingAbsoluteDay;
        public int MarriedYears => this.DaysMarried / DaysPerYear;
        public int CurrentSeasonIndex => (int)Game1.season;

        public bool IsHoneymoon()
        {
            return this.Config.EnableHoneymoonPhase
                && this.Data.WeddingAbsoluteDay >= 0
                && this.DaysMarried >= 0
                && this.DaysMarried < this.Config.HoneymoonDuration;
        }

        // ── Items ─────────────────────────────────────────────────
        public Item CreateItem(string internalName, int qty = 1)
        {
            if (string.IsNullOrWhiteSpace(internalName))
                return null;
            try
            {
                // A fully-qualified id like "(O)FlashShifter...Butterfish" is created directly (lets content
                // packs reference modded items precisely); plain names go through fuzzy search.
                if (internalName.StartsWith("("))
                    return ItemRegistry.Create(internalName, qty, 0, true);
                return Utility.fuzzyItemSearch(internalName, qty);
            }
            catch { return null; }
        }

        /// <summary>Give an item directly to the player, falling back to the fridge if their inventory is full.</summary>
        public bool GiveItemToPlayerOrFridge(Item item)
        {
            if (item == null)
                return false;
            try
            {
                if (Game1.player.addItemToInventoryBool(item))
                    return true;
            }
            catch { }
            return this.PutInFridge(item);
        }

        /// <summary>Drop an item into any of the player's fridges (built-in, mini-fridges, cellar) if there's room.</summary>
        public bool PutInFridge(Item item)
        {
            if (item == null)
                return false;
            try
            {
                foreach (Chest fridge in this.GetSpouseFridgeCandidates())
                {
                    if (fridge == null)
                        continue;
                    item = fridge.addItem(item);
                    if (item == null)
                        return true;
                }
                return false;
            }
            catch { return false; }
        }

        // ── Children ──────────────────────────────────────────────
        public List<Child> GetChildren()
        {
            try { return Game1.player.getChildren() ?? new List<Child>(); }
            catch { return new List<Child>(); }
        }
        public bool HasChildren() => this.GetChildren().Count > 0;
        public string FirstChildName()
        {
            var kids = this.GetChildren();
            return kids.Count > 0 ? kids[0].displayName : "the little one";
        }

        // ── Interaction / friendship tracking ─────────────────────
        public bool TalkedToSpouseToday()
        {
            try
            {
                string name = this.SpouseName;
                if (name != null && Game1.player.friendshipData.TryGetValue(name, out var f))
                    return f.TalkedToToday;
            }
            catch { }
            return false;
        }

        private void RecordRollingFriendship()
        {
            this.Data.RollingFriendship.Add(this.GetSpousePoints());
            while (this.Data.RollingFriendship.Count > 7)
                this.Data.RollingFriendship.RemoveAt(0);
        }
        public double AvgFriendshipHearts()
        {
            var list = this.Data.RollingFriendship;
            if (list == null || list.Count == 0)
                return this.GetSpousePoints() / 250.0;
            return list.Average() / 250.0;
        }

        // ── F17: rolling mood score (drives chore quality) ────────
        private void RecordMoodScore(int score)
        {
            this.Data.RollingMoodScores.Add(score);
            while (this.Data.RollingMoodScores.Count > 7)
                this.Data.RollingMoodScores.RemoveAt(0);
        }
        public double AvgMoodScore()
        {
            var list = this.Data.RollingMoodScores;
            return (list == null || list.Count == 0) ? 0 : list.Average();
        }

        /// <summary>Combined gift friendship multiplier from the honeymoon phase and favorite season.</summary>
        public float GiftFriendshipMultiplier(NPC spouse)
        {
            float m = 1f;
            if (spouse == null)
                return m;
            if (this.IsHoneymoon())
                m *= 1.5f;
            if (this.Config.EnableSeasonalAffection)
            {
                var sp = ExtendedContent.GetSeasonPref(spouse.Name);
                if (sp != null && sp.Favorite == this.CurrentSeasonIndex)
                    m *= 1.15f;
            }
            return m;
        }
    }
}
