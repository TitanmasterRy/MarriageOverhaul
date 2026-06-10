# Marriage Overhaul — Loot & Reward Reference

A reference for the items spouses can give and the dialogue tied to them. All values below
are the defaults; everything is tunable in `config.json` / GMCM.

---

## 🌿 Forage Loot Tables

When your spouse does the **forage** chore (and `EnableForageLoot` is on), they bring back
`ForageHaulQuantity` items (default **3**) rolled from their personal table. Each item rolls a
rarity tier using the configured weights; higher-quality foraging (high hearts + good mood)
skews toward the better tiers.

**Default tier weights:** Common **55** · Uncommon **30** · Rare **15**
*(high-quality foraging halves Common weight and doubles Rare weight)*

| Spouse | Common | Uncommon | Rare |
|---|---|---|---|
| **Abigail** | Daffodil, Hazelnut, Common Mushroom, Cave Carrot | Quartz, Amethyst, Purple Mushroom | Geode, Frozen Geode |
| **Penny** | Wild Horseradish, Spring Onion, Daffodil, Common Mushroom | Holly, Morel, Chanterelle | Sweet Pea, Fiddlehead Fern |
| **Haley** | Daffodil, Dandelion, Tulip, Sweet Pea | Crocus, Fairy Rose | Summer Spangle, Coconut |
| **Emily** | Wild Horseradish, Leek, Common Mushroom, Holly | Fiddlehead Fern, Snow Yam | Coral, Rainbow Shell |
| **Leah** | Wild Horseradish, Hazelnut, Blackberry, Common Mushroom, Morel | Chanterelle, Purple Mushroom, Wild Plum | Truffle, Magma Cap |
| **Maru** | Quartz, Common Mushroom, Spring Onion | Fire Quartz, Frozen Tear | Refined Quartz, Earth Crystal |
| **Alex** | Daffodil, Leek, Cave Carrot | Coconut, Cactus Fruit | Ostrich Egg, Coral |
| **Elliott** | Cockle, Mussel, Seaweed, Daffodil | Coral, Nautilus Shell, Coconut | Rainbow Shell, Pearl |
| **Harvey** | Wild Horseradish, Spring Onion, Common Mushroom | Holly, Snow Yam, Winter Root | Chanterelle, Quartz |
| **Sam** | Daffodil, Dandelion, Cave Carrot, Leek | Coconut, Cactus Fruit | Coral, Quartz |
| **Sebastian** | Common Mushroom, Hazelnut, Blackberry | Purple Mushroom, Chanterelle, Magma Cap | Truffle, Frozen Tear |
| **Shane** | Wild Horseradish, Cave Carrot, Common Mushroom | Hazelnut, Holly, Blackberry | Void Mushroom, Quartz |
| **Generic** *(modded spouses)* | Wild Horseradish, Daffodil, Leek, Common Mushroom, Dandelion | Hazelnut, Blackberry, Holly, Morel | Chanterelle, Purple Mushroom, Coconut, Quartz |

---

## 💎 Prismatic Shard Jackpot

Every spouse has a tiny chance per forage (`ForagePrismaticShardChance`, default **0.5%**) to
find a **Prismatic Shard**. The shard is handed to you (or left in the fridge if your inventory
is full), and the spouse reacts with their own shocked/confused line when you talk to them that
morning.

