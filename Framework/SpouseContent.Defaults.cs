using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class SpouseContent
    {
        // ── Shared translation helpers (English in the dictionaries above is the fallback) ──

        /// <summary>Translate a per-spouse line that lives in a name-keyed dictionary, with a generic fallback.</summary>
        private static string LocDict(string category, string name, Dictionary<string, string> dict, string generic)
        {
            bool has = IsVanilla(name) && dict.ContainsKey(name);
            return I18n.Get($"{category}.{(has ? name : "generic")}", has ? dict[name] : generic);
        }

        private static void CollectDict(IDictionary<string, string> map, string category, Dictionary<string, string> dict, string generic)
        {
            foreach (var kv in dict)
                map[$"{category}.{kv.Key}"] = kv.Value;
            map[$"{category}.generic"] = generic;
        }

        // ── Default-translation collection (used to generate i18n/default.json) ──

        internal static void CollectDefaults(IDictionary<string, string> map)
        {
            // Arguments (structured, per scenario).
            foreach (var kv in Arguments)
                CollectArguments(map, $"argument.{kv.Key}", kv.Value);
            CollectArguments(map, "argument.generic", GenericArguments);

            // Simple per-spouse line dictionaries.
            CollectDict(map, "jealousy", Jealousy, GenericJealousy);
            CollectDict(map, "farewell", Farewell, GenericFarewell);
            CollectDict(map, "mood.happy", HappyMood, GenericHappy);
            CollectDict(map, "mood.grumpy", GrumpyMood, GenericGrumpy);

            // General everyday pool (index-keyed).
            for (int i = 0; i < GeneralPool.Count; i++)
                map[$"general.{i}"] = GeneralPool[i];

            // Feeding hints (switch-backed; collect each vanilla spouse + generic).
            foreach (string name in VanillaSpouses)
            {
                map[$"feeding.cooking.{name}"] = CookingEnglish(name);
                map[$"feeding.provide.{name}"] = ProvideEnglish(name);
                map[$"feeding.hungry.{name}"] = HungryEnglish(name);
            }
            map["feeding.cooking.generic"] = CookingEnglish("__generic__");
            map["feeding.provide.generic"] = ProvideEnglish("__generic__");
            map["feeding.hungry.generic"] = HungryEnglish("__generic__");

            // Anniversary (structured).
            foreach (var kv in Anniversaries)
                CollectAnniversary(map, $"anniversary.{kv.Key}", kv.Value);
            CollectAnniversary(map, "anniversary.generic", GenericAnniversary);

            // Makeup (structured; Category is an identifier, not translated).
            foreach (var kv in Makeup)
                CollectMakeup(map, $"makeup.{kv.Key}", kv.Value);
            CollectMakeup(map, "makeup.generic", GenericMakeup);

            // Morning greetings (defined in SpouseContent.Morning.cs).
            CollectMorningDefaults(map);
        }

        private static void CollectArguments(IDictionary<string, string> map, string prefix, List<ArgumentScenario> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ArgumentScenario s = list[i];
                map[$"{prefix}.{i}.intro"] = s.Intro;
                map[$"{prefix}.{i}.goodchoice"] = s.GoodChoice;
                map[$"{prefix}.{i}.goodreply"] = s.GoodReply;
                map[$"{prefix}.{i}.neutralchoice"] = s.NeutralChoice;
                map[$"{prefix}.{i}.neutralreply"] = s.NeutralReply;
                map[$"{prefix}.{i}.badchoice"] = s.BadChoice;
                map[$"{prefix}.{i}.badreply"] = s.BadReply;
            }
        }

        private static void CollectAnniversary(IDictionary<string, string> map, string prefix, AnniversaryInfo a)
        {
            map[$"{prefix}.reminder"] = a.Reminder;
            map[$"{prefix}.sweet"] = a.Sweet;
            map[$"{prefix}.disappointed"] = a.Disappointed;
        }

        private static void CollectMakeup(IDictionary<string, string> map, string prefix, MakeupInfo m)
        {
            map[$"{prefix}.hint"] = m.Hint;
            map[$"{prefix}.reconcile"] = m.Reconcile;
            map[$"{prefix}.resigned"] = m.Resigned;
        }
    }
}
