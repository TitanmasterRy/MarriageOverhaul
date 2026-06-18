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
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.systems", "Systems"));

            api.AddBoolOption(manifest,
                () => this.Config.EnableFeeding, v => this.Config.EnableFeeding = v,
                () => I18n.Get("config.EnableFeeding.name", "Enable Feeding"),
                () => I18n.Get("config.EnableFeeding.tooltip", "Spouse expects food in the fridge on 'player provides' days, and cooks on others."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableArguments, v => this.Config.EnableArguments = v,
                () => I18n.Get("config.EnableArguments.name", "Enable Arguments"),
                () => I18n.Get("config.EnableArguments.tooltip", "Trigger evening argument events when the relationship is strained."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableDivorceWarning, v => this.Config.EnableDivorceWarning = v,
                () => I18n.Get("config.EnableDivorceWarning.name", "Enable Divorce Warning"),
                () => I18n.Get("config.EnableDivorceWarning.tooltip", "The spouse mails an in-character warning letter when friendship gets dangerously low."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableAutoDivorce, v => this.Config.EnableAutoDivorce = v,
                () => I18n.Get("config.EnableAutoDivorce.name", "Enable Auto Divorce"),
                () => I18n.Get("config.EnableAutoDivorce.tooltip", "If things stay broken after the warning, the spouse initiates divorce automatically."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableJealousy, v => this.Config.EnableJealousy = v,
                () => I18n.Get("config.EnableJealousy.name", "Enable Jealousy"),
                () => I18n.Get("config.EnableJealousy.tooltip", "The spouse may get jealous when you gift other NPCs they'd consider rivals."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableMoodSystem, v => this.Config.EnableMoodSystem = v,
                () => I18n.Get("config.EnableMoodSystem.name", "Enable Mood System"),
                () => I18n.Get("config.EnableMoodSystem.tooltip", "The spouse's daily mood (Happy / Neutral / Grumpy) flavors their greeting dialogue."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableAnniversary, v => this.Config.EnableAnniversary = v,
                () => I18n.Get("config.EnableAnniversary.name", "Enable Anniversary"),
                () => I18n.Get("config.EnableAnniversary.tooltip", "Yearly anniversary reminder, with a bonus for gifting and a penalty for forgetting."));

            api.AddBoolOption(manifest,
                () => this.Config.AnniversaryCalendarMarker, v => this.Config.AnniversaryCalendarMarker = v,
                () => I18n.Get("config.AnniversaryCalendarMarker.name", "Anniversary Calendar Marker"),
                () => I18n.Get("config.AnniversaryCalendarMarker.tooltip", "Mark your wedding anniversary with a heart icon on the in-game calendar."));

            api.AddBoolOption(manifest,
                () => this.Config.BirthdayCalendarMarker, v => this.Config.BirthdayCalendarMarker = v,
                () => I18n.Get("config.BirthdayCalendarMarker.name", "Birthday Calendar Marker"),
                () => I18n.Get("config.BirthdayCalendarMarker.tooltip", "Mark your character's birthday with a gift icon on the in-game calendar (requires 'Your Birthday Day' to be set below)."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableMakeupGifts, v => this.Config.EnableMakeupGifts = v,
                () => I18n.Get("config.EnableMakeupGifts.name", "Enable Makeup Gifts"),
                () => I18n.Get("config.EnableMakeupGifts.tooltip", "After a bad argument, the spouse wants a specific category of gift to reconcile."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableCheating, v => this.Config.EnableCheating = v,
                () => I18n.Get("config.EnableCheating.name", "Enable Cheating (Ultimate Punishment)"),
                () => I18n.Get("config.EnableCheating.tooltip", "If you badly neglect your spouse, they may have an affair with another single candidate and leave you. Harsh - disable to turn it off entirely."));

            // ── Custom NPC Framework ─────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.customnpcframework", "Custom NPC Framework"));

            api.AddBoolOption(manifest,
                () => this.Config.EnableCustomNpcFramework, v => this.Config.EnableCustomNpcFramework = v,
                () => I18n.Get("config.EnableCustomNpcFramework.name", "Enable Custom NPC Framework"),
                () => I18n.Get("config.EnableCustomNpcFramework.tooltip", "Let installed content packs give personalized dialogue and behavior to their own custom NPCs (modded spouses). When off, those NPCs use the generic fallback dialogue as before. Doesn't affect vanilla spouses."));

            api.AddBoolOption(manifest,
                () => this.Config.AllowVanillaOverride, v => this.Config.AllowVanillaOverride = v,
                () => I18n.Get("config.AllowVanillaOverride.name", "Allow Vanilla Override"),
                () => I18n.Get("config.AllowVanillaOverride.tooltip", "Allow content packs to replace the built-in personalized content for the twelve vanilla spouses. Off by default, so packs can only add content for custom NPCs and never change vanilla characters."));

            // ── Compatibility ────────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.compatibility", "Compatibility"));

            api.AddBoolOption(manifest,
                () => this.Config.MultiplayerCompatibilityMode, v => this.Config.MultiplayerCompatibilityMode = v,
                () => I18n.Get("config.MultiplayerCompatibilityMode.name", "Multiplayer Compatibility Mode"),
                () => I18n.Get("config.MultiplayerCompatibilityMode.tooltip", "Turns off features known to cause problems in multiplayer (currently: Shared Dreams, which can freeze other players' screens). Recommended ON for multiplayer games, OFF for single-player."));

            api.AddBoolOption(manifest,
                () => this.Config.AllowSpouseKiss, v => this.Config.AllowSpouseKiss = v,
                () => I18n.Get("config.AllowSpouseKiss.name", "Allow Spouse Kiss/Hug"),
                () => I18n.Get("config.AllowSpouseKiss.tooltip", "Keep the normal kiss/hug available even when the mod has queued morning dialogue. Walk up empty-handed to kiss; talk again to read their dialogue. Keeps kiss-based mods working. Turn off to revert to dialogue-only mornings."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableDialogueCompat, v => this.Config.EnableDialogueCompat = v,
                () => I18n.Get("config.EnableDialogueCompat.name", "Dialogue Mod Compatibility"),
                () => I18n.Get("config.EnableDialogueCompat.tooltip", "Keep this mod's spouse greetings working alongside marriage-dialogue expansion mods (e.g. Haley Ever After). Without it, those mods replace this mod's morning dialogue. Our line shows first, then theirs."));

            api.AddBoolOption(manifest,
                () => this.Config.FeedingSearchExtraStorage, v => this.Config.FeedingSearchExtraStorage = v,
                () => I18n.Get("config.FeedingSearchExtraStorage.name", "Feeding: Search Extra Storage"),
                () => I18n.Get("config.FeedingSearchExtraStorage.tooltip", "Also search mini-fridges and the cellar fridge when feeding the spouse and storing chore meals. Helps with modded houses that put the fridge on a different level or have no main-level kitchen."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableForRoommate, v => this.Config.EnableForRoommate = v,
                () => I18n.Get("config.EnableForRoommate.name", "Overhaul a Roommate (Krobus)"),
                () => I18n.Get("config.EnableForRoommate.tooltip", "Apply Marriage Overhaul's romantic dialogue and systems to a roommate (Krobus). Off by default, so a roommate stays platonic with their normal vanilla dialogue."));

            // ── Morning Dialogue ─────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.morningdialogue", "Morning Dialogue"));

            api.AddBoolOption(manifest,
                () => this.Config.FriendshipTieredMorningDialogue, v => this.Config.FriendshipTieredMorningDialogue = v,
                () => I18n.Get("config.FriendshipTieredMorningDialogue.name", "Friendship-Tiered Morning Dialogue"),
                () => I18n.Get("config.FriendshipTieredMorningDialogue.tooltip", "The spouse's morning greeting tone scales with your current hearts: cold and clipped when low, loving when high. The mood system still adds day-to-day variance on top. Turn off to use the old single mood greeting."));

            api.AddNumberOption(manifest,
                () => this.Config.MorningVeryLowHeartsMax, v => this.Config.MorningVeryLowHeartsMax = v,
                () => I18n.Get("config.MorningVeryLowHeartsMax.name", "Cold Tone Below (hearts)"),
                () => I18n.Get("config.MorningVeryLowHeartsMax.tooltip", "Below this many hearts the spouse is cold and resentful in the morning."),
                min: 0, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.MorningHighHeartsMin, v => this.Config.MorningHighHeartsMin = v,
                () => I18n.Get("config.MorningHighHeartsMin.name", "Warm Tone At (hearts)"),
                () => I18n.Get("config.MorningHighHeartsMin.tooltip", "At or above this many hearts the spouse is warm and friendly in the morning (below it, the tone is flat and polite)."),
                min: 0, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.MorningVeryHighHeartsMin, v => this.Config.MorningVeryHighHeartsMin = v,
                () => I18n.Get("config.MorningVeryHighHeartsMin.name", "Affectionate Tone At (hearts)"),
                () => I18n.Get("config.MorningVeryHighHeartsMin.tooltip", "At or above this many hearts the spouse is openly affectionate in the morning."),
                min: 0, max: 14);

            // ── Thresholds ───────────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.thresholds", "Thresholds"));

            api.AddNumberOption(manifest,
                () => this.Config.ArgumentThresholdHearts, v => this.Config.ArgumentThresholdHearts = v,
                () => I18n.Get("config.ArgumentThresholdHearts.name", "Argument Threshold (hearts)"),
                () => I18n.Get("config.ArgumentThresholdHearts.tooltip", "Arguments can trigger when friendship drops below this many hearts."),
                min: 1, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.DivorceWarningThresholdHearts, v => this.Config.DivorceWarningThresholdHearts = v,
                () => I18n.Get("config.DivorceWarningThresholdHearts.name", "Divorce Warning Threshold (hearts)"),
                () => I18n.Get("config.DivorceWarningThresholdHearts.tooltip", "The warning letter is sent when friendship drops below this many hearts."),
                min: 1, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.ConsecutiveDaysBeforeAutoDivorce, v => this.Config.ConsecutiveDaysBeforeAutoDivorce = v,
                () => I18n.Get("config.ConsecutiveDaysBeforeAutoDivorce.name", "Consecutive Days Before Auto Divorce"),
                () => I18n.Get("config.ConsecutiveDaysBeforeAutoDivorce.tooltip", "Days the relationship must stay below the warning threshold (after the letter) before divorce."),
                min: 1, max: 60);

            api.AddNumberOption(manifest,
                () => this.Config.CheatingThresholdHearts, v => this.Config.CheatingThresholdHearts = v,
                () => I18n.Get("config.CheatingThresholdHearts.name", "Cheating Threshold (hearts)"),
                () => I18n.Get("config.CheatingThresholdHearts.tooltip", "Below this many hearts, a neglected spouse may start an affair and leave you."),
                min: 0, max: 14);

            api.AddNumberOption(manifest,
                () => this.Config.CheatingChance, v => this.Config.CheatingChance = v,
                () => I18n.Get("config.CheatingChance.name", "Cheating Chance (per day)"),
                () => I18n.Get("config.CheatingChance.tooltip", "Daily chance the spouse cheats while below the cheating threshold (0.0 - 1.0)."),
                min: 0f, max: 1f, interval: 0.05f);

            // ── Jealousy ─────────────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.jealousy", "Jealousy"));

            api.AddNumberOption(manifest,
                () => this.Config.JealousyChance, v => this.Config.JealousyChance = v,
                () => I18n.Get("config.JealousyChance.name", "Jealousy Chance"),
                () => I18n.Get("config.JealousyChance.tooltip", "Chance the spouse notices a gift to a rival NPC (0.0 - 1.0)."),
                min: 0f, max: 1f, interval: 0.05f);

            api.AddNumberOption(manifest,
                () => this.Config.JealousyChanceLoved, v => this.Config.JealousyChanceLoved = v,
                () => I18n.Get("config.JealousyChanceLoved.name", "Jealousy Chance (Loved Gift)"),
                () => I18n.Get("config.JealousyChanceLoved.tooltip", "Chance the spouse notices when the rival gift was a loved item (0.0 - 1.0)."),
                min: 0f, max: 1f, interval: 0.05f);

            // ── Extended Systems ─────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.extendedsystems", "Extended Systems"));

            api.AddBoolOption(manifest,
                () => this.Config.EnableMilestones, v => this.Config.EnableMilestones = v,
                () => I18n.Get("config.EnableMilestones.name", "Enable Milestones"),
                () => I18n.Get("config.EnableMilestones.tooltip", "One-time scenes on your 1st, 3rd and 5th anniversary as the relationship grows."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseChores, v => this.Config.EnableSpouseChores = v,
                () => I18n.Get("config.EnableSpouseChores.name", "Enable Spouse Chores"),
                () => I18n.Get("config.EnableSpouseChores.tooltip", "The spouse may do a helpful farm chore in the morning; chance and quality scale with hearts."));

            api.AddNumberOption(manifest,
                () => this.Config.ChoreChanceAtMaxHearts, v => this.Config.ChoreChanceAtMaxHearts = v,
                () => I18n.Get("config.ChoreChanceAtMaxHearts.name", "Chore Chance (at 14 hearts)"),
                () => I18n.Get("config.ChoreChanceAtMaxHearts.tooltip", "The daily chore chance at maximum friendship (0.0 - 1.0)."),
                min: 0f, max: 1f, interval: 0.05f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableEvolvingPreferences, v => this.Config.EnableEvolvingPreferences = v,
                () => I18n.Get("config.EnableEvolvingPreferences.name", "Enable Evolving Preferences"),
                () => I18n.Get("config.EnableEvolvingPreferences.tooltip", "After a year of marriage, the spouse comes to love new gifts tied to your shared history."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseSickness, v => this.Config.EnableSpouseSickness = v,
                () => I18n.Get("config.EnableSpouseSickness.name", "Enable Spouse Sickness"),
                () => I18n.Get("config.EnableSpouseSickness.tooltip", "Once per season the spouse may fall ill and need soup, medicine, or tea to recover."));

            api.AddNumberOption(manifest,
                () => this.Config.SicknessChancePerSeason, v => this.Config.SicknessChancePerSeason = v,
                () => I18n.Get("config.SicknessChancePerSeason.name", "Sickness Chance (per season)"),
                () => I18n.Get("config.SicknessChancePerSeason.tooltip", "The chance the spouse gets sick at some point each season (0.0 - 1.0)."),
                min: 0f, max: 1f, interval: 0.05f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableInsideJokes, v => this.Config.EnableInsideJokes = v,
                () => I18n.Get("config.EnableInsideJokes.name", "Enable Inside Jokes"),
                () => I18n.Get("config.EnableInsideJokes.tooltip", "The spouse occasionally references shared moments built up over the marriage."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableAchievementDialogue, v => this.Config.EnableAchievementDialogue = v,
                () => I18n.Get("config.EnableAchievementDialogue.name", "Enable Achievement Dialogue"),
                () => I18n.Get("config.EnableAchievementDialogue.tooltip", "The spouse proudly references your big accomplishments (mines, perfection, legendary fish, etc.)."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableChildrenInteractions, v => this.Config.EnableChildrenInteractions = v,
                () => I18n.Get("config.EnableChildrenInteractions.name", "Enable Children Interactions"),
                () => I18n.Get("config.EnableChildrenInteractions.tooltip", "Extra dialogue and a weekly request involving your children, plus harsher arguments with kids home."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableRomanticLetters, v => this.Config.EnableRomanticLetters = v,
                () => I18n.Get("config.EnableRomanticLetters.name", "Enable Romantic Letters"),
                () => I18n.Get("config.EnableRomanticLetters.tooltip", "Every week or two the spouse leaves a handwritten love letter in your mailbox (flavor only)."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableSeasonalAffection, v => this.Config.EnableSeasonalAffection = v,
                () => I18n.Get("config.EnableSeasonalAffection.name", "Enable Seasonal Affection"),
                () => I18n.Get("config.EnableSeasonalAffection.tooltip", "Warmer dialogue and a gift bonus in the spouse's favorite season; moodier in their least favorite."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableBadDays, v => this.Config.EnableBadDays = v,
                () => I18n.Get("config.EnableBadDays.name", "Enable Bad Days"),
                () => I18n.Get("config.EnableBadDays.tooltip", "The spouse occasionally has a rough day. Comfort them (talk, food, or time) to help."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableBirthdaySystem, v => this.Config.EnableBirthdaySystem = v,
                () => I18n.Get("config.EnableBirthdaySystem.name", "Enable Birthday System"),
                () => I18n.Get("config.EnableBirthdaySystem.tooltip", "On your birthday the spouse has a small gift and special dialogue waiting for you when you next talk to them."));

            api.AddTextOption(manifest,
                () => this.Config.PlayerBirthdaySeason, v => this.Config.PlayerBirthdaySeason = v,
                () => I18n.Get("config.PlayerBirthdaySeason.name", "Your Birthday Season"),
                () => I18n.Get("config.PlayerBirthdaySeason.tooltip", "The season of your character's birthday (vanilla has no player birthday, so set it here)."),
                allowedValues: new[] { "spring", "summer", "fall", "winter" });

            api.AddNumberOption(manifest,
                () => this.Config.PlayerBirthdayDay, v => this.Config.PlayerBirthdayDay = v,
                () => I18n.Get("config.PlayerBirthdayDay.name", "Your Birthday Day"),
                () => I18n.Get("config.PlayerBirthdayDay.tooltip", "The day of the month of your birthday (1-28). Set to 0 to disable the birthday system."),
                min: 0, max: 28);

            api.AddNumberOption(manifest,
                () => this.Config.BirthdayGiftChance, v => this.Config.BirthdayGiftChance = v,
                () => I18n.Get("config.BirthdayGiftChance.name", "Birthday Helpful Gift Chance"),
                () => I18n.Get("config.BirthdayGiftChance.tooltip", "Chance your spouse gives you a generally useful item instead of a personalized breakfast on your birthday (0.0 - 1.0)."),
                min: 0f, max: 1f, interval: 0.05f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableTownGossip, v => this.Config.EnableTownGossip = v,
                () => I18n.Get("config.EnableTownGossip.name", "Enable Town Gossip"),
                () => I18n.Get("config.EnableTownGossip.tooltip", "Townsfolk may mention how your spouse speaks of you, for better or worse (flavor only)."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableHoneymoonPhase, v => this.Config.EnableHoneymoonPhase = v,
                () => I18n.Get("config.EnableHoneymoonPhase.name", "Enable Honeymoon Phase"),
                () => I18n.Get("config.EnableHoneymoonPhase.tooltip", "For the first weeks of marriage: boosted friendship, no arguments or bad days, warm dialogue."));

            api.AddNumberOption(manifest,
                () => this.Config.HoneymoonDuration, v => this.Config.HoneymoonDuration = v,
                () => I18n.Get("config.HoneymoonDuration.name", "Honeymoon Duration (days)"),
                () => I18n.Get("config.HoneymoonDuration.tooltip", "How many in-game days the honeymoon phase lasts after the wedding."),
                min: 0, max: 112);

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseRequests, v => this.Config.EnableSpouseRequests = v,
                () => I18n.Get("config.EnableSpouseRequests.name", "Enable Spouse Requests"),
                () => I18n.Get("config.EnableSpouseRequests.tooltip", "The spouse occasionally mails a request; fulfill it within 3 days for a reward."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableSpouseRequestQuest, v => this.Config.EnableSpouseRequestQuest = v,
                () => I18n.Get("config.EnableSpouseRequestQuest.name", "Spouse Requests as Journal Quest"),
                () => I18n.Get("config.EnableSpouseRequestQuest.tooltip", "Show each spouse request as a real tracked quest in the journal, not just a letter. Turn off for letter-only requests."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableSharedDreams, v => this.Config.EnableSharedDreams = v,
                () => I18n.Get("config.EnableSharedDreams.name", "Enable Shared Dreams"),
                () => I18n.Get("config.EnableSharedDreams.tooltip", "Now and then you wake to a journal entry describing a dream the spouse had about you."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableVisitorJealousy, v => this.Config.EnableVisitorJealousy = v,
                () => I18n.Get("config.EnableVisitorJealousy.name", "Enable Visitor Jealousy"),
                () => I18n.Get("config.EnableVisitorJealousy.tooltip", "The spouse may make a dry comment when other NPCs visit the farm (flavor only)."));

            api.AddBoolOption(manifest,
                () => this.Config.EnableProductivityScaling, v => this.Config.EnableProductivityScaling = v,
                () => I18n.Get("config.EnableProductivityScaling.name", "Enable Productivity Scaling"),
                () => I18n.Get("config.EnableProductivityScaling.tooltip", "Chore quality and frequency follow the spouse's recent happiness (no visible score)."));

            // ── Forage Loot ──────────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.forageloot", "Forage Loot"));

            api.AddBoolOption(manifest,
                () => this.Config.EnableForageLoot, v => this.Config.EnableForageLoot = v,
                () => I18n.Get("config.EnableForageLoot.name", "Enable Forage Loot Table"),
                () => I18n.Get("config.EnableForageLoot.tooltip", "When the spouse forages, they bring back items from a per-spouse loot table instead of picking up forage on the farm. Requires Spouse Chores to be enabled."));

            api.AddNumberOption(manifest,
                () => this.Config.ForageHaulQuantity, v => this.Config.ForageHaulQuantity = v,
                () => I18n.Get("config.ForageHaulQuantity.name", "Forage Haul Quantity"),
                () => I18n.Get("config.ForageHaulQuantity.tooltip", "How many items the spouse brings back from a successful forage."),
                min: 1, max: 10);

            api.AddNumberOption(manifest,
                () => this.Config.ForageCommonWeight, v => this.Config.ForageCommonWeight = v,
                () => I18n.Get("config.ForageCommonWeight.name", "Forage Common Weight"),
                () => I18n.Get("config.ForageCommonWeight.tooltip", "Relative chance of a common-tier item per foraged item (higher = more common items)."),
                min: 0f, max: 100f, interval: 1f);

            api.AddNumberOption(manifest,
                () => this.Config.ForageUncommonWeight, v => this.Config.ForageUncommonWeight = v,
                () => I18n.Get("config.ForageUncommonWeight.name", "Forage Uncommon Weight"),
                () => I18n.Get("config.ForageUncommonWeight.tooltip", "Relative chance of an uncommon-tier item per foraged item."),
                min: 0f, max: 100f, interval: 1f);

            api.AddNumberOption(manifest,
                () => this.Config.ForageRareWeight, v => this.Config.ForageRareWeight = v,
                () => I18n.Get("config.ForageRareWeight.name", "Forage Rare Weight"),
                () => I18n.Get("config.ForageRareWeight.tooltip", "Relative chance of a rare-tier item per foraged item."),
                min: 0f, max: 100f, interval: 1f);

            api.AddNumberOption(manifest,
                () => this.Config.ForagePrismaticShardChance, v => this.Config.ForagePrismaticShardChance = v,
                () => I18n.Get("config.ForagePrismaticShardChance.name", "Forage Prismatic Shard Chance"),
                () => I18n.Get("config.ForagePrismaticShardChance.tooltip", "Chance per forage that the spouse finds a Prismatic Shard, with a unique shocked reaction. Very small by design (0.0 - 0.05)."),
                min: 0f, max: 0.05f, interval: 0.001f);

            api.AddBoolOption(manifest,
                () => this.Config.EnableSkillMilestoneRewards, v => this.Config.EnableSkillMilestoneRewards = v,
                () => I18n.Get("config.EnableSkillMilestoneRewards.name", "Enable Skill Milestone Rewards"),
                () => I18n.Get("config.EnableSkillMilestoneRewards.tooltip", "When Farming, Fishing, Foraging, Mining, or Combat reaches level 5 or 10, the spouse gives special dialogue and a small gift."));

            api.AddTextOption(manifest,
                () => this.Config.MilestoneGiftItemId, v => this.Config.MilestoneGiftItemId = v,
                () => I18n.Get("config.MilestoneGiftItemId.name", "Milestone Gift Item"),
                () => I18n.Get("config.MilestoneGiftItemId.tooltip", "The name of the item the spouse gives as a skill milestone gift (e.g. 'Life Elixir')."));

            api.AddNumberOption(manifest,
                () => this.Config.MilestoneGiftQuantity, v => this.Config.MilestoneGiftQuantity = v,
                () => I18n.Get("config.MilestoneGiftQuantity.name", "Milestone Gift Quantity"),
                () => I18n.Get("config.MilestoneGiftQuantity.tooltip", "How many of the milestone gift item the spouse gives."),
                min: 1, max: 99);

            // ── Debug ────────────────────────────────────────────────
            api.AddSectionTitle(manifest, () => I18n.Get("config.section.debug", "Debug"));

            api.AddBoolOption(manifest,
                () => this.Config.EnableDebugCommands, v => this.Config.EnableDebugCommands = v,
                () => I18n.Get("config.EnableDebugCommands.name", "Enable Debug Commands"),
                () => I18n.Get("config.EnableDebugCommands.tooltip", "Register the mo_* testing console commands. Takes effect after restarting the game."));
        }
    }
}
