namespace MarriageOverhaul
{
    /// <summary>User-configurable options for the mod. Mirrored to GMCM.</summary>
    public class ModConfig
    {
        // Systems
        public bool EnableFeeding { get; set; } = true;
        public bool EnableArguments { get; set; } = true;
        public bool EnableDivorceWarning { get; set; } = true;
        public bool EnableAutoDivorce { get; set; } = true;
        public bool EnableJealousy { get; set; } = true;
        public bool EnableMoodSystem { get; set; } = true;
        public bool EnableAnniversary { get; set; } = true;

        // ── Custom NPC Framework ────────────────────────────────────
        /// <summary>Master toggle for the Custom NPC Framework: lets content packs provide personalized dialogue/behavior for their own custom NPCs. When off, custom NPCs use the generic fallback as before.</summary>
        public bool EnableCustomNpcFramework { get; set; } = true;
        /// <summary>Allow content packs to override the built-in personalized content for the twelve vanilla spouses. Off by default so vanilla behavior is never changed by a pack.</summary>
        public bool AllowVanillaOverride { get; set; } = false;

        // ── Compatibility ───────────────────────────────────────────
        /// <summary>Turn off features known to cause problems in multiplayer (currently: Shared Dreams, which can freeze other players' screens). Recommended ON for multiplayer, OFF for single-player.</summary>
        public bool MultiplayerCompatibilityMode { get; set; } = false;
        /// <summary>Keep the vanilla kiss/hug available even when the mod has queued spouse dialogue (preserves compatibility with kiss-based mods).</summary>
        public bool AllowSpouseKiss { get; set; } = true;
        /// <summary>Keep the mod's spouse dialogue working alongside marriage-dialogue expansion mods (e.g. Haley Ever After), which otherwise replace it.</summary>
        public bool EnableDialogueCompat { get; set; } = true;
        /// <summary>Also search mini-fridges and the cellar fridge when feeding the spouse (helps with modded houses that put the fridge on a different level or have no main-level kitchen).</summary>
        public bool FeedingSearchExtraStorage { get; set; } = false;
        /// <summary>Compatibility for the "Spouses Cook For You" mod: treat every day as a spouse cooking day, so the mod never expects you to provide food, never eats the meals that mod prepares, and never applies a hunger penalty. Off by default.</summary>
        public bool SpousesCookForYouCompat { get; set; } = false;
        /// <summary>Apply the marriage overhaul to a roommate (Krobus). Off by default so a roommate stays platonic with their normal dialogue.</summary>
        public bool EnableForRoommate { get; set; } = false;
        /// <summary>Compatibility for multi-spouse mods (Polyamory Sweet Love, Free Love): also give the overhaul's morning greetings to your other spouses. Off by default. Stateful systems still apply to the primary spouse only.</summary>
        public bool PolyamoryCompat { get; set; } = false;

        // ── Friendship-tiered morning dialogue ──────────────────────
        /// <summary>The spouse's morning greeting tone scales with your current friendship hearts (cold when low, loving when high). The mood system adds day-to-day variance on top.</summary>
        public bool FriendshipTieredMorningDialogue { get; set; } = true;
        /// <summary>Below this many hearts the spouse is cold and resentful in the morning.</summary>
        public int MorningVeryLowHeartsMax { get; set; } = 3;
        /// <summary>At or above this many hearts the spouse is warm and friendly in the morning.</summary>
        public int MorningHighHeartsMin { get; set; } = 8;
        /// <summary>At or above this many hearts the spouse is openly affectionate in the morning.</summary>
        public int MorningVeryHighHeartsMin { get; set; } = 12;

        /// <summary>Mark your wedding anniversary on the in-game calendar (Billboard).</summary>
        public bool AnniversaryCalendarMarker { get; set; } = false;
        /// <summary>Mark your character's birthday on the in-game calendar (Billboard).</summary>
        public bool BirthdayCalendarMarker { get; set; } = false;
        public bool EnableMakeupGifts { get; set; } = true;
        /// <summary>The "ultimate punishment": a badly neglected spouse can cheat and leave you.</summary>
        public bool EnableCheating { get; set; } = true;

