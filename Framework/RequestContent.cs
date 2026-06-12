using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>A personalized spouse request: a note in the mailbox and what fulfills it.</summary>
    public class SpouseRequest
    {
        public string Id;
        public string Note;          // letter body delivered to the mailbox
        public string[] Items;       // internal item names that fulfill it (null = any gift / attention)
        public int[] Categories;     // object category ids that fulfill it (optional)
        public string Thank;         // thank-you scene line
        public string RewardItem;    // optional: item the spouse makes and gives back a few days later (null = none)
        public int RewardQty = 1;    // quantity of the reward item
        public string RewardLine;    // spoken line when the spouse hands over the reward
    }

    public static partial class ExtendedContent
    {
        // ── F14: Spouse requests (>= 3 per vanilla spouse) ────────
        private static readonly Dictionary<string, List<SpouseRequest>> Requests = new Dictionary<string, List<SpouseRequest>>
        {
            ["Penny"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "penny_book", Note = "My dearest,^I've been longing for a new story to lose myself in. Would you bring me a book sometime soon? Curling up with one beside you sounds perfect.^- Penny", Categories = new[] { -102 }, Thank = "A new book! Oh, you remembered. I'm going to read every page beside you. Thank you, truly." },
                new SpouseRequest { Id = "penny_meal", Note = "My love,^I'd so love a home-cooked meal from your hands. There's something about being cared for that I never quite get used to. Whenever you can.^- Penny", Categories = new[] { -7 }, Thank = "You cooked this for me? I... thank you. Being looked after by you means more than I can say." },
                new SpouseRequest { Id = "penny_library", Note = "Sweetheart,^Would you spend a little time with me at home, away from the fields? Just us, somewhere quiet. I miss simply being near you.^- Penny", Items = null, Thank = "Just having you here with me, no rush, no fields... this is all I ever wanted. Thank you for the time." } },
            ["Maru"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "maru_gem", Note = "Hi!^I'm running an experiment and I need a good gemstone for it. Could you bring me one when you get the chance? For science. And, okay, a little for the excuse to see you.^- Maru", Categories = new[] { -2 }, Thank = "A gem! Perfect specimen. My experiment AND my heart rate just improved. Thank you, you wonderful variable." },
                new SpouseRequest { Id = "maru_quartz", Note = "Hey,^I could really use some refined quartz for a project I'm building. Would you bring me some? I promise to explain the whole thing in exhausting, loving detail.^- Maru", Items = new[] { "Refined Quartz" }, Thank = "Refined quartz! Exactly what I needed. The prototype lives. So does my enormous crush on you. Thank you!", RewardItem = "Quality Sprinkler", RewardLine = "It's finished! Remember that refined quartz you brought me? It's humming away inside this little beauty now — a sprinkler I tuned myself. It's yours. Go let it do your watering for you. The data says you work too hard." },
                new SpouseRequest { Id = "maru_clinic", Note = "Hi you,^Would you come spend some time with me at home today? I've been heads-down in work and I miss your face. That's a documented condition, by the way.^- Maru", Items = null, Thank = "You came! My focus is shot now, in the best way. Thank you for the company. You're my favorite distraction." } },
            ["Sam"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "sam_pizza", Note = "Yo!^I am CRAVING pizza so bad it's unreal. Bring me a pizza and you're officially my favorite person forever. (You already are. But still.)^- Sam", Items = new[] { "Pizza" }, Thank = "PIZZA! You absolute legend. Best spouse in the valley, no contest. C'mere, I'm sharing. ...A little." },
                new SpouseRequest { Id = "sam_practice", Note = "Hey,^I've been working on a new song and I really want you to hear it. Come hang with me today? Your opinion's the only one that actually matters.^- Sam", Items = null, Thank = "You came to hear it! Okay that means a ton. You're my whole audience and my favorite person. Thanks, seriously." },
                new SpouseRequest { Id = "sam_game", Note = "Dude,^Let's hang out and just goof around today, yeah? Games, music, whatever. I miss messing around with you. Make some time?^- Sam", Items = null, Thank = "Best time. Seriously. Doesn't matter what we do as long as it's with you. Thanks for hanging, superstar." } },
            ["Abigail"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "abby_amethyst", Note = "Hey adventurer,^Bring me an amethyst, would you? Purple's my color and you know it. Bonus points if you pulled it out of the ground yourself. ;)^- Abigail", Items = new[] { "Amethyst" }, Thank = "An amethyst! My favorite. You get me, you really do. Okay, you've earned a kiss. Get over here." },
                new SpouseRequest { Id = "abby_mines", Note = "Hey,^Let's do something together, just us — meet me and we'll cause a little trouble. I've got energy to burn and I want you with me. Don't leave me hanging.^- Abigail", Items = null, Thank = "There you are! Just us against the world for a bit. THIS is what I married you for. Best." },
                new SpouseRequest { Id = "abby_late", Note = "You,^Stay up late with me tonight. No farm talk, no responsibilities. Just us and the dark and bad ideas. Come on. You know you want to.^- Abigail", Items = null, Thank = "You stayed up with me. Knew you would. These are my favorite hours, and you're my favorite person. Don't tell anyone I'm this soft." } },
            ["Haley"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "haley_sunflower", Note = "Hey gorgeous,^Bring me a sunflower, would you? They're cheerful and pretty and they remind me of summer. And of you, kind of. Don't read into that.^- Haley", Items = new[] { "Sunflower" }, Thank = "A sunflower! It's perfect. You're so getting a kiss for this. Ugh, you're too cute. Stop it." },
                new SpouseRequest { Id = "haley_beach", Note = "You,^Take me to the beach sometime soon, okay? I want sun and pretty views and you carrying my stuff. It'll be adorable. Don't argue.^- Haley", Items = null, Thank = "The beach with you... okay, that was actually perfect. I got the cutest mental picture of us. Thank you, gorgeous." },
                new SpouseRequest { Id = "haley_gold", Note = "Hey,^Bring me something gold-colored and shiny. I have refined taste and you knew that when you married me. Spoil me a little. xoxo^- Haley", Items = new[] { "Gold Bar" }, Thank = "Ooh, shiny. You spoil me and I love it. ...And you. I love you. There, I said it. Happy?" } },
            ["Emily"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "emily_cloth", Note = "Beloved,^Would you bring me some cloth? I feel a creation calling to me, and I want to weave a little of our energy into it. Whenever the universe allows.^- Emily", Items = new[] { "Cloth" }, Thank = "Cloth! I can already feel what it wants to become. A piece of us, woven together. Thank you, my love.", RewardItem = "(S)1004", RewardLine = "The weaving became this — I sewed you a shirt, and dyed it by hand with colors that felt like us. Wear it out in the fields, beloved, and a little piece of me goes with you everywhere." },
                new SpouseRequest { Id = "emily_gem", Note = "My love,^A gemstone is calling to me — its energy would be perfect for what I'm sensing. Would you bring me one? The crystals will thank you.^- Emily", Categories = new[] { -2 }, Thank = "Oh, its energy is humming! Perfect. You always find exactly what the moment needs. Thank you, beloved." },
                new SpouseRequest { Id = "emily_meditate", Note = "Beloved,^Sit and breathe with me today? Just the two of us, aligning our energy. I miss the quiet hum of being still beside you.^- Emily", Items = null, Thank = "Our energies, perfectly in sync. I feel centered for the first time in days. Thank you for being still with me." } },
            ["Leah"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "leah_forage", Note = "Hey you,^Bring me something foraged, would you? A bit of the wild for inspiration. Something you found out there, thinking of me. That's the good stuff.^- Leah", Categories = new[] { -81 }, Thank = "Something straight from the wild — and you thought of me. That's the most romantic thing, honestly. Thank you." },
                new SpouseRequest { Id = "leah_salad", Note = "You,^I'm craving a fresh salad and I'm too deep in a sculpture to make one. Would you whip one up for me? Rustic and good, the way I like it.^- Leah", Items = new[] { "Salad" }, Thank = "A salad, made by you. Simple and perfect, just like this life. Thank you, love." },
                new SpouseRequest { Id = "leah_studio", Note = "Hey,^Come spend some time in the studio with me? You're the only person I let watch me work, and lately I just want you near while I create.^- Leah", Items = null, Thank = "You sat with me while I worked. You're the only one I let do that, you know. It means everything. Thank you." } },
            ["Elliott"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "elliott_pom", Note = "My darling,^Might you bring me a pomegranate? Its jeweled seeds inspire me endlessly, and the thought of sharing one with you inspires me more.^- Elliott", Items = new[] { "Pomegranate" }, Thank = "A pomegranate, ruby and resplendent! And from your hand. I shall write something glorious. Thank you, my muse.", RewardItem = "(O)Book_Friendship", RewardLine = "It's finished — the piece your pomegranate stirred in me. I had it bound, just one copy, just for you. A little book about loving and being loved. Read it knowing every word is yours." },
                new SpouseRequest { Id = "elliott_docks", Note = "My love,^Walk with me down by the water sometime soon? The sea and I have much to say, and it all sounds finer with you beside me.^- Elliott", Items = null, Thank = "The tide, the salt air, and you. The perfect trio. My heart is full and my next chapter, assured. Thank you." },
                new SpouseRequest { Id = "elliott_inspire", Note = "Darling,^Bring me something — anything — that stirred a thought in you today. I wish to see the world through your eyes. It's where I find my best lines.^- Elliott", Items = null, Thank = "You brought me a piece of your day, your thoughts. That's worth more than any treasure. You inspire me endlessly." } },
            ["Harvey"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "harvey_coffee", Note = "My dear,^Would you bring me a coffee? I've a long day ahead and the thought of a cup from you would carry me through it splendidly.^- Harvey", Items = new[] { "Coffee" }, Thank = "Coffee — and from you, no less. My day is immeasurably improved. Thank you, dear. You take such good care of me." },
                new SpouseRequest { Id = "harvey_meal", Note = "My dear,^Would you make me a wholesome meal? As a doctor I really ought to eat better, and as your spouse, I'd much rather you fed me. A win-win.^- Harvey", Categories = new[] { -7 }, Thank = "A proper meal, made with care. Doctor's orders followed at last — and so lovingly. Thank you, my dear." },
                new SpouseRequest { Id = "harvey_walk", Note = "Dear,^Would you take a walk with me through town sometime? Nothing in particular — just your company and the fresh air. It does wonders for the heart.^- Harvey", Items = null, Thank = "A simple walk, your hand in mine. The best medicine I know. Thank you for the time, my love." } },
            ["Alex"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "alex_meal", Note = "Hey babe,^Cook me up something, would ya? An athlete's gotta eat, and your cooking beats anything. Plus I just like stuff you made for me.^- Alex", Categories = new[] { -7 }, Thank = "You cooked for me? Aw, babe. Tastes even better knowin' you made it. Thanks. You're the best." },
                new SpouseRequest { Id = "alex_beach", Note = "Babe,^Let's hit the beach together, just us. Toss a ball around, watch the water. I like showin' you off, not gonna lie.^- Alex", Items = null, Thank = "Beach day with you. Doesn't get better. I'm the luckiest guy in the valley and everybody knows it. Thanks, babe." },
                new SpouseRequest { Id = "alex_gift", Note = "Hey,^Bring me a little somethin', anything from you, today? Dunno why, just been missin' you. Don't make it weird. ...Okay it's a little weird. Do it anyway.^- Alex", Items = null, Thank = "You brought me somethin'. ...Man. That got me. Thanks, babe. Means more than I can say without gettin' mushy." } },
            ["Sebastian"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "seb_tear", Note = "Hey,^Bring me a frozen tear if you find one in the caves? They're kind of perfect — cold and quiet and they catch the light. Reminds me of good things. Like you.^- Sebastian", Items = new[] { "Frozen Tear" }, Thank = "A frozen tear. ...You actually found one. That's — yeah. Thanks. I'll keep it. Right where I'll see it." },
                new SpouseRequest { Id = "seb_part", Note = "Hey,^I need a component for a build I'm working on. Bring me a refined quartz? Don't ask what it does. ...Okay you can ask. I'll explain everything. Loudly.^- Sebastian", Items = new[] { "Refined Quartz" }, Thank = "Perfect part. The build's gonna work now. ...And I get to ramble at you about it. Win-win. Thanks.", RewardItem = "Mini-Jukebox", RewardLine = "Hey. That build I was working on? Done. ...I made you one too. It's a jukebox. Now you can play our music wherever you want. Don't make a thing of it. ...Okay, maybe make a little thing of it." },
                new SpouseRequest { Id = "seb_late", Note = "Hey,^Stay up late with me tonight? Just us and the quiet and maybe a game. I do my best talking when the rest of the world's asleep. Come find me.^- Sebastian", Items = null, Thank = "You stayed up with me. The quiet hours are the only ones I really like. ...They're better with you. Thanks." } },
            ["Shane"] = new List<SpouseRequest> {
                new SpouseRequest { Id = "shane_popper", Note = "Hey,^Bring me some pepper poppers, would ya? It's stupid, but it's my comfort food. And one made by you would hit different. Don't make it a thing.^- Shane", Items = new[] { "Pepper Poppers" }, Thank = "Pepper poppers. From you. ...Okay that actually got me. Thanks. Means more than it should. C'mere." },
                new SpouseRequest { Id = "shane_meal", Note = "Hey,^Cook me somethin' homemade? I, uh — I like eatin' stuff you made. Feels like bein' taken care of. Not somethin' I'm used to. So. Yeah.^- Shane", Categories = new[] { -7 }, Thank = "You cooked for me. ...Nobody used to do that. Feels good. Real good. Thanks. I mean it." },
                new SpouseRequest { Id = "shane_ranch", Note = "Hey,^Come by and spend some time with me, would ya? Watch the animals, do nothin' in particular. The quiet's better when you're in it.^- Shane", Items = null, Thank = "You came by. The chickens missed you. ...I missed you. There, I said it. Don't make it weird. Thanks." } }
        };
        private static readonly List<SpouseRequest> GenericRequests = new List<SpouseRequest>
        {
            new SpouseRequest { Id = "gen_meal", Note = "My love,^Would you make me a home-cooked meal sometime soon? Being fed by you is one of my favorite things.^- Your spouse", Categories = new[] { -7 }, Thank = "A meal made by your hands. There's nothing better. Thank you, my love." },
            new SpouseRequest { Id = "gen_forage", Note = "My love,^Bring me something you foraged out there, would you? A little piece of the world, gathered with me in mind.^- Your spouse", Categories = new[] { -81 }, Thank = "Something from the wild, gathered for me. That's so thoughtful. Thank you." },
            new SpouseRequest { Id = "gen_gem", Note = "My love,^Would you bring me a gemstone? Something that caught your eye and made you think of me.^- Your spouse", Categories = new[] { -2 }, Thank = "A gem that made you think of me. I'll treasure it. Thank you." },
            new SpouseRequest { Id = "gen_flower", Note = "My love,^Would you bring me a flower? Something pretty to brighten the house and remind me of you.^- Your spouse", Categories = new[] { -80 }, Thank = "A flower, just because. You always know how to make a day brighter. Thank you." },
            new SpouseRequest { Id = "gen_time", Note = "My love,^Would you spend a little time with me today? I just miss being near you. That's all.^- Your spouse", Items = null, Thank = "Just having you near is all I ever really want. Thank you for the time, my love." }
        };
        public static List<SpouseRequest> GetRequests(string name)
        {
            List<SpouseRequest> src = IsVanilla(name) && Requests.ContainsKey(name) ? Requests[name] : GenericRequests;
            var outList = new List<SpouseRequest>(src.Count);
            foreach (SpouseRequest r in src)
            {
                outList.Add(new SpouseRequest
                {
                    Id = r.Id,
                    Items = r.Items,
                    Categories = r.Categories,
                    RewardItem = r.RewardItem,
                    RewardQty = r.RewardQty,
                    Note = I18n.Get($"request.{r.Id}.note", r.Note),
                    Thank = I18n.Get($"request.{r.Id}.thank", r.Thank),
                    RewardLine = r.RewardLine == null ? null : I18n.Get($"request.{r.Id}.reward", r.RewardLine)
                });
            }
            return outList;
        }

        /// <summary>Add every request note/thank key + English to the i18n default map (keyed by stable request Id).</summary>
        internal static void CollectRequestDefaults(IDictionary<string, string> map)
        {
            foreach (var kv in Requests)
                foreach (SpouseRequest r in kv.Value)
                    AddRequestDefaults(map, r);
            foreach (SpouseRequest r in GenericRequests)
                AddRequestDefaults(map, r);
        }

        private static void AddRequestDefaults(IDictionary<string, string> map, SpouseRequest r)
        {
            map[$"request.{r.Id}.note"] = r.Note;
            map[$"request.{r.Id}.thank"] = r.Thank;
            if (r.RewardLine != null)
                map[$"request.{r.Id}.reward"] = r.RewardLine;
        }
    }
}
