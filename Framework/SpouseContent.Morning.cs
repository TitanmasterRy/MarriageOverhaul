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
                    "You're still here, huh. Great.",
                    "Morning. I'd honestly rather be in the mines than here right now.",
                    "You. Hi. I'm gonna go find something more interesting to do.",
                    "Yeah, yeah, morning. Spare me the small talk, okay?",
                    "Don't. Just... don't, today. I'm not feeling it." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I'm gonna do my own thing today.",
                    "Hey. Sleep okay? ...Cool. Anyway.",
                    "Morning. I might head out for a walk later. Alone.",
                    "Oh, morning. I was just messing with my game.",
                    "Morning. I might lose myself in a game for a few hours. Don't wait up.",
                    "Hey. There's coffee somewhere. Probably. Look around.",
                    "Oh, morning. I had weird dreams about the caves again.",
                    "Morning. I'm gonna reorganize my amethyst collection or something." },
                [MorningTier.High] = new List<string> {
                    "Hey, you! Ready to take on the day? I kinda am.",
                    "Morning! I was up late reading about cave monsters. Don't judge.",
                    "There's my favorite person. Mornings are better with you around.",
                    "Hey! Wanna sneak off on an adventure later? ...A small one. Maybe.",
                    "Morning! I dreamt we found a secret level under the mines. We'd crush it, you and me.",
                    "Hey, you're up! I saved you the comics page. Don't say I never do anything sweet.",
                    "Morning! I'm itching to do something reckless today. ...In a fun way. Wanna?",
                    "There you are. I like that the house gets louder when you wake up. Less boring." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning, you! I had the best dream about us raiding a dungeon side by side. Let's make today an adventure.",
                    "Ugh, I love waking up next to you. Don't tell anyone I got this sappy, okay? It's our secret.",
                    "Hey, my partner in crime. Whatever chaos today brings, we handle it together. Deal?",
                    "I lay here for ten minutes just watching you sleep. Creepy? Maybe. Worth it? Totally.",
                    "Morning, you. I made a whole plan for a date in the mines. Romantic AND dangerous. You're welcome.",
                    "Ugh, look at you all sleepy. I could marry you all over again right now. Don't tempt me.",
                    "Hey. I stayed up half the night just talking with you and I'd do it again every night. I'm obsessed with you, okay?",
                    "C'mere, my favorite adventurer. One kiss for luck before you go save the world, hm?" }
            },
            ["Penny"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "...Good morning. I'd rather not talk right now.",
                    "Oh. Morning. I'll keep out of your way.",
                    "Morning. ...Never mind. It doesn't matter.",
                    "Hello. I didn't sleep well. I won't bother you.",
                    "Good morning. I'll keep to my reading. I won't be in your way.",
                    "Oh. You're awake. I... never mind. Have a good day.",
                    "Morning. I didn't want to bother you, so I've stayed quiet.",
                    "Hello. I'm a bit fragile today. Please, just go easy." },
                [MorningTier.Low] = new List<string> {
                    "Good morning. I hope your day goes smoothly.",
                    "Morning. I'll tidy up around here while you're out.",
                    "Oh, you're awake. I made tea, if you want some.",
                    "Morning. I've got some reading to catch up on today.",
                    "Good morning. I thought I might visit the library later.",
                    "Morning. I've tidied the kitchen already, if you need anything.",
                    "Oh, you're up. There's porridge on the stove, if you'd like.",
                    "Morning. I'll see to a few chores around the house today." },
                [MorningTier.High] = new List<string> {
                    "Good morning, dear. It's so nice to start the day with you.",
                    "Morning! I put the kettle on already. I know how you like it.",
                    "There you are. The house always feels brighter once you're up.",
                    "Good morning. I was just thinking how lucky I am to be here.",
                    "Good morning, dear. I already opened the windows — it's such a lovely day.",
                    "Morning! I read the sweetest story last night. It made me think of us.",
                    "There you are. I always sleep better knowing you're beside me.",
                    "Good morning. Don't rush off without breakfast — I worry, you know." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Good morning, sweetheart. Waking up beside you makes everything in the world feel right.",
                    "I used to dream of a home like this. Quiet, safe, full of love. You gave me that. Thank you.",
                    "Morning, my love. Come find me before you head out — even a moment with you sets my whole day right.",
                    "I watched the sunrise from the window and all I could think about was how grateful I am for you.",
                    "Good morning, my love. I lay awake just listening to you breathe. Is that strange? I don't care. It made me happy.",
                    "Waking up to you, safe and warm... I never imagined I could have a life this gentle. Thank you, truly.",
                    "Morning, sweetheart. Whatever the day holds, come home to me. That's all I ever need.",
                    "I made your favorite for breakfast. Small thing, I know. But loving you is all the little things, isn't it?" }
            },
            ["Haley"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Ugh. Morning. Don't even look at me right now.",
                    "Oh, it's you. Great. Just... give me space.",
                    "Morning. I'm not in the mood, okay?",
                    "Whatever. I have nothing to say to you.",
                    "Ugh. Don't talk to me before I've fixed my hair, okay?",
                    "Morning. I'm busy. With... important things. Not you things.",
                    "Oh, you're up. Cool. I'll be over here. Far over here.",
                    "Whatever. Just... give me a minute. Or an hour. Or the day." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I've got photos to sort, so... yeah.",
                    "Oh, hi. Try not to track mud in today, please.",
                    "Morning. I might go take some pictures later.",
                    "Hey. You look tired. ...Just saying.",
                    "Morning. I've got a whole shoot planned, so don't get in the frame.",
                    "Oh, hey. You might want to comb that hair before you go out. Just saying.",
                    "Morning. The light's perfect today. For photos, I mean.",
                    "Hey. There's juice in the fridge. I didn't make it for you specifically. But it's there." },
                [MorningTier.High] = new List<string> {
                    "Morning, gorgeous. You actually look pretty good today, you know.",
                    "Hey you. I'm in a good mood, so enjoy it while it lasts.",
                    "Morning! I was thinking I'd take some photos of the farm. Of us, maybe.",
                    "There you are. Mornings used to be boring. Not anymore.",
                    "Morning, cutie! Stand by the window — the light's making you look really good.",
                    "Hey, you. I'm having a great hair day, which means it's gonna be a great day.",
                    "Morning! I want to take some pictures of the flowers today. And maybe you. Definitely you.",
                    "There you are. You know you're kind of growing on me, right? Don't make it weird." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning, gorgeous. Life's actually pretty perfect right now. Don't tell anyone I admitted it.",
                    "I took a photo of you sleeping. You looked so sweet I couldn't help it. It's MINE, you can't have it.",
                    "Ugh, I love you. There, I said it first thing in the morning. That's how you know I mean it.",
                    "Come here and kiss me before you go play farmer. I need my morning dose of you.",
                    "Morning, gorgeous. I took, like, five photos of you sleeping. You can't have them. They're for me.",
                    "Ugh, you're too cute in the morning, it's unfair. Come here and kiss me before I change my mind.",
                    "I used to think I'd marry someone rich and fancy. Turns out I just wanted you. Don't let it go to your head.",
                    "Good morning, my favorite person to wake up to. Yes, I said it. Yes, I mean it. Now go be amazing." }
            },
            ["Emily"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Morning. My aura's all tangled. I need space.",
                    "Oh. You're up. The energy in here feels... off.",
                    "...Morning. I can't read you lately. I'm not sure I want to.",
                    "Hello. I'll be meditating. Alone.",
                    "Morning. The energy between us feels frayed. I need to sit with it.",
                    "Oh. You're up. The colors are muddled today. I'd rather be still.",
                    "...Hello. My spirit's pulled inward. Please don't tug at it.",
                    "Morning. I'll be with my crystals. Alone, for now." },
                [MorningTier.Low] = new List<string> {
                    "Morning. The crystals say it'll be an ordinary sort of day.",
                    "Oh, hello. I had odd dreams. Nothing to share, really.",
                    "Good morning. I'll be sewing something today, I think.",
                    "Morning. The colors feel muted today. It happens.",
                    "Good morning. The spirits are quiet today. Just an ordinary day, I think.",
                    "Oh, hello. I dreamt in greys last night. Nothing worth sharing.",
                    "Morning. I've some fabric I want to dye today. We'll see what the colors say.",
                    "Morning. The house feels calm and plain today. That's alright." },
                [MorningTier.High] = new List<string> {
                    "Good morning! Your aura's lovely and bright today. It's contagious.",
                    "Morning, you. The whole house hums with good energy when you're near.",
                    "Hello, love. I dreamt in warm colors last night. I think it was about you.",
                    "Morning! The spirits are gentle today. A good omen for us.",
                    "Good morning! I felt the sun's energy the moment you stirred. It's a beautiful day.",
                    "Morning, love. I had the most colorful dream — all warm hues. You were the gold.",
                    "Hello, you. The crystals were practically singing when I woke. A good sign for us.",
                    "Morning! Come find me later — your aura always brightens my afternoon." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Mmm, your aura is glowing this morning, and so is mine. Our energies are so woven I can hardly tell them apart.",
                    "I felt the universe align the moment I woke beside you. That hum? That's us, my love.",
                    "Good morning, beloved. The light pouring in this morning is nothing next to you.",
                    "I danced a little while you slept. The spirits insisted. They're happy for us, you know.",
                    "Mmm, good morning, beloved. Our energies braided together while we slept. I can still feel the warmth of it.",
                    "I woke before you and just watched the light play on your face. The universe outdid itself, making you.",
                    "Morning, my love. The spirits whispered your name and mine in the same breath. We're meant, you and I.",
                    "Come here before you go. Let me hold your hands a moment — I want to carry your warmth all day." }
            },
            ["Leah"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Morning. ...Yeah. Just leave me to my work.",
                    "Oh. You're up. I'm busy.",
                    "Morning. I'm not really feeling talkative.",
                    "Hey. ...I've got nothing for you today.",
                    "Morning. I've got nothing in me for talking today. Sorry.",
                    "Oh. You're up. I'll be in the studio. Door closed.",
                    "Hey. ...Just let me have my coffee in peace, yeah?",
                    "Morning. I'm in a mood. Best to leave me to it." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I'll be out at the studio most of the day.",
                    "Oh, hey. Sleep alright? I'm gonna get to work.",
                    "Morning. The light's good today — gonna sketch a while.",
                    "Hey you. Coffee's on, if you want it.",
                    "Morning. Might walk down to the forest and forage a bit today.",
                    "Oh, hey. The kettle's warm if you want tea. I'm heading out soon.",
                    "Morning. I've got a block to push through. Wish me luck. Or don't.",
                    "Hey you. Slept alright? I'm gonna get my hands dirty with some clay." },
                [MorningTier.High] = new List<string> {
                    "Morning, you. I woke up wanting to sculpt something. Funny how that happens around you.",
                    "Hey. The farm looks beautiful in this light. So do you, honestly.",
                    "Morning! Come by the studio later if you get a minute. I'd like the company.",
                    "There you are. Best part of my morning, seeing you stumble out of bed.",
                    "Morning, you. The light's gorgeous today — makes me want to paint the whole farm.",
                    "Hey. I picked some wild berries on my walk. Left the good ones for you.",
                    "Morning! Come by the studio later. I could use my favorite muse around.",
                    "There you are. Mornings with you feel like the start of something good. Every time." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning, you. I woke up inspired — might sculpt something just from thinking about us. Sappy, I know.",
                    "I left a whole life behind to find something real. I found it. It's you, it's here, it's this.",
                    "Come here. Five minutes, just us, before the day swallows us up. I want to remember this morning.",
                    "I'd choose this life — you, this farm, these quiet mornings — every single time, without thinking twice.",
                    "Morning, love. I sketched you while you slept. Don't be shy — you're my best work yet.",
                    "I gave up so much for a real life, and here it is, warm beside me. You're everything I ran toward.",
                    "C'mere. Let's steal five quiet minutes before the day starts pulling. Just you, me, and the morning.",
                    "I woke up wanting to make something beautiful, then I looked at you and thought — already done." }
            },
            ["Maru"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Oh. You're up. I've got a lot on my mind, so... later.",
                    "Morning. I'd rather focus on my work right now.",
                    "...Hi. I'm not in a talking mood. Sorry.",
                    "Morning. Let's just get through the day, okay?",
                    "Morning. My focus is elsewhere right now. We'll talk later. Maybe.",
                    "Oh. You're up. I've got variables to sort that aren't... us.",
                    "...Hi. Running low on patience this morning. Give me room.",
                    "Morning. Let's not, okay? I'm not in the mood to process anything." },
                [MorningTier.Low] = new List<string> {
                    "Morning. I've got a project I want to tinker with today.",
                    "Oh, hello. Did you sleep enough? Statistically, you don't.",
                    "Morning. I'll be at the workbench if you need me.",
                    "Hey. Coffee's optimal temperature right now. Just so you know.",
                    "Morning. I'm calibrating something in the lab today. Don't touch anything.",
                    "Oh, hello. Statistically you slept too little again. I'll let it go. For now.",
                    "Morning. There's coffee — I timed the brew precisely. You're welcome.",
                    "Hey. I've got a shift at the clinic later. Long day ahead." },
                [MorningTier.High] = new List<string> {
                    "Good morning! I ran some quick math — you're a solid highlight of my day.",
                    "Morning, you. I had an idea for an invention while watching you sleep. Don't ask.",
                    "Hey! The data's clear: mornings are better with you in them.",
                    "Morning. Come find me later — I work better knowing you're around.",
                    "Good morning! I had a breakthrough idea at 3 a.m. and you were oddly central to it.",
                    "Morning, you. My readings say it's going to be a good day. I trust the data. And you.",
                    "Hey! I built a little something for the kitchen. Wanted to make your mornings easier.",
                    "Morning. Come by the lab later — productivity measurably spikes when you're nearby." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Good morning! I ran the numbers and you're statistically the best part of my life. Every day. Don't question the data.",
                    "You're the one variable I could never quantify, and I've stopped trying. I just love you. Completely.",
                    "Morning, my love. I lay awake calculating how lucky I am. The result kept overflowing.",
                    "Come here before you go. A kiss measurably improves my entire day. It's science. Indulge me.",
                    "Good morning, my love. I tried to graph how happy you make me and the line just... left the page. Beautiful, unscientific chaos.",
                    "I ran every model of a good life, and somehow they all converge on you. Statistically improbable. Personally, inevitable.",
                    "Morning. I lay here recording how peaceful you look asleep. For science. ...Okay, for me. I love you.",
                    "Come here before you go. A goodbye kiss optimizes my entire day. Repeatable result. Indulge the scientist." }
            },
            ["Alex"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Yeah. Morning. Not really feelin' it today.",
                    "Oh. You're up. Don't take it personal, I just don't wanna talk.",
                    "Morning. ...Whatever. I'll be outside.",
                    "Hey. I got nothing to say right now.",
                    "Morning. Gonna go clear my head with a workout. Don't wait.",
                    "Oh. You're up. Yeah, I'm just... not in a talkin' place. Sorry.",
                    "Hey. Rough headspace today. Gimme some room, alright?",
                    "Morning. ...Yeah. I got nothin' good to say, so I'll keep quiet." },
                [MorningTier.Low] = new List<string> {
                    "Morning. Gonna get a workout in, I think.",
                    "Oh, hey. Sleep good? I'm gonna toss the gridball around.",
                    "Morning. Got a lot of energy to burn off today.",
                    "Hey you. Tryin' to wake up. Gimme a sec.",
                    "Morning. Thinkin' I'll hit the gym, then maybe nap. Wild day, I know.",
                    "Oh, hey. There's protein stuff in the fridge if you're hungry. Help yourself.",
                    "Morning. Gonna toss the gridball around with Dusty later.",
                    "Hey you. Still wakin' up. Gimme a sec to find my brain." },
                [MorningTier.High] = new List<string> {
                    "Hey, babe! Feelin' pretty good today. Havin' you here helps, y'know?",
                    "Morning! You sleep okay? I always sleep better with you next to me.",
                    "There's my favorite person. C'mon, let's make today a good one.",
                    "Mornin'. I was thinkin' about you before I even opened my eyes. Weird, huh? Good weird.",
                    "Mornin', babe! Got a good feelin' today. Might be the sun. Probably it's you.",
                    "Hey! I was thinkin' we could go for a jog later. ...Or a walk. A slow one. Whatever you want.",
                    "There's my favorite person. C'mon, let's make today count, yeah?",
                    "Morning! Slept like a champ. Always do when you're next to me." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Hey, babe! I'm feelin' on top of the world today, and it's all 'cause of you. Every single day.",
                    "I used to put on this whole tough-guy act. With you? I just get to be me. Best feeling there is.",
                    "Morning, gorgeous. C'mere. A day that starts with you is a day I already won.",
                    "I watched you sleep for a sec and got all mushy about it. You make me soft, you know that? I love it.",
                    "Hey, babe. Wakin' up to you is better than any win I've ever had. And I've had some good ones.",
                    "I used to think being strong meant never needin' anybody. Then you came along and proved me wrong. Best loss of my life.",
                    "C'mere, gorgeous. One kiss for the road. A day that starts with you is already a win.",
                    "I just laid here grinnin' at you sleepin'. You turn the tough guy to total mush. Don't tell the team." }
            },
            ["Elliott"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Good morning. Forgive my brevity. The warmth simply isn't in me today.",
                    "Ah. You've risen. I'll not trouble you with words.",
                    "Morning. ...The page is blank, and so, it seems, am I.",
                    "A cold sort of morning. I'd rather be left to it.",
                    "Good morning. The words have abandoned me, and with them, my warmth. Forgive me.",
                    "Ah. You wake. I'll keep my silence; it's all I have today.",
                    "Morning. A grey tide has rolled in. I'd sooner weather it alone.",
                    "Good day. ...Or so one hopes. I've little cheer to lend it." },
                [MorningTier.Low] = new List<string> {
                    "Good morning. I shall be at my writing for much of the day.",
                    "Ah, awake. The sea calls; I may walk its edge a while.",
                    "Morning. A quiet one, I think. The muse keeps her distance.",
                    "Good day to you. I've tea steeping, should you wish some.",
                    "Good morning. The sea and a blank page await me. A quiet day, I think.",
                    "Ah, risen at last. I'll be walking the shore, gathering what thoughts I can.",
                    "Morning. The muse keeps her distance, so I shall court her with tea and patience.",
                    "Good day to you. I've a chapter to wrestle into submission. Wish me well." },
                [MorningTier.High] = new List<string> {
                    "Good morning. I woke with half a verse in mind — you were in it, naturally.",
                    "Ah, there you are. A fine morning made finer by your company.",
                    "Morning, my dear. The light on the water is lovely, though it pales beside you.",
                    "Good morning. Stay a moment — your presence is its own kind of poetry.",
                    "Good morning. I woke with a fine turn of phrase — and you, of course, were its subject.",
                    "Ah, there you are, like the first good line of a poem. The day may begin now.",
                    "Morning, my dear. The waves are gentle today. Almost as lovely as your sleepy face.",
                    "Good morning. Linger a moment, won't you? You make even the ordinary worth writing down." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Ah, good morning, my muse. I woke with poetry in my heart and your name upon my lips.",
                    "The greatest story I shall ever write is the one I live each morning, waking beside you.",
                    "Come, sit with me a moment before the day claims you. Let me memorize you in this light.",
                    "I have filled pages trying to capture you and failed beautifully every time. What joy, to keep failing forever.",
                    "Ah, my muse wakes. I've half a sonnet already, and every line bends toward you, as all my lines do.",
                    "I watched the dawn paint the sea and thought only of you. The horizon is grand, but it is not you.",
                    "Come, sit a breath with me before the day claims you. Let me hold this moment like a favorite verse.",
                    "Each morning beside you is a stanza I'd reread forever. What fortune, to live inside the poem I longed to write." }
            },
            ["Harvey"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Oh. Good morning. I'm, ah, not quite myself. Best give me room.",
                    "Morning. I'd rather not get into it right now.",
                    "...Hello. I didn't rest well. I'll keep to myself.",
                    "Good morning. Forgive me, I'm out of sorts.",
                    "Oh. Morning. I'm, ah, not quite right today. Best to give me space.",
                    "Good morning. Forgive me — I haven't the heart for conversation just now.",
                    "...Hello. A restless night. I'll keep to myself, I think.",
                    "Morning. I'm out of sorts. Nothing a quiet day won't mend, I hope." },
                [MorningTier.Low] = new List<string> {
                    "Good morning. I've some paperwork from the clinic to see to.",
                    "Oh, hello. Did you sleep enough? Rest is important, you know.",
                    "Morning. I'll be reviewing some medical journals today.",
                    "Good morning. There's coffee, though perhaps go easy on it.",
                    "Good morning. I've patient files to review this morning, I'm afraid.",
                    "Oh, hello. Did you rest enough? ...Right. I'll not lecture. Today.",
                    "Morning. There's coffee, though do go easy — for your heart, you know.",
                    "Good morning. I'll be at the clinic for a spell. Routine matters." },
                [MorningTier.High] = new List<string> {
                    "Good morning, dear! I slept well — there's something about you being near.",
                    "Morning! I, ah, find myself smiling before I'm even fully awake these days.",
                    "There you are. Do be careful out on the farm today. For my peace of mind.",
                    "Good morning. A cup of coffee and your face — the best start I could ask for.",
                    "Good morning, dear! I, ah, woke with a smile before I quite knew why. Then I saw you.",
                    "Morning! Do mind yourself on the farm today. I do worry, you know — fondly.",
                    "There you are. A good night's sleep and your face — quite the prescription for happiness.",
                    "Good morning. I made coffee just the way you like it. Small thing, but... well, I like caring for you." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Good morning, dear! I slept wonderfully. There's something about waking beside you — it's the best medicine I know.",
                    "I, ah, check my own pulse some mornings, just looking at you. Still racing. After all this time.",
                    "Come here a moment before you go. ...Doctor's orders. A kiss does wonders for the heart, you see.",
                    "I worried I was too quiet, too careful, for someone like you. Every morning with you proves me wrong. Thank you.",
                    "Good morning, my love. I, ah, took my own pulse just now, watching you. Still racing. You're remarkably bad for my composure.",
                    "Waking beside you is the finest medicine I know, and I've read a great deal about medicine. Trust me on this one.",
                    "Come here a moment before you go. ...Doctor's orders. A kiss does wonders for the heart, you understand.",
                    "I spent years being cautious, careful, closed. You opened every door. Each morning with you, I'm grateful you knocked." }
            },
            ["Sam"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Hey. ...Yeah, I'm kinda in a funk. Don't worry about it.",
                    "Oh. Mornin'. I'm just gonna keep to myself today.",
                    "...Hey. Not really in a talking place right now.",
                    "Morning. Eh. Just one of those days.",
                    "Hey. ...Yeah, kinda in a funk. Don't worry about it, I'll deal.",
                    "Oh. Mornin'. I'm just gonna lay low today, if that's cool.",
                    "...Hey. Not feelin' super chatty. It's not you. Just one of those.",
                    "Morning. Eh. Brain's all static today. Gimme some space, yeah?" },
                [MorningTier.Low] = new List<string> {
                    "Morning. Gonna mess around with my guitar later, I guess.",
                    "Oh, hey. Sleep okay? I'm still kinda waking up.",
                    "Mornin'. Might hit up the skate spot today.",
                    "Hey you. There's cereal if you want. Living the dream.",
                    "Morning. Gonna noodle on the guitar for a bit, I think.",
                    "Oh, hey. There's cereal. Breakfast of champions. Or just me.",
                    "Mornin'. Might meet up with Seb later. Or not. We'll see.",
                    "Hey you. Still bootin' up over here. Need like, three more minutes." },
                [MorningTier.High] = new List<string> {
                    "Yo, morning! I'm feelin' pretty good today. You do that, y'know?",
                    "Hey! I had a song stuck in my head all morning. Might be about you. Probably is.",
                    "Mornin', you! Let's make today awesome. I'm in the mood for awesome.",
                    "There's my favorite person. Mornings don't suck anymore. That's a you thing.",
                    "Yo, morning! I'm feelin' good today. You've got somethin' to do with that, I bet.",
                    "Hey! I came up with a sick riff last night. Might play it for you later if you're nice.",
                    "Mornin', you! Let's do somethin' fun today. I'm in a fun mood, gotta use it.",
                    "There's my favorite human. Mornings stopped suckin' when you showed up, y'know?" },
                [MorningTier.VeryHigh] = new List<string> {
                    "Yo, morning! I'm so stoked today. Honestly? Marrying you was the coolest thing I've ever done. By a mile.",
                    "I wrote a song about waking up next to you. It's mostly just your name with a sick beat. It's a banger, trust me.",
                    "C'mere, gimme a kiss before you go save the farm. Best part of my whole day, right here.",
                    "I just laid here grinnin' like an idiot watching you sleep. You make me dumb-happy. I love it.",
                    "Yo, morning! I literally wrote 'I love you' into a guitar solo last night. It shreds. You're welcome.",
                    "Dude — spouse — wakin' up next to you never gets old. Best part of every single day, hands down.",
                    "C'mere, gimme a kiss before you go be awesome. Gotta send my favorite person off right.",
                    "I was just layin' here watchin' you sleep, grinnin' like a dork. You make me so dumb-happy it's embarrassing. Love it." }
            },
            ["Sebastian"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "Morning. I'd rather not talk right now, if that's okay.",
                    "...Hey. Just one of those days. Leave me to it.",
                    "Oh. You're up. I'm gonna be in the basement.",
                    "Morning. Don't really have anything to say.",
                    "Morning. Gonna be in the basement. Don't really wanna talk.",
                    "...Hey. One of those grey ones. Just leave me to it, okay?",
                    "Oh. You're up. I'm not great company today. Sorry.",
                    "Morning. Got nothing to say that's worth saying right now." },
                [MorningTier.Low] = new List<string> {
                    "Morning. Got some code I want to work on today.",
                    "Oh, hey. Sleep okay? I'll be at my computer.",
                    "Morning. Might take the bike out later. We'll see.",
                    "Hey. Coffee's there. I'm still booting up, basically.",
                    "Morning. Got a build to finish today. I'll be at the desk.",
                    "Oh, hey. Coffee's on. I'm still mostly asleep, honestly.",
                    "Morning. Might take the bike out if the weather holds. We'll see.",
                    "Hey. Slept okay, I guess. Long day of code ahead." },
                [MorningTier.High] = new List<string> {
                    "Morning. ...I slept good, actually. That's kind of your fault.",
                    "Hey, you. The quiet mornings out here? I didn't know I needed them. Or you.",
                    "Morning. Come find me later. I, uh, like having you around. There, I said it.",
                    "Hey. Rainy morning. My favorite. Even better with you here.",
                    "Morning. ...Slept good, actually. Pretty sure that's a you thing.",
                    "Hey, you. The quiet out here with you? Didn't know I needed it this bad.",
                    "Morning. Come bug me later, if you want. I, uh, like it when you do.",
                    "Hey. It's raining. My favorite. Even better with you here, not gonna lie." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Morning. I'm actually in a good mood, which is rare, so enjoy it. ...It's because of you. Obviously.",
                    "I spent most of my life feeling invisible. You wake up every day and make sure I never feel that way again.",
                    "C'mere before you go. I'm not good with words this early — or ever — so just... let me show you instead.",
                    "I watched the rain with you still asleep on my arm and thought, yeah. This is it. This is the good life.",
                    "Morning. I'm in a good mood, which is weird for me, so document it. ...It's you. It's always you.",
                    "I spent years feeling like background noise. You wake up every day and make me feel like the main thing. I don't take that lightly.",
                    "C'mere before you go. I'm bad with words this early — or, you know, ever — so just let me hold onto you a sec.",
                    "Watched the rain with you asleep on my arm and thought, yeah. This is the life I never thought I'd get. I'm keeping it." }
            },
            ["Shane"] = new Dictionary<MorningTier, List<string>>
            {
                [MorningTier.VeryLow] = new List<string> {
                    "...Morning. I'm not in a great place today. Don't push it.",
                    "Oh. You're up. Yeah. I'm just gonna keep to myself.",
                    "Morning. Don't got much to say. Sorry.",
                    "Hey. ...It's one of those mornings. Leave it.",
                    "...Morning. Rough headspace today. Don't push it, okay?",
                    "Oh. You're up. Yeah. Gonna keep to myself for a while.",
                    "Morning. Got nothin' in the tank today. Sorry.",
                    "Hey. ...One of those mornings. Just let it be, alright?" },
                [MorningTier.Low] = new List<string> {
                    "Morning. Gotta go check on the chickens.",
                    "Oh, hey. Sleep alright? I'm draggin' a bit.",
                    "Morning. Gonna be a long day. Let's just get to it.",
                    "Hey you. There's coffee. Strong, the way I need it.",
                    "Morning. Gotta head out and check on the blue chickens.",
                    "Oh, hey. Coffee's strong, the way I need it. Help yourself.",
                    "Morning. Gonna be a slog of a day. Let's just get into it.",
                    "Hey you. Draggin' a bit this morning. Gimme a minute." },
                [MorningTier.High] = new List<string> {
                    "Hey. Slept decent for once. Think it's 'cause you're here.",
                    "Morning. Gonna go say hi to the chickens. ...You can come, if you want.",
                    "There you are. Mornings are easier with you around. Didn't expect that.",
                    "Hey, you. Woke up and you were the first thing on my mind. Not the worst way to start.",
                    "Hey. Slept alright for once. Pretty sure that's 'cause you're here.",
                    "Morning. Goin' to say hi to the chickens. ...You can tag along, if you feel like it.",
                    "There you are. Didn't expect mornings to get easier. Then you happened.",
                    "Hey, you. Woke up and you were the first good thought I had. Beats how mornings used to go." },
                [MorningTier.VeryHigh] = new List<string> {
                    "Hey. You know, I woke up and actually felt good. Hopeful, even. That's all you. ...Thanks. For everything.",
                    "A while back I figured I'd never get anything good in this life. Then I got you. Still can't believe it some mornings.",
                    "C'mere before you head out. I'm bad at this stuff, so just — let me hold onto you a second, okay?",
                    "Every morning I wake up and you're still here is one the old me wouldn't have believed. I don't waste a single one.",
                    "Hey. Woke up and actually felt okay. Hopeful, even. That's you. Just... thanks. For all of it.",
                    "Used to figure I'd never get anything worth keeping. Then there's you, every morning. Still can't believe my luck.",
                    "C'mere before you head out. I'm garbage at this stuff, so just — let me hold you a sec, alright?",
                    "Every morning you're still here is one the old me wouldn't have believed. I'm not wastin' a single one. Promise." }
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
                "Morning. I didn't sleep well. I'd rather be left alone.",
                "Morning. I'm not really up for talking today. Sorry.",
                "Oh. You're awake. I'd rather keep to myself for now.",
                "Morning. Didn't sleep well. Best give me some room.",
                "Hey. ...Not much to say today. Leave me be, would you?" },
            [MorningTier.Low] = new List<string> {
                "Good morning. I'll be keeping busy today.",
                "Oh, hello. Did you sleep alright? I'll be around.",
                "Morning. There's coffee, if you want some.",
                "Good morning. Try to take it easy out there.",
                "Good morning. I'll be keeping busy around here today.",
                "Morning. There's something warm on the stove if you're hungry.",
                "Oh, you're up. I've a few things to see to today.",
                "Good morning. Take care of yourself out there." },
            [MorningTier.High] = new List<string> {
                "Good morning! It's so nice to start the day with you.",
                "Morning, you. The house feels brighter once you're up.",
                "There you are. Come find me later, would you?",
                "Good morning. Be safe out there — and hurry home.",
                "Good morning! The house feels brighter the moment you wake up.",
                "Morning, you. I slept well, knowing you were right beside me.",
                "There you are. Come find me later, would you? I like having you near.",
                "Good morning. Be safe out there today — and hurry back to me." },
            [MorningTier.VeryHigh] = new List<string> {
                "Good morning, my love. Waking up beside you makes everything feel right.",
                "There's nowhere I'd rather be than right here, with you, every single morning.",
                "Come give me a kiss before you head out. Even a moment with you sets my whole day right.",
                "I lay here just watching you sleep this morning. I'm the luckiest person in the valley.",
                "Good morning, my love. Waking up beside you is the best part of every single day.",
                "I lay here just watching you sleep. I still can't believe this wonderful life is ours.",
                "Come give me a kiss before you go. A moment with you sets my whole heart right.",
                "There's nowhere in the valley I'd rather be than right here, with you, this morning and every morning." }
        };

        public static string GetMorningLine(string name, MorningTier tier, System.Random rng, string avoid)
        {
            // Custom-NPC tier: serve registered pack content (verbatim, no i18n) when present for this tier.
            if (CustomNpcRegistry.TryGetMorning(name, tier, out var customPool))
                return PickAvoiding(customPool, rng, avoid);

            bool has = IsVanilla(name) && MorningLines.ContainsKey(name) && MorningLines[name].ContainsKey(tier);
            List<string> pool = has ? MorningLines[name][tier] : GenericMorning[tier];
            string prefix = $"morning.{(has ? name : "generic")}.{tier}";

            if (pool == null || pool.Count == 0)
                return I18n.Get($"morning.generic.{tier}.0", GenericMorning[tier][0]);
            if (pool.Count == 1)
                return I18n.Get($"{prefix}.0", pool[0]);

            int idx = rng.Next(pool.Count);
            string pick = I18n.Get($"{prefix}.{idx}", pool[idx]);
            int guard = 0;
            while (pick == avoid && guard++ < 6)  // avoid repeating yesterday's line
            {
                idx = rng.Next(pool.Count);
                pick = I18n.Get($"{prefix}.{idx}", pool[idx]);
            }
            return pick;
        }

        /// <summary>Pick a line at random from a verbatim (non-i18n) pool, avoiding yesterday's line where possible.</summary>
        internal static string PickAvoiding(List<string> pool, System.Random rng, string avoid)
        {
            if (pool == null || pool.Count == 0)
                return "";
            if (pool.Count == 1)
                return pool[0];
            string pick = pool[rng.Next(pool.Count)];
            int guard = 0;
            while (pick == avoid && guard++ < 6)
                pick = pool[rng.Next(pool.Count)];
            return pick;
        }

        /// <summary>Add every morning-line key + English to the i18n default map.</summary>
        internal static void CollectMorningDefaults(System.Collections.Generic.IDictionary<string, string> map)
        {
            foreach (var byName in MorningLines)
                foreach (var byTier in byName.Value)
                    for (int i = 0; i < byTier.Value.Count; i++)
                        map[$"morning.{byName.Key}.{byTier.Key}.{i}"] = byTier.Value[i];

            foreach (var byTier in GenericMorning)
                for (int i = 0; i < byTier.Value.Count; i++)
                    map[$"morning.generic.{byTier.Key}.{i}"] = byTier.Value[i];
        }
    }
}
