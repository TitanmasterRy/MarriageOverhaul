using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F2 + F17: Spouse daily chores (quality scales with happiness) ──
        private static readonly string[] ChoreKeys = { "water", "collect", "cook", "gold", "forage" };

        private void Chores_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableSpouseChores)
                return;
            if (this.Data.LastChoreDay == this.AbsoluteDay)
                return;
            if (this.Data.SpouseSickToday) // a sick spouse doesn't do chores
                return;

            double hearts = this.GetSpousePoints() / 250.0;
            if (hearts < 10)
                return;

            // Chance scales from low (10 hearts) to ChoreChanceAtMaxHearts (14 hearts).
            double factor = 0.2 + 0.8 * Math.Min(1.0, Math.Max(0.0, (hearts - 10) / 4.0));
            double chance = this.Config.ChoreChanceAtMaxHearts * factor;

            // F17: low rolling mood drops the frequency further.
            double mood = this.Config.EnableProductivityScaling ? this.AvgMoodScore() : 0;
            if (mood <= -1) chance *= 0.5;

            if (this.Rng.NextDouble() >= chance)
                return;

            // F17: quality scales with happiness.
            bool highQuality = !this.Config.EnableProductivityScaling
                ? hearts >= 13
                : (hearts >= 12 && mood >= 1);

            this.Data.LastChoreDay = this.AbsoluteDay;

            // Only offer chores that make sense: the "collect" chore (clearing the coop/barn) requires
            // owning a coop/barn with animals, so its dialogue can't fire when you have none.
            var candidates = new List<string>(ChoreKeys);
            if (!this.HasFarmAnimals())
                candidates.Remove("collect");
            string key = candidates[this.Rng.Next(candidates.Count)];

            // A chore may return an override line (e.g. the forage jackpot reaction); otherwise use the normal chore line.
            string overrideLine = this.PerformChore(key, highQuality);
            if (overrideLine != null)
                this.PushDialogue(spouse, overrideLine, "neutral");
            else
                this.PushDialogue(spouse, ExtendedContent.ChoreLine(key, highQuality, this.Rng), "happy");
        }

        /// <summary>Apply a chore's effect. Returns a dialogue line to use in place of the normal chore line, or null.</summary>
        private string PerformChore(string key, bool high)
        {
            try
            {
                switch (key)
                {
                    case "water": this.Chore_Water(high ? 8 : 3); break;
                    case "collect": this.Chore_Collect(high ? 6 : 2); break;
                    case "cook": this.Chore_Cook(high); break;
                    case "gold": Game1.player.Money += high ? 250 : 50; break;
                    case "forage": return this.Chore_Forage(high);
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Chore '{key}' effect failed (line still shows): {ex.Message}", LogLevel.Trace);
            }
            return null;
        }

        private void Chore_Water(int max)
        {
            var farm = Game1.getFarm();
            if (farm == null) return;
            int n = 0;
            foreach (var pair in farm.terrainFeatures.Pairs)
            {
                if (pair.Value is HoeDirt hd && hd.crop != null && hd.state.Value == 0)
                {
                    hd.state.Value = 1;
                    if (++n >= max) break;
                }
            }
        }

        /// <summary>Whether the player owns a coop/barn that actually has animals in it (checked at day-start, before they're let out).</summary>
        private bool HasFarmAnimals()
        {
            try
            {
                var farm = Game1.getFarm();
                if (farm == null) return false;
                foreach (var b in farm.buildings)
                {
                    if (b.GetIndoors() is AnimalHouse ah)
                    {
                        foreach (var _ in ah.animals.Values)
                            return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void Chore_Collect(int max)
        {
            var farm = Game1.getFarm();
            if (farm == null) return;
            int n = 0;
            foreach (var b in farm.buildings)
            {
                if (b.GetIndoors() is AnimalHouse ah)
                {
                    foreach (var animal in ah.animals.Values)
                    {
                        string id = animal.currentProduce.Value;
                        if (!string.IsNullOrEmpty(id) && id != "-1")
                        {
                            Item item = ItemRegistry.Create("(O)" + id, 1);
                            if (item != null)
                            {
                                farm.getShippingBin(Game1.player).Add(item);
                                animal.currentProduce.Value = null;
                                if (++n >= max) return;
                            }
                        }
                    }
                }
            }
        }

        private void Chore_Cook(bool high)
        {
            Item meal = this.CreateItem(high ? "Complete Breakfast" : "Fried Egg");
            if (meal is SObject obj)
            {
                if (high) obj.Quality = 2;
                this.PutInFridge(obj);
            }
        }

        // ── F3: Evolving gift preferences ─────────────────────────
        private void Preferences_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableEvolvingPreferences || this.Data.PreferencesEvolved)
                return;
            if (this.DaysMarried >= DaysPerYear)
                this.Data.PreferencesEvolved = true;
        }

        /// <summary>
        /// After a year of marriage, returns an improved gift taste for items tied to shared history,
        /// or -1 for no change. Called from the gift-taste Harmony postfix.
        /// </summary>
        public int Preferences_EvolvedTaste(string spouseName, Item item)
        {
            if (!this.Config.EnableEvolvingPreferences || !this.Data.PreferencesEvolved || item == null)
                return -1;
            if (spouseName == null || spouseName != this.SpouseName)
                return -1;

            int love = NPC.gift_taste_love;
            int like = NPC.gift_taste_like;
            int cat = item.Category;
            string n = item.Name;
            bool raining = this.IsRainingNow();

            switch (spouseName)
            {
                case "Penny":     if (cat == -102 || cat == -7) return love; break;
                case "Shane":     if (n == "Pepper Poppers") return love; if (cat == -7) return like; break;
                case "Abigail":   if (n == "Amethyst" || cat == -7) return love; break;
                case "Haley":     if (n == "Sunflower") return love; if (!raining) return like; break;
                case "Emily":     if (n == "Cloth" || cat == -2 || cat == -12) return love; break;
                case "Leah":      if (n == "Salad" || cat == -81) return love; break;
                case "Maru":      if (n == "Refined Quartz") return love; if (cat == -2) return like; break;
                case "Alex":      if (cat == -7) return love; break;
                case "Elliott":   if (n == "Pomegranate" || n == "Duck Feather") return love; if (cat == -102) return like; break;
                case "Harvey":    if (n == "Coffee") return love; if (cat == -7) return like; break;
                case "Sam":       if (n == "Pizza" || n == "Maple Bar") return love; if (cat == -8 || cat == -26) return like; break;
                case "Sebastian": if (n == "Frozen Tear") return love; if (raining) return like; break;
                default:          if (cat == -7 || cat == -81) return like; break; // modded spouses
            }
            return -1;
        }
    }
}
