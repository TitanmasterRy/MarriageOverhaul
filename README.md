# Marriage Overhaul

A deep marriage overhaul for [Stardew Valley](https://www.stardewvalley.net/) that makes married life feel alive. Your spouse now has moods, expects to be fed, gets jealous, remembers your anniversary, picks fights when things go wrong, and may even leave you if you neglect them. Every system is fully toggleable and works with **vanilla and modded spouses** alike.

## Features

Eight independent systems, each individually configurable:

### 🍳 Feeding
Every day is either a **cooking day** (your spouse handles dinner) or a **player-provides day** (they expect food left in the fridge). The weekly schedule is generated with a little randomness so it never feels mechanical, and your spouse hints at it in their morning dialogue. Leave food on a provides day for **+15 friendship**; forget and they go to bed hungry for **−40** and a grumpy morning.

### 💢 Arguments
When friendship drops below the configured threshold, an argument can break out in the farmhouse in the evening. Each argument is a **dialogue tree** — good responses recover **+50**, bad responses cost an extra **−80**. Every vanilla spouse has at least **three unique, in-character argument scenarios** that rotate so the same one never repeats back to back.

### 💔 Divorce Warning & Auto-Divorce
Let things slide too far and your spouse mails an in-character **warning letter**. Ignore it and let friendship stay low for several consecutive days, and they will **initiate divorce automatically** after a personalized farewell scene.

### 😒 Jealousy
Give a gift to someone your spouse considers a rival and they might notice — losing **−15 friendship** and giving you an earful the next morning. The odds rise if the gift was a loved item.

### 🙂 Mood
Each day your spouse is **Happy, Neutral, or Grumpy**, based on recent friendship trends, whether they were fed, recent arguments, the weather (most are gloomier in the rain — though Sebastian and Abigail are cheered by it), and a dash of randomness. Mood is expressed entirely through their greeting dialogue — you only know by talking to them, and the spouse's portrait reflects their mood. Neutral days pull from a pool of everyday married-life lines.

### 💝 Anniversary
Your wedding date is remembered. Each year you get a morning reminder; gift your spouse that day for a **+200** bonus and a sweet scene, or forget and face **−100** and a disappointed morning.

### 🎁 Makeup Gifts
After an argument you handled badly, your spouse wants to make up. They drop hints about a **category** of gift (something sweet, something from nature, something homemade). Give the right kind for **+150** and a reconciliation. Regular gifts give half friendship until you do.

### 💔 Ultimate Punishment (Cheating)
If you badly neglect your spouse — letting friendship fall below the configurable threshold (default **4 hearts**) — they may begin an affair with another single marriage candidate, reveal it in a heartbroken letter, and leave you. Off by a single config toggle if it's too harsh for your game.

### ➕ Extended Systems
A further seventeen optional systems deepen married life, each individually toggleable under the **Extended Systems** GMCM section: relationship **milestones** (year 1/3/5 scenes), spouse **daily chores**, **evolving gift preferences**, **sickness**, **inside jokes**, **achievement pride**, **children interactions**, **romantic letters**, **seasonal affection**, **bad days**, a **birthday system**, **town gossip**, a **honeymoon phase**, **spouse requests**, **shared dreams**, **visitor jealousy**, and **productivity scaling**. All are personalized per vanilla spouse with generic fallbacks for modded ones. See the [changelog](CHANGELOG.md) for details.

> Note: vanilla Stardew has no player birthday, so the birthday system uses a configurable date (`PlayerBirthdaySeason` / `PlayerBirthdayDay`; day `0` disables it).

## Compatibility

All systems fall back gracefully to natural, generic behavior for **modded spouses** (such as those from Stardew Valley Expanded) that don't have personalized content defined. Nothing is hardcoded in a way that breaks when NPC mods are installed.

## Requirements

- Stardew Valley 1.6+
- [SMAPI](https://smapi.io/) 3.18.0 or later
- (Optional) [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) for in-game configuration

## Installation

1. Install [SMAPI](https://smapi.io/).
2. Download this mod and unzip it into your `Stardew Valley/Mods` folder.
3. Run the game through SMAPI.

## Configuration

Every system can be toggled and tuned. Edit `config.json` (created after the first run) or use Generic Mod Config Menu, where the options are organized under **Systems**, **Thresholds**, and **Jealousy** headers with tooltips.

| Option | Default | Description |
| --- | --- | --- |
| `EnableFeeding` | `true` | Fridge-feeding mechanic and cooking/provides schedule. |
| `EnableArguments` | `true` | Evening argument events. |
| `EnableDivorceWarning` | `true` | In-character warning letter when friendship gets low. |
| `EnableAutoDivorce` | `true` | Automatic divorce if things stay broken after the warning. |
| `EnableJealousy` | `true` | Jealousy over gifts to rival NPCs. |
| `EnableMoodSystem` | `true` | Daily Happy/Neutral/Grumpy mood dialogue. |
| `EnableAnniversary` | `true` | Yearly anniversary reminders, bonuses, and penalties. |
| `EnableMakeupGifts` | `true` | Makeup-gift state after a bad argument. |
| `EnableCheating` | `true` | The "ultimate punishment" — a neglected spouse can have an affair and leave you. |
| `ArgumentThresholdHearts` | `10` | Friendship (in hearts) below which arguments can trigger. |
| `DivorceWarningThresholdHearts` | `8` | Friendship (in hearts) below which the warning letter is sent. |
| `ConsecutiveDaysBeforeAutoDivorce` | `7` | Days below the warning threshold (after the letter) before divorce. |
| `CheatingThresholdHearts` | `4` | Friendship (in hearts) below which a neglected spouse may cheat. |
| `CheatingChance` | `0.25` | Daily chance the spouse cheats while below the cheating threshold. |
| `JealousyChance` | `0.20` | Chance the spouse notices a gift to a rival. |
| `JealousyChanceLoved` | `0.40` | Chance the spouse notices when the gift was a loved item. |
| `EnableDebugCommands` | `false` | Register the `mo_*` testing console commands (see below). |

## Debug commands

Set `EnableDebugCommands` to `true` (and restart) to register a suite of `mo_*` console commands for testing — for example `mo_status`, `mo_argue`, `mo_makeup`, `mo_hungry`, `mo_jealousy`, `mo_warn`, `mo_anniversary`, `mo_mood <mood>`, `mo_hearts <n>`, and `mo_cheat`. These are intended for testing and development and are off by default.

## Translations

All dialogue is translatable through SMAPI's translation system. The English source lives in
[`i18n/default.json`](i18n/default.json). To add a language, copy that file to `i18n/<locale>.json`
(e.g. `i18n/es.json` for Spanish — see SMAPI's locale codes) and translate the values, leaving the
keys unchanged. Any line you don't translate falls back to English automatically. A few values are
deliberately left as-is because they're identifiers, not dialogue (item names like `Pumpkin Pie`,
and makeup gift categories) — translating those would break item resolution. Contributions welcome.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the full version history.

## License

You are free to use, modify, and learn from this mod.
