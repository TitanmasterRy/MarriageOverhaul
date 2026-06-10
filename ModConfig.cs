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
        public bool EnableMakeupGifts { get; set; } = true;
        public bool EnableDateNights { get; set; } = true;
        public bool EnableMovieDates { get; set; } = true;
        /// <summary>The "ultimate punishment": a badly neglected spouse can cheat and leave you.</summary>
        public bool EnableCheating { get; set; } = true;
        /// <summary>EXPERIMENTAL: play a real positioned cutscene for the date instead of a narration line.</summary>
        public bool EnableDateCutscenes { get; set; } = false;

        // Thresholds
        public int ArgumentThresholdHearts { get; set; } = 10;
        public int DivorceWarningThresholdHearts { get; set; } = 8;
        public int ConsecutiveDaysBeforeAutoDivorce { get; set; } = 7;
        public int CheatingThresholdHearts { get; set; } = 4;
        public float CheatingChance { get; set; } = 0.25f;

        // Jealousy
        public float JealousyChance { get; set; } = 0.20f;
        public float JealousyChanceLoved { get; set; } = 0.40f;

        // Debug
        public bool EnableDebugCommands { get; set; } = false;
    }
}
