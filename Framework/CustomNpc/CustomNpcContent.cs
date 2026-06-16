using System;
using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>
    /// Root of a content pack's <c>content.json</c>. Content packs declare
    /// <c>"ContentPackFor": { "UniqueID": "TitanmasterRy.MarriageOverhaul" }</c> in their manifest
    /// and provide personalized dialogue/behavior for their own custom (modded) NPCs.
    /// Unknown fields are ignored on load, so new optional sections can be added in future
    /// versions without breaking existing packs.
    /// </summary>
    public class CustomNpcContentFile
    {
        /// <summary>Schema version the pack was authored against (informational; currently "1.0").</summary>
        public string Format { get; set; }

        /// <summary>Map of NPC internal name → personalized content. Every NPC and every section is optional.</summary>
        public Dictionary<string, CustomNpcContent> NPCs { get; set; }
    }

    /// <summary>
    /// Personalized content for a single custom NPC. Every section is optional; any omitted section
    /// falls through to Marriage Overhaul's existing generic fallback pool for that one system.
    /// All text here is supplied verbatim by the pack author and is NOT routed through Marriage
    /// Overhaul's i18n — authors are responsible for their own translations.
    /// </summary>
    public class CustomNpcContent
    {
        /// <summary>Per-NPC behavior tuning (weather mood, seasonal preference, system allow-list).</summary>
        public CustomNpcBehavior Behavior { get; set; }

        /// <summary>Friendship-tiered morning greetings. Keys: VeryLow, Low, High, VeryHigh.</summary>
        public CustomMorning Morning { get; set; }

        /// <summary>Daily mood greetings. Happy / Neutral / Grumpy line pools.</summary>
        public CustomMood Mood { get; set; }

        /// <summary>Argument scenarios (dialogue trees with good/neutral/bad responses).</summary>
        public List<CustomArgument> Arguments { get; set; }

        /// <summary>Anniversary lines: reminder letter, gifted reaction, forgotten reaction.</summary>
        public CustomAnniversary Anniversary { get; set; }

        /// <summary>Jealousy line pool plus an optional explicit rival-NPC list.</summary>
        public CustomJealousy Jealousy { get; set; }

        /// <summary>Makeup-gift hint, reconciliation, and resignation lines plus the wanted category.</summary>
        public CustomMakeup Makeup { get; set; }

        /// <summary>True when a given system has usable data AND is permitted by the Behavior allow-list.</summary>
        public bool SystemEnabled(string system)
        {
            if (this.Behavior?.EnabledSystems == null)
                return true; // no allow-list → any provided section applies
            foreach (string s in this.Behavior.EnabledSystems)
                if (string.Equals(s, system, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
    }

    /// <summary>Per-NPC behavior block.</summary>
    public class CustomNpcBehavior
    {
        /// <summary>If true the NPC is cheered (rather than dampened) by rain, like Sebastian/Abigail. Default false.</summary>
        public bool? LovesRain { get; set; }

        /// <summary>Favorite season: "spring" | "summer" | "fall" | "winter" (or 0–3). Optional.</summary>
        public string FavoriteSeason { get; set; }

        /// <summary>Least-favorite season: "spring" | "summer" | "fall" | "winter" (or 0–3). Optional.</summary>
        public string LeastSeason { get; set; }

        /// <summary>
        /// Optional explicit allow-list of systems this NPC participates in
        /// (e.g. ["Morning","Mood","Arguments","Anniversary","Jealousy","Makeup"]).
        /// If omitted, every section the pack provides applies.
        /// </summary>
        public List<string> EnabledSystems { get; set; }
    }

    /// <summary>Friendship-tiered morning greeting pools.</summary>
    public class CustomMorning
    {
        public List<string> VeryLow { get; set; }
        public List<string> Low { get; set; }
        public List<string> High { get; set; }
        public List<string> VeryHigh { get; set; }
    }

    /// <summary>Mood greeting pools (Neutral mirrors the shared "general" everyday pool).</summary>
    public class CustomMood
    {
        public List<string> Happy { get; set; }
        public List<string> Neutral { get; set; }
        public List<string> Grumpy { get; set; }
    }

    /// <summary>One argument scenario: an opener and three weighted player responses.</summary>
    public class CustomArgument
    {
        public string Intro { get; set; }
        public string GoodChoice { get; set; }
        public string GoodReply { get; set; }
        public string NeutralChoice { get; set; }
        public string NeutralReply { get; set; }
        public string BadChoice { get; set; }
        public string BadReply { get; set; }

        /// <summary>All seven fields must be non-empty for the scenario to be usable.</summary>
        public bool IsComplete()
            => !string.IsNullOrWhiteSpace(this.Intro)
            && !string.IsNullOrWhiteSpace(this.GoodChoice) && !string.IsNullOrWhiteSpace(this.GoodReply)
            && !string.IsNullOrWhiteSpace(this.NeutralChoice) && !string.IsNullOrWhiteSpace(this.NeutralReply)
            && !string.IsNullOrWhiteSpace(this.BadChoice) && !string.IsNullOrWhiteSpace(this.BadReply);
    }

    /// <summary>Anniversary lines.</summary>
    public class CustomAnniversary
    {
        /// <summary>Morning reminder letter body (use ^ for line breaks, like vanilla mail).</summary>
        public string Reminder { get; set; }
        /// <summary>Shown when gifted on the anniversary.</summary>
        public string Sweet { get; set; }
        /// <summary>Shown the morning after a forgotten anniversary.</summary>
        public string Disappointed { get; set; }
    }

    /// <summary>Jealousy content.</summary>
    public class CustomJealousy
    {
        /// <summary>One or more jealousy lines (one is chosen at random).</summary>
        public List<string> Lines { get; set; }

        /// <summary>
        /// Optional explicit list of rival NPC internal names. When present, the spouse only gets
        /// jealous over gifts to these NPCs (overriding the default same-gender targeting).
        /// </summary>
        public List<string> Rivals { get; set; }
    }

    /// <summary>Makeup-gift content.</summary>
    public class CustomMakeup
    {
        /// <summary>Wanted gift category — must be "sweet", "nature", or "homemade" (a logic identifier, never shown/translated).</summary>
        public string Category { get; set; }
        /// <summary>Dialogue hint at the wanted category (not the specific item).</summary>
        public string Hint { get; set; }
        /// <summary>Shown when the right category of gift is given.</summary>
        public string Reconcile { get; set; }
        /// <summary>Shown when the makeup window lapses without a gift.</summary>
        public string Resigned { get; set; }
    }
}
