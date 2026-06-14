using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── F5: Inside jokes ──────────────────────────────────────
        /// <summary>Record a new shared moment (called after good arguments, makeups, milestones).</summary>
        public void InsideJokes_Record(NPC spouse)
        {
            if (!this.Config.EnableInsideJokes || spouse == null)
                return;
            var pool = ExtendedContent.GetJokes(spouse.Name);
            if (!this.Data.InsideJokes.TryGetValue(spouse.Name, out var list))
            {
                list = new List<int>();
                this.Data.InsideJokes[spouse.Name] = list;
            }
            var unused = Enumerable.Range(0, pool.Count).Where(i => !list.Contains(i)).ToList();
            if (unused.Count > 0)
                list.Add(unused[this.Rng.Next(unused.Count)]);
        }

        private void InsideJokes_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableInsideJokes)
                return;
            if (!this.Data.InsideJokes.TryGetValue(spouse.Name, out var list) || list.Count < 5)
                return;
            if (this.AbsoluteDay - this.Data.LastInsideJokeDay < 3)
                return;
            if (this.Rng.NextDouble() >= 0.4)
                return;

            this.Data.LastInsideJokeDay = this.AbsoluteDay;
            var pool = ExtendedContent.GetJokes(spouse.Name);
            int idx = list[this.Rng.Next(list.Count)];
            if (idx >= 0 && idx < pool.Count)
                this.PushDialogue(spouse, pool[idx]);
        }

        // ── F6: Achievement pride ─────────────────────────────────
        private void Achievements_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableAchievementDialogue || string.IsNullOrEmpty(this.Data.PendingAchievement))
                return;
            string label = this.Data.PendingAchievement;
            this.Data.PendingAchievement = "";
            this.PushDialogue(spouse, ExtendedContent.GetAchievementLine(spouse.Name, label), "happy");
        }

        private void Achievements_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableAchievementDialogue)
                return;

            foreach (var ach in this.GetAchievementChecks())
            {
                if (this.Data.AchievementsCelebrated.Contains(ach.Key))
                    continue;
                bool done = false;
                try { done = ach.Value(); } catch { }
                if (done)
                {
                    this.Data.AchievementsCelebrated.Add(ach.Key);
                    this.Data.PendingAchievement = AchievementLabels[ach.Key];
                }
            }
        }

        private static readonly Dictionary<string, string> AchievementLabels = new Dictionary<string, string>
        {
            ["mine120"]   = "reached the very bottom of the mines",
            ["skull100"]  = "conquered the depths of the Skull Cavern",
            ["cc"]        = "restored the whole Community Center",
            ["perfection"]= "achieved perfection in the valley",
            ["fish159"]   = "caught the legendary Crimsonfish",
            ["fish160"]   = "caught the legendary Angler",
            ["fish163"]   = "caught the legendary Legend",
            ["fish775"]   = "caught the legendary Glacierfish",
            ["fish682"]   = "caught the legendary Mutant Carp",
            ["masterFarming"]  = "mastered the art of farming",
            ["masterMining"]   = "mastered the art of mining",
            ["masterForaging"] = "mastered the art of foraging",
            ["masterFishing"]  = "mastered the art of fishing",
            ["masterCombat"]   = "mastered the art of combat"
        };

        private Dictionary<string, Func<bool>> GetAchievementChecks()
        {
            var p = Game1.player;
            return new Dictionary<string, Func<bool>>
            {
                ["mine120"]    = () => p.deepestMineLevel >= 120,
                ["skull100"]   = () => p.deepestMineLevel >= 220,
                ["cc"]         = () => Game1.MasterPlayer != null && Game1.MasterPlayer.mailReceived.Contains("ccIsComplete"),
                ["perfection"] = () => Utility.percentGameComplete() >= 1f,
                ["fish159"]    = () => p.fishCaught.ContainsKey("(O)159"),
                ["fish160"]    = () => p.fishCaught.ContainsKey("(O)160"),
                ["fish163"]    = () => p.fishCaught.ContainsKey("(O)163"),
                ["fish775"]    = () => p.fishCaught.ContainsKey("(O)775"),
                ["fish682"]    = () => p.fishCaught.ContainsKey("(O)682"),
                ["masterFarming"]  = () => p.farmingLevel.Value >= 10,
                ["masterMining"]   = () => p.miningLevel.Value >= 10,
                ["masterForaging"] = () => p.foragingLevel.Value >= 10,
                ["masterFishing"]  = () => p.fishingLevel.Value >= 10,
                ["masterCombat"]   = () => p.combatLevel.Value >= 10
            };
        }

        // ── F7: Children ──────────────────────────────────────────
        private void Children_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableChildrenInteractions || !this.HasChildren())
                return;

            string child = this.FirstChildName();
            double hearts = this.GetSpousePoints() / 250.0;

            // Mood-appropriate references to the children.
            if (hearts < 10 && this.Rng.NextDouble() < 0.5)
                this.PushDialogue(spouse, ExtendedContent.ChildConcernLine(child, this.Rng), "sad");
            else if (hearts >= 12 && this.Rng.NextDouble() < 0.4)
                this.PushDialogue(spouse, ExtendedContent.ChildWarmLine(child, this.Rng), "happy");

            // Weekly request to spend time with the kids (one per week).
            // The forced question dialogue fires at day-start, which can freeze other players in
            // multiplayer, so it's skipped when Multiplayer Compatibility Mode is on.
            if (this.Data.LastChildRequestWeek != this.WeekIndex
                && hearts >= 8
                && !this.Config.MultiplayerCompatibilityMode
                && Game1.activeClickableMenu == null && !Game1.eventUp
                && this.Rng.NextDouble() < 0.5)
            {
                this.Data.LastChildRequestWeek = this.WeekIndex;
                var responses = new Response[]
                {
                    new Response("MO_kid_yes", "Of course. I'd love to."),
                    new Response("MO_kid_no", "Not today, I'm afraid.")
                };
                Game1.currentLocation.createQuestionDialogue(
                    ExtendedContent.ChildAskLine(child, this.Rng),
                    responses,
                    (Farmer who, string answer) => this.ResolveChildRequest(spouse, answer),
                    spouse);
            }
        }

        private void ResolveChildRequest(NPC spouse, string answer)
        {
            if (answer == "MO_kid_yes")
            {
                this.ChangeSpouseFriendship(30);
                this.ShowSpouseSpeech(spouse, ExtendedContent.ChildEngageLine(this.Rng), "happy");
            }
            else
            {
                this.ChangeSpouseFriendship(-20);
                this.forceGrumpyToday = true;
            }
        }

        /// <summary>Extra argument heart-loss when children are in the house (called from the argument system).</summary>
        public void Children_ExtraArgumentPenalty(NPC spouse)
        {
            if (this.Config.EnableChildrenInteractions && this.HasChildren())
                this.ChangeSpouseFriendship(-16); // +20% on the -80 bad-argument loss
        }

        private void Children_OnDayEnding(NPC spouse) { /* resolution is immediate via the question */ }

        // ── F16: Visitor jealousy ─────────────────────────────────
        private static readonly string[] FarmVisitors = { "Robin", "Willy", "Clint", "Demetrius", "Marnie" };

        private void Visitor_OnTimeChanged(NPC spouse, int time)
        {
            if (!this.Config.EnableVisitorJealousy || this.Data.VisitorSeenToday)
                return;
            try
            {
                var farm = Game1.getFarm();
                if (farm == null) return;
                foreach (string v in FarmVisitors)
                {
                    if (v == spouse.Name) continue;
                    NPC npc = farm.getCharacterFromName(v);
                    if (npc != null)
                    {
                        this.Data.VisitorSeenToday = true;
                        if (this.Rng.NextDouble() < 0.25)
                        {
                            this.Data.PendingVisitorComment = true;
                            this.Data.VisitorName = npc.displayName;
                        }
                        return;
                    }
                }
            }
            catch { }
        }

        private void Visitor_OnDayStarted(NPC spouse)
        {
            if (this.Config.EnableVisitorJealousy && this.Data.PendingVisitorComment)
            {
                this.Data.PendingVisitorComment = false;
                this.PushDialogue(spouse, ExtendedContent.VisitorComment(spouse.Name, this.Data.VisitorName), "angry");
            }
            this.Data.VisitorSeenToday = false;
        }
    }
}
