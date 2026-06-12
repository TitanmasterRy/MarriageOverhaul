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
            return map;
        }
    }
}
