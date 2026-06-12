using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>F8: handwritten romantic letters, one writing style per vanilla spouse.</summary>
    public static class LetterContent
    {
        private static bool IsVanilla(string name) => SpouseContent.IsVanilla(name);

        private static readonly Dictionary<string, List<string>> Letters = new Dictionary<string, List<string>>
        {
            ["Penny"] = new List<string> {
                "My dearest,^Another quiet morning, another reason to be grateful. The kettle sang and I thought of you. Come home safe.^Ever yours,^Penny",
                "My love,^I find I write your name in the margins of my books without meaning to. Some habits are too sweet to break.^With all my heart,^Penny",
                "Dearest,^The house is warm and tidy and waiting for you, as am I. Hurry home when you can.^Forever yours,^Penny",
                "My dear,^I never imagined a life this gentle was meant for me. Thank you for making it so, every single day.^All my love,^Penny",
                "My love,^I baked too much again, thinking of you. Come eat. Come rest. Come be loved a while.^Tenderly,^Penny",
                "Dearest,^You are the steady thing I never thought I'd have. I treasure you more than these small words can hold.^Devotedly,^Penny",
                "My darling,^I watched you sleep this morning and could not bear to wake you. My heart is so very full.^Always,^Penny",
                "My love,^Whatever the day brings you, know there is a home and a heart here that are entirely yours.^With deepest affection,^Penny" },
            ["Sebastian"] = new List<string> {
                "Hey.^Couldn't sleep. Watched the rain. Thought about you the whole time.^- Seb",
                "You left early.^House feels wrong without you in it. Come back.^- Seb",
                "Not good with these.^Just know I'd choose you again. Every time.^- Seb",
                "Quiet morning.^You're the only quiet I actually like.^- Seb",
                "Hey.^I don't say it enough. So: I love you. There.^- Seb",
                "Found a song that reminded me of you.^Track's yours now. Everything is, kind of.^- Seb",
                "You looked happy yesterday.^I did that. Felt good.^- Seb",
                "Rain again.^Wish you were under it with me.^- Seb" },
            ["Elliott"] = new List<string> {
                "My beloved,^Dawn broke this morning as though it had something to prove, and still it could not rival the way you say my name.^Eternally yours,^Elliott",
                "My darling,^I have written ten thousand words today and discarded them all, for none could hold what a single glance from you contains.^With boundless devotion,^Elliott",
                "My muse,^The sea grows jealous, I think, of how often my thoughts wander from its shores to the harbor of your arms.^Forever,^Elliott",
                "My love,^Last night I dreamt our story bound in leather on every shelf in every land, and woke gladder still to simply live it.^Yours always,^Elliott",
                "Beloved,^You are the comma in my breathless sentence, the rest in my frantic music. Beside you, I finally slow.^Adoringly,^Elliott",
                "My darling,^Were I to spend a lifetime cataloguing your wonders, I should die mid-list, content and woefully incomplete.^Devotedly,^Elliott",
                "My love,^The gulls cried your name this morning. I am certain of it. I have begun a sonnet defending their good sense.^Yours,^Elliott",
                "My muse,^Come find me by the water, where the only thing more endless than the tide is what I feel for you.^Always and ever,^Elliott" },
            ["Haley"] = new List<string> {
                "Hey you,^Looking cute today, obviously. The house is a mess but I don't even care. ...Okay I care a little. Mostly I just missed you.^- Haley",
                "You,^Come home at a reasonable hour. I made an effort with my hair and someone should appreciate it. ...I mean you. Only you.^- Haley",
                "Hey,^Saw the cutest couple in town and thought, ew, that's literally us now. ...I'm not even mad about it.^- Haley",
                "You,^Quit being so charming all the time, it's exhausting. ...Don't actually quit. I'd miss it. A lot.^- Haley",
                "Hey gorgeous,^I'm framing another photo of us. Don't tell anyone I'm this sappy. ...Okay you can know. Just you.^- Haley",
                "You,^Today's going to be a good day, I can tell. Everything's better when I wake up next to you, even mud season.^- Haley",
                "Hey,^Bring me something pretty if you find it. Or just bring yourself back. ...Honestly the second one's better.^- Haley",
                "You,^People used to say I married down. They were so wrong it's almost funny. You're the best thing about my life.^- Haley" },
            ["Abigail"] = new List<string> {
                "HEY!^Big news: I named the spider in the corner Gerald and he's our pet now, no take-backs. Also I love you. Mostly the second thing.^- Abby",
                "You!^Had the BEST dream where we fought a giant slug made of pudding and WON. Anyway, good morning, I adore you.^- Abby",
                "Hey weirdo,^I rearranged all the cans in the pantry by spookiness level. You're welcome. Come admire my work, then kiss me.^- Abby",
                "YO,^Found a rock shaped exactly like your face and I'm keeping it on the windowsill forever. It's not as cute as you though.^- Abby",
                "Hey,^I've decided we're nocturnal now. Just us. The town can deal. Stay up with me tonight, it'll be great, trust me.^- Abby",
                "You!^Played my flute for the chickens this morning and I SWEAR one of them vibed. We're basically a band now. Love you, byeee.^- Abby",
                "Hey adventurer,^Drew a map of all the places we should 'explore,' which is mostly just our house but with cooler names. Let's go.^- Abby",
                "Hey,^Sappy moment, brace yourself: you're my favorite adventure and I'd raid any dungeon to keep you. Okay bye, that was a lot.^- Abby" },
            ["Emily"] = new List<string> {
                "Beloved,^The crystals were singing when I woke, a clear bright note. It was your name, I'm certain. The universe knows.^- Emily",
                "My love,^Your aura left warmth in the doorway this morning. I stood in it a while, just breathing you in.^- Emily",
                "Beloved,^I dreamt we were two stars orbiting the same quiet center. I woke and realized we already are.^- Emily",
                "My love,^The energy in our home hums in a major key these days. That's you. That's us. I'm so grateful.^- Emily",
                "Beloved,^Every color feels truer since you. The universe handed me a fuller spectrum and signed it with your love.^- Emily",
                "My love,^I felt your heartbeat from across the valley yesterday, I swear it. We're woven now, you and I.^- Emily",
                "Beloved,^The moon was generous last night. I made a wish for you on it, though my heart's already had every wish granted.^- Emily",
                "My love,^Wherever your path winds today, know two souls and the whole humming cosmos are cheering you home.^- Emily" },
            ["Leah"] = new List<string> {
                "Hey you,^The light came through the trees this morning all honey and gold, and all I could think was how it'd look on you.^- Leah",
                "Hey,^Woke to the smell of rain and cedar and you, still warm beside me. I'd paint it if I could bottle a morning.^- Leah",
                "You,^The forest is wearing its best colors today. Come walk it with me. I want to watch you notice things.^- Leah",
                "Hey,^Tried to sculpt the curve of your smile and gave up, laughing. Some things you just have to live, not carve.^- Leah",
                "Hey you,^The wine's chilled, the sky's turning peach, and there's a spot in the grass shaped exactly like the two of us.^- Leah",
                "Hey,^I keep a little of you in every piece I make now. The critics call it warmth. I call it you.^- Leah",
                "You,^Cold morning, hot coffee, your handwriting on the grocery list. The whole still life of a life I love.^- Leah",
                "Hey,^I left the city to find something real. Found you instead, which turned out to be the same thing.^- Leah" },
            ["Maru"] = new List<string> {
                "Hi!^Slept exactly 7.5 hours and woke at peak fondness-for-you levels, which honestly is just my baseline now.^- Maru",
                "Hey you,^Coefficient of how much I missed you while you were in the field yesterday: undefined. It broke the equation.^- Maru",
                "Hi,^Ran the numbers on our compatibility again. Still statistically absurd. Still completely, joyfully real.^- Maru",
                "Hey,^You're my favorite constant. Everything in the lab changes; you're the one value I can always count on.^- Maru",
                "Hi you,^My pulse does a measurable, unscientific thing when you walk in. I've stopped trying to correct for it.^- Maru",
                "Hey,^Built a little something that lights up when you come home. Over-engineered, like my feelings for you.^- Maru",
                "Hi,^Conservation of love: it only ever increases in this house. I've checked. Repeatedly. The data's adorable.^- Maru",
                "Hey you,^The probability we'd ever meet was vanishingly small. So every morning with you feels like winning twice.^- Maru" },
            ["Alex"] = new List<string> {
                "Hey babe,^Just wanted you to know I'm thinking about you. That's it. That's the note. ...Love you.^- Alex",
                "Babe,^You make the hard stuff easier. Don't know how. Glad you do.^- Alex",
                "Hey,^Be safe out there. Come home to me. That's all I ask.^- Alex",
                "Babe,^I'm not great with words. But you're it for me. Always.^- Alex",
                "Hey you,^Proud to call you mine. Tell everybody, honestly. Wanna shout it.^- Alex",
                "Babe,^Best part of my day is the front door opening. Every single time.^- Alex",
                "Hey,^You believe in me. Nobody really did before. Means everything.^- Alex",
                "Babe,^Whatever I become, it's because you saw me first. Thank you. Love you.^- Alex" },
            ["Harvey"] = new List<string> {
                "My dear,^I find myself counting the hours until you return, which is, professionally speaking, a wonderful sort of ailment.^- Harvey",
                "Dear,^Please remember to eat something today. And to know, throughout it, how deeply you are loved.^- Harvey",
                "My dear,^A quiet morning, a cup of tea, the thought of you. I require very little else to be content.^- Harvey",
                "Dear,^I rehearsed something heartfelt to tell you and forgot it entirely when you smiled. The smile said it better.^- Harvey",
                "My dear,^You have made a careful, cautious man feel quietly brave. I shall never be able to thank you enough.^- Harvey",
                "Dear,^Do be careful with that machinery today. My heart, as your doctor and your spouse, insists upon it.^- Harvey",
                "My dear,^The clinic was busy, the day was long, and still the best part was knowing I'd come home to you.^- Harvey",
                "Dear,^I love you. I've written it plainly, because you deserve plainness as much as poetry. Both are yours.^- Harvey" },
            ["Sam"] = new List<string> {
                "Yo!^You're souper important to me. ...Get it? Because soup? Okay it was bad. But I mean it. Love you, dude.^- Sam",
                "Hey,^I'm not saying you're the best spouse ever, but the other spouses should stop trying. ...You ARE the best.^- Sam",
                "Babe,^Wrote a song called 'You.' It's just me yelling your name over guitar. Critics are confused. I'm in love.^- Sam",
                "Yo,^Why'd the farmer marry me? For my charming pun-sonality. ...And maybe 'cause I'm crazy about you.^- Sam",
                "Hey,^Missed you so much today it was un-bear-able. ...Vincent told me that one. Anyway, you're my favorite.^- Sam",
                "Babe,^You plus me equals the best band that never plays gigs. Just vibes. Forever. Love ya, seriously.^- Sam",
                "Yo,^Heads up, I rearranged the fridge magnets to say something dumb and sweet. Go look. ...I love you, basically.^- Sam",
                "Hey,^Jokes aside for one sec: marrying you was the smartest thing this goofball ever did. Okay, jokes back on.^- Sam" },
            ["Shane"] = new List<string> {
                "Hey.^Not good at this. But I'm glad it's you. That's the whole letter.^- Shane",
                "You.^Slept okay for once. Think it's 'cause you were there. ...Thanks.^- Shane",
                "Hey.^Don't say it enough. So here it is in writing: I love you.^- Shane",
                "You.^Made it through another rough one. You helped. You always help.^- Shane",
                "Hey.^The chickens like you better. Smart birds. ...Me too.^- Shane",
                "You.^Didn't think I'd get a life like this. Got it 'cause of you.^- Shane",
                "Hey.^Wrote this three times. Kept it simple in the end: you matter. A lot.^- Shane",
                "You.^Whatever I am now, it's better than what I was. That's all you. Thanks.^- Shane" }
        };
        private static readonly List<string> Generic = new List<string>
        {
            "My love,^I woke thinking of you and decided to put it in writing. The house is brighter with you in it. Come home soon.^- Your spouse",
            "My dear,^Just a small note to say I'm grateful for you. For the ordinary mornings and the quiet nights. All of it.^- Your spouse",
            "My love,^Be safe out there today. And know that whatever you face, you don't face it alone. Not anymore.^- Your spouse",
            "Dearest,^I never tire of this life we're building. You are the best part of every single day.^- Your spouse",
            "My love,^No reason for this letter except that I love you and wanted you to hold it in your hands. So: I love you.^- Your spouse"
        };
        public static List<string> GetLetters(string name)
        {
            bool has = IsVanilla(name) && Letters.ContainsKey(name);
            string who = has ? name : "generic";
            List<string> src = has ? Letters[name] : Generic;
            var outList = new List<string>(src.Count);
            for (int i = 0; i < src.Count; i++)
                outList.Add(I18n.Get($"letter.{who}.{i}", src[i]));
            return outList;
        }

        internal static void CollectDefaults(IDictionary<string, string> map)
        {
            foreach (var kv in Letters)
                for (int i = 0; i < kv.Value.Count; i++)
                    map[$"letter.{kv.Key}.{i}"] = kv.Value[i];
            for (int i = 0; i < Generic.Count; i++)
                map[$"letter.generic.{i}"] = Generic[i];
        }
    }
}
