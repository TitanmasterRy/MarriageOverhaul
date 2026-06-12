using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class ExtendedContent
    {
        /// <summary>Add every ExtendedContent key + English line to the i18n default map.</summary>
        internal static void CollectDefaults(IDictionary<string, string> map)
        {
            // Seasonal affection.
            for (int i = 0; i < FavoriteSeasonLines.Count; i++)
                map[$"season.favorite.{i}"] = FavoriteSeasonLines[i];
            for (int i = 0; i < LeastSeasonLines.Count; i++)
                map[$"season.least.{i}"] = LeastSeasonLines[i];

            // Honeymoon.
            for (int i = 0; i < HoneymoonLines.Count; i++)
                map[$"honeymoon.{i}"] = HoneymoonLines[i];

            // Chores.
            CollectChoreMap(map, "low", ChoreLinesLow);
            CollectChoreMap(map, "high", ChoreLinesHigh);
            map["chore.fallback"] = "I helped out around the farm a little this morning.";

            // Children.
            CollectList(map, "child.warm", ChildWarm);
            CollectList(map, "child.concern", ChildConcern);
            CollectList(map, "child.ask", ChildAsk);
            CollectList(map, "child.engage", ChildEngage);

            // Town gossip.
            foreach (var kv in GossipPositive)
                CollectList(map, $"gossip.positive.{kv.Key}", kv.Value);
            foreach (var kv in GossipConcerned)
                CollectList(map, $"gossip.concerned.{kv.Key}", kv.Value);

            // Visitor jealousy.
            foreach (var kv in VisitorComments)
                map[$"visitor.{kv.Key}"] = kv.Value;
            map["visitor.generic"] = GenericVisitor;

            // Milestones.
            foreach (var kv in Milestones)
                foreach (var yr in kv.Value)
                    map[$"milestone.{kv.Key}.{yr.Key}"] = yr.Value;
            foreach (var yr in GenericMilestone)
                map[$"milestone.generic.{yr.Key}"] = yr.Value;

            // Sickness.
            foreach (var kv in Sick)
                CollectSick(map, $"sick.{kv.Key}", kv.Value);
            CollectSick(map, "sick.generic", GenericSick);

            // Bad days.
            foreach (var kv in BadDays)
                CollectBadDay(map, $"badday.{kv.Key}", kv.Value);
            CollectBadDay(map, "badday.generic", GenericBadDay);

            // Achievement pride.
            foreach (var kv in Achievements)
                map[$"achievement.{kv.Key}"] = kv.Value;
            map["achievement.generic"] = GenericAchievement;

            // Player birthday (personalized breakfast scene).
            foreach (var kv in Birthdays)
                CollectBirthday(map, $"birthday.{kv.Key}", kv.Value);
            CollectBirthday(map, "birthday.generic", GenericBirthday);

            // Skill milestones.
            foreach (var kv in SkillMilestone5)
                map[$"skillmilestone.5.{kv.Key}"] = kv.Value;
            map["skillmilestone.5.generic"] = GenericSkillMilestone5;
            foreach (var kv in SkillMilestone10)
                map[$"skillmilestone.10.{kv.Key}"] = kv.Value;
            map["skillmilestone.10.generic"] = GenericSkillMilestone10;

            // Forage jackpot reactions (item lists are identifiers, not translated).
            foreach (var kv in ForageTables)
                map[$"forage.jackpot.{kv.Key}"] = kv.Value.JackpotReaction;
            map["forage.jackpot.generic"] = GenericForage.JackpotReaction;

            // Helpful birthday gift lines.
            foreach (var kv in HelpfulBirthdayLines)
                map[$"birthdaygift.{kv.Key}"] = kv.Value;
            map["birthdaygift.generic"] = GenericHelpfulBirthdayLine;

            // Spouse requests (note/thank, keyed by request Id).
            CollectRequestDefaults(map);

            // Inside jokes (defined in InsideJokeContent.cs).
            CollectJokeDefaults(map);
        }

        private static void CollectList(IDictionary<string, string> map, string prefix, IReadOnlyList<string> list)
        {
            for (int i = 0; i < list.Count; i++)
                map[$"{prefix}.{i}"] = list[i];
        }

        private static void CollectChoreMap(IDictionary<string, string> map, string quality, Dictionary<string, List<string>> dict)
        {
            foreach (var kv in dict)
                for (int i = 0; i < kv.Value.Count; i++)
                    map[$"chore.{quality}.{kv.Key}.{i}"] = kv.Value[i];
        }

        private static void CollectSick(IDictionary<string, string> map, string prefix, SickInfo s)
        {
            map[$"{prefix}.sick"] = s.Sick;
            map[$"{prefix}.grateful"] = s.Grateful;
            map[$"{prefix}.tired"] = s.Tired;
        }

        private static void CollectBadDay(IDictionary<string, string> map, string prefix, BadDayInfo b)
        {
            for (int i = 0; i < b.Openers.Count; i++)
                map[$"{prefix}.opener.{i}"] = b.Openers[i];
            map[$"{prefix}.recovered"] = b.Recovered;
            map[$"{prefix}.flat"] = b.Flat;
        }

        private static void CollectBirthday(IDictionary<string, string> map, string prefix, BirthdayInfo b)
        {
            map[$"{prefix}.line"] = b.Line;
            map[$"{prefix}.addon"] = b.PointedAddon;
        }
    }
}
