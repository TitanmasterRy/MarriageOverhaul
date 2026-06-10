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
        /// <summary>Mark your wedding anniversary on the in-game calendar (Billboard).</summary>
        public bool ShowAnniversaryOnCalendar { get; set; } = false;
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
        public bool EnableTownGossip { get; set; } = true;
        public bool EnableHoneymoonPhase { get; set; } = true;
        public int HoneymoonDuration { get; set; } = 30;
        public bool EnableSpouseRequests { get; set; } = true;
        public bool EnableSharedDreams { get; set; } = true;
        public bool EnableVisitorJealousy { get; set; } = true;
        public bool EnableProductivityScaling { get; set; } = true;

        // Debug
        public bool EnableDebugCommands { get; set; } = false;
    }
}
