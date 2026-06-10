using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        private const int MakeupWindowDays = 7;

        /// <summary>Unified dispatcher for any gift given to the spouse (called from the gift patch).</summary>
        public void Gift_OnGiftToSpouse(NPC spouse, SObject gift)
        {
            if (spouse == null)
                return;
            this.Anniversary_OnGiftToSpouse(spouse);
            this.Makeup_OnGiftToSpouse(spouse, gift);
            this.Extended_OnGiftToSpouse(spouse, gift);
        }

        /// <summary>Put the spouse into the "needs makeup gift" state (after a bad argument).</summary>
        private void Makeup_Begin(NPC spouse)
        {
            this.Data.MakeupNeeded = true;
            this.Data.MakeupCategory = SpouseContent.GetMakeup(spouse.Name).Category;
            this.Data.MakeupStartDay = this.AbsoluteDay;
        }

        public bool IsMakeupActive()
        {
            return this.Config.EnableMakeupGifts && this.Data.MakeupNeeded;
        }

        /// <summary>Morning: drop a category hint, or clear the state with a resigned line after the window.</summary>
        private void Makeup_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableMakeupGifts || !this.Data.MakeupNeeded)
                return;

            if (this.AbsoluteDay - this.Data.MakeupStartDay >= MakeupWindowDays)
            {
                this.ClearMakeup();
                this.PushDialogue(spouse, SpouseContent.GetMakeup(spouse.Name).Resigned, "sad");
                return;
            }

            this.PushDialogue(spouse, SpouseContent.GetMakeup(spouse.Name).Hint);
        }

        private void Makeup_OnGiftToSpouse(NPC spouse, SObject gift)
        {
            if (!this.Config.EnableMakeupGifts || !this.Data.MakeupNeeded || gift == null)
                return;

            if (this.GiftMatchesCategory(gift, this.Data.MakeupCategory))
            {
                this.ClearMakeup();
                this.ChangeSpouseFriendship(150);
                this.ShowNarration(SpouseContent.GetMakeup(spouse.Name).Reconcile);
                this.InsideJokes_Record(spouse);
            }
        }

        private void ClearMakeup()
        {
            this.Data.MakeupNeeded = false;
            this.Data.MakeupCategory = "";
            this.Data.MakeupStartDay = -1000;
        }

        // Stardew object category ids (kept as literals so the mapping can't break on a renamed field).
        private const int CatCooking = -7;
        private const int CatCrafting = -8;
        private const int CatArtisan = -26;
        private const int CatGreens = -81;   // foraged greens
        private const int CatFlowers = -80;
        private const int CatFruits = -79;
        private const int CatVegetable = -75;
        private const int CatGem = -2;
        private const int CatMinerals = -12;

        /// <summary>Map a gift item to a makeup category by its object category id.</summary>
        private bool GiftMatchesCategory(SObject gift, string category)
        {
            int cat = gift.Category;
            switch (category)
            {
                case "sweet":
                    // Cooked dishes (and other prepared foods).
                    return cat == CatCooking;

                case "nature":
                    // Forage, flowers, fruit, vegetables, gems and minerals.
                    return cat == CatGreens
                        || cat == CatFlowers
                        || cat == CatFruits
                        || cat == CatVegetable
                        || cat == CatGem
                        || cat == CatMinerals;

                case "homemade":
                    // Things the player makes: cooked dishes, artisan goods, crafted items.
                    return cat == CatCooking
                        || cat == CatArtisan
                        || cat == CatCrafting;

                default:
                    return false;
            }
        }
    }
}
