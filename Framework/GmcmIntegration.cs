using StardewModdingAPI;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        /// <summary>Register the config with Generic Mod Config Menu, if it's installed (optional dependency).</summary>
        private void SetupGmcm()
        {
            var api = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (api == null)
                return;

            var manifest = this.ModManifest;

            api.Register(
                mod: manifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config));

            // ── Systems ──────────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Systems");

            api.AddBoolOption(manifest,
                () => this.Config.EnableFeeding, v => this.Config.EnableFeeding = v,
                () => "Enable Feeding",
                () => "Spouse expects food in the fridge on 'player provides' days, and cooks on others.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableArguments, v => this.Config.EnableArguments = v,
                () => "Enable Arguments",
                () => "Trigger evening argument events when the relationship is strained.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableDivorceWarning, v => this.Config.EnableDivorceWarning = v,
                () => "Enable Divorce Warning",
                () => "The spouse mails an in-character warning letter when friendship gets dangerously low.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableAutoDivorce, v => this.Config.EnableAutoDivorce = v,
                () => "Enable Auto Divorce",
                () => "If things stay broken after the warning, the spouse initiates divorce automatically.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableJealousy, v => this.Config.EnableJealousy = v,
                () => "Enable Jealousy",
                () => "The spouse may get jealous when you gift other NPCs they'd consider rivals.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableMoodSystem, v => this.Config.EnableMoodSystem = v,
                () => "Enable Mood System",
                () => "The spouse's daily mood (Happy / Neutral / Grumpy) flavors their greeting dialogue.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableAnniversary, v => this.Config.EnableAnniversary = v,
                () => "Enable Anniversary",
                () => "Yearly anniversary reminder, with a bonus for gifting and a penalty for forgetting.");

            api.AddBoolOption(manifest,
                () => this.Config.AnniversaryCalendarMarker, v => this.Config.AnniversaryCalendarMarker = v,
                () => "Anniversary Calendar Marker",
                () => "Mark your wedding anniversary with a heart icon on the in-game calendar.");

            api.AddBoolOption(manifest,
                () => this.Config.BirthdayCalendarMarker, v => this.Config.BirthdayCalendarMarker = v,
                () => "Birthday Calendar Marker",
                () => "Mark your character's birthday with a gift icon on the in-game calendar (requires 'Your Birthday Day' to be set below).");

            api.AddBoolOption(manifest,
                () => this.Config.EnableMakeupGifts, v => this.Config.EnableMakeupGifts = v,
                () => "Enable Makeup Gifts",
                () => "After a bad argument, the spouse wants a specific category of gift to reconcile.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableCheating, v => this.Config.EnableCheating = v,
                () => "Enable Cheating (Ultimate Punishment)",
                () => "If you badly neglect your spouse, they may have an affair with another single candidate and leave you. Harsh - disable to turn it off entirely.");

            // ── Compatibility ────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Compatibility");

            api.AddBoolOption(manifest,
                () => this.Config.AllowSpouseKiss, v => this.Config.AllowSpouseKiss = v,
                () => "Allow Spouse Kiss/Hug",
                () => "Keep the normal kiss/hug available even when the mod has queued morning dialogue. Walk up empty-handed to kiss; talk again to read their dialogue. Keeps kiss-based mods working. Turn off to revert to dialogue-only mornings.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableDialogueCompat, v => this.Config.EnableDialogueCompat = v,
                () => "Dialogue Mod Compatibility",
                () => "Keep this mod's spouse greetings working alongside marriage-dialogue expansion mods (e.g. Haley Ever After). Without it, those mods replace this mod's morning dialogue. Our line shows first, then theirs.");

            api.AddBoolOption(manifest,
                () => this.Config.FeedingSearchExtraStorage, v => this.Config.FeedingSearchExtraStorage = v,
                () => "Feeding: Search Extra Storage",
                () => "Also search mini-fridges and the cellar fridge when feeding the spouse and storing chore meals. Helps with modded houses that put the fridge on a different level or have no main-level kitchen.");

            // ── Morning Dialogue ─────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Morning Dialogue");

            api.AddBoolOption(manifest,
                () => this.Config.FriendshipTieredMorningDialogue, v => this.Config.FriendshipTieredMorningDialogue = v,
                () => "Friendship-Tiered Morning Dialogue",
                () => "The spouse's morning greeting tone scales with your current hearts: cold and clipped when low, loving when high. The mood system still adds day-to-day variance on top. Turn off to use the old single mood greeting.");

            api.AddNumberOption(manifest,
                () => this.Config.MorningVeryLowHeartsMax, v => this.Config.MorningVeryLowHeartsMax = v,
                () => "Cold Tone Below (hearts)",
                () => "Below this many hearts the spouse is cold and resentful in the morning.",
                min: 0, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.MorningHighHeartsMin, v => this.Config.MorningHighHeartsMin = v,
                () => "Warm Tone At (hearts)",
                () => "At or above this many hearts the spouse is warm and friendly in the morning (below it, the tone is flat and polite).",
                min: 0, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.MorningVeryHighHeartsMin, v => this.Config.MorningVeryHighHeartsMin = v,
                () => "Affectionate Tone At (hearts)",
                () => "At or above this many hearts the spouse is openly affectionate in the morning.",
                min: 0, max: 14);

            // ── Thresholds ───────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Thresholds");

            api.AddNumberOption(manifest,
                () => this.Config.ArgumentThresholdHearts, v => this.Config.ArgumentThresholdHearts = v,
                () => "Argument Threshold (hearts)",
                () => "Arguments can trigger when friendship drops below this many hearts.",
                min: 1, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.DivorceWarningThresholdHearts, v => this.Config.DivorceWarningThresholdHearts = v,
                () => "Divorce Warning Threshold (hearts)",
                () => "The warning letter is sent when friendship drops below this many hearts.",
                min: 1, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.ConsecutiveDaysBeforeAutoDivorce, v => this.Config.ConsecutiveDaysBeforeAutoDivorce = v,
                () => "Consecutive Days Before Auto Divorce",
                () => "Days the relationship must stay below the warning threshold (after the letter) before divorce.",
                min: 1, max: 60);

            api.AddNumberOption(manifest,
                () => this.Config.CheatingThresholdHearts, v => this.Config.CheatingThresholdHearts = v,
                () => "Cheating Threshold (hearts)",
                () => "Below this many hearts, a neglected spouse may start an affair and leave you.",
                min: 0, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.CheatingChance, v => this.Config.CheatingChance = v,
                () => "Cheating Chance (per day)",
                () => "Daily chance the spouse cheats while below the cheating threshold (0.0 - 1.0).",
                min: 0f, max: 1f, interval: 0.05f);

            // ── Jealousy ─────────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Jealousy");

            api.AddNumberOption(manifest,
                () => this.Config.JealousyChance, v => this.Config.JealousyChance = v,
                () => "Jealousy Chance",
                () => "Chance the spouse notices a gift to a rival NPC (0.0 - 1.0).",
                min: 0f, max: 1f, interval: 0.05f);

            api.AddNumberOption(manifest,
                () => this.Config.JealousyChanceLoved, v => this.Config.JealousyChanceLoved = v,
                () => "Jealousy Chance (Loved Gift)",
                () => "Chance the spouse notices when the rival gift was a loved item (0.0 - 1.0).",
                min: 0f, max: 1f, interval: 0.05f);

            // ── Extended Systems ─────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Extended Systems");

            api.AddBoolOption(manifest,
                () => this.Config.EnableMilestones, v => this.Config.EnableMilestones = v,
                () => "Enable Milestones",
                () => "One-time scenes on your 1st, 3rd and 5th anniversary as the relationship grows.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseChores, v => this.Config.EnableSpouseChores = v,
                () => "Enable Spouse Chores",
                () => "The spouse may do a helpful farm chore in the morning; chance and quality scale with hearts.");

            api.AddNumberOption(manifest,
                () => this.Config.ChoreChanceAtMaxHearts, v => this.Config.ChoreChanceAtMaxHearts = v,
                () => "Chore Chance (at 14 hearts)",
                () => "The daily chore chance at maximum friendship (0.0 - 1.0).",
                min: 0f, max: 1f, interval: 0.05f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableEvolvingPreferences, v => this.Config.EnableEvolvingPreferences = v,
                () => "Enable Evolving Preferences",
                () => "After a year of marriage, the spouse comes to love new gifts tied to your shared history.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseSickness, v => this.Config.EnableSpouseSickness = v,
                () => "Enable Spouse Sickness",
                () => "Once per season the spouse may fall ill and need soup, medicine, or tea to recover.");

            api.AddNumberOption(manifest,
                () => this.Config.SicknessChancePerSeason, v => this.Config.SicknessChancePerSeason = v,
                () => "Sickness Chance (per season)",
                () => "The chance the spouse gets sick at some point each season (0.0 - 1.0).",
                min: 0f, max: 1f, interval: 0.05f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableInsideJokes, v => this.Config.EnableInsideJokes = v,
                () => "Enable Inside Jokes",
                () => "The spouse occasionally references shared moments built up over the marriage.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableAchievementDialogue, v => this.Config.EnableAchievementDialogue = v,
                () => "Enable Achievement Dialogue",
                () => "The spouse proudly references your big accomplishments (mines, perfection, legendary fish, etc.).");

            api.AddBoolOption(manifest,
                () => this.Config.EnableChildrenInteractions, v => this.Config.EnableChildrenInteractions = v,
                () => "Enable Children Interactions",
                () => "Extra dialogue and a weekly request involving your children, plus harsher arguments with kids home.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableRomanticLetters, v => this.Config.EnableRomanticLetters = v,
                () => "Enable Romantic Letters",
                () => "Every week or two the spouse leaves a handwritten love letter in your mailbox (flavor only).");

            api.AddBoolOption(manifest,
                () => this.Config.EnableSeasonalAffection, v => this.Config.EnableSeasonalAffection = v,
                () => "Enable Seasonal Affection",
                () => "Warmer dialogue and a gift bonus in the spouse's favorite season; moodier in their least favorite.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableBadDays, v => this.Config.EnableBadDays = v,
                () => "Enable Bad Days",
                () => "The spouse occasionally has a rough day. Comfort them (talk, food, or time) to help.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableBirthdaySystem, v => this.Config.EnableBirthdaySystem = v,
                () => "Enable Birthday System",
                () => "On your birthday the spouse has a small gift and special dialogue waiting for you when you next talk to them.");

            api.AddTextOption(manifest,
                () => this.Config.PlayerBirthdaySeason, v => this.Config.PlayerBirthdaySeason = v,
                () => "Your Birthday Season",
                () => "The season of your character's birthday (vanilla has no player birthday, so set it here).",
                allowedValues: new[] { "spring", "summer", "fall", "winter" });

            api.AddNumberOption(manifest,
                () => this.Config.PlayerBirthdayDay, v => this.Config.PlayerBirthdayDay = v,
                () => "Your Birthday Day",
                () => "The day of the month of your birthday (1-28). Set to 0 to disable the birthday system.",
                min: 0, max: 28);

            api.AddNumberOption(manifest,
                () => this.Config.BirthdayGiftChance, v => this.Config.BirthdayGiftChance = v,
                () => "Birthday Helpful Gift Chance",
                () => "Chance your spouse gives you a generally useful item instead of a personalized breakfast on your birthday (0.0 - 1.0).",
                min: 0f, max: 1f, interval: 0.05f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableTownGossip, v => this.Config.EnableTownGossip = v,
                () => "Enable Town Gossip",
                () => "Townsfolk may mention how your spouse speaks of you, for better or worse (flavor only).");

            api.AddBoolOption(manifest,
                () => this.Config.EnableHoneymoonPhase, v => this.Config.EnableHoneymoonPhase = v,
                () => "Enable Honeymoon Phase",
                () => "For the first weeks of marriage: boosted friendship, no arguments or bad days, warm dialogue.");

            api.AddNumberOption(manifest,
                () => this.Config.HoneymoonDuration, v => this.Config.HoneymoonDuration = v,
                () => "Honeymoon Duration (days)",
                () => "How many in-game days the honeymoon phase lasts after the wedding.",
                min: 0, max: 112);

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseRequests, v => this.Config.EnableSpouseRequests = v,
                () => "Enable Spouse Requests",
                () => "The spouse occasionally mails a request; fulfill it within 3 days for a reward.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseRequestQuest, v => this.Config.EnableSpouseRequestQuest = v,
                () => "Spouse Requests as Journal Quest",
                () => "Show each spouse request as a real tracked quest in the journal, not just a letter. Turn off for letter-only requests.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableSharedDreams, v => this.Config.EnableSharedDreams = v,
                () => "Enable Shared Dreams",
                () => "Now and then you wake to a journal entry describing a dream the spouse had about you.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableVisitorJealousy, v => this.Config.EnableVisitorJealousy = v,
                () => "Enable Visitor Jealousy",
                () => "The spouse may make a dry comment when other NPCs visit the farm (flavor only).");

            api.AddBoolOption(manifest,
                () => this.Config.EnableProductivityScaling, v => this.Config.EnableProductivityScaling = v,
                () => "Enable Productivity Scaling",
                () => "Chore quality and frequency follow the spouse's recent happiness (no visible score).");

            // ── Forage Loot ──────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Forage Loot");

            api.AddBoolOption(manifest,
                () => this.Config.EnableForageLoot, v => this.Config.EnableForageLoot = v,
                () => "Enable Forage Loot Table",
                () => "When the spouse forages, they bring back items from a per-spouse loot table instead of picking up forage on the farm. Requires Spouse Chores to be enabled.");

            api.AddNumberOption(manifest,
                () => this.Config.ForageHaulQuantity, v => this.Config.ForageHaulQuantity = v,
                () => "Forage Haul Quantity",
                () => "How many items the spouse brings back from a successful forage.",
                min: 1, max: 10);

            api.AddNumberOption(manifest,
                () => this.Config.ForageCommonWeight, v => this.Config.ForageCommonWeight = v,
                () => "Forage Common Weight",
                () => "Relative chance of a common-tier item per foraged item (higher = more common items).",
                min: 0f, max: 100f, interval: 1f);

            api.AddNumberOption(manifest,
                () => this.Config.ForageUncommonWeight, v => this.Config.ForageUncommonWeight = v,
                () => "Forage Uncommon Weight",
                () => "Relative chance of an uncommon-tier item per foraged item.",
                min: 0f, max: 100f, interval: 1f);

            api.AddNumberOption(manifest,
                () => this.Config.ForageRareWeight, v => this.Config.ForageRareWeight = v,
                () => "Forage Rare Weight",
                () => "Relative chance of a rare-tier item per foraged item.",
                min: 0f, max: 100f, interval: 1f);

            api.AddNumberOption(manifest,
                () => this.Config.ForagePrismaticShardChance, v => this.Config.ForagePrismaticShardChance = v,
                () => "Forage Prismatic Shard Chance",
                () => "Chance per forage that the spouse finds a Prismatic Shard, with a unique shocked reaction. Very small by design (0.0 - 0.05).",
                min: 0f, max: 0.05f, interval: 0.001f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableSkillMilestoneRewards, v => this.Config.EnableSkillMilestoneRewards = v,
                () => "Enable Skill Milestone Rewards",
                () => "When Farming, Fishing, Foraging, Mining, or Combat reaches level 5 or 10, the spouse gives special dialogue and a small gift.");

            api.AddTextOption(manifest,
                () => this.Config.MilestoneGiftItemId, v => this.Config.MilestoneGiftItemId = v,
                () => "Milestone Gift Item",
                () => "The name of the item the spouse gives as a skill milestone gift (e.g. 'Life Elixir').");

            api.AddNumberOption(manifest,
                () => this.Config.MilestoneGiftQuantity, v => this.Config.MilestoneGiftQuantity = v,
                () => "Milestone Gift Quantity",
                () => "How many of the milestone gift item the spouse gives.",
                min: 1, max: 99);

            // ── Debug ────────────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Debug");

            api.AddBoolOption(manifest,
                () => this.Config.EnableDebugCommands, v => this.Config.EnableDebugCommands = v,
                () => "Enable Debug Commands",
                () => "Register the mo_* testing console commands. Takes effect after restarting the game.");
        }
    }
}
