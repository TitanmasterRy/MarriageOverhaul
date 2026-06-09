using System.Collections.Generic;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>Generate the weekly "cooking / player provides" schedule if a new week has begun.</summary>
        private void Feeding_EnsureWeeklySchedule()
        {
            int week = this.WeekIndex;
            if (this.Data.FeedingScheduleWeek == week && this.Data.ProvideDays.Count == 7)
                return;

            // Deterministic per-save, per-week RNG so reloads stay consistent.
            var rng = Utility.CreateRandom(Game1.uniqueIDForThisGame, week * 13.0 + 7.0);

            // Aim for ~3 cooking days and ~4 provide days, with slight variance (3-5 provide days).
            int provideTarget = 3 + rng.Next(3); // 3, 4, or 5
            var days = new List<bool>(new bool[7]); // false = cooking day
            int assigned = 0;
            var order = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            // Shuffle the day order.
            for (int i = order.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (order[i], order[j]) = (order[j], order[i]);
            }
            foreach (int d in order)
            {
                if (assigned >= provideTarget)
                    break;
                days[d] = true; // player provides
                assigned++;
            }

            this.Data.ProvideDays = days;
            this.Data.FeedingScheduleWeek = week;
        }

        private bool IsProvideDayToday()
        {
            if (this.Data.ProvideDays == null || this.Data.ProvideDays.Count != 7)
                return false;
            return this.Data.ProvideDays[this.DayOfWeekIndex];
        }

        /// <summary>Morning: inject the cooking / provide hint, and grumpy line if the spouse went hungry.</summary>
        private void Feeding_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableFeeding)
                return;

            string name = spouse.Name;

            if (this.Data.WentHungry)
            {
                this.PushDialogue(spouse, SpouseContent.GetHungryLine(name));
                this.Data.WentHungry = false;
                // A hungry spouse is grumpy regardless of other factors.
                this.forceGrumpyToday = true;
            }
            else if (this.IsProvideDayToday())
            {
                this.PushDialogue(spouse, SpouseContent.GetProvideLine(name));
            }
            else
            {
                this.PushDialogue(spouse, SpouseContent.GetCookingLine(name));
            }
        }

        /// <summary>End of day: on provide days, check the fridge and reward / penalize accordingly.</summary>
        private void Feeding_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableFeeding)
            {
                this.Data.FedYesterday = true;
                return;
            }

            if (!this.IsProvideDayToday())
            {
                // Spouse cooked; they're fed.
                this.Data.FedYesterday = true;
                this.Data.WentHungry = false;
                return;
            }

            Chest fridge = this.GetSpouseFridge();
            Item food = this.FindEdibleItem(fridge);

            if (food != null)
            {
                this.ConsumeOne(fridge, food);
                this.ChangeSpouseFriendship(15);
                this.Data.FedYesterday = true;
                this.Data.WentHungry = false;
            }
            else
            {
                this.ChangeSpouseFriendship(-40);
                this.Data.FedYesterday = false;
                this.Data.WentHungry = true;
            }
        }

        private Chest GetSpouseFridge()
        {
            try
            {
                if (Utility.getHomeOfFarmer(Game1.player) is FarmHouse house)
                    return house.fridge.Value;
            }
            catch { }
            return null;
        }

        private Item FindEdibleItem(Chest fridge)
        {
            if (fridge?.Items == null)
                return null;
            foreach (Item item in fridge.Items)
            {
                if (item is SObject obj && !obj.bigCraftable.Value && obj.Edibility > 0)
                    return item;
            }
            return null;
        }

        private void ConsumeOne(Chest fridge, Item item)
        {
            if (fridge?.Items == null || item == null)
                return;
            if (item.Stack > 1)
                item.Stack--;
            else
                fridge.Items.Remove(item);
        }
    }
}