        // Thresholds
        public int ArgumentThresholdHearts { get; set; } = 10;
        public int DivorceWarningThresholdHearts { get; set; } = 8;
        public int ConsecutiveDaysBeforeAutoDivorce { get; set; } = 7;
        public int CheatingThresholdHearts { get; set; } = 4;
        public float CheatingChance { get; set; } = 0.25f;

        // Jealousy
        public float JealousyChance { get; set; } = 0.20f;
        public float JealousyChanceLoved { get; set; } = 0.40f;

        // ── Extended systems ──────────────────────────────────────
        public bool EnableMilestones { get; set; } = true;
        public bool EnableSpouseChores { get; set; } = true;
        public float ChoreChanceAtMaxHearts { get; set; } = 0.6f;
        public bool EnableEvolvingPreferences { get; set; } = true;
        public bool EnableSpouseSickness { get; set; } = true;
        public float SicknessChancePerSeason { get; set; } = 0.4f;
        public bool EnableInsideJokes { get; set; } = true;
        public bool EnableAchievementDialogue { get; set; } = true;
        public bool EnableChildrenInteractions { get; set; } = true;
        public bool EnableRomanticLetters { get; set; } = true;
        public bool EnableSeasonalAffection { get; set; } = true;
        public bool EnableBadDays { get; set; } = true;
        public bool EnableBirthdaySystem { get; set; } = true;
        // Vanilla has no player birthday, so set yours here for the birthday system (day 0 = disabled).
        public string PlayerBirthdaySeason { get; set; } = "spring";
        public int PlayerBirthdayDay { get; set; } = 0;
        /// <summary>Chance the spouse gives a generally useful item instead of a personalized breakfast on your birthday.</summary>
        public float BirthdayGiftChance { get; set; } = 0.5f;
        public bool EnableTownGossip { get; set; } = true;
        public bool EnableHoneymoonPhase { get; set; } = true;
        public int HoneymoonDuration { get; set; } = 30;
        public bool EnableSpouseRequests { get; set; } = true;
        /// <summary>Show the spouse's request as a real tracked quest in the journal (in addition to the letter). Turn off for letter-only requests.</summary>
        public bool EnableSpouseRequestQuest { get; set; } = true;
        public bool EnableSharedDreams { get; set; } = true;
        public bool EnableVisitorJealousy { get; set; } = true;
        public bool EnableProductivityScaling { get; set; } = true;

        // ── Forage loot ─────────────────────────────────────────────
        /// <summary>When the spouse does the forage chore, they bring back items from a per-spouse loot table (with a rare Prismatic Shard chance) instead of picking up forage already on the farm.</summary>
        public bool EnableForageLoot { get; set; } = true;
        /// <summary>How many items the spouse brings back from a successful forage.</summary>
        public int ForageHaulQuantity { get; set; } = 3;
        /// <summary>Relative weight of the common tier when rolling each foraged item.</summary>
        public float ForageCommonWeight { get; set; } = 55f;
        /// <summary>Relative weight of the uncommon tier when rolling each foraged item.</summary>
        public float ForageUncommonWeight { get; set; } = 30f;
        /// <summary>Relative weight of the rare tier when rolling each foraged item.</summary>
        public float ForageRareWeight { get; set; } = 15f;
        /// <summary>Chance per forage that the spouse finds a Prismatic Shard (the jackpot). Very small by design.</summary>
        public float ForagePrismaticShardChance { get; set; } = 0.005f;

        // ── Skill milestones ────────────────────────────────────────
        /// <summary>When a skill reaches level 5 or 10, the spouse gives special dialogue and a small gift.</summary>
        public bool EnableSkillMilestoneRewards { get; set; } = true;
        /// <summary>Item name/ID used as the skill milestone gift (resolved by name, e.g. "Life Elixir").</summary>
        public string MilestoneGiftItemId { get; set; } = "Life Elixir";
        /// <summary>How many of the milestone gift item to give.</summary>
        public int MilestoneGiftQuantity { get; set; } = 1;

        // Debug
        public bool EnableDebugCommands { get; set; } = false;
    }
}
