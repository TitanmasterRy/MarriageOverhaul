using System.Linq;
using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F14: Spouse requests ──────────────────────────────────
        private void Requests_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableSpouseRequests)
                return;

            // An active request ignored past its 3-day window costs friendship.
            if (this.Data.RequestActive)
            {
                if (this.AbsoluteDay - this.Data.RequestStartDay >= 3)
                {
                    this.ChangeSpouseFriendship(-40);
                    this.ClearRequest();
                }
                return; // only one active request at a time
            }

            // Otherwise, occasionally issue a new request note (every 10-14 days).
            if (this.AbsoluteDay - this.Data.LastRequestDay < 10)
                return;
            if (this.Rng.NextDouble() >= 0.34)
                return;

            var reqs = ExtendedContent.GetRequests(spouse.Name);
            if (reqs == null || reqs.Count == 0)
                return;

            int last = this.Data.LastRequestIndex.TryGetValue(spouse.Name, out int v) ? v : -1;
            int idx = this.Rng.Next(reqs.Count);
            if (reqs.Count > 1 && idx == last)
                idx = (idx + 1) % reqs.Count;
            this.Data.LastRequestIndex[spouse.Name] = idx;

            var req = reqs[idx];
            this.Data.RequestActive = true;
            this.Data.RequestId = req.Id;
            this.Data.RequestStartDay = this.AbsoluteDay + 1;
            this.Data.LastRequestDay = this.AbsoluteDay;
            this.Data.PendingRequestLetterText = req.Note;
            this.Helper.GameContent.InvalidateCache("Data/mail");
            Game1.addMailForTomorrow("MO.Request");
        }

        public void Requests_OnGiftToSpouse(NPC spouse, SObject gift)
        {
            if (!this.Config.EnableSpouseRequests || !this.Data.RequestActive || gift == null)
                return;

            var req = ExtendedContent.GetRequests(spouse.Name).FirstOrDefault(r => r.Id == this.Data.RequestId);
            if (req == null)
                return;

            bool match;
            if (req.Items == null && req.Categories == null)
                match = true; // an "attention" request — any gift fulfills it
            else
                match = (req.Items != null && req.Items.Contains(gift.Name))
                     || (req.Categories != null && req.Categories.Contains(gift.Category));

            if (match)
            {
                this.ChangeSpouseFriendship(100);
                this.ShowNarration(req.Thank);
                this.ClearRequest();
            }
        }

        private void ClearRequest()
        {
            this.Data.RequestActive = false;
            this.Data.RequestId = "";
            this.Data.RequestStartDay = -1000;
            this.Data.PendingRequestLetterText = "";
        }
    }
}
