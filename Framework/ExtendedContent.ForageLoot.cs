using System.Collections.Generic;

namespace MarriageOverhaul
{
    public static partial class ExtendedContent
    {
        // ── Spouse forage loot tables ─────────────────────────────
        // When a spouse does the "forage" chore they bring back items rolled from their personal
        // table (tiered by rarity). Every spouse also has a very small chance to find a Prismatic
        // Shard, which triggers their unique shocked/confused JackpotReaction line.
        public class ForageTable
        {
            public string[] Common;
            public string[] Uncommon;
            public string[] Rare;
            /// <summary>Shown (in place of the normal forage line) when the spouse finds a Prismatic Shard.</summary>
            public string JackpotReaction;
        }

        private static readonly Dictionary<string, ForageTable> ForageTables = new Dictionary<string, ForageTable>
        {
            ["Abigail"] = new ForageTable
            {
                Common   = new[] { "Daffodil", "Hazelnut", "Common Mushroom", "Cave Carrot" },
                Uncommon = new[] { "Quartz", "Amethyst", "Purple Mushroom" },
                Rare     = new[] { "Geode", "Frozen Geode" },
                JackpotReaction = "Okay, WHAT is this?! It was just sitting in the dirt, glowing like it was radioactive! It's giving me chills - and also I kind of want to lick it? Here, you take it before I do something dumb."
            },
            ["Penny"] = new ForageTable
            {
                Common   = new[] { "Wild Horseradish", "Spring Onion", "Daffodil", "Common Mushroom" },
                Uncommon = new[] { "Holly", "Morel", "Chanterelle" },
                Rare     = new[] { "Sweet Pea", "Fiddlehead Fern" },
                JackpotReaction = "I... I found this by the river and I just stared at it for ten minutes. It's so beautiful it's almost frightening. I don't even know what it's called. Here - I think this should belong to you."
            },
            ["Haley"] = new ForageTable
            {
                Common   = new[] { "Daffodil", "Dandelion", "Tulip", "Sweet Pea" },
                Uncommon = new[] { "Crocus", "Fairy Rose" },
                Rare     = new[] { "Summer Spangle", "Coconut" },
                JackpotReaction = "Okay, this is either the prettiest rock I have ever seen or I just found something that's going to make us famous. It changes color when I tilt it?! I literally screamed. Here, I can't be trusted with this."
            },
            ["Emily"] = new ForageTable
            {
                Common   = new[] { "Wild Horseradish", "Leek", "Common Mushroom", "Holly" },
                Uncommon = new[] { "Fiddlehead Fern", "Snow Yam" },
                Rare     = new[] { "Coral", "Rainbow Shell" },
                JackpotReaction = "Whoa. Whoa. My aura just did something it has never done before. This thing is humming with energy I can't even name - and I have names for a LOT of energy. I think... I think this needs to be with you."
            },
            ["Leah"] = new ForageTable
            {
                Common   = new[] { "Wild Horseradish", "Hazelnut", "Blackberry", "Common Mushroom", "Morel" },
                Uncommon = new[] { "Chanterelle", "Purple Mushroom", "Wild Plum" },
                Rare     = new[] { "Truffle", "Magma Cap" },
                JackpotReaction = "I've foraged that forest for years and I have never - NEVER - seen anything like this. It's not a mineral, it's not a mushroom, it's like... light, but solid? I don't trust myself with it. Here."
            },
            ["Maru"] = new ForageTable
            {
                Common   = new[] { "Quartz", "Common Mushroom", "Spring Onion" },
                Uncommon = new[] { "Fire Quartz", "Frozen Tear" },
                Rare     = new[] { "Refined Quartz", "Earth Crystal" },
                JackpotReaction = "I ran every test I know on this and my instruments just... stopped working. Readings off the charts, then nothing. I have absolutely no scientific explanation. This is either incredible or mildly terrifying. You should hold onto it."
            },
            ["Alex"] = new ForageTable
            {
                Common   = new[] { "Daffodil", "Leek", "Cave Carrot" },
                Uncommon = new[] { "Coconut", "Cactus Fruit" },
                Rare     = new[] { "Ostrich Egg", "Coral" },
                JackpotReaction = "Dude. DUDE. I tripped over this on my run and almost broke my ankle, but look at it! It's like every color at once! I don't know what it is but it feels important. Here, you're smarter about this stuff."
            },
            ["Elliott"] = new ForageTable
            {
                Common   = new[] { "Cockle", "Mussel", "Seaweed", "Daffodil" },
                Uncommon = new[] { "Coral", "Nautilus Shell", "Coconut" },
                Rare     = new[] { "Rainbow Shell", "Pearl" },
                JackpotReaction = "The tide deposited the strangest thing at my feet this morning - a shard of what looks like captured starlight. I have no verse for this; words fail me entirely, which, as you know, is exceedingly rare. It should be yours."
            },
            ["Harvey"] = new ForageTable
            {
                Common   = new[] { "Wild Horseradish", "Spring Onion", "Common Mushroom" },
                Uncommon = new[] { "Holly", "Snow Yam", "Winter Root" },
                Rare     = new[] { "Chanterelle", "Quartz" },
                JackpotReaction = "I, ah, found this on my walk and I'll admit - I checked it for a pulse. It doesn't have one. But it's warm, and it shifts color, and I genuinely cannot identify it. Please, take it before I write a concerned report about it."
            },
            ["Sam"] = new ForageTable
            {
                Common   = new[] { "Daffodil", "Dandelion", "Cave Carrot", "Leek" },
                Uncommon = new[] { "Coconut", "Cactus Fruit" },
                Rare     = new[] { "Coral", "Quartz" },
                JackpotReaction = "Bro. BRO. I wiped out on my board and this thing just... rolled out from under a rock?! It's like a tiny disco ball but it's a ROCK?! I don't know what it is but it's the raddest thing I've ever found. Here, hold this."
            },
            ["Sebastian"] = new ForageTable
            {
                Common   = new[] { "Common Mushroom", "Hazelnut", "Blackberry" },
                Uncommon = new[] { "Purple Mushroom", "Chanterelle", "Magma Cap" },
                Rare     = new[] { "Truffle", "Frozen Tear" },
                JackpotReaction = "...Found this under a log. Stared at it for way too long. It's not a rock. It's not glass. It's doing something with the light that shouldn't be possible. I don't really want it in my room. Here, you take it."
            },
            ["Shane"] = new ForageTable
            {
                Common   = new[] { "Wild Horseradish", "Cave Carrot", "Common Mushroom" },
                Uncommon = new[] { "Hazelnut", "Holly", "Blackberry" },
                Rare     = new[] { "Void Mushroom", "Quartz" },
                JackpotReaction = "So I'm out back with the chickens and I find... this. Thing. It's glowing. Like, actually glowing. I almost chucked it in the recycling. Glad I didn't, I guess? Here. No idea what it is, but it's yours now."
            }
        };

        private static readonly ForageTable GenericForage = new ForageTable
        {
            Common   = new[] { "Wild Horseradish", "Daffodil", "Leek", "Common Mushroom", "Dandelion" },
            Uncommon = new[] { "Hazelnut", "Blackberry", "Holly", "Morel" },
            Rare     = new[] { "Chanterelle", "Purple Mushroom", "Coconut", "Quartz" },
            JackpotReaction = "I found the strangest thing while I was out - it's small, it shimmers with every color at once, and I have absolutely no idea what it is. Here, I think you should have it."
        };

        public static ForageTable GetForageTable(string name)
            => IsVanilla(name) && ForageTables.ContainsKey(name) ? ForageTables[name] : GenericForage;
    }
}
