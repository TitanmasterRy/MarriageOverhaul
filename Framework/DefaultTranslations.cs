using System;
using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>
    /// Aggregates every translatable line (key → English) from the content classes. Used to generate
    /// i18n/default.json so translators have a complete, accurate source file. The English here is the
    /// same text the getters fall back to, so the file never drifts from the code.
    /// </summary>
    public static class DefaultTranslations
    {
        public static IDictionary<string, string> BuildAll()
        {
            var map = new SortedDictionary<string, string>(StringComparer.Ordinal);
            SpouseContent.CollectDefaults(map);
            ExtendedContent.CollectDefaults(map);
            LetterContent.CollectDefaults(map);
            DreamContent.CollectDefaults(map);

            // Spouse-request journal quest UI (kept in sync with ExtRequests.cs fallbacks).
            map["quest.title"] = "{0}'s Request";
            map["quest.objective.item"] = "Take {0} to {1}.";
            map["quest.objective.category"] = "Bring {0} {1}.";
            map["quest.objective.time"] = "Spend some quality time with {0}.";
            map["quest.category.cooking"] = "a home-cooked dish";
            map["quest.category.gem"] = "a gemstone";
            map["quest.category.flower"] = "a flower";
            map["quest.category.forage"] = "a foraged item";
            map["quest.category.book"] = "a book";
            map["quest.category.other"] = "something they'd like";

            return map;
        }
    }
}
