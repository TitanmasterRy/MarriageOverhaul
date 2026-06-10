using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class SpouseContent
    {
        /// <summary>
        /// Friendship-driven baseline tone for the morning greeting. The mood system shifts this up
        /// (Happy) or down (Grumpy) by one tier for day-to-day variance; see MorningDialogueSystem.
        /// </summary>
        public enum MorningTier
        {
            VeryLow = 0,  // cold, distant, resentful — short and clipped
            Low = 1,      // flat, polite but disengaged
            High = 2,     // warm and friendly
            VeryHigh = 3  // affectionate / loving
        }

        // ─────────────────────────────────────────────────────────────
        //  TIERED MORNING GREETINGS (per spouse, per friendship tier)
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, Dictionary<MorningTier, List<string>>> MorningLines =
            new Dictionary<string, Dictionary<MorningTier, List<string>>>
        {
            ["Abigail"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Oh. You're up. Whatever.",
                    "Morning. Don't expect me to be all cheery about it.",
                    "...Yeah. Hi. I've got nothing to say to you right now.",
                    "You're still here, huh. Great." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I'm gonna do my own thing today.",
                    "Hey. Sleep okay? ...Cool. Anyway.",
                    "Morning. I might head out for a walk later. Alone.",
                    "Oh, morning. I was just messing with my game." },
                [MorningTier.High] = new List<string> {
                    "Hey, you! Ready to take on the day? I kinda am.",
                    "Morning! I was up late reading about cave monsters. Don't judge.",
                    "There's my favorite person. Mornings are better with you around.",
                    "Hey! Wanna sneak off on an adventure later? ...A small one. Maybe." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning, you! I had the best dream about us raiding a dungeon side by side. Let's make today an adventure.",
                    "Ugh, I love waking up next to you. Don't tell anyone I got this sappy, okay? It's our secret.",
                    "Hey, my partner in crime. Whatever chaos today brings, we handle it together. Deal?",
                    "I lay here for ten minutes just watching you sleep. Creepy? Maybe. Worth it? Totally." }
            },
            ["Penny"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "...Good morning. I'd rather not talk right now.",
                    "Oh. Morning. I'll keep out of your way.",
                    "Morning. ...Never mind. It doesn't matter.",
                    "Hello. I didn't sleep well. I won't bother you." },
                [MorningTier.Low] = new List<string> {
                    "Good morning. I hope your day goes smoothly.",
                    "Morning. I'll tidy up around here while you're out.",
                    "Oh, you're awake. I made tea, if you want some.",
                    "Morning. I've got some reading to catch up on today." },
                [MorningTier.High] = new List<string> {
                    "Good morning, dear. It's so nice to start the day with you.",
                    "Morning! I put the kettle on already. I know how you like it.",
                    "There you are. The house always feels brighter once you're up.",
                    "Good morning. I was just thinking how lucky I am to be here." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Good morning, sweetheart. Waking up beside you makes everything in the world feel right.",
                    "I used to dream of a home like this. Quiet, safe, full of love. You gave me that. Thank you.",
                    "Morning, my love. Come find me before you head out — even a moment with you sets my whole day right.",
                    "I watched the sunrise from the window and all I could think about was how grateful I am for you." }
            },
            ["Haley"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Ugh. Morning. Don't even look at me right now.",
                    "Oh, it's you. Great. Just... give me space.",
                    "Morning. I'm not in the mood, okay?",
                    "Whatever. I have nothing to say to you." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I've got photos to sort, so... yeah.",
                    "Oh, hi. Try not to track mud in today, please.",
                    "Morning. I might go take some pictures later.",
                    "Hey. You look tired. ...Just saying." },
                [MorningTier.High] = new List<string> {
                    "Morning, gorgeous. You actually look pretty good today, you know.",
                    "Hey you. I'm in a good mood, so enjoy it while it lasts.",
                    "Morning! I was thinking I'd take some photos of the farm. Of us, maybe.",
                    "There you are. Mornings used to be boring. Not anymore." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning, gorgeous. Life's actually pretty perfect right now. Don't tell anyone I admitted it.",
                    "I took a photo of you sleeping. You looked so sweet I couldn't help it. It's MINE, you can't have it.",
                    "Ugh, I love you. There, I said it first thing in the morning. That's how you know I mean it.",
                    "Come here and kiss me before you go play farmer. I need my morning dose of you." }
            },
            ["Emily"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Morning. My aura's all tangled. I need space.",
                    "Oh. You're up. The energy in here feels... off.",
                    "...Morning. I can't read you lately. I'm not sure I want to.",
                    "Hello. I'll be meditating. Alone." },
                [MorningTier.Low] = new List<string> {
                    "Morning. The crystals say it'll be an ordinary sort of day.",
                    "Oh, hello. I had odd dreams. Nothing to share, really.",
                    "Good morning. I'll be sewing something today, I think.",
                    "Morning. The colors feel muted today. It happens." },
                [MorningTier.High] = new List<string> {
                    "Good morning! Your aura's lovely and bright today. It's contagious.",
                    "Morning, you. The whole house hums with good energy when you're near.",
                    "Hello, love. I dreamt in warm colors last night. I think it was about you.",
                    "Morning! The spirits are gentle today. A good omen for us." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Mmm, your aura is glowing this morning, and so is mine. Our energies are so woven I can hardly tell them apart.",
                    "I felt the universe align the moment I woke beside you. That hum? That's us, my love.",
                    "Good morning, beloved. The light pouring in this morning is nothing next to you.",
                    "I danced a little while you slept. The spirits insisted. They're happy for us, you know." }
            },
            ["Leah"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Morning. ...Yeah. Just leave me to my work.",
                    "Oh. You're up. I'm busy.",
                    "Morning. I'm not really feeling talkative.",
                    "Hey. ...I've got nothing for you today." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I'll be out at the studio most of the day.",
                    "Oh, hey. Sleep alright? I'm gonna get to work.",
                    "Morning. The light's good today — gonna sketch a while.",
                    "Hey you. Coffee's on, if you want it." },
                [MorningTier.High] = new List<string> {
                    "Morning, you. I woke up wanting to sculpt something. Funny how that happens around you.",
                    "Hey. The farm looks beautiful in this light. So do you, honestly.",
                    "Morning! Come by the studio later if you get a minute. I'd like the company.",
                    "There you are. Best part of my morning, seeing you stumble out of bed." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning, you. I woke up inspired — might sculpt something just from thinking about us. Sappy, I know.",
                    "I left a whole life behind to find something real. I found it. It's you, it's here, it's this.",
                    "Come here. Five minutes, just us, before the day swallows us up. I want to remember this morning.",
                    "I'd choose this life — you, this farm, these quiet mornings — every single time, without thinking twice." }
            },
            ["Maru"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Oh. You're up. I've got a lot on my mind, so... later.",
                    "Morning. I'd rather focus on my work right now.",
                    "...Hi. I'm not in a talking mood. Sorry.",
                    "Morning. Let's just get through the day, okay?" },
                [MorningTier.Low] = new List<string> {
                    "Morning. I've got a project I want to tinker with today.",
                    "Oh, hello. Did you sleep enough? Statistically you don't.",
                    "Morning. I'll be at the workbench if you need me.",
                    "Hey. Coffee's optimal temperature right now. Just so you know." },
                [MorningTier.High] = new List<string> {
                    "Good morning! I ran some quick math — you're a solid highlight of my day.",
                    "Morning, you. I had an idea for an invention while watching you sleep. Don't ask.",
                    "Hey! The data's clear: mornings are better with you in them.",
                    "Morning. Come find me later — I work better knowing you're around." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Good morning! I ran the numbers and you're statistically the best part of my life. Every day. Don't question the data.",
                    "You're the one variable I could never quantify, and I've stopped trying. I just love you. Completely.",
                    "Morning, my love. I lay awake calculating how lucky I am. The result kept overflowing.",
                    "Come here before you go. A kiss measurably improves my entire day. It's science. Indulge me." }
            },
            ["Alex"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Yeah. Morning. Not really feelin' it today.",
                    "Oh. You're up. Don't take it personal, I just don't wanna talk.",
                    "Morning. ...Whatever. I'll be outside.",
                    "Hey. I got nothing to say right now." },
                [MorningTier.Low] = new List<string> {
                    "Morning. Gonna get a workout in, I think.",
                    "Oh, hey. Sleep good? I'm gonna toss the gridball around.",
                    "Morning. Got a lot of energy to burn off today.",
                    "Hey you. Tryin' to wake up. Gimme a sec." },
                [MorningTier.High] = new List<string> {
                    "Hey, babe! Feelin' pretty good today. Havin' you here helps, y'know?",
                    "Morning! You sleep okay? I always sleep better with you next to me.",
                    "There's my favorite person. C'mon, let's make today a good one.",
                    "Mornin'. I was thinkin' about you before I even opened my eyes. Weird, huh? Good weird." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Hey, babe! I'm feelin' on top of the world today, and it's all 'cause of you. Every single day.",
                    "I used to put on this whole tough-guy act. With you? I just get to be me. Best feeling there is.",
                    "Morning, gorgeous. C'mere. A day that starts with you is a day I already won.",
                    "I watched you sleep for a sec and got all mushy about it. You make me soft, you know that? I love it." }
            },
            ["Elliott"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Good morning. Forgive my brevity. The warmth simply isn't in me today.",
                    "Ah. You've risen. I'll not trouble you with words.",
                    "Morning. ...The page is blank, and so, it seems, am I.",
                    "A cold sort of morning. I'd rather be left to it." },
                [MorningTier.Low] = new List<string> {
                    "Good morning. I shall be at my writing for much of the day.",
                    "Ah, awake. The sea calls; I may walk its edge a while.",
                    "Morning. A quiet one, I think. The muse keeps her distance.",
                    "Good day to you. I've tea steeping, should you wish some." },
                [MorningTier.High] = new List<string> {
                    "Good morning. I woke with half a verse in mind — you were in it, naturally.",
                    "Ah, there you are. A fine morning made finer by your company.",
                    "Morning, my dear. The light on the water is lovely, though it pales beside you.",
                    "Good morning. Stay a moment — your presence is its own kind of poetry." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Ah, good morning, my muse. I woke with poetry in my heart and your name upon my lips.",
                    "The greatest story I shall ever write is the one I live each morning, waking beside you.",
                    "Come, sit with me a moment before the day claims you. Let me memorize you in this light.",
                    "I have filled pages trying to capture you and failed beautifully every time. What joy, to keep failing forever." }
            },
            ["Harvey"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Oh. Good morning. I'm, ah, not quite myself. Best give me room.",
                    "Morning. I'd rather not get into it right now.",
                    "...Hello. I didn't rest well. I'll keep to myself.",
                    "Good morning. Forgive me, I'm out of sorts." },
                [MorningTier.Low] = new List<string> {
                    "Good morning. I've some paperwork from the clinic to see to.",
                    "Oh, hello. Did you sleep enough? Rest is important, you know.",
                    "Morning. I'll be reviewing some medical journals today.",
                    "Good morning. There's coffee, though perhaps go easy on it." },
                [MorningTier.High] = new List<string> {
                    "Good morning, dear! I slept well — there's something about you being near.",
                    "Morning! I, ah, find myself smiling before I'm even fully awake these days.",
                    "There you are. Do be careful out on the farm today. For my peace of mind.",
                    "Good morning. A cup of coffee and your face — the best start I could ask for." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Good morning, dear! I slept wonderfully. There's something about waking beside you — it's the best medicine I know.",
                    "I, ah, check my own pulse some mornings, just looking at you. Still racing. After all this time.",
                    "Come here a moment before you go. ...Doctor's orders. A kiss does wonders for the heart, you see.",
                    "I worried I was too quiet, too careful, for someone like you. Every morning with you proves me wrong. Thank you." }
            },
            ["Sam"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Hey. ...Yeah, I'm kinda in a funk. Don't worry about it.",
                    "Oh. Mornin'. I'm just gonna keep to myself today.",
                    "...Hey. Not really in a talking place right now.",
                    "Morning. Eh. Just one of those days." },
                [MorningTier.Low] = new List<string> {
                    "Morning. Gonna mess around with my guitar later, I guess.",
                    "Oh, hey. Sleep okay? I'm still kinda waking up.",
                    "Mornin'. Might hit up the skate spot today.",
                    "Hey you. There's cereal if you want. Living the dream." },
                [MorningTier.High] = new List<string> {
                    "Yo, morning! I'm feelin' pretty good today. You do that, y'know?",
                    "Hey! I had a song stuck in my head all morning. Might be about you. Probably is.",
                    "Mornin', you! Let's make today awesome. I'm in the mood for awesome.",
                    "There's my favorite person. Mornings don't suck anymore. That's a you thing." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Yo, morning! I'm so stoked today. Honestly? Marrying you was the coolest thing I've ever done. By a mile.",
                    "I wrote a song about waking up next to you. It's mostly just your name with a sick beat. It's a banger, trust me.",
                    "C'mere, gimme a kiss before you go save the farm. Best part of my whole day, right here.",
                    "I just laid here grinnin' like an idiot watching you sleep. You make me dumb-happy. I love it." }
            },
            ["Sebastian"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Morning. I'd rather not talk right now, if that's okay.",
                    "...Hey. Just one of those days. Leave me to it.",
                    "Oh. You're up. I'm gonna be in the basement.",
                    "Morning. Don't really have anything to say." },
                [MorningTier.Low] = new List<string> {
                    "Morning. Got some code I want to work on today.",
                    "Oh, hey. Sleep okay? I'll be at my computer.",
                    "Morning. Might take the bike out later. We'll see.",
                    "Hey. Coffee's there. I'm still booting up, basically." },
                [MorningTier.High] = new List<string> {
                    "Morning. ...I slept good, actually. That's kind of your fault.",
                    "Hey, you. The quiet mornings out here? I didn't know I needed them. Or you.",
                    "Morning. Come find me later. I, uh, like having you around. There, I said it.",
                    "Hey. Rainy morning. My favorite. Even better with you here." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning. I'm actually in a good mood, which is rare, so enjoy it. ...It's because of you. Obviously.",
                    "I spent most of my life feeling invisible. You wake up every day and make sure I never feel that way again.",
                    "C'mere before you go. I'm not good with words this early — or ever — so just... let me show you instead.",
                    "I watched the rain with you still asleep on my arm and thought, yeah. This is it. This is the good life." }
            },
            ["Shane"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "...Morning. I'm not in a great place today. Don't push it.",
                    "Oh. You're up. Yeah. I'm just gonna keep to myself.",
                    "Morning. Don't got much to say. Sorry.",
                    "Hey. ...It's one of those mornings. Leave it." },
                [MorningTier.Low] = new List<string> {
                    "Morning. Gotta go check on the chickens.",
                    "Oh, hey. Sleep alright? I'm draggin' a bit.",
                    "Morning. Gonna be a long day. Let's just get to it.",
                    "Hey you. There's coffee. Strong, the way I need it." },
                [MorningTier.High] = new List<string> {
                    "Hey. Slept decent for once. Think it's 'cause you're here.",
                    "Morning. Gonna go say hi to the chickens. ...You can come, if you want.",
                    "There you are. Mornings are easier with you around. Didn't expect that.",
                    "Hey, you. Woke up and you were the first thing on my mind. Not the worst way to start." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Hey. You know, I woke up and actually felt good. Hopeful, even. That's all you. ...Thanks. For everything.",
                    "A while back I figured I'd never get anything good in this life. Then I got you. Still can't believe it some mornings.",
                    "C'mere before you head out. I'm bad at this stuff, so just — let me hold onto you a second, okay?",
                    "Every morning I wake up and you're still here is one the old me wouldn't have believed. I don't waste a single one." }
            }
        };

        // ─────────────────────────────────────────────────────────────
        //  GENERIC FALLBACK POOLS (modded spouses), per tier
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<MorningTier, List<string>> GenericMorning = new Dictionary<MorningTier, List<string>>
        {
            [MorningTier.VeryLow] = new List<string> {
                "Oh. Morning. I'm not really up for talking right now.",
                "...Morning. Just give me some space today, okay?",
                "You're up. Yeah. I've got nothing to say.",
                "Morning. I didn't sleep well. I'd rather be left alone." },
            [MorningTier.Low] = new List<string> {
                "Good morning. I'll be keeping busy today.",
                "Oh, hello. Did you sleep alright? I'll be around.",
                "Morning. There's coffee, if you want some.",
                "Good morning. Try to take it easy out there." },
            [MorningTier.High] = new List<string> {
                "Good morning! It's so nice to start the day with you.",
                "Morning, you. The house feels brighter once you're up.",
                "There you are. Come find me later, would you?",
                "Good morning. Be safe out there — and hurry home." },
            [MorningTier.VeryHigh] = new List<string> {
                "Good morning, my love. Waking up beside you makes everything feel right.",
                "There's nowhere I'd rather be than right here, with you, every single morning.",
                "Come give me a kiss before you head out. Even a moment with you sets my whole day right.",
                "I lay here just watching you sleep this morning. I'm the luckiest person in the valley." }
        };

        public static string GetMorningLine(string name, MorningTier tier, System.Random rng, string avoid)
        {
            List<string> pool = (IsVanilla(name) && MorningLines.ContainsKey(name) && MorningLines[name].ContainsKey(tier))
                ? MorningLines[name][tier]
                : GenericMorning[tier];

            if (pool == null || pool.Count == 0)
                return GenericMorning[tier][0];
            if (pool.Count == 1)
                return pool[0];

            string pick = pool[rng.Next(pool.Count)];
            int guard = 0;
            while (pick == avoid && guard++ < 6)  // avoid repeating yesterday's line
                pick = pool[rng.Next(pool.Count)];
            return pick;
        }
    }
}
