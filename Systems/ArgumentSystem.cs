using System.Collections.Generic;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        private const int ArgumentCooldownDays = 5;

        /// <summary>Evening trigger: if the relationship is strained, start an argument event.</summary>
        private void Argument_OnTimeChanged(NPC spouse, int time)
        {
            if (!this.Config.EnableArguments || this.argumentTriggeredToday)
                return;

            // Evening only, and only when the spouse is home with the player and no menu is open.
            if (time < 1800 || time > 2200)
                return;
            if (Game1.activeClickableMenu != null || Game1.eventUp)
                return;
            if (!this.SpouseIsHomeWithPlayer(spouse))
                return;

            int threshold = this.HeartsToPoints(this.Config.ArgumentThresholdHearts);
            if (this.GetSpousePoints() >= threshold)
                return;

            if (this.AbsoluteDay - this.Data.LastArgumentDay < ArgumentCooldownDays)
                return;

            this.argumentTriggeredToday = true;
            this.Data.LastArgumentDay = this.AbsoluteDay;
            this.MarkArgumentThisWeek();
            this.StartArgument(spouse);
        }

        private void MarkArgumentThisWeek()
        {
            this.Data.ArgumentThisWeek = true;
            this.Data.ArgumentWeek = this.WeekIndex;
        }

        public bool ArgumentHappenedThisWeek()
        {
            return this.Data.ArgumentThisWeek && this.Data.ArgumentWeek == this.WeekIndex;
        }

        private void StartArgument(NPC spouse)
        {
            string name = spouse.Name;
            List<ArgumentScenario> scenarios = SpouseContent.GetArguments(name);
            if (scenarios == null || scenarios.Count == 0)
                return;

            // Pick a scenario that isn't the same one used last time for this spouse.
            int last = this.Data.LastArgumentTopic.TryGetValue(name, out int v) ? v : -1;
            int index = Game1.random.Next(scenarios.Count);
            if (scenarios.Count > 1 && index == last)
                index = (index + 1) % scenarios.Count;
            this.Data.LastArgumentTopic[name] = index;

            ArgumentScenario scenario = scenarios[index];

            var responses = new Response[]
            {
                new Response("MO_good", scenario.GoodChoice),
                new Response("MO_neutral", scenario.NeutralChoice),
                new Response("MO_bad", scenario.BadChoice)
            };

            GameLocation location = Game1.currentLocation;
            location.createQuestionDialogue(
                scenario.Intro,
                responses,
                (Farmer who, string answer) => this.ResolveArgument(spouse, scenario, answer));
        }

        private void ResolveArgument(NPC spouse, ArgumentScenario scenario, string answer)
        {
            switch (answer)
            {
                case "MO_good":
                    this.ChangeSpouseFriendship(50);
                    this.ShowNarration(scenario.GoodReply);
                    break;

                case "MO_bad":
                    this.ChangeSpouseFriendship(-80);
                    this.ShowNarration(scenario.BadReply);
                    // A bad argument puts the spouse into a makeup-gift state.
                    if (this.Config.EnableMakeupGifts)
                        this.Makeup_Begin(spouse);
                    this.Data.Mood = "Grumpy";
                    break;

                default:
                    this.ShowNarration(scenario.NeutralReply);
                    break;
            }
        }
    }
}
