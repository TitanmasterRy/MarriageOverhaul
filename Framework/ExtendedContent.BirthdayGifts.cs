using System;
using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class ExtendedContent
    {
        // ── F11: Birthday "helpful gift" alternative to the breakfast gift ──
        private static readonly List<string> HelpfulBirthdayGifts = new List<string>
        {
            "Coffee", "Triple Shot Espresso", "Energy Tonic", "Spicy Eel", "Field Snack", "Life Elixir"
        };
        public static string PickHelpfulBirthdayGift(Random rng) => Pick(HelpfulBirthdayGifts, rng);

        // {0} = item name
        private static readonly Dictionary<string, string> HelpfulBirthdayLines = new Dictionary<string, string>
        {
            ["Abigail"]   = "Happy birthday! I figured you'd rather have something useful than another cake, so I left {0} for you. Go have an adventure, birthday person.",
            ["Penny"]     = "Happy birthday, my love. I thought something practical might help you more today, so I left {0} for you. I hope it makes your day a little easier.",
            ["Haley"]     = "Happy birthday! Okay, I know I usually do something cute, but I thought {0} would actually be useful today. You're welcome.",
            ["Emily"]     = "Happy birthday, beloved! The universe whispered that you'd need a little extra energy today, so I left {0} for you. Use it well.",
            ["Leah"]      = "Happy birthday, you. Thought you could use {0} more than something fancy today. Simple, but it's yours.",
            ["Maru"]      = "Happy birthday! I calculated that {0} would be the most statistically useful gift for your day, so here it is. Enjoy!",
            ["Alex"]      = "Happy birthday, babe! Figured {0} would help you power through the day. Go get 'em, champ.",
            ["Elliott"]   = "Happy birthday, my darling! In place of a feast, I offer something more practical: {0}. May it serve you well today.",
            ["Harvey"]    = "Happy birthday, dear. I thought {0} might be more useful for you today than something heavy. Take care of yourself out there.",
            ["Sam"]       = "Happy birthday, dude! Skipped the usual breakfast thing and got you {0} instead. Thought it'd come in handy!",
            ["Sebastian"] = "Happy birthday. Got you {0}. Figured it'd be more useful than food. Hope it helps.",
            ["Shane"]     = "Happy birthday. Didn't make breakfast this time — left you {0} instead. Thought it might come in handy. Hope you have a good one."
        };
        private const string GenericHelpfulBirthdayLine = "Happy birthday, my love! I thought you might find {0} more useful today, so that's what I left for you. I hope it helps make your day a little easier.";

        public static string GetHelpfulBirthdayLine(string name, string itemName)
            => string.Format(IsVanilla(name) && HelpfulBirthdayLines.ContainsKey(name) ? HelpfulBirthdayLines[name] : GenericHelpfulBirthdayLine, itemName);
    }
}
