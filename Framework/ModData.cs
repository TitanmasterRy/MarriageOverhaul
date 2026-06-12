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
        /// <summary>The morning greeting line shown yesterday, so we don't repeat it two mornings in a row.</summary>
        public string LastMorningLine { get; set; } = "";
        /// <summary>Friendship points recorded at the start of yesterday, for trend detection.</summary>
        public int LastFriendshipPoints { get; set; } = -1;

        // ── Anniversary ───────────────────────────────────────────
        public int WeddingAbsoluteDay { get; set; } = -1;
        public bool IsAnniversaryToday { get; set; } = false;
        public bool AnniversaryGiftGivenToday { get; set; } = false;
        public int LastAnniversaryYearProcessed { get; set; } = -1;
        /// <summary>Set when an anniversary passed without a gift; drives the disappointed morning.</summary>
        public bool AnniversaryMissed { get; set; } = false;
        /// <summary>Absolute day the most recent anniversary was forgotten (used by the birthday callback).</summary>
        public int LastMissedAnniversaryDay { get; set; } = -1000;
        /// <summary>Text injected into the anniversary reminder mail asset.</summary>
        public string PendingAnniversaryLetterText { get; set; } = "";

        // ── Makeup gifts ──────────────────────────────────────────
        public bool MakeupNeeded { get; set; } = false;
        public string MakeupCategory { get; set; } = "";
        public int MakeupStartDay { get; set; } = -1000;

        // ── Cheating (ultimate punishment) ────────────────────────
        public int LastCheatDay { get; set; } = -1000;
        public bool PendingCheatReveal { get; set; } = false;
        public string CheatPartner { get; set; } = "";
        /// <summary>Text injected into the cheating reveal letter mail asset.</summary>
        public string PendingCheatLetterText { get; set; } = "";

        // ── F1: Milestones ────────────────────────────────────────
        /// <summary>Milestone years (1/3/5) whose scene has already played this save.</summary>
        public List<int> MilestonesFired { get; set; } = new List<int>();

        // ── F2/F17: Chores ────────────────────────────────────────
        public int LastChoreDay { get; set; } = -1000;
        /// <summary>Recent per-day mood scores (rolling window) driving chore quality.</summary>
        public List<int> RollingMoodScores { get; set; } = new List<int>();

        // ── F3: Evolving preferences ──────────────────────────────
        public bool PreferencesEvolved { get; set; } = false;

        // ── F4: Sickness ──────────────────────────────────────────
        public bool SpouseSickToday { get; set; } = false;
        public bool SickCuredToday { get; set; } = false;
        public int LastSicknessSeason { get; set; } = -1;
        public int TiredDaysRemaining { get; set; } = 0;

        // ── F5: Inside jokes ──────────────────────────────────────
        /// <summary>Accumulated inside-joke callback line indexes (per spouse name).</summary>
        public Dictionary<string, List<int>> InsideJokes { get; set; } = new Dictionary<string, List<int>>();
        public int LastInsideJokeDay { get; set; } = -1000;

        // ── F6: Achievements ──────────────────────────────────────
        public List<string> AchievementsCelebrated { get; set; } = new List<string>();
        public string PendingAchievement { get; set; } = "";

        // ── F7: Children ──────────────────────────────────────────
        public int LastChildRequestWeek { get; set; } = -1;
        public bool ChildRequestActive { get; set; } = false;

        // ── F8: Romantic letters ──────────────────────────────────
        public int NextLetterDay { get; set; } = -1;
        public string PendingLoveLetterText { get; set; } = "";
        public List<int> RecentLetters { get; set; } = new List<int>();

        // ── F10: Bad days ─────────────────────────────────────────
        public bool BadDayToday { get; set; } = false;
        public bool BadDayResolved { get; set; } = false;
        public int LastBadDay { get; set; } = -1000;
        public int FlatDaysRemaining { get; set; } = 0;
        public int BadDayLineIndex { get; set; } = -1;
        public bool BadDayRecoveredPending { get; set; } = false;

        // ── F11: Birthday ─────────────────────────────────────────
        public int LastBirthdayYearProcessed { get; set; } = -1;

        // ── F12: Town gossip ──────────────────────────────────────
        public int LastGossipWeek { get; set; } = -1;
        /// <summary>Rolling record of recent friendship points for the 7-day average.</summary>
        public List<int> RollingFriendship { get; set; } = new List<int>();

        // ── F14: Spouse requests ──────────────────────────────────
        public bool RequestActive { get; set; } = false;
        public string RequestId { get; set; } = "";
        public int RequestStartDay { get; set; } = -1000;
        public int LastRequestDay { get; set; } = -1000;
        public Dictionary<string, int> LastRequestIndex { get; set; } = new Dictionary<string, int>();
        public string PendingRequestLetterText { get; set; } = "";
        /// <summary>A "made for you" reward the spouse delivers a few days after a project-style request is fulfilled.</summary>
        public string PendingRewardItem { get; set; } = "";
        public int PendingRewardQty { get; set; } = 0;
        public int PendingRewardDay { get; set; } = -1000;
        public string PendingRewardLine { get; set; } = "";

        // ── F15: Shared dreams ────────────────────────────────────
        public int LastDreamDay { get; set; } = -1000;
        public List<int> RecentDreams { get; set; } = new List<int>();

        // ── F16: Visitor jealousy ─────────────────────────────────
        public bool VisitorSeenToday { get; set; } = false;
        public string VisitorName { get; set; } = "";
        public bool PendingVisitorComment { get; set; } = false;

        // ── Interaction tracking (seasonal decay / bad days) ──────
        public int LastTalkedDay { get; set; } = -1000;

        // ── Skill milestones ───────────────────────────────────────
        /// <summary>The most recently recorded level for each skill (Farming/Fishing/Foraging/Mining/Combat).</summary>
        public Dictionary<string, int> SkillLevelSnapshot { get; set; } = new Dictionary<string, int>();
        /// <summary>"{Skill}{Level}" keys (e.g. "Farming5") for milestones already celebrated this save.</summary>
        public List<string> SkillMilestonesFired { get; set; } = new List<string>();
    }
}
