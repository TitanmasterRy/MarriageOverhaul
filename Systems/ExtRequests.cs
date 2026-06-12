using System.Linq;
using StardewValley;
using StardewValley.Quests;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F14: Spouse requests ──────────────────────────────────
        private const string RequestQuestId = "MO.SpouseRequest";

        /// <summary>Morning: show the active request as a real quest in the journal (rebuilt fresh each day).</summary>
        private void Requests_OnDayStarted(NPC spouse)
        {
            // Always clear any stale quest first; only the host-readable local player's log is touched.
            this.RemoveRequestQuest();

            if (!this.Config.EnableSpouseRequests)
                return;

            // Deliver a project reward that's now due (a few days after fulfilling a "build" request).
            this.Requests_DeliverPendingReward(spouse);

            if (!this.Config.EnableSpouseRequestQuest || !this.Data.RequestActive)
                return;

            var req = ExtendedContent.GetRequests(spouse.Name).FirstOrDefault(r => r.Id == this.Data.RequestId);
            if (req == null)
                return;

            var quest = new Quest
            {
                questTitle = string.Format(I18n.Get("quest.title", "{0}'s Request"), spouse.displayName),
                questDescription = (req.Note ?? "").Replace("^", " ").Trim(),
                currentObjective = this.BuildRequestObjective(spouse, req)
            };
            quest.id.Value = RequestQuestId;
            quest.questType.Value = Quest.type_itemDelivery;
            quest.canBeCancelled.Value = false;
            quest.moneyReward.Value = 0;
            // Only flag "new" on the morning the request first becomes active, not on every daily rebuild.
            quest.showNew.Value = this.AbsoluteDay == this.Data.RequestStartDay;

            Game1.player.questLog.Add(quest);
        }

        private void Requests_OnDayEnding(NPC spouse)
        {
            // Remove the live quest so a custom quest object is never written into the save.
            this.RemoveRequestQuest();

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
            this.QueueRepeatableMail("MO.Request");
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
                this.CompleteRequestQuest(); // "Quest Complete!" feedback before we clear it
                this.ChangeSpouseFriendship(100);
                this.ShowNarration(req.Thank);

                // Project-style requests: the spouse makes something and gives it back a few days later.
                if (!string.IsNullOrEmpty(req.RewardItem))
                {
                    this.Data.PendingRewardItem = req.RewardItem;
                    this.Data.PendingRewardQty = req.RewardQty > 0 ? req.RewardQty : 1;
                    this.Data.PendingRewardDay = this.AbsoluteDay + 3;
                    this.Data.PendingRewardLine = req.RewardLine ?? "";
                }

                this.ClearRequest();
            }
        }

        /// <summary>Hand over a project reward that's now due, with the spouse's "I made you this" line.</summary>
        private void Requests_DeliverPendingReward(NPC spouse)
        {
            if (string.IsNullOrEmpty(this.Data.PendingRewardItem) || this.AbsoluteDay < this.Data.PendingRewardDay)
                return;

            Item gift = this.CreateItem(this.Data.PendingRewardItem, this.Data.PendingRewardQty > 0 ? this.Data.PendingRewardQty : 1);
            if (gift != null)
                this.GiveItemToPlayerOrFridge(gift);
            if (!string.IsNullOrWhiteSpace(this.Data.PendingRewardLine))
                this.ShowSpouseSpeech(spouse, this.Data.PendingRewardLine, "love");

            this.Data.PendingRewardItem = "";
            this.Data.PendingRewardQty = 0;
            this.Data.PendingRewardDay = -1000;
            this.Data.PendingRewardLine = "";
        }

        /// <summary>Build the quest objective line from what fulfills the request (item / category / attention).</summary>
        private string BuildRequestObjective(NPC spouse, SpouseRequest req)
        {
            string who = spouse.displayName;
            if (req.Items != null && req.Items.Length > 0)
            {
                string item = this.CreateItem(req.Items[0])?.DisplayName ?? req.Items[0];
                return string.Format(I18n.Get("quest.objective.item", "Take {0} to {1}."), item, who);
            }
            if (req.Categories != null && req.Categories.Length > 0)
                return string.Format(I18n.Get("quest.objective.category", "Bring {0} {1}."), who, CategoryPhrase(req.Categories[0]));
            return string.Format(I18n.Get("quest.objective.time", "Spend some quality time with {0}."), who);
        }

        private static string CategoryPhrase(int category)
        {
            switch (category)
            {
                case -7:   return I18n.Get("quest.category.cooking", "a home-cooked dish");
                case -2:   return I18n.Get("quest.category.gem", "a gemstone");
                case -80:  return I18n.Get("quest.category.flower", "a flower");
                case -81:  return I18n.Get("quest.category.forage", "a foraged item");
                case -102: return I18n.Get("quest.category.book", "a book");
                default:   return I18n.Get("quest.category.other", "something they'd like");
            }
        }

        private void RemoveRequestQuest()
        {
            var log = Game1.player?.questLog;
            if (log == null)
                return;
            for (int i = log.Count - 1; i >= 0; i--)
            {
                if (log[i] != null && log[i].id.Value == RequestQuestId)
                    log.RemoveAt(i);
            }
        }

        private void CompleteRequestQuest()
        {
            var log = Game1.player?.questLog;
            if (log == null)
                return;
            foreach (Quest q in log)
            {
                if (q != null && q.id.Value == RequestQuestId)
                {
                    try { q.questComplete(); } catch { }
                    break;
                }
            }
        }

        private void ClearRequest()
        {
            this.RemoveRequestQuest();
            this.Data.RequestActive = false;
            this.Data.RequestId = "";
            this.Data.RequestStartDay = -1000;
            this.Data.PendingRequestLetterText = "";
        }
    }
}
