using Newtonsoft.Json;
using StardewModdingAPI;

namespace MarriageOverhaul
{
    /// <summary>
    /// Public API for advanced C# mods that want to register custom-NPC content programmatically instead
    /// of (or in addition to) shipping a JSON content pack. Consumers copy this interface into their own
    /// project and obtain an instance via
    /// <c>helper.ModRegistry.GetApi&lt;ICustomNpcApi&gt;("TitanmasterRy.MarriageOverhaul")</c>.
    /// Every method uses only primitive types so no shared assembly is required.
    /// </summary>
    public interface ICustomNpcApi
    {
        /// <summary>Whether the Custom NPC Framework is currently enabled in the player's config.</summary>
        bool IsFrameworkEnabled { get; }

        /// <summary>Whether content is currently registered for the given NPC internal name.</summary>
        bool IsNpcRegistered(string npcName);

        /// <summary>
        /// Register (or replace) content for one NPC. <paramref name="contentJson"/> is the JSON for a single
        /// NPC content object — the same shape as one value under <c>"NPCs"</c> in a content pack's
        /// <c>content.json</c> (Morning / Mood / Arguments / Anniversary / Jealousy / Makeup / Behavior).
        /// Returns true if at least one usable section was registered.
        /// </summary>
        bool RegisterNpc(string npcName, string contentJson);
    }

    /// <inheritdoc cref="ICustomNpcApi"/>
    internal class CustomNpcApi : ICustomNpcApi
    {
        private readonly IMonitor monitor;

        public CustomNpcApi(IMonitor monitor)
        {
            this.monitor = monitor;
        }

        public bool IsFrameworkEnabled => CustomNpcRegistry.Enabled;

        public bool IsNpcRegistered(string npcName) => CustomNpcRegistry.IsRegistered(npcName);

        public bool RegisterNpc(string npcName, string contentJson)
        {
            if (string.IsNullOrWhiteSpace(npcName) || string.IsNullOrWhiteSpace(contentJson))
                return false;

            CustomNpcContent content;
            try
            {
                content = JsonConvert.DeserializeObject<CustomNpcContent>(contentJson);
            }
            catch (System.Exception ex)
            {
                this.monitor.Log($"Custom NPC Framework: API RegisterNpc('{npcName}') got invalid JSON: {ex.Message}", LogLevel.Warn);
                return false;
            }

            return CustomNpcContentPackLoader.TryRegisterSingle(npcName, content, "C# API", this.monitor);
        }
    }
}
