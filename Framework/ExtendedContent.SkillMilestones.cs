using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class ExtendedContent
    {
        // ── Skill milestones (level 5 / 10 in Farming/Fishing/Foraging/Mining/Combat) ──
        // {0} = skill name, {1} = level
        private static readonly Dictionary<string, string> SkillMilestone5 = new Dictionary<string, string>
        {
            ["Abigail"]   = "Wait, level {1} {0} already?! That's so cool! I knew you had it in you. I left you a little something to celebrate.",
            ["Penny"]     = "I heard you reached level {1} in {0}. I'm so proud of how hard you've been working. I left a little something for you.",
            ["Haley"]     = "Okay, level {1} {0}? That's actually impressive. Don't let it go to your head... I left you a little gift anyway.",
            ["Emily"]     = "Your energy around {0} has grown so much — level {1} now! The universe noticed, and so did I. A little gift, for you.",
            ["Leah"]      = "Level {1} in {0}, huh? You're really coming into your own out there. I left something small to mark the occasion.",
            ["Maru"]      = "I ran the numbers — level {1} {0} is a real milestone! I'm proud of you. Left you a little something for it.",
            ["Alex"]      = "Yo, level {1} {0}?! That's a big deal, babe. I'm proud of you. Got you a little something to celebrate.",
            ["Elliott"]   = "Level {1} in {0} — a quiet triumph, but a triumph nonetheless. I've left a small token of my admiration.",
            ["Harvey"]    = "I, ah, heard you reached level {1} in {0}. That's wonderful progress. I left a small something to mark it.",
            ["Sam"]       = "Dude, level {1} {0}! That's awesome! I left you a little something to celebrate, you've earned it.",
            ["Sebastian"] = "...Level {1} in {0}. That's actually pretty solid. Left you something small. Don't make it weird.",
            ["Shane"]     = "Level {1} {0}, huh. That's somethin'. I left you a little something. You're doin' good."
        };
        private static readonly Dictionary<string, string> SkillMilestone10 = new Dictionary<string, string>
        {
            ["Abigail"]   = "LEVEL {1} {0}?! That's the max, you absolute legend! I am SO proud of you. There's a little something waiting for you.",
            ["Penny"]     = "Level {1} in {0} — you've truly mastered it. I'm beyond proud of everything you've accomplished. I left you a small gift.",
            ["Haley"]     = "Okay, level {1} {0} is genuinely amazing. I'm really, really proud of you. I left you a little something.",
            ["Emily"]     = "You've reached the highest level of {0} — level {1}! Your spirit shines so brightly. A small gift, with all my love.",
            ["Leah"]      = "Level {1} in {0}. You've mastered it completely. I'm so proud of everything you've built. I left a little something for you.",
            ["Maru"]      = "Level {1} {0} — maximum level, fully optimized! I'm incredibly proud of you. I left a little something to celebrate.",
            ["Alex"]      = "Level {1} {0}?! That's the top, babe! MVP stuff. I'm so proud of you. Left you a little something.",
            ["Elliott"]   = "Level {1} in {0} — true mastery, my love. I've left a small token, a humble gift for a momentous achievement.",
            ["Harvey"]    = "Level {1} in {0}... that's the maximum. Truly remarkable. I left a small something to celebrate this achievement.",
            ["Sam"]       = "DUDE. Level {1} {0}! Max level! That's huge! I left you something small to celebrate, you totally earned it.",
            ["Sebastian"] = "...Level {1} in {0}. Max level. That's really impressive. Left you something. Seriously, nice work.",
            ["Shane"]     = "Level {1} {0}. Max level. That's... that's really something. I left you a little something. Proud of you."
        };
        private const string GenericSkillMilestone5 = "I heard you reached level {1} in {0}! I'm proud of you. I left a little something to celebrate.";
        private const string GenericSkillMilestone10 = "Level {1} in {0} — that's the maximum! I'm so proud of everything you've accomplished. I left a little something to celebrate.";

        public static string GetSkillMilestone(string name, string skill, int level)
        {
            var map = level >= 10 ? SkillMilestone10 : SkillMilestone5;
            string generic = level >= 10 ? GenericSkillMilestone10 : GenericSkillMilestone5;
            string template = IsVanilla(name) && map.ContainsKey(name) ? map[name] : generic;
            return string.Format(template, skill, level);
        }
    }
}
