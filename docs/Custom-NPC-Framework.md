# Marriage Overhaul — Custom NPC Framework (author guide)

Marriage Overhaul ships personalized dialogue and behavior for the twelve vanilla marriage
candidates. Every other (modded) spouse falls back to a generic pool. The **Custom NPC Framework**
lets you, as the author of a custom NPC, replace that generic fallback with your own personalized
content — **using a plain JSON content pack. No C# and no compiling required.**

The framework resolves content in three tiers:

```
vanilla built-in content  →  your registered custom content  →  generic fallback
```

- For **your custom NPC**, anything you provide is used; anything you omit falls back to the generic
  pool, **per system**. You can fill in just morning greetings and leave everything else generic.
- For the **vanilla spouses**, your pack is ignored by default. (The player can opt into
  "Allow Vanilla Override" in the mod's config, but you should design as if they won't.)

> **Translations:** the framework does **not** run your pack text through Marriage Overhaul's i18n.
> Your strings are shown exactly as written. If you want to localize your NPC, ship one content pack
> per language (or use your own translation layer) — Marriage Overhaul will not translate it for you.

---

## 1. Content pack setup

A content pack is just a folder in `Stardew Valley/Mods/` containing two files:

```
Mods/
  [MO] My Custom NPC/
    manifest.json
    content.json
```

### manifest.json

The only thing that makes this a Marriage Overhaul content pack is the `ContentPackFor` block
pointing at the mod's UniqueID.

```json
{
  "Name": "[MO] My Custom NPC",
  "Author": "Your Name Here",
  "Version": "1.0.0",
  "Description": "Personalized Marriage Overhaul content for My Custom NPC.",
  "UniqueID": "YourName.MOMyCustomNpc",
  "ContentPackFor": {
    "UniqueID": "TitanmasterRy.MarriageOverhaul",
    "MinimumVersion": "1.6.3"
  }
}
```

- `UniqueID` must be globally unique (use your own name as a prefix).
- `ContentPackFor.UniqueID` **must** be exactly `TitanmasterRy.MarriageOverhaul`.
- SMAPI automatically discovers the pack; you don't register it anywhere else.

### content.json

The content file is a map of **NPC internal name → content**. The internal name is the NPC's
`Name` (not display name) — the same string the game uses in friendship data. You can define
multiple NPCs in one pack.

```json
{
  "Format": "1.0",
  "NPCs": {
    "MyCustomNpc": {
      "Behavior": { ... },
      "Morning": { ... },
      "Mood": { ... },
      "Arguments": [ ... ],
      "Anniversary": { ... },
      "Jealousy": { ... },
      "Makeup": { ... }
    }
  }
}
```

Every NPC entry and **every section inside it is optional.** Omit what you don't want to customize.

---

## 2. Schema reference

### `Behavior` (optional)

Per-NPC tuning that isn't dialogue.

| Field | Type | Notes |
|---|---|---|
| `LovesRain` | bool | `true` = the NPC is cheered by rain (like Sebastian/Abigail). `false`/omitted = gloomier in rain (the default for everyone else). Affects daily mood. |
| `FavoriteSeason` | string | `"spring"`, `"summer"`, `"fall"`, or `"winter"` (numbers `0`–`3` also accepted). Warmer mood + a gift bonus that season. Optional. |
| `LeastSeason` | string | Same values. Moodier + a small extra friendship decay if neglected that season. Optional. |
| `EnabledSystems` | string[] | Optional allow-list. If present, **only** the listed systems are served from your pack (others fall back to generic even if you provide data). If omitted, every section you provide applies. Valid names: `Morning`, `Mood`, `Arguments`, `Anniversary`, `Jealousy`, `Makeup`, `Loot`. |

> Seasonal *trigger* uses your preference, but the seasonal *line* shown is Marriage Overhaul's
> generic (spouse-agnostic) seasonal line. Favorite/least season is behavior tuning, not a dialogue
> section.

### `Morning` (optional) — friendship-tiered morning greetings

The greeting shown each morning. The tier is chosen from your **current friendship hearts** with the
NPC (thresholds are configurable by the player); the mood system nudges it one tier up/down.

| Field | Type | Tier meaning |
|---|---|---|
| `VeryLow` | string[] | Cold, distant, resentful (low hearts). |
| `Low` | string[] | Flat, polite but disengaged. |
| `High` | string[] | Warm and friendly. |
| `VeryHigh` | string[] | Affectionate / loving (high hearts). |

Provide as many lines per tier as you like; one is chosen at random, avoiding yesterday's line.
**Per-tier fallback:** any tier you leave out uses the generic pool for that tier only.

### `Mood` (optional) — daily mood greetings

Used when the player has "Friendship-Tiered Morning Dialogue" turned **off** (legacy mode), or
wherever the mood greeting is requested.

| Field | Type | Notes |
|---|---|---|
| `Happy` | string[] | Good-mood greeting. |
| `Neutral` | string[] | Everyday/neutral greeting (replaces the shared "general" pool for this NPC). |
| `Grumpy` | string[] | Bad-mood greeting. |