| Spouse | Reaction |
|---|---|
| **Abigail** | "Okay, WHAT is this?! It was just sitting in the dirt, glowing like it was radioactive! It's giving me chills — and also I kind of want to lick it? Here, you take it before I do something dumb." |
| **Penny** | "I... I found this by the river and I just stared at it for ten minutes. It's so beautiful it's almost frightening. I don't even know what it's called. Here — I think this should belong to you." |
| **Haley** | "Okay, this is either the prettiest rock I have ever seen or I just found something that's going to make us famous. It changes color when I tilt it?! I literally screamed. Here, I can't be trusted with this." |
| **Emily** | "Whoa. Whoa. My aura just did something it has never done before. This thing is humming with energy I can't even name — and I have names for a LOT of energy. I think... I think this needs to be with you." |
| **Leah** | "I've foraged that forest for years and I have never — NEVER — seen anything like this. It's not a mineral, it's not a mushroom, it's like... light, but solid? I don't trust myself with it. Here." |
| **Maru** | "I ran every test I know on this and my instruments just... stopped working. Readings off the charts, then nothing. I have absolutely no scientific explanation. This is either incredible or mildly terrifying. You should hold onto it." |
| **Alex** | "Dude. DUDE. I tripped over this on my run and almost broke my ankle, but look at it! It's like every color at once! I don't know what it is but it feels important. Here, you're smarter about this stuff." |
| **Elliott** | "The tide deposited the strangest thing at my feet this morning — a shard of what looks like captured starlight. I have no verse for this; words fail me entirely, which, as you know, is exceedingly rare. It should be yours." |
| **Harvey** | "I, ah, found this on my walk and I'll admit — I checked it for a pulse. It doesn't have one. But it's warm, and it shifts color, and I genuinely cannot identify it. Please, take it before I write a concerned report about it." |
| **Sam** | "Bro. BRO. I wiped out on my board and this thing just... rolled out from under a rock?! It's like a tiny disco ball but it's a ROCK?! I don't know what it is but it's the raddest thing I've ever found. Here, hold this." |
| **Sebastian** | "...Found this under a log. Stared at it for way too long. It's not a rock. It's not glass. It's doing something with the light that shouldn't be possible. I don't really want it in my room. Here, you take it." |
| **Shane** | "So I'm out back with the chickens and I find... this. Thing. It's glowing. Like, actually glowing. I almost chucked it in the recycling. Glad I didn't, I guess? Here. No idea what it is, but it's yours now." |
| **Generic** | "I found the strangest thing while I was out — it's small, it shimmers with every color at once, and I have absolutely no idea what it is. Here, I think you should have it." |

---

## 🏆 Skill Milestone Rewards

When any skill (**Farming, Fishing, Foraging, Mining, Combat**) reaches **level 5** or **level
10**, your spouse gives special one-time dialogue and a gift. Each milestone fires exactly once
per save. If several land on the same day, you still get every gift but only one scene plays.

| Setting | Default | Notes |
|---|---|---|
| `EnableSkillMilestoneRewards` | `true` | Master toggle |
| `MilestoneGiftItemId` | `Life Elixir` | Item handed over (resolved by name) |
| `MilestoneGiftQuantity` | `1` | How many |

Dialogue is personalized per vanilla spouse (level 5 and level 10 variants) with a generic
fallback for modded spouses, and references the skill and milestone reached.

---

## 🎂 Birthday Gifts

On your configured birthday (`PlayerBirthdaySeason` + `PlayerBirthdayDay`; day `0` = disabled),
your spouse has a chance (`BirthdayGiftChance`, default **50%**) to give a **generally useful
item** instead of their usual personalized breakfast. Gifts are handed directly to you (or left
in the fridge if full).

**Helpful-gift pool:** Coffee · Triple Shot Espresso · Energy Tonic · Spicy Eel · Field Snack · Life Elixir

**Personalized breakfast gift (per spouse, the other ~50%):**

| Spouse | Breakfast Gift | | Spouse | Breakfast Gift |
|---|---|---|---|---|
| **Abigail** | Pumpkin Pie | | **Maru** | Complete Breakfast |
| **Penny** | Pancakes | | **Alex** | Complete Breakfast |
| **Haley** | Pink Cake | | **Elliott** | Crab Cakes |
| **Emily** | Salad | | **Harvey** | Omelet |
| **Leah** | Salad | | **Sam** | Pancakes |
| **Sebastian** | Pizza | | **Shane** | Pepper Poppers |
| **Generic** | Complete Breakfast | | | |

The spouse also has a "pointed" follow-up line if you missed your anniversary in the past year.

---

## 📅 Calendar Markers

| Marker | Config | Default | Icon |
|---|---|---|---|
| Anniversary | `AnniversaryCalendarMarker` | off | White heart (top-left of the day) |
| Birthday | `BirthdayCalendarMarker` | off | Gold heart (top-right of the day) |

Both draw only on the matching season's calendar page, on the correct day, and never on the
daily-quest / special-orders board.
