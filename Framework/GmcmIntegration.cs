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
                () => this.Config.EnableDateNights, v => this.Config.EnableDateNights = v,
                () => "Enable Date Nights",
                () => "Every couple of weeks the spouse may ask to go out together in the evening.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableMovieDates, v => this.Config.EnableMovieDates = v,
                () => "Enable Movie Dates",
                () => "Once the movie theater is unlocked, some date nights become a trip to the movies.");

            api.AddBoolOption(manifest,
                () => this.Config.EnableDateCutscenes, v => this.Config.EnableDateCutscenes = v,
                () => "Date Cutscenes (Experimental)",
                () => "Play a real positioned cutscene for dates (beach spouses only for now) instead of a narration line. Experimental - leave off if you experience crashes.");

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