One line is chosen at random per pool. Any omitted pool falls back to generic.

### `Arguments` (optional) — argument scenarios

An array of dialogue trees. When an argument triggers, one scenario is chosen; the player picks one
of three responses, each with its own reply and relationship outcome.

Each scenario object requires **all seven** fields (incomplete scenarios are skipped with a warning):

| Field | Type | Notes |
|---|---|---|
| `Intro` | string | The NPC's opening complaint. |
| `GoodChoice` / `GoodReply` | string | The conciliatory option and the NPC's warm reply. |
| `NeutralChoice` / `NeutralReply` | string | The middling option and reply. |
| `BadChoice` / `BadReply` | string | The dismissive option and the NPC's hurt reply. |

### `Anniversary` (optional)

| Field | Type | Notes |
|---|---|---|
| `Reminder` | string | Letter body delivered the morning of your anniversary. Use `^` for line breaks (vanilla mail convention). |
| `Sweet` | string | Shown when you gift them on the anniversary. |
| `Disappointed` | string | Shown the morning after you forget. |

Provide all three for a complete experience (a provided Anniversary section is used as-is).

### `Jealousy` (optional)

| Field | Type | Notes |
|---|---|---|
| `Lines` | string[] | One is chosen when the NPC notices a gift to a rival. |
| `Rivals` | string[] | Optional explicit list of NPC internal names. When present, the NPC is **only** jealous of gifts to these NPCs. When omitted, the default rule applies (jealous of gifts to NPCs of the same gender as the NPC). |

### `Makeup` (optional) — reconciliation gifts

After a bad argument the NPC wants a category of gift to make up.

| Field | Type | Notes |
|---|---|---|
| `Category` | string | **Required** for this section. One of `"sweet"`, `"nature"`, `"homemade"` — a logic identifier that decides which items count. Invalid values disable the section (with a warning). |
| `Hint` | string | Dialogue hint at the category (not the specific item). |
| `Reconcile` | string | Shown when you give the right category. |
| `Resigned` | string | Shown if the makeup window lapses. |

### `Loot` (optional) — forage chore loot table

When the spouse does the **forage** chore, an item is rolled from these rarity tiers (using the player's
configured tier weights). Without this section, the NPC uses the generic forage table.

| Field | Type | Notes |
|---|---|---|
| `Common` | string[] | Common-tier items (most frequent). |
| `Uncommon` | string[] | Uncommon-tier items. |
| `Rare` | string[] | Rare-tier items. |
| `JackpotReaction` | string | Shown in place of the normal forage line when the spouse finds a Prismatic Shard. |

Item entries may be **plain names** (`"Sunflower"`, `"Truffle"`, `"Nautilus Shell"`) which resolve by
fuzzy search, or **fully-qualified IDs** (`"(O)FlashShifter.StardewValleyExpandedCP_Butterfish"`) for
precise modded-item references. Provide at least one tier with items for the section to apply.

---

## 3. Fallback rules (important)

- **Per-system, per-NPC.** Missing sections fall back to the generic pool independently. Custom
  morning + generic everything-else is perfectly valid.
- **Per-tier for Morning.** A missing morning tier falls back to the generic line for that tier.
- **Vanilla is protected.** Packs cannot change the twelve vanilla spouses unless the player enables
  "Allow Vanilla Override" in config (off by default).
- **Master toggle.** If the player turns off "Enable Custom NPC Framework", every NPC uses the
  generic fallback as if no pack were installed. Nothing in vanilla/generic behavior changes.
- **Malformed data is skipped, not fatal.** Invalid JSON skips the pack; an incomplete section or a
  bad value is dropped (with a clear SMAPI warning) while the rest of the NPC still loads.

## 4. Forward compatibility

New optional sections may be added in future Marriage Overhaul versions. Unknown fields are ignored
on load, and missing sections always fall back to generic — so **older packs keep working** and you
can adopt new sections whenever you like. Bump your pack's `Format` when you start using newer fields
(it's informational; the loader does not reject old formats).

## 5. Advanced: register from C# (optional)

C# mods can register the same content programmatically. Copy this interface into your project:

```csharp
public interface ICustomNpcApi
{
    bool IsFrameworkEnabled { get; }
    bool IsNpcRegistered(string npcName);
    bool RegisterNpc(string npcName, string contentJson);
}
```

Then, after the game launches:

```csharp
var api = this.Helper.ModRegistry.GetApi<ICustomNpcApi>("TitanmasterRy.MarriageOverhaul");
if (api != null)
{
    string json = /* a single NPC content object, same shape as one value under "NPCs" */;
    api.RegisterNpc("MyCustomNpc", json);
}
```

`RegisterNpc` returns `true` if at least one usable section was registered. Programmatic
registrations go through the same validation and the same registry as content packs.

---

A complete, copy-and-edit example pack lives in
[`examples/[MO] Aria Example`](../examples/%5BMO%5D%20Aria%20Example) — it demonstrates every section
for a fictional NPC named **Aria**.
