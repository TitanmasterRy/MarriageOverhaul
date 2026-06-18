using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;

namespace MarriageOverhaul
{
    /// <summary>
    /// Discovers and loads Custom NPC Framework content packs — any content pack whose manifest declares
    /// <c>"ContentPackFor": { "UniqueID": "TitanmasterRy.MarriageOverhaul" }</c> — and registers their
    /// per-NPC content into <see cref="CustomNpcRegistry"/>. Malformed packs/entries are logged and
    /// skipped rather than crashing the game.
    /// </summary>
    internal static class CustomNpcContentPackLoader
    {
        /// <summary>The content file every pack must provide at its root.</summary>
        public const string ContentFileName = "content.json";

        public static void LoadAll(IModHelper helper, IMonitor monitor)
        {
            CustomNpcRegistry.Clear();

            IContentPack[] packs;
            try
            {
                packs = helper.ContentPacks.GetOwned().ToArray();
            }
            catch (System.Exception ex)
            {
                monitor.Log($"Custom NPC Framework: could not enumerate content packs: {ex.Message}", LogLevel.Warn);
                return;
            }

            if (packs.Length == 0)
            {
                monitor.Log("Custom NPC Framework: no content packs installed.", LogLevel.Trace);
                return;
            }

            var summary = new List<string>();
            int npcCount = 0;

            foreach (IContentPack pack in packs)
            {
                string packLabel = $"'{pack.Manifest.Name}' ({pack.Manifest.UniqueID})";

                CustomNpcContentFile file;
                try
                {
                    if (!pack.HasFile(ContentFileName))
                    {
                        monitor.Log($"Custom NPC Framework: pack {packLabel} has no {ContentFileName}; skipping.", LogLevel.Warn);
                        continue;
                    }
                    file = pack.ReadJsonFile<CustomNpcContentFile>(ContentFileName);
                }
                catch (System.Exception ex)
                {
                    monitor.Log($"Custom NPC Framework: failed to read {ContentFileName} from {packLabel}: {ex.Message}. Skipping pack.", LogLevel.Warn);
                    continue;
                }

                if (file?.NPCs == null || file.NPCs.Count == 0)
                {
                    monitor.Log($"Custom NPC Framework: pack {packLabel} defines no NPCs; skipping.", LogLevel.Warn);
                    continue;
                }

                foreach (KeyValuePair<string, CustomNpcContent> entry in file.NPCs)
                {
                    string npcName = entry.Key;
                    CustomNpcContent content = entry.Value;

                    if (string.IsNullOrWhiteSpace(npcName) || content == null)
                    {
                        monitor.Log($"Custom NPC Framework: {packLabel} has an NPC entry with no name; skipping that entry.", LogLevel.Warn);
                        continue;
                    }

                    List<string> sections = Validate(npcName, content, packLabel, monitor);
                    if (sections.Count == 0)
                    {
                        monitor.Log($"Custom NPC Framework: NPC '{npcName}' in {packLabel} has no usable content; skipping.", LogLevel.Warn);
                        continue;
                    }

                    if (CustomNpcRegistry.IsRegistered(npcName))
                        monitor.Log($"Custom NPC Framework: '{npcName}' is registered by more than one pack; {packLabel} overrides the earlier registration.", LogLevel.Warn);

                    CustomNpcRegistry.Register(npcName, content);
                    npcCount++;
                    summary.Add($"{npcName} [{string.Join(", ", sections)}] from {pack.Manifest.Name}");
                }
            }

            if (npcCount == 0)
                monitor.Log($"Custom NPC Framework: found {packs.Length} content pack(s) but registered no NPCs.", LogLevel.Info);
            else
            {
                monitor.Log($"Custom NPC Framework: registered {npcCount} NPC(s) from {packs.Length} content pack(s).", LogLevel.Info);
                foreach (string s in summary)
                    monitor.Log($"  • {s}", LogLevel.Info);
            }
        }

        /// <summary>Validate, sanitize and register a single NPC's content (used by the C# API). Returns true if anything usable was registered.</summary>
        internal static bool TryRegisterSingle(string npcName, CustomNpcContent content, string sourceLabel, IMonitor monitor)
        {
            if (string.IsNullOrWhiteSpace(npcName) || content == null)
                return false;

            List<string> sections = Validate(npcName, content, sourceLabel, monitor);
            if (sections.Count == 0)
            {
                monitor.Log($"Custom NPC Framework: '{npcName}' from {sourceLabel} has no usable content; not registered.", LogLevel.Warn);
                return false;
            }

            CustomNpcRegistry.Register(npcName, content);
            monitor.Log($"Custom NPC Framework: registered '{npcName}' [{string.Join(", ", sections)}] from {sourceLabel}.", LogLevel.Info);
            return true;
        }

