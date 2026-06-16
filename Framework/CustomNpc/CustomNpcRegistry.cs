using System;
using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>
    /// In-memory registry of custom-NPC content, keyed by NPC internal name. This is the new middle
    /// tier in the content-resolution chain:
    ///   vanilla built-in content → registered custom content → generic fallback.
    /// The content getters in <see cref="SpouseContent"/> / <see cref="ExtendedContent"/> consult this
    /// registry between their vanilla and generic branches, so vanilla and generic-fallback behavior is
    /// unchanged whenever a name isn't registered (or the framework is disabled).
    /// All <c>TryGet*</c> methods return text verbatim (no i18n) — pack authors own their translations.
    /// </summary>
    public static class CustomNpcRegistry
    {
        // NPC internal names are matched case-insensitively to be forgiving of pack typos.
        private static readonly Dictionary<string, CustomNpcContent> Registry =
            new Dictionary<string, CustomNpcContent>(StringComparer.OrdinalIgnoreCase);

        private static ModConfig Cfg => ModEntry.Instance?.Config;

        /// <summary>Master toggle (config), read live so GMCM changes apply without a restart.</summary>
        public static bool Enabled => Cfg?.EnableCustomNpcFramework ?? false;

        /// <summary>Whether packs are allowed to override built-in vanilla-spouse content (config, default false).</summary>
        public static bool AllowVanillaOverride => Cfg?.AllowVanillaOverride ?? false;

        /// <summary>How many NPCs are currently registered.</summary>
        public static int Count => Registry.Count;

        /// <summary>The internal names of every registered NPC.</summary>
        public static IEnumerable<string> Names => Registry.Keys;

        /// <summary>Register (or replace) content for an NPC. Validation is the caller's responsibility.</summary>
        public static void Register(string name, CustomNpcContent content)
        {
            if (string.IsNullOrWhiteSpace(name) || content == null)
                return;
            Registry[name] = content;
        }

        public static bool IsRegistered(string name)
            => !string.IsNullOrEmpty(name) && Registry.ContainsKey(name);

        /// <summary>Remove every registration (used before a reload).</summary>
        public static void Clear() => Registry.Clear();

        // ── Resolution gate ───────────────────────────────────────────

        /// <summary>
        /// Resolve the content for a name+system, honoring the master toggle and the vanilla-override rule.
        /// Returns false (→ caller keeps its existing vanilla/generic path) unless a registered NPC has a
        /// usable, allow-listed section for the requested system.
        /// </summary>
        private static bool TryResolve(string name, string system, out CustomNpcContent content)
        {
            content = null;
            if (!Enabled || string.IsNullOrEmpty(name))
                return false;
            // Never override built-in vanilla content unless explicitly allowed.
            if (SpouseContent.IsVanilla(name) && !AllowVanillaOverride)
                return false;
            if (!Registry.TryGetValue(name, out var c) || c == null)
                return false;
            if (!c.SystemEnabled(system))
                return false;
            content = c;
            return true;
        }

        // ── Per-system accessors (used by the content getters) ─────────

        public static bool TryGetMorning(string name, SpouseContent.MorningTier tier, out List<string> pool)
        {
            pool = null;
            if (!TryResolve(name, "Morning", out var c) || c.Morning == null)
                return false;
            switch (tier)
            {
                case SpouseContent.MorningTier.VeryLow: pool = c.Morning.VeryLow; break;
                case SpouseContent.MorningTier.Low: pool = c.Morning.Low; break;
                case SpouseContent.MorningTier.High: pool = c.Morning.High; break;
                case SpouseContent.MorningTier.VeryHigh: pool = c.Morning.VeryHigh; break;
            }
            return NonEmpty(pool);
        }

        /// <summary>kind = "happy" | "neutral" | "grumpy".</summary>
        public static bool TryGetMoodPool(string name, string kind, out List<string> pool)
        {
            pool = null;
            if (!TryResolve(name, "Mood", out var c) || c.Mood == null)
                return false;
            switch ((kind ?? "").ToLowerInvariant())
            {
                case "happy": pool = c.Mood.Happy; break;
                case "neutral": pool = c.Mood.Neutral; break;
                case "grumpy": pool = c.Mood.Grumpy; break;
            }
            return NonEmpty(pool);
        }

        public static bool TryGetArguments(string name, out List<ArgumentScenario> result)
        {
            result = null;
            if (!TryResolve(name, "Arguments", out var c) || c.Arguments == null)
                return false;

            var outList = new List<ArgumentScenario>();
            foreach (CustomArgument a in c.Arguments)
            {
                if (a == null || !a.IsComplete())
                    continue;
                outList.Add(new ArgumentScenario
                {
                    Intro = a.Intro,
                    GoodChoice = a.GoodChoice,
                    GoodReply = a.GoodReply,
                    NeutralChoice = a.NeutralChoice,
                    NeutralReply = a.NeutralReply,
                    BadChoice = a.BadChoice,
                    BadReply = a.BadReply
                });
            }
            if (outList.Count == 0)
                return false;
            result = outList;
            return true;
        }

        public static bool TryGetAnniversary(string name, out AnniversaryInfo result)
        {
            result = null;
            if (!TryResolve(name, "Anniversary", out var c) || c.Anniversary == null)
                return false;
            CustomAnniversary a = c.Anniversary;
            if (string.IsNullOrWhiteSpace(a.Reminder) && string.IsNullOrWhiteSpace(a.Sweet) && string.IsNullOrWhiteSpace(a.Disappointed))
                return false;
            result = new AnniversaryInfo { Reminder = a.Reminder, Sweet = a.Sweet, Disappointed = a.Disappointed };
            return true;
        }

        public static bool TryGetJealousy(string name, out List<string> lines)
        {
            lines = null;
            if (!TryResolve(name, "Jealousy", out var c) || c.Jealousy == null)
                return false;
            lines = c.Jealousy.Lines;
            return NonEmpty(lines);
        }

        public static bool TryGetMakeup(string name, out MakeupInfo result)
        {
            result = null;
            if (!TryResolve(name, "Makeup", out var c) || c.Makeup == null)
                return false;
            CustomMakeup m = c.Makeup;
            string cat = NormalizeCategory(m.Category);
            if (cat == null)
                return false;
            result = new MakeupInfo { Category = cat, Hint = m.Hint, Reconcile = m.Reconcile, Resigned = m.Resigned };
            return true;
        }

        /// <summary>Explicit rival NPC list for jealousy targeting (behavior block — not gated by the allow-list).</summary>
        public static bool TryGetRivals(string name, out List<string> rivals)
        {
            rivals = null;
            if (!Enabled || string.IsNullOrEmpty(name))
                return false;
            if (SpouseContent.IsVanilla(name) && !AllowVanillaOverride)
                return false;
            if (!Registry.TryGetValue(name, out var c) || c?.Jealousy?.Rivals == null)
                return false;
            rivals = c.Jealousy.Rivals;
            return NonEmpty(rivals);
        }

        public static bool TryGetSeasonPref(string name, out ExtendedContent.SeasonPref pref)
        {
            pref = null;
            if (!Enabled || string.IsNullOrEmpty(name))
                return false;
            if (SpouseContent.IsVanilla(name) && !AllowVanillaOverride)
                return false;
            if (!Registry.TryGetValue(name, out var c) || c?.Behavior == null)
                return false;
            int? fav = SeasonToIndex(c.Behavior.FavoriteSeason);
            int? least = SeasonToIndex(c.Behavior.LeastSeason);
            if (fav == null && least == null)
                return false;
            // Use -1 for "no preference on this axis" so it never matches a real season (0-3).
            pref = new ExtendedContent.SeasonPref { Favorite = fav ?? -1, Least = least ?? -1 };
            return true;
        }

        /// <summary>Whether this NPC is cheered (true) rather than dampened by rain. False if unspecified.</summary>
        public static bool TryGetLovesRain(string name, out bool lovesRain)
        {
            lovesRain = false;
            if (!Enabled || string.IsNullOrEmpty(name))
                return false;
            if (SpouseContent.IsVanilla(name) && !AllowVanillaOverride)
                return false;
            if (!Registry.TryGetValue(name, out var c) || c?.Behavior?.LovesRain == null)
                return false;
            lovesRain = c.Behavior.LovesRain.Value;
            return true;
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static bool NonEmpty<T>(List<T> list) => list != null && list.Count > 0;

        /// <summary>Parse a season name or 0–3 index. Returns null if unrecognized.</summary>
        public static int? SeasonToIndex(string season)
        {
            if (string.IsNullOrWhiteSpace(season))
                return null;
            switch (season.Trim().ToLowerInvariant())
            {
                case "spring": case "0": return 0;
                case "summer": case "1": return 1;
                case "fall": case "autumn": case "2": return 2;
                case "winter": case "3": return 3;
                default: return null;
            }
        }

        /// <summary>Validate a makeup category against the three logic identifiers. Returns the normalized value or null.</summary>
        public static string NormalizeCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return null;
            switch (category.Trim().ToLowerInvariant())
            {
                case "sweet": return "sweet";
                case "nature": return "nature";
                case "homemade": return "homemade";
                default: return null;
            }
        }
    }
}
