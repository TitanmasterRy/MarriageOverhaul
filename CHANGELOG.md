# Changelog

All notable changes to Marriage Overhaul are documented here.

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
