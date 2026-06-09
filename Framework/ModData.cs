using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>Per-save persisted state for all marriage systems.</summary>
    public class ModData
    {
        // ── Feeding ───────────────────────────────────────────────
        /// <summary>Week index (TotalDays / 7) the current schedule was generated for.</summary>
        public int FeedingScheduleWeek { get; set; } = -1;
        /// <summary>One entry per day of week (0 = first day). true = player must provide food, false = spouse cooks.</summary>
        public List<bool> ProvideDays { get; set; } = new List<bool>();
        /// <summary>Whether the spouse went hungry last night (drives grumpy morning dialogue).</summary>
        public bool WentHungry { get; set; } = false;
        /// <summary>Whether the spouse was fed yesterday (used by the mood system).</summary>
        public bool FedYesterday { get; set; } = true;

        // ── Arguments ─────────────────────────────────────────────
        public int LastArgumentDay { get; set; } = -1000;
        /// <summary>Last argument scenario index used per spouse, to avoid back-to-back repeats.</summary>
        public Dictionary<string, int> LastArgumentTopic { get; set; } = new Dictionary<string, int>();
        /// <summary>Set when an argument happened during the current in-game week (blocks date nights).</summary>
        public bool ArgumentThisWeek { get; set; } = false;
        public int ArgumentWeek { get; set; } = -1;

        // ── Divorce warning / auto divorce ────────────────────────
        public int WarningLetterSentDay { get; set; } = -1000;
        public bool WarningActive { get; set; } = false;
        public int ConsecutiveLowDays { get; set; } = 0;
        /// <summary>Text injected into the warning letter mail asset for the current spouse.</summary>
        public string PendingWarningLetterText { get; set; } = "";

        // ── Jealousy ──────────────────────────────────────────────
        public int LastJealousyDay { get; set; } = -1000;
        /// <summary>Queued jealousy hit to apply next morning.</summary>
        public bool PendingJealousy { get; set; } = false;

        // ── Mood ──────────────────────────────────────────────────
        public string Mood { get; set; } = "Neutral";
        /// <summary>Friendship points recorded at the start of yesterday, for trend detection.</summary>
        public int LastFriendshipPoints { get; set; } = -1;

        // ── Anniversary ───────────────────────────────────────────
        public int WeddingAbsoluteDay { get; set; } = -1;
        public bool IsAnniversaryToday { get; set; } = false;
        public bool AnniversaryGiftGivenToday { get; set; } = false;
        public int LastAnniversaryYearProcessed { get; set; } = -1;
        /// <summary>Set when an anniversary passed without a gift; drives the disappointed morning.</summary>
        public bool AnniversaryMissed { get; set; } = false;
        /// <summary>Text injected into the anniversary reminder mail asset.</summary>
        public string PendingAnniversaryLetterText { get; set; } = "";

        // ── Makeup gifts ──────────────────────────────────────────
        public bool MakeupNeeded { get; set; } = false;
        public string MakeupCategory { get; set; } = "";
        public int MakeupStartDay { get; set; } = -1000;

        // ── Date nights ───────────────────────────────────────────
        public int LastDateNightDay { get; set; } = -1000;
        /// <summary>Last date-event index used per spouse, to avoid back-to-back repeats.</summary>
        public Dictionary<string, int> LastDateEventIndex { get; set; } = new Dictionary<string, int>();
        public bool DateOfferedToday { get; set; } = false;
        public bool DateAcceptedTonight { get; set; } = false;
        public bool DatePlayedTonight { get; set; } = false;
        /// <summary>Whether tonight's accepted date is a movie-theater outing rather than a location date.</summary>
        public bool MovieDateTonight { get; set; } = false;
    }
}
