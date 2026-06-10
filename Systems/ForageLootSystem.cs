using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── Spouse forage chore (loot-table version) ──────────────
        /// <summary>
        /// Run the forage chore. When the loot table is enabled the spouse brings back items rolled
        /// from their personal table (handed to the player, falling back to the fridge), with a rare
        /// chance of a Prismatic Shard. Returns the jackpot reaction line to show in place of the
        /// normal forage line, or null. When disabled, falls back to picking up forage on the farm.
        /// </summary>
        private string Chore_Forage(bool high)
        {
            if (!this.Config.EnableForageLoot)
            {
                this.Chore_ForageFromFarm(high ? 6 : 2);
                return null;
            }

            ExtendedContent.ForageTable table = ExtendedContent.GetForageTable(this.SpouseName);

            // Normal haul.
            int qty = Math.Max(1, this.Config.ForageHaulQuantity);
            for (int i = 0; i < qty; i++)
            {
                string itemName = this.RollForageTierItem(table, high);
                this.GiveItemToPlayerOrFridge(this.CreateItem(itemName));
            }

            // Jackpot: a single Prismatic Shard, rolled once per forage so the chance stays small.
            if (this.Rng.NextDouble() < this.Config.ForagePrismaticShardChance)
            {
                this.GiveItemToPlayerOrFridge(this.CreateItem("Prismatic Shard"));
                return table.JackpotReaction;
            }

            return null;
        }

        /// <summary>Pick one item from the table's rarity tiers using the configured weights.</summary>
        private string RollForageTierItem(ExtendedContent.ForageTable table, bool high)
        {
            double c = Math.Max(0, this.Config.ForageCommonWeight);
            double u = Math.Max(0, this.Config.ForageUncommonWeight);
            double r = Math.Max(0, this.Config.ForageRareWeight);

            // High-quality foraging skews toward the better tiers.
            if (high) { c *= 0.5; r *= 2.0; }

            double total = c + u + r;
            if (total <= 0) total = 1;

            double roll = this.Rng.NextDouble() * total;
            string[] tier = roll < c ? table.Common
                          : roll < c + u ? table.Uncommon
                          : table.Rare;

            if (tier == null || tier.Length == 0)
                tier = table.Common;

            return tier[this.Rng.Next(tier.Length)];
        }

        /// <summary>Original behavior: pick up forage objects already spawned on the farm (used when the loot table is disabled).</summary>
        private void Chore_ForageFromFarm(int max)
        {
            var farm = Game1.getFarm();
            if (farm == null) return;
            var picked = new List<Vector2>();
            foreach (var pair in farm.objects.Pairs)
            {
                if (pair.Value is SObject o && o.IsSpawnedObject)
                {
                    Item copy = o.getOne();
                    if (copy != null && this.PutInFridge(copy))
                        picked.Add(pair.Key);
                    if (picked.Count >= max) break;
                }
            }
            foreach (var tile in picked)
                farm.objects.Remove(tile);
        }
    }
}
