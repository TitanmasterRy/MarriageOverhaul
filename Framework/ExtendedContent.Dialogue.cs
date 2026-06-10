using System;
using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class ExtendedContent
    {
        // ── F1: Relationship milestones (years 1 / 3 / 5) ─────────
        private static readonly Dictionary<string, Dictionary<int, string>> Milestones = new Dictionary<string, Dictionary<int, string>>
        {
            ["Abigail"] = new Dictionary<int, string> {
                [1] = "One whole year married to you. Honestly? Best adventure I've ever been on, and that's saying something. Here's to a hundred more.",
                [3] = "Three years. We've fought monsters and folded laundry and I wouldn't trade a second of it. You're still my favorite person, weirdo.",
                [5] = "Five years. The girl who dreamed of running away got everything she wanted, right here with you. I love this life. I love you." },
            ["Penny"] = new Dictionary<int, string> {
                [1] = "A whole year as your spouse... I keep pinching myself. You gave me the stable, loving home I always dreamed of. Thank you.",
                [3] = "Three years together. I used to be so afraid of wanting things. You taught me it was safe to want — and to be wanted. I'm so grateful.",
                [5] = "Five years. Look how far we've come. Whatever I was before, you helped me become someone who feels whole. I love you, completely." },
            ["Haley"] = new Dictionary<int, string> {
                [1] = "One year. Ugh, I'm getting emotional and I refuse to cry. ...Okay, maybe a little. You've made this the best year of my life.",
                [3] = "Three years married. I used to think love had to look perfect. Turns out the messy, real version with you is so much better.",
                [5] = "Five years. I'd marry you again tomorrow, in a heartbeat, mud and all. You're the best thing that ever happened to me." },
            ["Emily"] = new Dictionary<int, string> {
                [1] = "A year of our spirits intertwined. I felt the universe align the day we wed, and it hasn't stopped humming since.",
                [3] = "Three years. Our auras have grown so deeply woven I can hardly tell where you end and I begin. What a beautiful thing.",
                [5] = "Five years of shared light. The cosmos brought us together, but we're the ones who kept the fire burning. I adore you." },
            ["Leah"] = new Dictionary<int, string> {
                [1] = "One year married. I left a whole life to find something real, and I found it with you. Best decision I ever made.",
                [3] = "Three years. You're still my favorite thing to come home to. I've sculpted a hundred things, but nothing as good as this life.",
                [5] = "Five years. Funny — I ran away to be an artist, and you became the masterpiece. I'd choose this, choose you, every single time." },
            ["Maru"] = new Dictionary<int, string> {
                [1] = "One year of marriage. I ran the numbers — statistically, this has been the happiest year of my life. The data is conclusive.",
                [3] = "Three years. You're the one variable I could never quantify and never want to. I just love you. Unscientifically. Completely.",
                [5] = "Five years. Half a decade of impossible odds paying off. I'm so glad the universe miscalculated and gave me you." },
            ["Alex"] = new Dictionary<int, string> {
                [1] = "One year, babe. I used to put on this whole tough-guy act. With you I finally get to just... be me. Best year ever.",
                [3] = "Three years married. You saw the real me when nobody else bothered to look. I'm a better man for it. I love you.",
                [5] = "Five years. I went pro at the one thing that matters — loving you. Couldn't have drawn up a better play if I tried." },
            ["Elliott"] = new Dictionary<int, string> {
                [1] = "A year, my love — a single, glorious chapter, and already the finest I've ever penned. Here's to the volumes still to come.",
                [3] = "Three years. I have filled notebooks trying to capture you, and failed beautifully every time. What a privilege, to keep failing forever.",
                [5] = "Five years. The greatest love story I will ever write is the one I'm living, with you, every ordinary, extraordinary day." },
            ["Harvey"] = new Dictionary<int, string> {
                [1] = "One year as your spouse. I, ah... I check my own pulse sometimes when I look at you. It's still racing. Best year of my life.",
                [3] = "Three years. I worried I was too quiet, too cautious, for someone like you. You've spent three years proving me wrong. Thank you.",
                [5] = "Five years. Half a decade, and you still make this careful old doctor's heart do something I'd flag as concerning. I love you dearly." },
            ["Sam"] = new Dictionary<int, string> {
                [1] = "One year, dude! ...I mean, spouse. One year! I wrote a song about it. It's mostly just your name with a beat. Best year EVER.",
                [3] = "Three years married. You make me wanna be serious about stuff, and that's huge for me. Every plan I've got, you're in it.",
                [5] = "Five years. Half my biggest dreams came true and they were all just... this. You. Our life. I'm the luckiest guy in the valley." },
            ["Sebastian"] = new Dictionary<int, string> {
                [1] = "...One year. I'm bad at this stuff, so just — thank you. For seeing me. For staying. It's been the best year of my life.",
                [3] = "Three years. I spent most of my life feeling invisible. You've spent three of them making sure I never feel that way again.",
                [5] = "Five years. I don't say much, you know that. So hear this clearly: marrying you is the best thing I've ever done. By far." },
            ["Shane"] = new Dictionary<int, string> {
                [1] = "One year. A while back I didn't think I'd get anything good. Then I got you. ...Thanks for not giving up on me. Ever.",
                [3] = "Three years married. Every day with you is one I didn't think I'd live to see, back then. I don't waste a single one.",
                [5] = "Five years. The guy I used to be wouldn't believe this life. A home. A reason. You. ...I'm not gonna screw it up. Promise." }
        };
        private static readonly Dictionary<int, string> GenericMilestone = new Dictionary<int, string>
        {
            [1] = "A whole year married to you. It's been the best year of my life, and I can't wait for all the ones still to come.",
            [3] = "Three years together. Look at the life we've built. I'd choose you all over again without a moment's hesitation.",
            [5] = "Five years. Half a decade of you and me. I love you more today than the day we wed, and that's saying something."
        };
        public static string GetMilestone(string name, int year)
        {
            if (IsVanilla(name) && Milestones.TryGetValue(name, out var byYear) && byYear.TryGetValue(year, out var line))
                return line;
            return GenericMilestone.TryGetValue(year, out var g) ? g : GenericMilestone[1];
        }

        // ── F4: Sickness ──────────────────────────────────────────
        public class SickInfo { public string Sick; public string Grateful; public string Tired; }
        private static readonly Dictionary<string, SickInfo> Sick = new Dictionary<string, SickInfo>
        {
            ["Shane"]     = new SickInfo { Sick = "It's nothing. I'm fine. ...Okay I'm not fine, I feel like garbage, but don't make a thing of it. I'll just stay in bed.", Grateful = "You didn't have to do that. ...But you did. For me. Thanks. Really. I'm not used to people bothering.", Tired = "Still wiped out from being sick. Give me a day. ...And thanks again. For before." },
            ["Harvey"]    = new SickInfo { Sick = "I have diagnosed myself with, ah, definitely-not-dying. I'm a terrible patient, I know. I'll stay in bed and sulk about it.", Grateful = "You brought me a remedy? Following doctor's orders better than I do. ...Thank you. It means more than the medicine.", Tired = "Recovering. Slowly. Turns out doctors make the worst patients. Bear with me another day." },
            ["Penny"]     = new SickInfo { Sick = "Oh no, I've gone and gotten sick, and now I'm just a burden. I'm so sorry. Don't trouble yourself over me, please.", Grateful = "You took care of me... I'm not used to that. Being looked after. Thank you. I don't deserve someone so kind.", Tired = "Still a bit weak, I'm afraid. I'm sorry to be slow today. ...Thank you for yesterday. Truly." },
            ["Haley"]     = new SickInfo { Sick = "I am DYING. This is the worst thing that has ever happened to anyone. Look at me. I'm a disaster. Don't look at me.", Grateful = "You brought me something? Ugh, you're going to make me cry, and I'm already a mess. ...Thank you. You're the best.", Tired = "Still feeling gross and tragic. Don't expect me to be charming today. ...Thanks for taking care of me, though." },
            ["Abigail"]   = new SickInfo { Sick = "Ugh, I'm sick. Of all the dumb, boring ways to be taken down. Not a monster, not a cave-in — a sniffle. Lame. Staying in bed.", Grateful = "Aw, you brought me medicine? My hero. Okay, that actually made me feel a lot better. Thanks, you.", Tired = "Still kinda wrecked from being sick. Slow day for me. ...But thanks for yesterday. You're alright, you know that?" },
            ["Emily"]     = new SickInfo { Sick = "My energy's all clouded and grey today. I think I'm sick. The spirits want me to rest, so... I'll listen. Bed it is.", Grateful = "You brought me something warm and healing. I can feel my aura brightening already. Thank you, truly.", Tired = "Still recovering my energy. The colors are dim but mending. Thank you for tending to me." },
            ["Leah"]      = new SickInfo { Sick = "I think I caught something. Everything aches and I can't even hold a brush. I'll just stay in today. Don't fuss.", Grateful = "You came through for me. That's... really sweet. Thank you. I'll paint you something when I can hold a brush again.", Tired = "Still run down. Going to take it easy today. ...Thanks for looking after me, by the way." },
            ["Maru"]      = new SickInfo { Sick = "Running a fever — I'd estimate a degree and a half above baseline. Translation: I feel awful. Staying in bed for the data.", Grateful = "You brought the optimal remedy! And you. Mostly you. ...My recovery time just improved dramatically. Thank you.", Tired = "Still operating below capacity. Recovery's ongoing. Thanks for the care yesterday — measurably effective." },
            ["Alex"]      = new SickInfo { Sick = "Man, I'm sick. Big tough gridball guy taken out by a cold. Don't tell the guys. I'll just... be in bed. Ugh.", Grateful = "You took care of me? That's... that's real nice. Thanks. Nobody's done that for me in a long time.", Tired = "Still feelin' rough. Gonna take it slow today. ...Hey. Thanks for yesterday. Meant a lot." },
            ["Elliott"]   = new SickInfo { Sick = "Alas, I am felled — not by drama this time, but by a common malady. I shall convalesce dramatically in bed. Forgive my absence.", Grateful = "You brought a remedy to your ailing poet. I am moved. The cure is good; the gesture, immeasurable. Thank you, my love.", Tired = "Still weak as wet parchment, I'm afraid. The pen waits another day. ...Your kindness yesterday will not be forgotten." },
            ["Sam"]       = new SickInfo { Sick = "Bleh, I'm sick. Total bummer. Can't even play guitar, my head's all fuzzy. Gonna stay in bed and feel sorry for myself.", Grateful = "Dude, you brought me stuff to feel better? You're the best. Seriously. I'll write you a get-well song. ...Later. When I can think.", Tired = "Still kinda out of it. Low energy today. ...Thanks for takin' care of me though. You rule." },
            ["Sebastian"] = new SickInfo { Sick = "...I'm sick. Don't want to talk about it. Just gonna stay in bed in the dark. Leave the fussing to someone else, okay?", Grateful = "You actually brought me something. ...Most people would've just left me down here. Thanks. It — yeah. Thanks.", Tired = "Still feeling like garbage. Gonna be quiet today. ...What you did yesterday. I noticed. Thank you." }
        };
        private static readonly SickInfo GenericSick = new SickInfo
        {
            Sick = "I'm not feeling well at all today. I think I'd better stay in bed. Could you bring me something to help, maybe?",
            Grateful = "You brought me something to feel better... thank you. Being cared for like this means more than you know.",
            Tired = "Still feeling a little worn out from being sick. I'll be slow today. Thank you for looking after me."
        };
        public static SickInfo GetSick(string name)
            => IsVanilla(name) && Sick.ContainsKey(name) ? Sick[name] : GenericSick;

        // ── F10: Bad days ─────────────────────────────────────────
        public class BadDayInfo { public List<string> Openers; public string Recovered; public string Flat; }
        private static readonly Dictionary<string, BadDayInfo> BadDays = new Dictionary<string, BadDayInfo>
        {
            ["Shane"] = new BadDayInfo { Openers = new List<string> { "...It's one of those days. The heavy kind. Nothing happened, the dark just... rolled back in. Don't mind me.", "Woke up under a cloud today. You know the kind. Just gotta get through it. Sorry I'm not better company." }, Recovered = "Yesterday was rough, but you stuck around. That helped more than you'll ever know. Feeling more like myself today.", Flat = "Still kinda numb today. Not your fault. Just... one of those stretches." },
            ["Harvey"] = new BadDayInfo { Openers = new List<string> { "I, ah... my chest feels tight today. The anxious kind. I keep wondering if I'm really enough for you. I'm sorry, it's silly.", "Bad day for the nerves, I'm afraid. My mind won't stop running worst cases. I just need to breathe. And maybe you near." }, Recovered = "Yesterday the worry had me good. But you were there, and it quieted. Thank you. I feel steadier today.", Flat = "Still a bit anxious today, rattling around in my own head. It'll pass. It usually does." },
            ["Penny"] = new BadDayInfo { Openers = new List<string> { "I'm feeling overwhelmed today. Old worries — Mom, money, the trailer years. It all crowds in sometimes. I'm sorry.", "Some days the weight of everything just settles on me at once. Today's one of those. I'll be alright. I think." }, Recovered = "Yesterday everything felt like too much. You helped carry it. I don't know how to thank you for that. I'm better today.", Flat = "Still feeling a bit heavy today. It's not you. Some days are just like this for me." },
            ["Sebastian"] = new BadDayInfo { Openers = new List<string> { "...Not in a talking mood today. Everything feels far away. I'll be down in the basement if you need me. Don't worry about it.", "Just one of those quiet, gray days. I'm pulling inward. It's not you. I just get like this sometimes." }, Recovered = "Yesterday I went quiet on you. You didn't push, you just stayed. That's exactly what I needed. Thank you.", Flat = "Still kind of withdrawn today. Give me space and I'll come back around. I always do." },
            ["Abigail"] = new BadDayInfo { Openers = new List<string> { "Feeling restless and off today. Like my skin doesn't fit right. Nothing fixes it. Just gotta ride it out, I guess.", "Bleh. One of those days where everything's gray and nothing's fun. Even adventure sounds boring. Weird, right?" }, Recovered = "Yesterday was a weird, heavy one. You stuck by me through it. Feeling way more like myself today. Thanks, you.", Flat = "Still kinda off today. Not down exactly, just... flat. It'll lift. It always does." },
            ["Haley"] = new BadDayInfo { Openers = new List<string> { "I just feel awful today and I don't even have a good reason and that makes it worse. Ugh. Today is the worst.", "Some days I just feel small and grumpy and not myself. Today's one of them. Don't take it personally." }, Recovered = "Yesterday I was a mess for no reason. You were patient with me anyway. ...That's why it's you. Feeling better today.", Flat = "Still kind of in a mood today. It's not you. I'll be cute and charming again tomorrow, promise." },
            ["Emily"] = new BadDayInfo { Openers = new List<string> { "My energy is all tangled and dark today. The colors won't come. I think I just need to sit quietly with it.", "The universe feels heavy on me this morning. Some days the static drowns out the song. I'll find the melody again." }, Recovered = "Yesterday the dark crowded out my light. You sat in it with me until it lifted. Thank you. My colors are dancing again.", Flat = "Still a bit clouded today. The energy's slow to clear. Be gentle with me while it does." },
            ["Leah"] = new BadDayInfo { Openers = new List<string> { "Creative block, low mood — they always come together for me. Today I just feel stuck and sad. I'll work through it.", "One of those days where the well runs dry and the gray creeps in. I'll be quiet today. Don't mind me." }, Recovered = "Yesterday I was stuck in the gray. You pulled me out of my own head. The work's flowing again. Thank you, truly.", Flat = "Still feeling a little blocked and low. It happens. It'll break. It always does." },
            ["Maru"] = new BadDayInfo { Openers = new List<string> { "I can't focus on anything today. My mind keeps spinning its wheels and getting nowhere. It's frustrating. And exhausting.", "Bad brain day. Everything feels foggy and just out of reach. No diagnosis, no fix. Just have to wait it out." }, Recovered = "Yesterday my head was a fog. You helped clear it just by being there. Running at full capacity again. Thank you.", Flat = "Still operating in a bit of a haze today. It'll resolve. These things usually do." },
            ["Alex"] = new BadDayInfo { Openers = new List<string> { "Feelin' low today, and the tough-guy thing won't cover it. Sometimes the old fears just... show up. I hate it.", "Bad day, babe. The kind where I feel like that scared kid again. Don't really know how to shake it." }, Recovered = "Yesterday the heavy stuff caught up with me. You didn't let me hide behind the act. Feelin' a lot better today. Thanks.", Flat = "Still kinda low today. Not myself. It's not you — just gotta wait it out." },
            ["Elliott"] = new BadDayInfo { Openers = new List<string> { "A melancholy has settled over me today, heavy and wordless. Even the sea offers no verse. Forgive my gloom.", "The muse has abandoned me and taken my spirits with her. A gray, hollow sort of day. I shall endure it quietly." }, Recovered = "Yesterday the melancholy held me fast. Your presence was the light that broke it. The words return. Thank you, my love.", Flat = "The gray lingers yet today, I'm afraid. The pages stay blank. It shall pass. Such tides always do." },
            ["Sam"] = new BadDayInfo { Openers = new List<string> { "Not feelin' it today, man. Can't even fake the goofball thing. Just kinda... down. Dunno why. It sucks.", "Bad day. The music in my head went quiet, y'know? Everything's just gray and flat. Bummer." }, Recovered = "Yesterday I was way off, no jokes left in me. You stuck around anyway. Feelin' the beat again today. Thanks, seriously.", Flat = "Still kinda flat today. The funny'll come back, just... not yet. It's not you." }
        };
        private static readonly BadDayInfo GenericBadDay = new BadDayInfo
        {
            Openers = new List<string> { "I'm just having a rough day today. Nothing happened — I'm low for no reason I can name. I'm sorry.", "Bad day today. The kind that comes out of nowhere and sits on your chest. I'll be quiet. Don't mind me." },
            Recovered = "Yesterday was hard for no good reason, and you stayed with me through it. That meant everything. I feel better today.",
            Flat = "Still feeling a bit flat today. It's not you — some days are just like this. It'll pass."
        };
        public static BadDayInfo GetBadDay(string name)
            => IsVanilla(name) && BadDays.ContainsKey(name) ? BadDays[name] : GenericBadDay;

        // ── F6: Achievement pride (template, {0} = achievement) ───
        private static readonly Dictionary<string, string> Achievements = new Dictionary<string, string>
        {
            ["Abigail"]   = "WAIT. You {0}?! That is the COOLEST thing I have ever heard, I'm telling everyone, I'm SO proud of you, oh my gosh!",
            ["Penny"]     = "I heard that you {0}. I always knew you were capable of wonderful things. I'm so very proud to call you mine.",
            ["Haley"]     = "So... you {0}. ...Okay, that's actually really impressive. Don't let it go to your head. ...I'm proud of you. A lot.",
            ["Emily"]     = "The universe whispered to me that you {0}. I felt the pride bloom in my chest like a sunrise. You're remarkable.",
            ["Leah"]      = "You {0}? That's incredible. You inspire me, you know that? I might just have to sculpt this moment.",
            ["Maru"]      = "You {0}?! Do you understand how technically remarkable that is? I ran the difficulty — it's off the charts. I'm beyond proud.",
            ["Alex"]      = "Yo, you {0}?! That's championship-level stuff right there. I'm braggin' about you to everyone. So proud, babe.",
            ["Elliott"]   = "Word reached me that you {0}. I have already begun three pages immortalizing the feat. My spouse, the living legend.",
            ["Harvey"]    = "I, ah, heard that you {0}. That's... genuinely extraordinary. I find myself quite overcome with pride. Well done, my love.",
            ["Sam"]       = "DUDE. You {0}?! That's so sick! I'm writing a song about it, the chorus is just gonna be your name. So proud of you!",
            ["Sebastian"] = "...Heard you {0}. That's actually amazing. I'm not great at this, but — I'm proud of you. Really proud. Don't make it weird.",
            ["Shane"]     = "So you {0}, huh. ...That's something. That's really something. I'm — yeah. I'm proud of you. Dunno how to say it better than that."
        };
        private const string GenericAchievement = "I heard that you {0}. That's truly amazing. I'm so incredibly proud of you, you know that?";
        public static string GetAchievementLine(string name, string achievement)
            => string.Format(IsVanilla(name) && Achievements.ContainsKey(name) ? Achievements[name] : GenericAchievement, achievement);

        // ── F11: Birthday ─────────────────────────────────────────
        public class BirthdayInfo { public string GiftItem; public string Line; public string PointedAddon; }
        private static readonly Dictionary<string, BirthdayInfo> Birthdays = new Dictionary<string, BirthdayInfo>
        {
            ["Abigail"]   = new BirthdayInfo { GiftItem = "Pumpkin Pie", Line = "Happy birthday, you! I made you pie for breakfast because rules are fake and it's YOUR day. There's a slice in the fridge. Eat it. That's an order.", PointedAddon = "...Also, last anniversary? You kinda spaced on a gift. I'm not bitter. I'm just SAYING. Today's your freebie. Use it." },
            ["Penny"]     = new BirthdayInfo { GiftItem = "Pancakes", Line = "Happy birthday, my love. I woke early and made you pancakes — they're in the fridge, still warm if you hurry. I just wanted today to feel special for you.", PointedAddon = "I did notice our anniversary slipped by without a gift this year... I'm not upset, truly. I just hope I matter to you the way you matter to me." },
            ["Haley"]     = new BirthdayInfo { GiftItem = "Pink Cake", Line = "Happy birthday, gorgeous! I made you a cake and it's adorable AND delicious, you're welcome. It's in the fridge. Don't you dare skip breakfast.", PointedAddon = "By the way — you forgot to get me anything on our anniversary. I noticed. I ALWAYS notice. ...You can make it up to me. Cutely." },
            ["Emily"]     = new BirthdayInfo { GiftItem = "Salad", Line = "Happy birthday, beloved! The stars told me today was special, so I made you something fresh and full of good energy. It's in the fridge. Eat with intention.", PointedAddon = "I'll gently note the cosmos remembers our anniversary went... ungifted. The universe forgives. And so do I. Mostly." },
            ["Leah"]      = new BirthdayInfo { GiftItem = "Salad", Line = "Happy birthday, you. I threw together something fresh and good — it's in the fridge. Nothing fancy, just made with love. Like everything else around here.", PointedAddon = "I'll just mention our anniversary passed with no gift this year. I'm not keeping score... but I do have a very good memory." },
            ["Maru"]      = new BirthdayInfo { GiftItem = "Complete Breakfast", Line = "Happy birthday! I optimized a full nutritionally-balanced breakfast for you — it's in the fridge. Statistically the best way to start your special day.", PointedAddon = "I will note, for the record, that our anniversary registered zero gifts this year. The variable was you. I'm filing it under 'forgiven, but logged.'" },
            ["Alex"]      = new BirthdayInfo { GiftItem = "Complete Breakfast", Line = "Happy birthday, babe! Made you the full breakfast, champion's fuel, it's in the fridge. Today's all about you. You earned it.", PointedAddon = "...Not gonna lie though, you blanked on our anniversary gift this year. Stung a little. But hey — it's your day. We're good." },
            ["Elliott"]   = new BirthdayInfo { GiftItem = "Crab Cakes", Line = "Happy birthday, my darling! I rose with the gulls to prepare you a seaside feast — it awaits in the cold-box. May your day be as splendid as you are.", PointedAddon = "I shall delicately observe that our anniversary drifted past, giftless, like a ship in fog. No matter. The poet forgives. He simply... remembers." },
            ["Harvey"]    = new BirthdayInfo { GiftItem = "Omelet", Line = "Happy birthday, dear! I, ah, made you a proper omelet — a healthy start to your special day. It's in the fridge. I hope you like it.", PointedAddon = "I don't wish to make a fuss, but our anniversary did go by without a gift this year. I noticed. ...I forgive you, of course. I just noticed." },
            ["Sam"]       = new BirthdayInfo { GiftItem = "Pancakes", Line = "HAPPY BIRTHDAY! I made you a giant stack of pancakes, dude, they're in the fridge. It's your day, we're celebrating, no arguments!", PointedAddon = "Okay so, tiny thing — you totally forgot our anniversary gift this year, haha. It's cool! It's cool. ...It was a little uncool. But it's cool." },
            ["Sebastian"] = new BirthdayInfo { GiftItem = "Pizza", Line = "Hey. Happy birthday. I, uh, got up early and left pizza in the fridge for you. Breakfast pizza. Don't argue, it's the superior breakfast. Enjoy it.", PointedAddon = "...Also, you forgot our anniversary gift this year. I'm not gonna make a thing of it. Just — noticed, is all. It's your day. Forget I said it." },
            ["Shane"]     = new BirthdayInfo { GiftItem = "Pepper Poppers", Line = "Happy birthday. I, uh, made you pepper poppers for breakfast. It's the one thing I'm good at. They're in the fridge. ...Hope you like 'em.", PointedAddon = "Look, I gotta say it — you blanked on our anniversary gift this year. Hurt a little, not gonna lie. But it's your day. So we're square. Eat up." }
        };
        private static readonly BirthdayInfo GenericBirthday = new BirthdayInfo
        {
            GiftItem = "Complete Breakfast",
            Line = "Happy birthday, my love! I got up early and made you a special breakfast — it's waiting in the fridge. Today is all about you.",
            PointedAddon = "I'll gently mention our anniversary passed without a gift this year. I'm not upset... but I did notice. Just so you know."
        };
        public static BirthdayInfo GetBirthday(string name)
            => IsVanilla(name) && Birthdays.ContainsKey(name) ? Birthdays[name] : GenericBirthday;
    }
}
