using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class ExtendedContent
    {
        // ── F5: Inside-joke callback lines (>= 10 per vanilla spouse) ──
        private static readonly Dictionary<string, List<string>> Jokes = new Dictionary<string, List<string>>
        {
            ["Abigail"] = new List<string> {
                "Remember when you swore you weren't scared in the mines and then a bat made you yelp? Classic.",
                "I still can't believe you actually liked my mom's cooking that one time. You're braver than any adventurer.",
                "'Side by side in the dark.' That's still our thing, you know. Don't think I forgot.",
                "Heh, the flute. You're never living down how into it you pretended NOT to be.",
                "You and your 'I'll make more time for us.' Look at us now. Annoyingly happy.",
                "I dared you to eat that suspicious cave mushroom and you DID. Legend. Idiot. Legend.",
                "Still think about the night we stayed up planning that fake dungeon raid. Best non-date date ever.",
                "You always make that face when I say something weird. I live for that face.",
                "We're basically the valley's chaos couple at this point. I'm so proud of us.",
                "'Partner in crime.' I meant it then. I mean it more now." },
            ["Penny"] = new List<string> {
                "Do you remember reading that passage to me by the shelves? I think about it more than I should admit.",
                "You told me once that I mattered to you 'more than anything.' I've never forgotten the exact words.",
                "The flour on my nose. You bring it up every time I bake. I pretend to be annoyed. I'm not.",
                "'Something steady, together.' We built it, didn't we? Look at us.",
                "I still have the first little gift you gave me. Some things you just keep.",
                "Remember when you noticed I'd cleaned the whole house? Most people wouldn't have. You did.",
                "You once told me wanting things wasn't dangerous. I'm still learning that. With you.",
                "The girl from the trailer never imagined a morning like this. I owe her one.",
                "You always let me finish the story, even when I get shy. That's a small thing that's actually huge.",
                "'The shore loves the tide.' That's us. I decided. No take-backs." },
            ["Haley"] = new List<string> {
                "The mud thing. I STILL can't believe I find it charming now. What did you do to me?",
                "Remember when you came to look at every single one of my photos? Out loud, even? Swoon.",
                "'Marrying you was my best play.' You said that. I have it memorized. Don't tell anyone.",
                "You proved my whole 'love looks a certain way' theory completely wrong. I'm still mad about it. Lovingly.",
                "That photo I took of you in the sunset light is on the wall, by the way. My favorite one.",
                "You said you'd make me proud. ...You really did, you know.",
                "I gave you SUCH a hard time the first season. You stuck around anyway. Idiot. My idiot.",
                "I caught myself missing you while you were in the next room yesterday. This is your fault.",
                "Remember the staring contest? ...That was Abigail's thing, never mind. WE should do one.",
                "Ugh, you're going to make me say something sappy again, aren't you. ...Fine. I love you." },
            ["Emily"] = new List<string> {
                "Our auras touched before we even spoke. I still feel that first spark when you walk in.",
                "Remember when I made you something and you actually treasured it? My whole spirit lit up.",
                "'Two souls, one current.' I said that on a date and you didn't laugh. That's when I knew.",
                "The dream I had of us dancing on the lake — I'm convinced it was a memory from a future life.",
                "You let me put crystals under your pillow 'for the energy.' You sleep better. I TOLD you.",
                "The colors in this house got warmer the day you moved in. That's not a metaphor to me.",
                "You once told me my aura was glowing. Best compliment I've ever received. Still is.",
                "Remember the night we danced under the stars? The universe arranged that one personally.",
                "I felt your warmth across a crowded room once. I always know when you've entered.",
                "We're cosmically inevitable, you and I. The stars basically signed the paperwork." },
            ["Leah"] = new List<string> {
                "Remember the sculpture I was too scared to show anyone? You were the first. You're always the first.",
                "'You ruin all my seascapes.' You still do. I've stopped trying to paint the ocean without you in it.",
                "You called my art the bravest thing you knew. I think about that on the hard days.",
                "I left a whole life for the quiet. Then you made the quiet feel like the best decision ever.",
                "Remember the salad-and-wine sunset? That's the exact life I dreamed of. With you in it.",
                "You came to my gallery showing in the end. I saw you in the back. I'll never forget it.",
                "I made you part of the masterpiece, you know. You just didn't notice me sketching.",
                "You're the only person I'll let watch me work. That's basically a love confession from an artist.",
                "We drank wine in the clearing and talked till the stars gave up. Best night. One of many.",
                "I'd choose this — the trees, the quiet, you — a thousand times. I keep saying it because I mean it." },
            ["Maru"] = new List<string> {
                "Remember the constellation I named after you? It's still not officially recognized. It's still real to me.",
                "The heartbeat light on that gadget I built you. Too much? Probably. Worth it? Absolutely.",
                "'The numbers were wrong about us.' Statistically improbable, romantically inevitable. My favorite kind of error.",
                "You listened to my whole invention pitch and didn't laugh once. Do you know how rare that is?",
                "I ran the data again last night. Conclusion unchanged: you're the best part of every day.",
                "Remember when you handed me the soldering iron instead of an apology? Most romantic thing ever.",
                "You're the one variable I refuse to solve. Some things are better left a beautiful mystery.",
                "My pulse still does something unscientific when you look at me a certain way. I've stopped trying to fix it.",
                "We're a closed system, you and me. Conservation of love. It only ever increases.",
                "I optimized our whole morning routine just to get four extra minutes with you. Worth every calculation." },
            ["Alex"] = new List<string> {
                "Remember when I let you win that race and then immediately admitted it? Real smooth, me.",
                "You said you see all of me, not just the gridball guy. I think about that more than I let on.",
                "The beach. Me actually talking about my mom. You're still the only one I've ever told.",
                "'Best play I ever made.' That's marrying you, by the way. In case it wasn't obvious.",
                "You make me feel like I don't gotta pretend. That's the whole ballgame, babe.",
                "Remember when I tried the handstand in the sand and ate it completely? Worth it for your laugh.",
                "I used to be scared of the future. Now every version of it's got you in it. You did that.",
                "You watched me train once and actually got into it. Nobody does that. You did.",
                "I still got the grin you make when you celebrate stuck in my head. Drives me crazy. Good crazy.",
                "Tough guy on the outside, total sap for you on the inside. Don't tell the guys." },
            ["Elliott"] = new List<string> {
                "'And in her eyes, the tide and I found home.' You remember the line. I can tell. I treasure that.",
                "Remember when you stayed awake through my entire reading? A rarer gift than you know.",
                "You called me out for writing my heroine in your image. I confessed instantly. I always will.",
                "The crab cakes. You sold them before I could share them. We've never quite let that go, have we?",
                "'You are the whole story.' I meant it as the highest compliment a writer can give. I still do.",
                "We dined with only the tide for company once. Now I dine with you. The sea is jealous, I'm sure.",
                "You asked me to read it again, the embarrassing poem. So I did. For you, always again.",
                "I have filled notebooks trying to capture you and failed gloriously. My favorite failure.",
                "Remember our evening on the pier? The waves keep our secret. I keep the memory.",
                "Every love scene I write now is just plagiarism of us. The critics will never know." },
            ["Harvey"] = new List<string> {
                "Remember when I forgot my whole rehearsed speech the second I looked at you? Still happens, honestly.",
                "You told me 'quiet is just calm.' I've leaned on that more than you realize. Thank you.",
                "The tea. Chamomile. 'A life is mostly small things.' That's still the truest thing I know.",
                "You booked your check-up because I asked. I'm your doctor AND your spouse — that meant the world.",
                "'My heart rate did something I'd flag as concerning in a patient.' It still does. Around you. Daily.",
                "Remember the radio picking up that distant signal? I think about how far some things travel to be heard.",
                "I worried I was too dull for you once. You've spent every day since proving me wrong. Gently.",
                "You watched the planes with me from the roof. Held my hand. I was, ah... quite overcome.",
                "The bravest I ever feel is right after you tell me you love me. It's a documented effect.",
                "I'm not a man of grand gestures. But every day with you, I grow a little braver. You did that." },
            ["Sam"] = new List<string> {
                "Remember the song I wrote you that was mostly just your name with a beat? Still a banger, actually.",
                "The guitar lesson where I completely forgot the chord because you got too close? Worth it.",
                "'Best gig of my life and there wasn't even a crowd.' Our first real date. I still mean it.",
                "You take me seriously even when I'm being a total goofball. That's huge, dude. Huge.",
                "I almost chickened out of kissing you once. SO glad I didn't. Best non-chicken move ever.",
                "Every plan I make, you're just automatically in it. Can't even picture it any other way now.",
                "Remember when I tried to skate to impress you and bailed instantly? You clapped anyway. Hero.",
                "You make me wanna be the grown-up version of myself. And still goof off. Best of both, y'know?",
                "Vincent thinks you're the coolest person alive. ...He's not wrong. Don't tell him I agreed.",
                "I wrote you into a song once and never told you which one. ...Okay, all of them. It's all of them." },
            ["Sebastian"] = new List<string> {
                "Remember when I showed you the playlist? Track seven. ...You know what that one means. I know you do.",
                "'I don't let people in.' I told you that, then I let you all the way in anyway. Still surprises me.",
                "The rain. You didn't run from it, or from me. I think that's the night I knew.",
                "You said you saw me. Past the basement, past the gloom. Nobody else bothered. You did.",
                "We talked about riding the bike somewhere with a black sky and no names. I still want that. With you.",
                "I went quiet on you once for a whole day. You didn't push. You just stayed. That's everything.",
                "'You're the place I was trying to get to.' I don't say stuff like that. I said it to you.",
                "Remember when I actually smiled in front of people because of you? Caused a minor scandal.",
                "Frogger high score's still mine, by the way. But I let you sit close while I beat it. That counts.",
                "I'm bad with words. So just — that thing I said, in the rain. I meant all of it. Still do." },
            ["Shane"] = new List<string> {
                "Remember when you said you were rooting for me? Nobody'd ever said that. I think about it constantly.",
                "The pepper poppers. You actually liked 'em. The one thing I'm good at, and you SAW it.",
                "'Don't let me crawl back in that hole alone.' You never have. Not once. I notice. Every time.",
                "You stuck around when I was at my worst. I don't get why. But I'm never letting it go to waste.",
                "Jas thinks the world of you, y'know. ...So do I. Just takes me longer to say it.",
                "Remember the cliffs at sunset? I used to go there on bad nights. Now I just go there with you.",
                "I sleep better with you around. Said it once, meant it, still hate how sappy it sounded. Still true.",
                "The chickens like you more than they like me. Traitors. ...I get it, though.",
                "You didn't give up on me. People give up on me. You didn't. I keep that close.",
                "I'm bad at this. The feelings stuff. But you make me wanna try. That's — yeah. That's a lot, for me." }
        };
        private static readonly List<string> GenericJokes = new List<string>
        {
            "Remember that argument we actually worked through? Funny how the rough patches end up bringing us closer.",
            "I still can't believe you remembered my favorite thing. You pay more attention than you let on.",
            "We've got our little rhythm now, don't we? I wouldn't trade our ordinary mornings for anything.",
            "You proved me wrong about something important once. I'm still grateful you did.",
            "Every day with you adds another inside joke we'll never explain to anyone else. I love that.",
            "Remember when you made it up to me after that rough day? That's the moment I knew we'd be okay.",
            "We've built a whole private language, you and me. Half a sentence and I know exactly what you mean.",
            "Whatever today throws at us, we've handled worse together. That's the best feeling in the world."
        };
        public static List<string> GetJokes(string name)
        {
            bool has = IsVanilla(name) && Jokes.ContainsKey(name);
            string who = has ? name : "generic";
            List<string> src = has ? Jokes[name] : GenericJokes;
            var outList = new List<string>(src.Count);
            for (int i = 0; i < src.Count; i++)
                outList.Add(I18n.Get($"joke.{who}.{i}", src[i]));
            return outList;
        }

        internal static void CollectJokeDefaults(IDictionary<string, string> map)
        {
            foreach (var kv in Jokes)
                for (int i = 0; i < kv.Value.Count; i++)
                    map[$"joke.{kv.Key}.{i}"] = kv.Value[i];
            for (int i = 0; i < GenericJokes.Count; i++)
                map[$"joke.generic.{i}"] = GenericJokes[i];
        }
    }
}
