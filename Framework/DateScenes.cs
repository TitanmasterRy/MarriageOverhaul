using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>
    /// A handcrafted date-night cutscene: where it is staged and the choreographed beats.
    /// Beats are raw event-command snippets (joined with '/'); "{n}" is replaced with the spouse's name.
    /// Only verified-safe commands are used (speak / faceDirection / emote / pause / jump / shake /
    /// playSound / stopMusic), so scenes always reach their end and warp the player home.
    /// </summary>
    public class DateScene
    {
        public string Id;            // stable identifier, e.g. "abigail_1" or "generic_3" (auto-assigned)
        public string Owner;         // spouse internal name for unique scenes; null for generic
        public string Map;
        public int X;
        public int Y;
        public int Facing = 2;
        public string Weather;       // null = any weather; "Rain" = only plays while raining
        public List<string> Beats;
    }

    /// <summary>The library of vanilla unique date scenes and the generic pool for modded spouses.</summary>
    public static class DateScenes
    {
        // ── helpers ───────────────────────────────────────────────────

        /// <summary>Flatten a mix of single beats and beat-sequences into one list.</summary>
        private static List<string> S(params object[] parts)
        {
            var list = new List<string>();
            foreach (object p in parts)
            {
                if (p is string s) list.Add(s);
                else if (p is IEnumerable<string> seq) list.AddRange(seq);
            }
            return list;
        }

        /// <summary>
        /// The kiss: the couple turns to face each other and shares the in-game kiss (heart + the
        /// "dwop" kiss sound used by the marriage kiss), with the farmer reacting.
        /// </summary>
        private static List<string> Kiss() => new List<string>
        {
            "faceDirection {n} 3", "faceDirection farmer 1", "pause 500",
            // Lean the couple together (pixel offsets, no pathing), share the kiss, then settle back.
            "positionOffset farmer 16 0", "positionOffset {n} -16 0", "pause 150",
            "playSound dwop", "emote {n} 20", "emote farmer 20", "pause 850",
            "positionOffset farmer 0 0", "positionOffset {n} 0 0", "pause 250"
        };

        // ── vanilla unique scenes (3+ each) ───────────────────────────

        private static readonly Dictionary<string, List<DateScene>> Unique = new Dictionary<string, List<DateScene>>
        {
            ["Abigail"] = new List<DateScene>
            {
                new DateScene { Map = "Mountain", X = 54, Y = 7, Facing = 0, Beats = S(
                    "speak {n} \"Right here, at the mouth of the mines... this is where I feel most alive. Well. Second most, lately.\"",
                    "speak {n} \"Most people would call it creepy. The dark, the echoes. I call it ours. Dangerous and exciting, just how I like things.\"",
                    "pause 400",
                    "speak {n} \"You're not scared, standing in the dark with me? ...Good. I was hoping you'd be a little reckless tonight.\"",
                    Kiss(),
                    "speak {n} \"Mm. Forget the mines. Take me home. I've got a far more dangerous idea for how this night ends.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Okay, staring contest. Loser does whatever the winner says. First to blink. Ready? Go.\"",
                    "pause 600",
                    "speak {n} \"...You're really going for it, huh. Stars, your eyes are unfair.\"",
                    "jump {n}",
                    "speak {n} \"Ugh, fine, I blink, I lose. ...Honestly? I just wanted an excuse to look at you this long.\"",
                    Kiss(),
                    "speak {n} \"Don't tell anyone the spooky weird girl is this soft. It'd ruin my whole reputation. ...C'mere.\"") },

                new DateScene { Map = "Town", X = 45, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"When I was a kid I used to sneak out here and wish on the stars. Wished for something that felt like it'd never come.\"",
                    "speak {n} \"Adventure. Someone who'd actually get me. A life that didn't feel so small.\"",
                    "pause 400", "emote {n} 20",
                    "speak {n} \"And now I've got all of it, standing right next to me. ...Okay, that was embarrassingly sincere.\"",
                    "speak {n} \"Grab my hand. Let's go home before I say something even worse. ...Love you, weirdo.\"") }
            },

            ["Penny"] = new List<DateScene>
            {
                new DateScene { Map = "Town", X = 44, Y = 60, Facing = 2, Beats = S(
                    "speak {n} \"I found this passage earlier and couldn't stop thinking about it. Can I read it to you? ...'And he loved her, quietly, the way the shore loves the tide.'\"",
                    "speak {n} \"...Oh, I shouldn't have. That's silly, isn't it. I'm sorry.\"",
                    "pause 500",
                    "speak {n} \"...You want me to finish? Okay. ...'For she always returned to him, and he was always waiting.' ...It reminded me of us.\"",
                    Kiss(),
                    "speak {n} \"I never thought a life like this was meant for someone like me. ...Take me home? I don't want this night to end out here.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"I've been baking all evening. I wanted tonight to be perfect. Don't look at the burnt one, look at THIS one.\"",
                    "speak {n} \"...What? Why are you smiling like— oh no. I've got flour on my face, don't I.\"",
                    "speak {n} \"Don't you dare wipe it off. ...Okay, fine, you can. Gently. ...Your hands are so warm.\"",
                    Kiss(),
                    "speak {n} \"The oven can wait. ...Actually, everything can wait. Come here, you. I've been thinking about this all day.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 20, Facing = 2, Beats = S(
                    "speak {n} \"Look at all this. A whole farm, a home, you. If you'd told the girl in that trailer this was her future, she'd never have believed you.\"",
                    "speak {n} \"I used to be so afraid of wanting things. Wanting felt dangerous. But you... I let myself want you, and you stayed.\"",
                    "pause 700",
                    "speak {n} \"...You always know what to say without saying anything at all. Come here. Let me just lean on you a while.\"",
                    "emote {n} 20",
                    "speak {n} \"This. Right here. This is everything I was too scared to even ask for.\"") }
            },

            ["Haley"] = new List<DateScene>
            {
                new DateScene { Map = "Beach", X = 38, Y = 19, Facing = 2, Beats = S(
                    "speak {n} \"Look at that sunset. If I had my camera I'd take a picture of you in this light. ...Maybe I'll just keep this one in my head instead.\"",
                    "speak {n} \"You know what's better than a perfect photo? Not having to share you with anyone who'd look at it.\"",
                    "speak {n} \"Stop being so handsome for one second, I'm trying to make a point. ...Actually. Don't.\"",
                    Kiss(),
                    "speak {n} \"Mm. Been wanting to do that since sundown. ...Take me home, gorgeous. The view back there's better anyway.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"You tracked mud all across my clean floor and... ugh, why is that working for me right now. I used to HATE this.\"",
                    "speak {n} \"Don't tell anyone, but I think I actually love it. This little house. The mess. You smelling faintly like hay.\"",
                    "speak {n} \"I had it all wrong before you. I thought I wanted perfect. Turns out I just wanted real. I wanted you.\"",
                    Kiss(),
                    "speak {n} \"...Okay, that was almost romantic. Quick, kiss me again before I ruin it with a joke.\"") },

                new DateScene { Map = "Town", X = 45, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"I used to think love was supposed to look a certain way. Picture-perfect. Posed. Like a magazine.\"",
                    "speak {n} \"Then you came along and blew all of it up. Messy and real and so much better than anything I imagined.\"",
                    "speak {n} \"I don't want the magazine version anymore. I want this. I want you. ...Don't make that face, you'll make me cry my mascara off.\"",
                    Kiss(),
                    "speak {n} \"...Wow. Okay. Forget the square, forget everyone. Take me home. I'm not nearly finished with you tonight.\"") }
            },

            ["Emily"] = new List<DateScene>
            {
                new DateScene { Map = "Town", X = 45, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"Can you feel it? The air is practically singing tonight. The universe is paying attention to us.\"",
                    "speak {n} \"I felt your energy before I ever knew your name, you know. A warm thread, pulling toward me. I just had to wait for you to find it too.\"",
                    "speak {n} \"Give me your hands. Both of them. ...There. Two souls, one current. I knew it the moment our auras first touched.\"",
                    "emote {n} 20", "pause 500",
                    "speak {n} \"Stay close to me tonight. I want to feel your warmth against the whole rest of the world.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"I had a dream about you. We were dancing on the surface of a still black lake, and the stars were dancing with us.\"",
                    "speak {n} \"You pulled me close and whispered something I couldn't quite hear. But I woke up feeling it in my whole body.\"",
                    "speak {n} \"I think the dream was a promise. Of tonight. Of every night. ...The universe has wonderful taste.\"",
                    Kiss(),
                    "speak {n} \"Mm. Reality's even better than the dream. Come, let's go home and chase it a little further.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"I made you something. I've been stitching it for weeks, by candlelight, thinking of you with every thread.\"",
                    "speak {n} \"It's not finished, but I wanted you to have a piece of me you could wrap around yourself when I'm not near.\"",
                    "speak {n} \"...You're looking at me like that again. The look that makes my whole aura blush.\"",
                    Kiss(),
                    "speak {n} \"Put it down for now. The sewing can wait. Tonight, I'd much rather wrap you up myself.\"") }
            },

            ["Leah"] = new List<DateScene>
            {
                new DateScene { Map = "Forest", X = 50, Y = 40, Facing = 2, Beats = S(
                    "speak {n} \"I made something. A sculpture. I've never shown it to anyone. I couldn't. It's too... honest.\"",
                    "speak {n} \"It's you. Or, the way you make me feel. All warmth and open hands. I was scared to even carve it.\"",
                    "pause 700",
                    "speak {n} \"...You really mean that. I can tell. God, you make it so easy to be brave.\"",
                    Kiss(),
                    "speak {n} \"Come back to mine. I want to show you the rest of what I've been too scared to share.\"") },

                new DateScene { Map = "Beach", X = 35, Y = 19, Facing = 2, Beats = S(
                    "speak {n} \"See how the light sits on the water? I've tried to paint that a hundred times. It never comes out right.\"",
                    "speak {n} \"Want to know why? Because every time I sit down to paint it, I end up looking at you instead. You ruin all my seascapes.\"",
                    "speak {n} \"I'm not even mad about it. You're the better view. By far.\"",
                    Kiss(),
                    "speak {n} \"Forget the painting. Let's go home, and you can ruin the rest of my evening too. ...Please do.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"I left a whole life for this. The city, the gallery scene, someone who never really saw me. Everyone said I was crazy.\"",
                    "speak {n} \"And I'd do it again. In a heartbeat. Every single time, I'd choose this. The quiet. The trees. You.\"",
                    "emote {n} 20",
                    "speak {n} \"You're the masterpiece I didn't know I was working toward my whole life.\"",
                    "pause 500",
                    "speak {n} \"Come here. Just... let me hold onto this for a while. Onto you.\"") }
            },

            ["Maru"] = new List<DateScene>
            {
                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Okay, see those stars there? I, um, I named that constellation. After you. It's not officially recognized, obviously, but—\"",
                    "speak {n} \"The luminosity ratio reminded me of— okay that's not the point. The point is I look up and I see you. Everywhere. It's a documented problem.\"",
                    "speak {n} \"...I'm rambling. I always ramble when you look at me like that. My pulse is doing something deeply unscientific.\"",
                    Kiss(),
                    "speak {n} \"...Huh. That steadied my heart rate considerably. Fascinating. Let's go home and run the experiment again. Repeatedly.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"I'm building something for you! It monitors the greenhouse AND plays your favorite song when you walk in and—\"",
                    "speak {n} \"I may have over-engineered it. I added a little light that pulses like a heartbeat. Mine, specifically. That's probably too much, isn't it.\"",
                    "speak {n} \"I just want to take care of you, in every way I know how. Which is mostly gadgets and, apparently, oversharing.\"",
                    Kiss(),
                    "speak {n} \"...The prototype can wait until morning. You, however, cannot. Come here, genius.\"") },

                new DateScene { Map = "Town", X = 45, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"Do you know the actual odds we'd ever meet? I calculated them once. They're absurd. Vanishingly small.\"",
                    "speak {n} \"Two specific people, in a valley this size, at exactly the right moment in both our lives. Statistically, we shouldn't exist.\"",
                    "speak {n} \"And yet, here you are. Improbable. Impossible. Mine. I love that the numbers were so wrong about us.\"",
                    "emote {n} 20",
                    "speak {n} \"I'm very, very glad it all worked out. Now take my hand and walk this improbable girl home.\"") }
            },

            ["Alex"] = new List<DateScene>
            {
                new DateScene { Map = "Beach", X = 41, Y = 19, Facing = 2, Beats = S(
                    "speak {n} \"Mind if we just sit a minute? ...I don't do this much. The talking thing. The real talking.\"",
                    "speak {n} \"My mom used to bring me here when I was a kid. Before. ...You're the first person I've ever told that without making a joke right after.\"",
                    "speak {n} \"You make me feel like I don't have to be the tough guy. Like I can just be... me. You know how rare that is for me?\"",
                    Kiss(),
                    "speak {n} \"...C'mere. I don't want to share you with the ocean anymore. Let's go home. Just you and me.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Bet I can beat you to the gate and back. Loser cooks breakfast. ...Okay, GO!\"",
                    "pause 600", "jump {n}",
                    "speak {n} \"...Huh. You beat me. Me. The gridball star. ...Okay fine. I MIGHT have let you win. Maybe. Definitely.\"",
                    "speak {n} \"What can I say, I like watching you celebrate. You get this grin. Drives me crazy. The good kind.\"",
                    Kiss(),
                    "speak {n} \"Best loss of my life. C'mon, champ. Let's go home and you can collect the rest of your prize.\"") },

                new DateScene { Map = "Town", X = 45, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"I used to be scared of the future, y'know? Like it was this big dark thing coming for me. Didn't know who I'd even be.\"",
                    "speak {n} \"Now? I think about it and I just get... excited. 'Cause every version of it has you in it. Us. A life.\"",
                    "speak {n} \"I'm gonna build something good. For us. I mean it. You make me wanna be the guy who deserves you.\"",
                    "emote {n} 20",
                    "speak {n} \"...Come here. Let's go home. I just wanna hold you and think about all of it.\"") }
            },

            ["Elliott"] = new List<DateScene>
            {
                new DateScene { Map = "Beach", X = 38, Y = 19, Facing = 2, Beats = S(
                    "speak {n} \"I wrote you something. I've carried it for days, working up the nerve. ...'You are the harbor and the open sea, the anchor and the wind that sets me free—'\"",
                    "speak {n} \"...Oh, this is mortifying. Aloud it sounds so— forgive me, perhaps the page was its better home.\"",
                    "pause 600",
                    "speak {n} \"...You truly want me to go on? ...'And every tide of mine, henceforth, returns to only thee.' ...There. My whole heart, in ink.\"",
                    Kiss(),
                    "speak {n} \"Let the waves keep our secret. Come, my love. The night, like the poem, is far from finished.\"") },

                new DateScene { Map = "Beach", X = 35, Y = 19, Facing = 2, Beats = S(
                    "speak {n} \"Picture it: a sky bleeding amber into violet, the sea applauding softly, and there, the most beautiful soul in the valley, looking at me as though I hung the very sun.\"",
                    "speak {n} \"I could write a thousand pages and never capture this. The way you make an ordinary evening feel like the climax of the greatest love story ever told.\"",
                    "speak {n} \"So allow me to abandon the metaphor and simply say it plainly, for once. I am hopelessly, completely, devastatingly in love with you.\"",
                    Kiss(),
                    "speak {n} \"...The story does not end here, my darling. It merely moves somewhere more private. Take my hand. Let us write the next chapter at home.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"Allow me a passage from my novel. The heroine — wholly fictional, of course — has eyes that 'undo a man with a single glance.'\"",
                    "speak {n} \"She is clever, and kind, and maddeningly beautiful, and the protagonist would burn the world for one more evening at her side. Purely invented, naturally.\"",
                    "speak {n} \"...You're smiling. You know, don't you. ...Yes. Fine. It's you. It has always been you. Every heroine I've ever written wears your face.\"",
                    "emote {n} 20",
                    "speak {n} \"I am a writer of fictions, my love, but you are the one true thing in all of it. ...Come, sit closer. The author requires his muse.\"") }
            },

            ["Harvey"] = new List<DateScene>
            {
                new DateScene { Map = "Town", X = 50, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"I picked up the most remarkable signal on my radio tonight. Music, from somewhere far away. Faint, but lovely. It made me think of distance. Of how far some things travel to be heard.\"",
                    "speak {n} \"I worry, sometimes, that I'm too... quiet. Too cautious. That someone like you deserves a far more exciting man than a nervous country doctor.\"",
                    "pause 600", "emote {n} 32",
                    "speak {n} \"...You really mean that. Oh. I— well. My heart rate just did something I'd flag as concerning in a patient.\"",
                    Kiss(),
                    "speak {n} \"...Goodness. Come home with me, would you? I find I don't want this evening examined by anyone but us.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Look how much this place has grown since I first walked it with you. It's a bit like us, isn't it. Tended. Patient. Flourishing.\"",
                    "speak {n} \"I'm not a man of grand gestures. But every day with you, something in me grows a little braver. A little more whole.\"",
                    "speak {n} \"I love you. Quietly, completely, and more every season. I hope the smallness of how I say it never hides the size of how I mean it.\"",
                    Kiss(),
                    "speak {n} \"...There's nothing small about that, is there. Come. Let's go home, my love.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"I made us some tea. Chamomile. A small thing, I know. But I've come to believe a life is mostly small things, stacked gently atop one another.\"",
                    "speak {n} \"A cup of tea. A shared silence. Your hand finding mine without either of us looking. I'd never trade these for any grand adventure.\"",
                    "speak {n} \"...Although. I'll confess the moment I'm most looking forward to tonight isn't the tea at all.\"",
                    Kiss(),
                    "speak {n} \"...Oh my. I can't believe I said that out loud. ...I don't regret it. Let the tea go cold. Come here.\"") }
            },

            ["Sam"] = new List<DateScene>
            {
                new DateScene { Map = "Town", X = 43, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"Okay don't laugh, I wrote you a song. It goes — 'You're the beat I didn't know my heart was missing—' okay it rhymes WAY better with the guitar, I swear.\"",
                    "speak {n} \"...But for real. Underneath all the goofing around? I meant every word. You're my favorite song I've ever written. By a mile.\"",
                    "speak {n} \"Nobody ever made me wanna be serious about anything. You make me wanna be serious about everything. About us.\"",
                    Kiss(),
                    "speak {n} \"...Man. Best gig of my life and there wasn't even a crowd. C'mon, let's go home, superstar.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Here, lemme show you this chord. Put your fingers — no, here — okay you gotta press kinda hard, like, here, I'll just—\"",
                    "speak {n} \"...Whoa. Uh. You're really close right now. I forgot what I was— what were we— guitar. Right. The guitar thing.\"",
                    "speak {n} \"Okay I'm just gonna say it before I lose my nerve — I've been wanting to kiss you this whole— y'know what, forget the nerve—\"",
                    Kiss(),
                    "speak {n} \"...Whoa. Okay. WAY better than the chord. Forget the lesson. Let's go home. I've got way better ideas.\"") },

                new DateScene { Map = "Town", X = 46, Y = 57, Facing = 2, Beats = S(
                    "speak {n} \"Y'know what I want? Like, really want? A life that's loud in all the good ways. Music, a home, and you in every room of it.\"",
                    "speak {n} \"Every plan I make now, you're just... in it. Automatically. I can't even picture a future that isn't us anymore. Don't want to.\"",
                    "speak {n} \"I used to be such a mess about the future. You make it feel like the best part instead of the scary part.\"",
                    "emote {n} 20",
                    "speak {n} \"...C'mere. It's getting late, and I really, really don't wanna say goodnight out here. Let's go home.\"") }
            },

            ["Sebastian"] = new List<DateScene>
            {
                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Weather = "Rain", Beats = S(
                    "stopMusic",
                    "speak {n} \"...\"",
                    "pause 800",
                    "speak {n} \"...You don't mind the rain? Most people run from it. I always liked it. Feels like the world finally matches the quiet in my head.\"",
                    "speak {n} \"I spent a lot of my life feeling invisible. Like I was watching everything through a window. Then you... you actually looked. You saw me.\"",
                    "pause 500",
                    "speak {n} \"I don't say this. Ever. So listen. ...I love you. More than I know how to put into words, and I hate not having the words.\"",
                    Kiss(),
                    "speak {n} \"...Yeah. C'mon. Let's get out of the rain. I don't wanna share you with the storm tonight.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"I, uh... made you something. A playlist. Every song reminds me of you somehow. Took weeks to get the order right. Don't make it weird.\"",
                    "speak {n} \"Track seven's the one. Listen to it sometime when I'm not around and you miss me. ...Not that I'm assuming you'd miss me.\"",
                    "speak {n} \"...You're looking at me like that's the most romantic thing anyone's ever done. It's just a playlist. ...Okay. Maybe it's not just a playlist.\"",
                    Kiss(),
                    "speak {n} \"...Stay close tonight. The playlist can wait. I'd rather make some new memories worth writing songs about.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Some nights I just wanna get on the bike and ride until the valley's a dot behind us. Somewhere with a black sky and no one who knows our names.\"",
                    "speak {n} \"Just you on the back, holding on, the whole open road ahead. ...I think about that a lot, actually. Taking you somewhere that's only ours.\"",
                    "speak {n} \"But honestly? Anywhere's far enough, as long as it's with you. You're the place I was trying to get to this whole time.\"",
                    Kiss(),
                    "speak {n} \"...Come on. Let's go home. The road'll still be there tomorrow. You, I want tonight.\"") }
            },

            ["Shane"] = new List<DateScene>
            {
                new DateScene { Map = "Forest", X = 50, Y = 40, Facing = 2, Beats = S(
                    "speak {n} \"...The animals get quiet this time of night. Peaceful. I come out here when my head gets loud. Helps.\"",
                    "speak {n} \"I've been doing better. You know that. Some days are still hard, but... fewer of 'em. A lot fewer, since you.\"",
                    "pause 700",
                    "speak {n} \"...You really don't give up on me, do you. Even when I make it tough. I don't know what I did to deserve that. But I'm not gonna waste it.\"",
                    Kiss(),
                    "speak {n} \"...C'mon. Let's go home. I sleep better with you there. ...Yeah. I said it. Don't make it a thing.\"") },

                new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                    "speak {n} \"So I, uh, cooked. For you. Don't get excited, it might be terrible. ...Okay it's hot pepper stuff, the one thing I'm good at, just— try it.\"",
                    "speak {n} \"...You like it? Don't lie to me. ...You actually like it. Huh. Okay. That means more than it should, honestly.\"",
                    "speak {n} \"Nobody's ever let me take care of 'em before. Feels good. Feels right, doing it for you.\"",
                    "speak {n} \"...You know what, forget dinner. It'll keep. ...I'd rather it just be us right now. That alright with you?\"",
                    Kiss(),
                    "speak {n} \"...Didn't plan to do that. Don't regret it either. Come here.\"") },

                new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                    "speak {n} \"Funny thing — takin' care of those chickens every day? First time in my life somethin' needed me and I didn't let it down. Changed me.\"",
                    "speak {n} \"Then you came along and did the same thing. Needed me. Trusted me. And somehow I didn't screw it up.\"",
                    "speak {n} \"I was in a real dark place once. Didn't think I'd get this. A home. A reason. You. ...I don't take a second of it for granted.\"",
                    "emote {n} 20",
                    "speak {n} \"...Come here. Just let me hold onto you a minute. This is the part I never thought I'd get.\"") }
            }
        };

        // ── generic pool (15) for modded spouses / vanilla after unique are seen ──

        private static readonly List<DateScene> Generic = new List<DateScene>
        {
            // 1 — farmhouse, table, flirty, kiss
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"Just us, the quiet, and a whole evening with nowhere to be. ...I've been looking forward to this all week.\"",
                "speak {n} \"Come sit closer. ...Closer than that. There. Now I can actually see those eyes I married.\"",
                Kiss(),
                "speak {n} \"Mm. Hold that thought. ...Actually, don't hold it. Let's take it somewhere more comfortable.\"") },

            // 2 — farm crops, sincere
            new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                "speak {n} \"Look at all of it. Everything we've grown here, side by side. Dirt under our nails and a whole life out of it.\"",
                "speak {n} \"This farm is really us, you know? Patient. Stubborn. Alive. I'm proud of what we're building.\"",
                "emote {n} 20",
                "speak {n} \"And I'm proud I get to build it with you. Every season. Come here, let me just stand with you a while.\"") },

            // 3 — beach dock, future, close
            new DateScene { Map = "Beach", X = 38, Y = 19, Facing = 2, Beats = S(
                "speak {n} \"Let's just sit out here a minute. The water's so calm tonight. ...Feels like the whole future's out there, past the horizon.\"",
                "speak {n} \"Wherever it goes, whatever it looks like, I want all of it with you. The calm parts and the storms both.\"",
                "speak {n} \"...Lean on me. There. I could stay like this till the tide comes in.\"") },

            // 4 — farm field, stars, kiss
            new DateScene { Map = "Farm", X = 64, Y = 18, Facing = 2, Beats = S(
                "speak {n} \"Lie back with me and look up. ...The stars are showing off tonight. Almost as much as you.\"",
                "speak {n} \"You ever think how we're under the same sky we were the night we got married? ...Sap. I know. You make me sappy.\"",
                Kiss(),
                "speak {n} \"...Forget the stars. Best view in the valley's right next to me. Let's go home.\"") },

            // 5 — farmhouse kitchen, playful, suggestive
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"Okay, you stir, I'll season. ...No, like THIS. Here, let me— oh, now you've got it everywhere. Menace.\"",
                "speak {n} \"...You're a disaster in the kitchen and I adore you for it. Get over here.\"",
                Kiss(),
                "speak {n} \"...You know what, dinner can absolutely wait. ...Turn off the stove. I've got other plans for the evening.\"") },

            // 6 — farmhouse living, cards, warm
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"Best two out of three. Loser owes the winner a favor. ...And no, I'm not telling you what favor yet.\"",
                "speak {n} \"...Okay, you're terrible at this and I love it. You make the same face every time you bluff.\"",
                "speak {n} \"...Forget the cards. I'd rather sit here and talk to you till the candle burns out. This is my favorite thing.\"") },

            // 7 — town night walk, nostalgic
            new DateScene { Map = "Town", X = 45, Y = 57, Facing = 2, Beats = S(
                "speak {n} \"The whole town to ourselves at this hour. Quiet, isn't it? ...I like seeing it like this. Just us and the lamplight.\"",
                "speak {n} \"We've walked these streets a hundred times. Feels different tonight. Softer. ...Maybe that's just you.\"",
                "speak {n} \"Walk slow with me. I'm in no hurry to be anywhere that isn't right beside you.\"") },

            // 8 — saloon exterior, kiss
            new DateScene { Map = "Town", X = 43, Y = 57, Facing = 2, Beats = S(
                "speak {n} \"Everyone's gone home. Just the hum of the sign and the crickets. ...I like the quiet after the noise. Especially with you in it.\"",
                "speak {n} \"No crowd, no music, nowhere to be. Just my favorite person under a streetlight. Pretty perfect, honestly.\"",
                Kiss(),
                "speak {n} \"...C'mon. Let's take this somewhere warmer. The night's still young, and so are we.\"") },

            // 9 — farm sunrise, tired-happy
            new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                "speak {n} \"...Is that the sun? Did we seriously talk the whole night through again? ...I don't even care. Worth it.\"",
                "speak {n} \"There's no one else I'd lose a night's sleep for. You make three in the morning feel like the best part of the day.\"",
                "speak {n} \"...Come here. Let's watch the sun come up before we crash. One more minute. Just us and the light.\"") },

            // 10 — farmhouse, dance, kiss, suggestive
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"There's no music, but dance with me anyway. ...Come on. One hand here, one here. ...See? We don't need a song.\"",
                "speak {n} \"Just sway with me. Slow. ...This is the most at home I've ever felt. Right here, in your arms.\"",
                Kiss(),
                "speak {n} \"...Why stop at dancing? Come on. The night's ours. Let's not waste a second of it.\"") },

            // 11 — farmhouse, letters, emotional
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"I kept all the letters, you know. Every one. ...Listen to this. 'I think about you when the work gets hard, and it gets easy again.'\"",
                "speak {n} \"You wrote that our first season. I've read it so many times the ink's going soft. You meant it then. I feel it more now.\"",
                "speak {n} \"...Read me yours. The ones I sent you. I want to hear my own words in your voice, and then never let go of tonight.\"") },

            // 12 — farm, fire, marshmallows, kiss
            new DateScene { Map = "Farm", X = 64, Y = 16, Facing = 2, Beats = S(
                "speak {n} \"Careful, yours is on fire. ...That's how you like 'em? Burnt to a crisp? You're a menace, and I married you anyway.\"",
                "speak {n} \"...It's nice though, isn't it. The fire, the dark, the quiet. You. I don't need much more than this.\"",
                "speak {n} \"Actually — I don't need anything more than this. Just you, lit up by the firelight. Come here.\"",
                Kiss()) },

            // 13 — farmhouse, charged quiet, kiss, suggestive
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"...It got real quiet in here all of a sudden, didn't it. ...The good kind of quiet. The kind where I can hear my own heartbeat.\"",
                "speak {n} \"You're looking at me. I'm looking at you. ...I don't think either of us is thinking about the evening winding down.\"",
                Kiss(),
                "speak {n} \"...No. Definitely not winding down. Come here. The rest of the night is just for us.\"") },

            // 14 — forest path, peaceful
            new DateScene { Map = "Forest", X = 50, Y = 40, Facing = 2, Beats = S(
                "speak {n} \"Slow down, walk with me. ...Look — I've passed that little stream a hundred times and never noticed how it catches the moonlight.\"",
                "speak {n} \"Funny how you start seeing everything differently when you're happy. The whole forest looks softer with you in it.\"",
                "speak {n} \"...Let's just wander a while. No destination. I've got everywhere I need to be right here next to me.\"") },

            // 15 — farmhouse, book argument, kiss, suggestive
            new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = S(
                "speak {n} \"It is NOT the best book ever written, and I'll defend that to my last breath. ...Why are you smiling at me like that.\"",
                "speak {n} \"...Stop it. I'm trying to argue with you and you're being all— ugh. You're impossible. And distractingly cute. It's unfair.\"",
                Kiss(),
                "speak {n} \"...We are absolutely not finishing this argument tonight. ...Come here. I've got a much better use for the evening.\"") }
        };

        // ── ids ───────────────────────────────────────────────────────

        /// <summary>Assign a stable id (and owner) to every scene: "abigail_1", "generic_3", etc.</summary>
        static DateScenes()
        {
            foreach (var kv in Unique)
            {
                for (int i = 0; i < kv.Value.Count; i++)
                {
                    kv.Value[i].Id = kv.Key.ToLowerInvariant() + "_" + (i + 1);
                    kv.Value[i].Owner = kv.Key;
                }
            }
            for (int i = 0; i < Generic.Count; i++)
                Generic[i].Id = "generic_" + (i + 1);
        }

        /// <summary>Find a scene by its id (case-insensitive), searching unique scenes then the generic pool.</summary>
        public static DateScene GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            id = id.ToLowerInvariant();
            foreach (var kv in Unique)
                foreach (var scene in kv.Value)
                    if (scene.Id == id)
                        return scene;
            foreach (var scene in Generic)
                if (scene.Id == id)
                    return scene;
            return null;
        }

        /// <summary>Every scene's (id, owner, map) for listing in the console.</summary>
        public static IEnumerable<(string Id, string Owner, string Map)> AllSceneInfo()
        {
            foreach (var kv in Unique)
                foreach (var scene in kv.Value)
                    yield return (scene.Id, scene.Owner, scene.Map);
            foreach (var scene in Generic)
                yield return (scene.Id, "(generic)", scene.Map);
        }

        // ── accessors ─────────────────────────────────────────────────

        public static bool HasUnique(string name) => name != null && Unique.ContainsKey(name);

        public static List<DateScene> GetUnique(string name)
            => name != null && Unique.TryGetValue(name, out var list) ? list : null;

        public static int UniqueCount(string name) => GetUnique(name)?.Count ?? 0;

        public static IReadOnlyList<DateScene> GenericPool => Generic;

        public static int GenericCount => Generic.Count;
    }
}
