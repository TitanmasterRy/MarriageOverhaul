using System.Collections.Generic;
using System.Linq;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F13: Honeymoon phase ──────────────────────────────────
        private void Honeymoon_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableHoneymoonPhase || !this.IsHoneymoon())
                return;
            this.PushDialogue(spouse, ExtendedContent.HoneymoonLine(this.Rng), "love");
        }

        // ── F9: Seasonal affection ────────────────────────────────
        private void Seasonal_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableSeasonalAffection)
                return;
            var sp = ExtendedContent.GetSeasonPref(spouse.Name);
            if (sp == null)
                return;

            if (sp.Favorite == this.CurrentSeasonIndex && this.Rng.NextDouble() < 0.4)
                this.PushDialogue(spouse, ExtendedContent.FavoriteSeasonLine(this.Rng), "happy");
            else if (sp.Least == this.CurrentSeasonIndex && this.Rng.NextDouble() < 0.4)
                this.PushDialogue(spouse, ExtendedContent.LeastSeasonLine(this.Rng), "sad");
        }

        private void Seasonal_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableSeasonalAffection)
                return;
            var sp = ExtendedContent.GetSeasonPref(spouse.Name);
            if (sp == null || sp.Least != this.CurrentSeasonIndex)
                return;

            bool interacted = this.TalkedToSpouseToday();
            try
            {
                if (Game1.player.friendshipData.TryGetValue(spouse.Name, out var f) && f.GiftsToday > 0)
                    interacted = true;
            }
            catch { }

            if (!interacted)
                this.ChangeSpouseFriendship(-3); // ~10% extra decay in the least-favorite season
        }

        // ── F12: Town gossip ──────────────────────────────────────
        private void Gossip_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableTownGossip)
                return;
            if (this.Data.LastGossipWeek == this.WeekIndex)
                return;
            if (this.Rng.NextDouble() >= 0.25) // at most once per week, and not always
                return;

            double avg = this.AvgFriendshipHearts();
            bool positive;
            if (avg > 12) positive = true;
            else if (avg < 8) positive = false;
            else return; // nothing to gossip about in the middle range

            this.Data.LastGossipWeek = this.WeekIndex;

            // Inject a flavor line into a random townsperson's dialogue (no friendship effect).
            var villagers = ExtendedContent.GossipVillagers.OrderBy(_ => this.Rng.Next()).ToList();
            foreach (string v in villagers)
            {
                if (v == spouse.Name) continue;
                NPC npc = Game1.getCharacterFromName(v);
                if (npc == null) continue;
                string line = ExtendedContent.GossipLine(v, positive, this.Rng);
                if (line != null)
                {
                    this.PushDialogue(npc, line);
                    return;
                }
            }
        }

        // ── F8: Romantic letters ──────────────────────────────────
        private void Letters_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableRomanticLetters)
                return;

            if (this.Data.NextLetterDay < 0)
            {
                this.Data.NextLetterDay = this.AbsoluteDay + 1 + this.Rng.Next(7, 15); // 7-14 days
                return;
            }
            if (this.AbsoluteDay + 1 < this.Data.NextLetterDay)
                return;

            var letters = LetterContent.GetLetters(spouse.Name);
            var unused = Enumerable.Range(0, letters.Count).Where(i => !this.Data.RecentLetters.Contains(i)).ToList();
            if (unused.Count == 0) { this.Data.RecentLetters.Clear(); unused = Enumerable.Range(0, letters.Count).ToList(); }
            int idx = unused[this.Rng.Next(unused.Count)];

            this.Data.RecentLetters.Add(idx);
            while (this.Data.RecentLetters.Count > 3) this.Data.RecentLetters.RemoveAt(0);

            this.Data.PendingLoveLetterText = letters[idx];
            this.Helper.GameContent.InvalidateCache("Data/mail");
            this.QueueRepeatableMail("MO.LoveLetter");

            this.Data.NextLetterDay = this.AbsoluteDay + 1 + this.Rng.Next(7, 15);
        }

        // ── F15: Shared dreams (morning journal entry) ────────────
        private void Dreams_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableSharedDreams)
                return;
            if (this.AbsoluteDay - this.Data.LastDreamDay < 5)
                return;
            if (this.Rng.NextDouble() >= 0.35) // roughly every 5-7 days
                return;

            var dreams = DreamContent.GetDreams(spouse.Name);
            var avail = Enumerable.Range(0, dreams.Count).Where(i => !this.Data.RecentDreams.Contains(i)).ToList();
            if (avail.Count == 0) { this.Data.RecentDreams.Clear(); avail = Enumerable.Range(0, dreams.Count).ToList(); }
            int idx = avail[this.Rng.Next(avail.Count)];

            this.Data.LastDreamDay = this.AbsoluteDay;
            this.Data.RecentDreams.Add(idx);
            while (this.Data.RecentDreams.Count > 3) this.Data.RecentDreams.RemoveAt(0);

            this.ShowNarration($"~ From {spouse.displayName}'s journal ~^^\"{dreams[idx]}\"");
        }
    }
}
