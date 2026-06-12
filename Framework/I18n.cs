using StardewModdingAPI;

namespace MarriageOverhaul
{
    /// <summary>
    /// Translation gateway. English text always lives in the content classes as the guaranteed
    /// fallback; this looks up a per-line translation key and returns the localized text when a
    /// translation exists, otherwise the English. Keys + English are also dumped to i18n/default.json
    /// (see DefaultTranslations) so translators have a complete file to work from.
    /// </summary>
    public static class I18n
    {
        private static ITranslationHelper _translations;

        public static void Init(ITranslationHelper translations) => _translations = translations;

        /// <summary>Localized text for <paramref name="key"/>, or <paramref name="fallback"/> (English) if untranslated.</summary>
        public static string Get(string key, string fallback)
        {
            if (_translations == null || string.IsNullOrEmpty(key))
                return fallback;
            return _translations.Get(key).Default(fallback).ToString();
        }
    }
}
