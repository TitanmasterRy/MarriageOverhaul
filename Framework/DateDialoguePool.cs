using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>
    /// A pool of short romantic lines used to assemble a freeform date scene once a spouse has
    /// exhausted both their unique scenes and the generic pool. Tones span sincere love, playful
    /// teasing, bold flirting, quiet intimacy, and "the night is just getting started".
    /// </summary>
    public static class DateDialoguePool
    {
        public static readonly List<string> Lines = new List<string>
        {
            "I could stay right here with you forever and call it a life well spent.",
            "Come closer. The whole world can wait its turn.",
            "You still give me butterflies. After all this time. It's ridiculous.",
            "Don't tell me this night has to end. I'm not ready to let you go.",
            "Every day I'm a little more sure I'd choose you all over again.",
            "Stop looking at me like that or we'll never make it home.",
            "You're the best decision I ever made. You know that, right?",
            "Quiet. Just let me look at you a second longer.",
            "I love the life we built. I love you more.",
            "Careful. Keep being this charming and I'll never get anything done.",
            "Some nights I still can't believe you're really mine.",
            "Take my hand. I want to walk home slow.",
            "You make ordinary evenings feel like something worth remembering.",
            "I've been thinking about you all day. ...All day.",
            "There's nowhere I'd rather be than wherever you are.",
            "Kiss me before I lose the nerve to ask.",
            "The night's still young. And so are we. Let's not waste it.",
            "You're trouble. The very best kind.",
            "Come home with me. I'm not nearly done with you.",
            "I'd outshine every star in the sky before I'd let one outshine you.",
            "Stay close. The cold's just an excuse, but I'll take it.",
            "I married my favorite person. I get to do that. Lucky me.",
            "Whatever tomorrow brings, I've got you. Always.",
            "You felt that too, didn't you? That little spark. Still there.",
            "Let's go before I say something embarrassing. ...Too late.",
            "I love you. Plainly, completely, no metaphor required.",
            "One more minute. Then home. Then... we'll see.",
            "You're blushing. I did that. ...I'm very pleased with myself.",
            "Every version of my future has you in it. Every single one.",
            "Come here. Let me show you how much I missed you today.",
            "Tonight's just for us. The rest of the world can knock tomorrow.",
            "I keep falling for you. Over and over. It never gets old."
        };

        /// <summary>
        /// Assemble a short freeform scene's beats from a few random pool lines, ending in a kiss.
        /// Returned beats use "{n}" for the spouse's name (replaced by the event builder).
        /// </summary>
        public static List<string> BuildFreeformBeats(System.Random rng)
        {
            var picks = PickDistinct(rng, 3);
            var beats = new List<string>();
            foreach (string line in picks)
                beats.Add($"speak {{n}} \"{line}\"");

            // End on the kiss (lean together, kiss sound, settle back) + a closing pool line.
            beats.Add("faceDirection {n} 3");
            beats.Add("faceDirection farmer 1");
            beats.Add("pause 500");
            beats.Add("positionOffset farmer 16 0");
            beats.Add("positionOffset {n} -16 0");
            beats.Add("pause 150");
            beats.Add("playSound dwop");
            beats.Add("emote {n} 20");
            beats.Add("emote farmer 20");
            beats.Add("pause 850");
            beats.Add("positionOffset farmer 0 0");
            beats.Add("positionOffset {n} 0 0");
            beats.Add("pause 250");
            beats.Add($"speak {{n}} \"{PickDistinct(rng, 1)[0]}\"");
            return beats;
        }

        private static List<string> PickDistinct(System.Random rng, int count)
        {
            var pool = new List<string>(Lines);
            var result = new List<string>();
            for (int i = 0; i < count && pool.Count > 0; i++)
            {
                int idx = rng.Next(pool.Count);
                result.Add(pool[idx]);
                pool.RemoveAt(idx);
            }
            return result;
        }
    }
}
