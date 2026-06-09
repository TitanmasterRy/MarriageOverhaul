using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using SObject = StardewValley.Object;

namespace MarriageOverhaul
{
    /// <summary>Harmony patches that hook the gift-giving system for jealousy, anniversaries and makeup gifts.</summary>
    internal static class HarmonyPatches
    {
        private static IMonitor Monitor;

        public static void Apply(Harmony harmony, IMonitor monitor)
        {
            Monitor = monitor;

            var target = AccessTools.Method(typeof(NPC), nameof(NPC.receiveGift));
            if (target == null)
            {
                Monitor.Log("Could not find NPC.receiveGift to patch; gift-based systems disabled.", LogLevel.Warn);
                return;
            }

            harmony.Patch(
                original: target,
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ReceiveGift_Prefix)),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ReceiveGift_Postfix)));
        }

        /// <summary>During a makeup state, regular gifts to the spouse give half friendship.</summary>
        private static void ReceiveGift_Prefix(NPC __instance, SObject o, Farmer giver, ref float friendshipChangeMultiplier)
        {
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod == null || giver == null || !giver.IsLocalPlayer)
                    return;

                if (__instance != null && __instance.Name == mod.SpouseName && mod.IsMakeupActive())
                    friendshipChangeMultiplier *= 0.5f;
            }
            catch (Exception ex)
            {
                Monitor?.Log($"ReceiveGift prefix error: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Route the gift to the jealousy / anniversary / makeup systems.</summary>
        private static void ReceiveGift_Postfix(NPC __instance, SObject o, Farmer giver)
        {
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod == null || giver == null || !giver.IsLocalPlayer || __instance == null)
                    return;

                string spouseName = mod.SpouseName;
                if (string.IsNullOrEmpty(spouseName))
                    return;

                if (__instance.Name == spouseName)
                    mod.Gift_OnGiftToSpouse(__instance, o);
                else
                    mod.Jealousy_OnGiftToOther(__instance, o);
            }
            catch (Exception ex)
            {
                Monitor?.Log($"ReceiveGift postfix error: {ex.Message}", LogLevel.Trace);
            }
        }
    }
}
