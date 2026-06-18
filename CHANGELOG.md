# Changelog

All notable changes to Marriage Overhaul are documented here.

## 1.8.0

### Added — Custom NPC Framework: forage loot tables
- Content packs can now give a custom NPC their own **forage loot table** via a new optional `Loot` section (Common / Uncommon / Rare tiers plus a Prismatic Shard jackpot reaction line). When that NPC does the forage chore, they bring back themed items from their own table instead of the generic one. Resolves through the same vanilla → custom → generic tier as everything else, so existing behavior is unchanged.
- Loot entries (and other item references) accept either plain names (`"Sunflower"`) or fully-qualified IDs (`"(O)ModId_Item"`), so packs can point at modded items precisely.
- Documented in the author guide and the example content pack.

## 1.7.3

### Added
- **Polyamory / multi-spouse compatibility (optional).** For multi-spouse mods like **Polyamory Sweet Love** or **Free Love**, a new **Polyamory / Multi-Spouse Compatibility** option (off by default) gives every one of your spouses the overhaul's morning greetings, instead of only your primary spouse. Spouse detection reads vanilla friendship data, so it's mod-agnostic. The stateful relationship systems (feeding, anniversary, divorce, jealousy, makeup, requests, etc.) still apply to your primary spouse only, by design.
- **"Spouses Cook For You" compatibility (optional).** A new **'Spouses Cook For You' Compatibility** option (off by default) makes Marriage Overhaul treat every day as a spouse cooking day, so it never asks you to leave food in the fridge, never eats the meals that mod prepares, and never applies a hunger penalty. The spouse's cooking dialogue still plays.

### Fixed
- **Roommates (Krobus) are no longer forced into a romantic relationship.** The mod was treating a roommate exactly like a spouse, giving them romantic morning greetings, jealousy, anniversaries, and so on. Roommates are now left platonic with their normal vanilla dialogue. A new **Overhaul a Roommate (Krobus)** option (off by default) can re-enable the full overhaul for a roommate if you want it.
- **"Spend quality time" spouse requests can now actually be completed by spending time together.** These requests (e.g. "spend a little time with me at home", "take a walk with me") previously could only be finished by handing over a gift, which contradicted the quest objective. Talking to your spouse on the request day now completes them (giving any gift still works too).

### Notes
- The new roommate option is added to all three shipped languages (English, Simplified Chinese, German).
- Note: a roommate trying to share the bed is controlled by your roommate/multi-partner mod or the base game, not by Marriage Overhaul, so it isn't affected by this change.

## 1.7.2

### Translations
- **Added a German (`de`) translation.** Huge thanks to **Neko41** for the translation — it covers all dialogue, the new 1.7.0 content, and the config/GMCM menu.
- **Completed the Simplified Chinese (`zh`) translation.** Every line added or restructured in 1.7.0 (the new morning greetings, mood pools, general lines, and the config menu) is now fully translated, so Chinese no longer mixes with English. Thanks to **10100306**.

> Both translations have full key parity with the English source, with placeholders and formatting verified. Untranslated lines (in any language) still fall back to English automatically.

## 1.7.0

### Added — Custom NPC Framework
- **Content packs can now personalize custom (modded) NPCs.** Other modders can give their own custom spouses the same kind of personalized content the vanilla spouses get, using a **plain JSON content pack — no C# or compiling required.** A pack just declares `"ContentPackFor": { "UniqueID": "TitanmasterRy.MarriageOverhaul" }` in its manifest and ships a `content.json`.
- **Three-tier content resolution:** vanilla built-in content → registered custom content → generic fallback. The framework hooks in as a new middle tier inside the existing content getters, so **vanilla and generic-fallback behavior are unchanged** — a custom NPC with no pack behaves exactly as before.
- **Fully modular.** Every section is optional and falls back to the generic pool independently (per system, and per friendship tier for morning greetings). Authors can personalize just one system and leave the rest generic. Covered systems: friendship-tiered **morning greetings**, **mood greetings** (Happy / Neutral / Grumpy), **argument** dialogue trees, **anniversary** lines, **jealousy** lines + an optional rival-NPC list, **makeup-gift** hints, and a **behavior** block (rain mood preference, favorite/least season, per-NPC system allow-list).
- **Robust loading.** Packs are discovered automatically at launch; malformed files or incomplete sections are skipped with a clear SMAPI warning instead of crashing, and a summary of which NPCs were registered from which pack is logged. New optional sections can be added in future versions without breaking existing packs.
- **Optional C# API** via `GetApi()` (`ICustomNpcApi`) for advanced mods that prefer to register content programmatically, reusing the same registry and validation.
- **Author guide + example pack.** A full guide lives in [`docs/Custom-NPC-Framework.md`](docs/Custom-NPC-Framework.md), a complete copy-and-edit example pack in [`examples/[MO] Aria Example`](examples/), and a quick-start section in the README.

