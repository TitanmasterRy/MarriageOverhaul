using System;
using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>
    /// External content library for the extended marriage systems. All personalized dialogue and
    /// per-spouse data lives here (and in the partial files / Letter &amp; Dream content classes),
    /// never hardcoded in ModEntry.
    /// </summary>
    public static partial class ExtendedContent
    {
        private static bool IsVanilla(string name) => SpouseContent.IsVanilla(name);

        private static T Pick<T>(IReadOnlyList<T> list, Random rng) => list[rng.Next(list.Count)];

        // ── F9: Seasonal affection ────────────────────────────────
        // Season indexes: 0 = spring, 1 = summer, 2 = fall, 3 = winter.
        public class SeasonPref { public int Favorite; public int Least; }

        private static readonly Dictionary<string, SeasonPref> Seasons = new Dictionary<string, SeasonPref>
        {
            ["Abigail"]   = new SeasonPref { Favorite = 3, Least = 1 },
            ["Penny"]     = new SeasonPref { Favorite = 0, Least = 3 },
            ["Haley"]     = new SeasonPref { Favorite = 1, Least = 3 },
            ["Emily"]     = new SeasonPref { Favorite = 0, Least = 2 },
            ["Leah"]      = new SeasonPref { Favorite = 2, Least = 0 },
            ["Maru"]      = new SeasonPref { Favorite = 2, Least = 1 },
            ["Alex"]      = new SeasonPref { Favorite = 1, Least = 3 },
            ["Elliott"]   = new SeasonPref { Favorite = 2, Least = 0 },
            ["Harvey"]    = new SeasonPref { Favorite = 0, Least = 3 },
            ["Sam"]       = new SeasonPref { Favorite = 1, Least = 2 },
            ["Sebastian"] = new SeasonPref { Favorite = 3, Least = 1 },
            ["Shane"]     = new SeasonPref { Favorite = 0, Least = 3 }
        };

        /// <summary>The spouse's seasonal preferences, or null for modded spouses (no modifier).</summary>
        public static SeasonPref GetSeasonPref(string name)
            => IsVanilla(name) && Seasons.ContainsKey(name) ? Seasons[name] : null;

        private static readonly List<string> FavoriteSeasonLines = new List<string>
        {
            "This is my favorite time of year, you know. Everything just feels a little brighter with you.",
            "I love this season. I'm in such a good mood I could kiss you. ...So I might.",
            "Something about this time of year makes me fall for you all over again.",
            "Perfect weather, perfect company. I'm exactly where I want to be."
        };
        public static string FavoriteSeasonLine(Random rng)
        {
            int i = rng.Next(FavoriteSeasonLines.Count);
            return I18n.Get($"season.favorite.{i}", FavoriteSeasonLines[i]);
        }

        private static readonly List<string> LeastSeasonLines = new List<string>
        {
            "This season always puts me in a bit of a funk. ...A little extra attention wouldn't hurt.",
            "I'm not at my best this time of year. Be patient with me?",
            "I get kind of low this season. Just having you around helps more than you know."
        };
        public static string LeastSeasonLine(Random rng)
        {
            int i = rng.Next(LeastSeasonLines.Count);
            return I18n.Get($"season.least.{i}", LeastSeasonLines[i]);
        }

        // ── F13: Honeymoon phase ──────────────────────────────────
        private static readonly List<string> HoneymoonLines = new List<string>
        {
            "I still can't believe we're actually married. I keep grinning at nothing.",
            "Good morning, my love. Waking up next to you is my new favorite thing in the world.",
            "Is it normal to be THIS happy? Because I am. Completely.",
            "Come back to bed for five more minutes. ...Okay, the farm can wait ten.",
            "I married you. ME. I get to say that now. Best decision of my life.",
            "Every little thing feels like a honeymoon right now. I never want it to end."
        };
        public static string HoneymoonLine(Random rng)
        {
            int i = rng.Next(HoneymoonLines.Count);
            return I18n.Get($"honeymoon.{i}", HoneymoonLines[i]);
        }

        // ── F2: Chore description lines (by chore key) ────────────
        private static readonly Dictionary<string, List<string>> ChoreLinesLow = new Dictionary<string, List<string>>
        {
            ["water"]   = new List<string> { "I gave a few of the crops near the house a little water this morning. Just helping out.", "Watered some of the closest plants for you. Don't strain yourself today." },
            ["collect"] = new List<string> { "I grabbed a couple things from the animals and tossed them in the bin. Small thing.", "Collected a bit from the coop and shipped it. Saved you a trip." },
            ["cook"]    = new List<string> { "I left a little something in the fridge in case you get hungry.", "There's a bite to eat in the fridge. Nothing fancy." },
            ["gold"]    = new List<string> { "Oh — I found some extra change around the house. Left it on the table for you.", "Found a bit of loose change and set it on the table. Treat yourself." },
            ["forage"]  = new List<string> { "I picked up a few odds and ends from around the farm and put them in a chest.", "Tidied up some forage lying around and stashed it for you." }
        };
        private static readonly Dictionary<string, List<string>> ChoreLinesHigh = new Dictionary<string, List<string>>
        {
            ["water"]   = new List<string> { "I watered all the crops near the house this morning — wanted to take some weight off you. I love this life we're building.", "Got up early and watered a good stretch of the field for you. Anything to make your day easier, love." },
            ["collect"] = new List<string> { "I cleared out the coop and the barn and shipped everything for you. We make a good team, don't we?", "Collected from all the animals and got it in the bin. Figured I'd handle it so you could rest a little." },
            ["cook"]    = new List<string> { "I made you a proper meal and left it in the fridge. Put real love into it. Eat well today, okay?", "There's a hot dish waiting in the fridge — your favorite. I wanted to take care of you today." },
            ["gold"]    = new List<string> { "I tidied up and found a nice little sum tucked away. Left it on the table — consider it a gift.", "Found some extra gold sorting through things. It's on the table. You work so hard, you've earned a treat." },
            ["forage"]  = new List<string> { "I walked the whole farm and gathered everything worth keeping — good stuff, too. It's all in the chest for you.", "Collected a chest's worth of nice forage from around the place. Thought it might be useful, love." }
        };

        public static string ChoreLine(string key, bool highQuality, Random rng)
        {
            var map = highQuality ? ChoreLinesHigh : ChoreLinesLow;
            string q = highQuality ? "high" : "low";
            if (map.TryGetValue(key, out var list))
            {
                int i = rng.Next(list.Count);
                return I18n.Get($"chore.{q}.{key}.{i}", list[i]);
            }
            return I18n.Get("chore.fallback", "I helped out around the farm a little this morning.");
        }

        // ── F7: Children ──────────────────────────────────────────
        private static readonly List<string> ChildWarm = new List<string>
        {
            "{child} said the funniest thing this morning. I wish you'd been there to hear it.",
            "I caught {child} trying to copy the way you walk. We're raising a little you, you know.",
            "The kids adore you. {child} asked when you'd be home before you even left.",
            "Watching you with {child} yesterday... that's the happiest I think I've ever been."
        };
        private static readonly List<string> ChildConcern = new List<string>
        {
            "...{child} asked if we were okay. Kids notice things, you know. It worries me.",
            "I don't want {child} growing up feeling the tension between us. Please, let's fix this.",
            "{child} has been quiet lately. I think they can tell something's wrong with us."
        };
        private static readonly List<string> ChildAsk = new List<string>
        {
            "{child} has been asking for you. Could you spend a little time with them today?",
            "Would you sit with the kids for a while today? It would mean the world to {child}.",
            "The little one misses you when you're in the fields all day. Make some time today?"
        };
        public static string ChildWarmLine(string child, Random rng)
        {
            int i = rng.Next(ChildWarm.Count);
            return I18n.Get($"child.warm.{i}", ChildWarm[i]).Replace("{child}", child);
        }
        public static string ChildConcernLine(string child, Random rng)
        {
            int i = rng.Next(ChildConcern.Count);
            return I18n.Get($"child.concern.{i}", ChildConcern[i]).Replace("{child}", child);
        }
        public static string ChildAskLine(string child, Random rng)
        {
            int i = rng.Next(ChildAsk.Count);
            return I18n.Get($"child.ask.{i}", ChildAsk[i]).Replace("{child}", child);
        }

        private static readonly List<string> ChildEngage = new List<string>
        {
            "Look at them light up. Thank you for this — it matters more than you know.",
            "That's exactly the kind of parent I hoped you'd be. The kids are lucky. So am I."
        };
        public static string ChildEngageLine(Random rng)
        {
            int i = rng.Next(ChildEngage.Count);
            return I18n.Get($"child.engage.{i}", ChildEngage[i]);
        }

        // ── F12: Town gossip ──────────────────────────────────────
        private static readonly Dictionary<string, List<string>> GossipPositive = new Dictionary<string, List<string>>
        {
            ["Robin"]  = new List<string> { "Your spouse stopped by and would NOT stop singing your praises. It's adorable, honestly." },
            ["Pierre"] = new List<string> { "Heard from your better half the other day — sounds like you two are doing wonderfully. Good for you." },
            ["Willy"]  = new List<string> { "Yer spouse was down by the water talkin' about ya. All good things, lad. All good things." },
            ["Emily"]  = new List<string> { "Your partner's aura has been positively glowing lately. Whatever you're doing, keep it up!" },
            ["Marnie"] = new List<string> { "Oh, your spouse came by and just gushed about you. Young love. Well — married love. It's sweet." },
            ["Gus"]    = new List<string> { "Your spouse was in the other night, going on about how happy they are. Warmed my heart, it did." }
        };
        private static readonly Dictionary<string, List<string>> GossipConcerned = new Dictionary<string, List<string>>
        {
            ["Robin"]  = new List<string> { "I, ah... saw your spouse the other day. They seemed a little down. Everything okay at home?" },
            ["Pierre"] = new List<string> { "Your spouse came in looking pretty glum recently. None of my business, of course, but... take care of each other." },
            ["Willy"]  = new List<string> { "Yer spouse looked a bit blue last I saw 'em. Hope things are alright between ya." },
            ["Emily"]  = new List<string> { "Your partner's energy felt... clouded, last time we spoke. I hope you find your way back to the light together." },
            ["Marnie"] = new List<string> { "I don't mean to pry, but your spouse seemed awfully sad recently. Is everything okay with you two?" },
            ["Gus"]    = new List<string> { "Your spouse nursed one drink in the corner for hours the other night. Didn't seem themselves. Just... thought you should know." }
        };
        public static string GossipLine(string villager, bool positive, Random rng)
        {
            var map = positive ? GossipPositive : GossipConcerned;
            if (!map.TryGetValue(villager, out var list))
                return null;
            int i = rng.Next(list.Count);
            return I18n.Get($"gossip.{(positive ? "positive" : "concerned")}.{villager}.{i}", list[i]);
        }
        public static readonly string[] GossipVillagers = { "Robin", "Pierre", "Willy", "Emily", "Marnie", "Gus" };

        // ── F16: Visitor jealousy ─────────────────────────────────
        private static readonly Dictionary<string, string> VisitorComments = new Dictionary<string, string>
        {
            ["Abigail"]   = "So {visitor} was hanging around our farm yesterday, huh. ...I'm not jealous. I could just take 'em in a fight, that's all.",
            ["Penny"]     = "I, um, noticed {visitor} visiting yesterday. It's silly, but I did wonder why they needed so much of your time.",
            ["Haley"]     = "Oh, {visitor} was here yesterday? How nice. ...You don't need to spend THAT long with them, though.",
            ["Emily"]     = "{visitor}'s energy was all over the farm yesterday. I don't mind sharing the space... mostly.",
            ["Leah"]      = "Saw {visitor} out here yesterday. Just don't go finding them more interesting than me, alright?",
            ["Maru"]      = "{visitor} was here a suspiciously long time yesterday. I ran the numbers. The numbers say I'm a little jealous.",
            ["Alex"]      = "{visitor} was on our farm yesterday, huh? ...I'm cooler than them, right? Right.",
            ["Elliott"]   = "Ah, so {visitor} darkened our doorstep yesterday. Should I be composing a tragic ballad of betrayal? ...I jest. Mostly.",
            ["Harvey"]    = "I, ah, noticed {visitor} visiting yesterday. Not that it's my business who comes by! Forget I mentioned it. ...But I did notice.",
            ["Sam"]       = "Yo, {visitor} was here forever yesterday. Should I be worried? Nah. ...Should I, though?",
            ["Sebastian"] = "{visitor} was around yesterday. ...Whatever. Just noticed. That's all.",
            ["Shane"]     = "{visitor} was sniffin' around the farm yesterday. Tch. ...Just keep 'em away from my chickens. And you."
        };
        private const string GenericVisitor = "I noticed {visitor} spending a lot of time on our farm yesterday. ...No reason. Just noticed, is all.";
        public static string VisitorComment(string name, string visitor)
        {
            bool has = IsVanilla(name) && VisitorComments.ContainsKey(name);
            string eng = has ? VisitorComments[name] : GenericVisitor;
            return I18n.Get($"visitor.{(has ? name : "generic")}", eng).Replace("{visitor}", visitor);
        }
    }
}