        /// <summary>
        /// Sanitize an entry in place and return the list of usable section names (for the summary log).
        /// Invalid sub-parts are dropped with a warning; the entry survives on whatever remains valid.
        /// </summary>
        private static List<string> Validate(string npcName, CustomNpcContent c, string packLabel, IMonitor monitor)
        {
            var sections = new List<string>();

            // Morning — keep only non-empty tiers.
            if (c.Morning != null && (HasLines(c.Morning.VeryLow) || HasLines(c.Morning.Low) || HasLines(c.Morning.High) || HasLines(c.Morning.VeryHigh)))
                sections.Add("Morning");
            else
                c.Morning = null;

            // Mood — keep if any of the three pools has lines.
            if (c.Mood != null && (HasLines(c.Mood.Happy) || HasLines(c.Mood.Neutral) || HasLines(c.Mood.Grumpy)))
                sections.Add("Mood");
            else
                c.Mood = null;

            // Arguments — drop incomplete scenarios; keep the section only if at least one complete remains.
            if (c.Arguments != null)
            {
                int before = c.Arguments.Count;
                c.Arguments = c.Arguments.Where(a => a != null && a.IsComplete()).ToList();
                if (c.Arguments.Count < before)
                    monitor.Log($"Custom NPC Framework: '{npcName}' in {packLabel} had {before - c.Arguments.Count} incomplete argument scenario(s); those were skipped.", LogLevel.Warn);
                if (c.Arguments.Count > 0)
                    sections.Add("Arguments");
                else
                    c.Arguments = null;
            }

            // Anniversary — keep if any field present.
            if (c.Anniversary != null && (NotBlank(c.Anniversary.Reminder) || NotBlank(c.Anniversary.Sweet) || NotBlank(c.Anniversary.Disappointed)))
                sections.Add("Anniversary");
            else
                c.Anniversary = null;

            // Jealousy — keep if it has lines and/or a rival list.
            if (c.Jealousy != null && (HasLines(c.Jealousy.Lines) || HasLines(c.Jealousy.Rivals)))
                sections.Add("Jealousy");
            else
                c.Jealousy = null;

            // Makeup — requires a valid category.
            if (c.Makeup != null)
            {
                string cat = CustomNpcRegistry.NormalizeCategory(c.Makeup.Category);
                if (cat == null)
                {
                    monitor.Log($"Custom NPC Framework: '{npcName}' in {packLabel} has an invalid makeup Category ('{c.Makeup.Category}'); must be sweet/nature/homemade. Makeup section ignored.", LogLevel.Warn);
                    c.Makeup = null;
                }
                else
                {
                    c.Makeup.Category = cat;
                    sections.Add("Makeup");
                }
            }

            // Loot — keep if any rarity tier has items.
            if (c.Loot != null && (HasLines(c.Loot.Common) || HasLines(c.Loot.Uncommon) || HasLines(c.Loot.Rare)))
                sections.Add("Loot");
            else
                c.Loot = null;

            // Behavior — validate seasons (informational; invalid axes are ignored at access time).
            if (c.Behavior != null)
            {
                if (NotBlank(c.Behavior.FavoriteSeason) && CustomNpcRegistry.SeasonToIndex(c.Behavior.FavoriteSeason) == null)
                    monitor.Log($"Custom NPC Framework: '{npcName}' in {packLabel} has an unrecognized FavoriteSeason ('{c.Behavior.FavoriteSeason}'); ignored.", LogLevel.Warn);
                if (NotBlank(c.Behavior.LeastSeason) && CustomNpcRegistry.SeasonToIndex(c.Behavior.LeastSeason) == null)
                    monitor.Log($"Custom NPC Framework: '{npcName}' in {packLabel} has an unrecognized LeastSeason ('{c.Behavior.LeastSeason}'); ignored.", LogLevel.Warn);

                bool hasBehavior = c.Behavior.LovesRain.HasValue
                    || CustomNpcRegistry.SeasonToIndex(c.Behavior.FavoriteSeason) != null
                    || CustomNpcRegistry.SeasonToIndex(c.Behavior.LeastSeason) != null;
                if (hasBehavior)
                    sections.Add("Behavior");
            }

            return sections;
        }

        private static bool HasLines(List<string> list) => list != null && list.Any(s => !string.IsNullOrWhiteSpace(s));
        private static bool NotBlank(string s) => !string.IsNullOrWhiteSpace(s);
    }
}
