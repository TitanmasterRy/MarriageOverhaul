# Changelog

All notable changes to Marriage Overhaul are documented here.

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
