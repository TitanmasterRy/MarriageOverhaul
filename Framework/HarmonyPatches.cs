using System;
using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
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

            // F3: evolving gift preferences — adjust the spouse's gift taste for items tied to shared history.
            var taste = AccessTools.Method(typeof(NPC), nameof(NPC.getGiftTasteForThisItem));
            if (taste != null)
                harmony.Patch(taste, postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(GiftTaste_Postfix)));

            // Optional: mark the anniversary on the calendar (Billboard).
            var billboardDraw = AccessTools.Method(typeof(Billboard), nameof(Billboard.draw), new[] { typeof(SpriteBatch) });
            if (billboardDraw != null)
                harmony.Patch(billboardDraw, postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Billboard_Draw_Postfix)));
        }

        /// <summary>Draw a small heart on the anniversary day of the calendar (if enabled).</summary>
        private static void Billboard_Draw_Postfix(Billboard __instance, SpriteBatch b)
        {
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod == null || mod.Config == null || !mod.Config.ShowAnniversaryOnCalendar)
                    return;
                if (mod.GetSpouse() == null)
                    return;

                int day = mod.AnniversaryDayThisSeason();
                if (day < 1)
                    return;

                List<ClickableTextureComponent> days = mod.GetCalendarDayComponents(__instance);
                if (days == null || days.Count < day)
                    return;

                Rectangle bounds = days[day - 1].bounds;
                // A heart icon in the top-left of the anniversary day's tile.
                b.Draw(
                    Game1.mouseCursors,
                    new Vector2(bounds.X + 6, bounds.Y + 6),
                    new Rectangle(211, 428, 7, 6),
                    Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            }
            catch (Exception ex)
            {
                Monitor?.Log($"Billboard draw postfix error: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>After a year of marriage, improve the spouse's taste for evolution items.</summary>
        private static void GiftTaste_Postfix(NPC __instance, Item item, ref int __result)
        {
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod == null || __instance == null || item == null)
                    return;
                if (__instance.Name != mod.SpouseName)
                    return;

                int evolved = mod.Preferences_EvolvedTaste(__instance.Name, item);
                if (evolved >= 0 && evolved < __result) // lower taste id = more positive
                    __result = evolved;
            }
            catch (Exception ex)
            {
                Monitor?.Log($"GiftTaste postfix error: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>During a makeup state, regular gifts to the spouse give half friendship.</summary>
        private static void ReceiveGift_Prefix(NPC __instance, SObject o, Farmer giver, ref float friendshipChangeMultiplier)
        {
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod == null || giver == null || !giver.IsLocalPlayer)
                    return;

                if (__instance == null || __instance.Name != mod.SpouseName)
                    return;

                if (mod.IsMakeupActive())
                    friendshipChangeMultiplier *= 0.5f;

                // F9 + F13: favorite-season and honeymoon gift bonuses.
                friendshipChangeMultiplier *= mod.GiftFriendshipMultiplier(__instance);
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
