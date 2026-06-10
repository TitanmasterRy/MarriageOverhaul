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
                () => this.Config.EnableMakeupGifts, v => this.Config.EnableMakeupGifts = v,
                () => "Enable Makeup Gifts",
                () => "After a bad argument, the spouse wants a specific category of gift to reconcile.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableCheating, v => this.Config.EnableCheating = v,
                () => "Enable Cheating (Ultimate Punishment)",
                () => "If you badly neglect your spouse, they may have an affair with another single candidate and leave you. Harsh - disable to turn it off entirely.");

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

            // ── Debug ────────────────────────────────────────────────
            api.AddSectionTitle(manifest, () => "Debug");

            api.AddBoolOption(manifest,
                () => this.Config.EnableDebugCommands, v => this.Config.EnableDebugCommands = v,
                () => "Enable Debug Commands",
                () => "Register the mo_* testing console commands. Takes effect after restarting the game.");
        }
    }
}