> Pack text is shown verbatim and is **not** run through Marriage Overhaul's i18n — pack authors supply their own dialogue and handle their own translations (e.g. one pack per language).

### Added — More everyday dialogue variety
- **Doubled the friendship-tiered morning greetings**, from 4 to **8 lines per tier** for all 12 vanilla spouses plus the generic/modded-spouse fallback (208 new lines). The greeting you see every morning now repeats far less often (and still avoids repeating yesterday's line).
- **Mood greetings are now randomized pools.** The Happy and Grumpy morning lines went from a single fixed line per spouse to a **pool of 4** each (original line kept), and the neutral-day **general pool grew from 24 to 40 lines**.
- All new lines are fully translatable; `i18n/default.json` was regenerated from source with every existing line preserved.

### Config
- Added **`EnableCustomNpcFramework`** (default `true`) and **`AllowVanillaOverride`** (default `false`), both exposed through GMCM under a new **Custom NPC Framework** section with tooltips. `AllowVanillaOverride` lets packs replace built-in vanilla-spouse content; off by default so vanilla characters are never changed by a pack.

### Translations
- **Chinese (`zh`) translation now available** — thanks to **10100306** for the translation.
- **The config / GMCM menu is now translatable.** Every option label, tooltip, and section title now goes through the translation system (`config.*` keys in `i18n/default.json`), so the in-game settings menu can be localized like the rest of the mod. Previously these were hard-coded in English.

## 1.6.3

### Added
- **Multiplayer Compatibility Mode** (config / GMCM, in the Compatibility section). Turn it on in multiplayer to disable features that can freeze other players' screens — the Shared Dreams journal pop-up and the weekly "spend time with the kids" question pop-up, both of which force a dialogue open during the shared morning. Leave it off in single-player.

### Fixed
- Fixed a crash (NullReferenceException in `NPC.checkAction`) that could occur when interacting with your spouse mid-transition — e.g. pressing the action button next to them while warping through a door. The kiss check now bails if the spouse is in a transitional state (no sprite, or not in your current location). As an extra safety net, the mod's interaction patch now catches and logs any error instead of letting it crash/freeze the game.

## 1.6.2

### Fixed
- **Multiplayer: farmhands now get their morning dialogue.** A farmhand's daily routine was gated behind a data-sync flag that isn't ready yet when the day starts, so farmhands silently got nothing. Removed that gate (host save-data is still protected from being overwritten).
- **Multiplayer: fixed the black-screen freeze.** When a player got a dream journal entry (or a skill-milestone / birthday scene), the mod forced a dialogue box open during the morning transition, which stalled the new-day sync and left everyone else stuck on a black screen. These now wait until the player actually has control before showing, so they can't desync the morning.
- The mod no longer acts during the engagement period or other events/festivals — it stays out of the way of festival and event dialogue.
- Compatibility with marriage-dialogue expansion mods (e.g. Haley Ever After): this mod's greeting is held aside and shown on your first talk so a dialogue mod can't wipe it. New **Dialogue Mod Compatibility** toggle (on by default).

### Notes
- For farmhands to get *saved* progress, the host must also have Marriage Overhaul installed; otherwise the mod still works per session but won't persist between days.

## 1.6.1

### Fixed
- The mod's morning dialogue (notably the skill-milestone scene) could fire during the wedding ceremony and break it. The mod now only acts once you're actually married — while engaged (including the wedding morning, before the ceremony), it stays quiet. As a bonus, the mod no longer runs its marriage mechanics during the engagement days.
- Skill milestones no longer fire retroactively for levels you'd already reached before marrying (or before installing the mod). The first time it sees each skill it records your current level as the baseline, so only milestones you cross *during* the marriage trigger a scene and gift.
- Compatibility with marriage-dialogue expansion mods (e.g. Haley Ever After). Those mods supply a marriage line every day, and the game clears the spouse's dialogue when you talk — which was wiping this mod's morning greeting before it could show. The mod now re-shows its greeting on top of theirs, so both appear (ours first, then the dialogue mod's). New toggle **Dialogue Mod Compatibility** (on by default) in config/GMCM.

## 1.6.0

### Added
- **Translation support (i18n).** All ~1,400 dialogue lines are now translatable through SMAPI's standard translation system. A complete `i18n/default.json` ships with the mod; translators just copy it to `i18n/<locale>.json` (e.g. `es.json`) and translate the values — no code editing. Untranslated lines automatically fall back to English, and item names/identifiers are intentionally left untranslated so gifts still resolve.
- **Spouse requests are now real journal quests.** When your spouse mails a request, it now also appears as a tracked quest in your quest log — with a title, the request note, and a clear objective ("Take X to <spouse>", "Bring <spouse> a gemstone", etc.) — instead of being just a letter. Fulfilling it shows the usual "Quest Complete!" feedback. The quest is rebuilt each morning from the mod's own data and removed before saving, so it never bloats or corrupts your save. New toggle **Spouse Requests as Journal Quest** (on by default) in config/GMCM; turn it off for letter-only requests.
- **Creative/project requests now pay off with a handmade gift.** When a spouse asks for materials for something they're making, they finish it and give you the result a few days later, with a line explaining it:
  - **Maru** (refined quartz → a project) gives a **Quality Sprinkler**.
  - **Sebastian** (refined quartz → a build) gives a **Mini-Jukebox**.
  - **Elliott** (a pomegranate → inspiration to write) gives a **book** he had bound for you (Friendship 101).
  - **Emily** (cloth → a weaving) gives a **shirt** she sewed and hand-dyed for you.
  - (Other requests — food cravings, flowers, time together — are unchanged.)

### Notes
- All new quest and reward text is fully translatable. Verified every requested item is real and obtainable.

## 1.5.3

### Fixed
- The anniversary used the day the mod was first installed, not your actual wedding day — so anyone who installed the mod after getting married had their anniversary on the wrong date. The wedding date is now read from vanilla's `Friendship.WeddingDate`, which is set the day you married and preserved across mod install/uninstall. Existing saves self-heal on next load: the stored date is corrected and this year's real anniversary is re-armed so it can still fire. Modded spouses without vanilla wedding data continue to use the "first day the mod sees the marriage" fallback.

### Added
- New config/GMCM option **Feeding: Search Extra Storage** (off by default). When enabled, feeding searches mini-fridges and the cellar fridge in addition to the built-in kitchen fridge, so modded house layouts that put the fridge on a different level (or have no main-level kitchen) work correctly. Chore meals, birthday gifts, and forage haul fallbacks also store across any available fridge. Enable this if your fridge isn't on the main level of the farmhouse.

## 1.5.2

### Fixed
- The kiss/hug now also works when the spouse is walking around — interacting stops them for the kiss, instead of it only triggering while they stood still.

## 1.5.1

### Fixed
- The 1.5.0 kiss/hug fix didn't fully work — interacting showed a single dialogue line, then a kiss, and the spouse's other dialogue was lost. The vanilla kiss actually requires both the spouse's dialogue stack *and* their daily marriage dialogue to be clear, but the previous fix only handled the first. The mod now correctly sets both aside (only when you're actually positioned for a kiss: 10+ hearts, slept in bed, empty-handed, standing beside your spouse, before 10pm) so the real kiss runs and all dialogue is preserved for the next interaction. Kiss-based mods work as expected.

## 1.5.0

### Added — Multiplayer support
- **The mod now works in multiplayer.** Previously farmhands couldn't join a host running this mod, and spouse interactions broke on client machines. Every player (host and farmhands) now gets the full marriage overhaul for their own spouse, independently.
- Persistence is host-authoritative: the host stores every player's data and syncs each farmhand's copy over mod messages (sent on join, saved each night). Farmhands no longer attempt host-only save-data calls, which is what was breaking the connection.
- Existing single-player saves migrate automatically to the new per-player storage with no loss of progress.

### Added — Friendship-tiered morning dialogue
- The spouse's morning greeting now scales with your **current** friendship hearts: cold and clipped when low, flat and polite mid-range, warm when high, openly affectionate when very high. Over 200 new personalized lines (12 vanilla spouses × 4 tiers) plus generic fallback pools for modded spouses.
- Lines are randomized and won't repeat two mornings in a row.
- Integrates cleanly with the existing mood system: **friendship sets the baseline tone, mood shifts it one tier up (Happy) or down (Grumpy)** for daily variance — they never stack or double-fire. Event dialogue (arguments, divorce, sickness, anniversaries) still takes priority.
- New "Morning Dialogue" GMCM section: master toggle `FriendshipTieredMorningDialogue` plus configurable heart thresholds for each tone tier. Turn it off to restore the old single mood greeting.

### Fixed
- Farmhands being unable to join a multiplayer game with the mod installed (host-only save-data call now guarded).
- The mod's queued morning dialogue was blocking the vanilla kiss/hug (and breaking kiss-based mods). New "Allow Spouse Kiss/Hug" option (on by default): walk up to your spouse empty-handed to kiss/hug as normal, then talk again to read their dialogue. The real vanilla kiss runs, so other kiss mods work again. Can be turned off in config/GMCM.

## 1.4.0

### Added
- **Spouse forage loot tables.** When your spouse does the forage chore they now bring back items from a personalized loot table (Common / Uncommon / Rare tiers) instead of just picking up forage already on the farm. All 12 vanilla spouses have their own themed table, with a generic table for modded spouses. Higher-quality foraging skews toward better tiers.
- **Prismatic Shard jackpot.** Every spouse has a very small chance per forage to find a Prismatic Shard, with a unique shocked/confused reaction line for each spouse. The shard is handed to you (or left in the fridge if your inventory is full).
- **Skill milestone rewards.** When any of the five skills (Farming, Fishing, Foraging, Mining, Combat) reaches level 5 or 10, the spouse gives special one-time dialogue and a small gift. Personalized lines for all 12 vanilla spouses with a generic fallback. Each milestone fires exactly once per save. If several milestones land on the same morning, all give a gift but only one dialogue plays (no spam).
- **Birthday calendar marker.** Optional gold marker on your birthday on the in-game calendar (distinct from the anniversary heart). Off by default.

### Changed
- **Birthday gift overhaul.** On your birthday the spouse now has a chance to give a generally useful item (coffee, espresso, energy tonic, etc.) instead of the usual breakfast, with matching dialogue. Gifts are handed directly to you (falling back to the fridge) rather than only left in the fridge.

### Fixed
- The anniversary calendar heart now skips the daily-quest / special-orders board (it previously could try to draw on the wrong Billboard page).

### Config
- Renamed `ShowAnniversaryOnCalendar` to `AnniversaryCalendarMarker`. **This one setting will reset to its default (off) on first load** — re-enable it in config or GMCM if you had it on.
- Added: `BirthdayCalendarMarker`, `BirthdayGiftChance`, `EnableForageLoot`, `ForageHaulQuantity`, `ForageCommonWeight`, `ForageUncommonWeight`, `ForageRareWeight`, `ForagePrismaticShardChance`, `EnableSkillMilestoneRewards`, `MilestoneGiftItemId`, `MilestoneGiftQuantity` — all exposed through GMCM with tooltips.

### Notes
- All new dialogue is stored in external content files; everything is additive and individually toggleable.

## 1.3.1

### Fixed
- Mod letters (divorce warning, anniversary reminder, romantic letters, spouse requests) used fixed mail IDs and so only ever delivered once per save. They now clear the received-flag before re-queueing, so they can be sent again. This also fixes the `mo_warn` debug command "not working" after its first use.

## 1.3.0

### Added — Extended Systems
Seventeen new, individually toggleable systems (GMCM section "Extended Systems"):
- **Relationship milestones** — one-time scenes on your 1st, 3rd and 5th anniversary.
- **Spouse daily chores** — morning farm help (watering, collecting, cooking, gold, forage); chance and quality scale with hearts and recent happiness.
- **Evolving gift preferences** — after a year, the spouse comes to love new gifts tied to your history.
- **Spouse sickness** — once per season they may fall ill and need soup, medicine, or tea.
- **Inside jokes** — shared moments accumulate and resurface in casual dialogue.
- **Achievement pride** — the spouse references big accomplishments (mines, Skull Cavern, Community Center, perfection, legendary fish, skill mastery).
- **Children interactions** — references, a weekly time-with-kids request, harsher arguments with kids home.
- **Romantic letters** — handwritten love letters in the mailbox every week or two.
- **Seasonal affection** — favorite/least season per spouse affects mood, gift bonus, and decay.
- **Bad days** — the spouse occasionally has a rough day; comfort them to help.
- **Birthday system** — a special gift and scene on your (config-set) birthday.
- **Town gossip** — townsfolk mention how your spouse speaks of you.
- **Honeymoon phase** — boosted friendship and no arguments for the first weeks.
- **Spouse requests** — mailed requests to fulfill within 3 days.
- **Shared dreams** — morning journal entries describing the spouse's dreams.
- **Visitor jealousy** — dry comments when other NPCs visit the farm.
- **Productivity scaling** — chore quality follows the spouse's recent happiness.

### Added — Other
- Optional `ShowAnniversaryOnCalendar` (default off) marks your anniversary with a heart on the in-game calendar.

### Notes
- All new dialogue is stored in external content files. No existing systems were changed; everything is additive.
- Vanilla has no player birthday, so the birthday system uses a configurable `PlayerBirthdaySeason` / `PlayerBirthdayDay` (day 0 = disabled).

## 1.2.0

### Removed
- **Date nights.** The entire date-night feature — invitations, movie-theater dates, positioned cutscenes, the scene library, and all related config, save data, and debug commands — has been removed. (Date-night functionality is better handled by a dedicated mod.)

### Notes
- All other systems are unchanged: feeding, arguments, divorce warnings / auto-divorce, jealousy, mood, anniversaries, makeup gifts, and the cheating "ultimate punishment".

## 1.1.0

### Added
- **Full date-night cutscenes.** Dates can now play as real positioned events with portrait dialogue, emotes, music, and a kiss. Each vanilla spouse has **3 unique handcrafted scenes**, plus a shared pool of **15 generic scenes** and a freeform fallback assembled from a 30+ line dialogue pool.
- **Per-save scene tracking.** Unique scenes play first in random order; once seen, dates draw from the generic pool (no repeat within 3 dates); once that's exhausted, a freeform scene is assembled. Tracked per spouse, per save.
- **Movie-theater dates.** Once the theater is unlocked, some date invitations become a night at the movies.
- **Ultimate Punishment (cheating).** A spouse neglected below the cheating threshold (default 4 hearts) can begin an affair with another single marriage candidate, reveal it in an in-character letter, and leave you. Fully toggleable.
- **Mood improvements.** Mood is now sometimes fully random, and is affected by the weather (most spouses are gloomier in the rain; Sebastian and Abigail are cheered by it).
- **General dialogue pool.** Neutral-mood mornings now pull from a pool of everyday married-life lines instead of staying silent.
- **Matching facial expressions.** Upset, angry, sad, and happy lines (arguments, jealousy, hunger, grumpy moods, disappointments, etc.) now show the spouse's matching portrait expression.
- **Liveliness layer.** Cutscenes now play a musical cue and weave in small reactions (hops, emotes, the player reacting back) between lines, with a mutual lean-in kiss.
- **Debug console commands** (gated behind `EnableDebugCommands`), including `mo_listscenes`, `mo_playscene <id> [npc]` to play any scene with any NPC, `mo_datescene`, `mo_datestatus`, `mo_resetdates`, and `mo_cheat`.

### Config
- Added `EnableMovieDates`, `EnableDateCutscenes`, `EnableCheating`, `CheatingThresholdHearts`, `CheatingChance`, and `EnableDebugCommands`, all exposed through GMCM.

## 1.0.0

### Added
- Initial release with eight toggleable systems: **feeding**, **argument events**, **divorce warnings / auto-divorce**, **jealousy**, **mood**, **anniversaries**, **makeup gifts**, and **date nights**.
- Personalized content for all 12 vanilla marriage candidates, with graceful generic fallbacks for modded spouses.
- Full configuration via `config.json` and Generic Mod Config Menu.
