using System;
using System.Collections.Generic;
using System.Linq;
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

            // Keep the vanilla spouse kiss/hug available even when the mod has queued dialogue.
            var checkAction = AccessTools.Method(typeof(NPC), nameof(NPC.checkAction));
            if (checkAction != null)
                harmony.Patch(checkAction,
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CheckAction_Prefix)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CheckAction_Postfix)));

            // Keep our spouse greetings alive when a marriage-dialogue mod replaces the dialogue on talk.
            var checkNewDialogue = AccessTools.Method(typeof(NPC), nameof(NPC.checkForNewCurrentDialogue));
            if (checkNewDialogue != null)
                harmony.Patch(checkNewDialogue, postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CheckForNewDialogue_Postfix)));
        }

        /// <summary>
        /// When the player talks, the game's checkForNewCurrentDialogue clears the spouse's dialogue stack
        /// and pushes the day's marriage dialogue (returns true). Marriage-dialogue expansion mods supply a
        /// line every day, which wipes the greeting this mod queued. Here we re-push our queued greeting on
        /// top (once per day) so it still shows — our line first, then the marriage-dialogue mod's line.
        /// </summary>
        private static void CheckForNewDialogue_Postfix(NPC __instance, bool __result)
        {
            try
            {
                if (!__result) // false = nothing was substituted, so our queued greeting is still intact
                    return;
                ModEntry mod = ModEntry.Instance;
                if (mod?.Config == null || !mod.Config.EnableDialogueCompat)
                    return;
                if (__instance == null || string.IsNullOrEmpty(mod.SpouseName) || __instance.Name != mod.SpouseName)
                    return;

                List<string> lines = mod.TakePendingSpouseLines();
                if (lines == null)
                    return;
                foreach (string line in lines)
                    __instance.CurrentDialogue.Push(new Dialogue(__instance, null, line));
            }
            catch (Exception ex)
            {
                Monitor?.Log($"checkForNewCurrentDialogue postfix error: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Dialogue set aside during a kiss interaction so it can be restored afterward.</summary>
        private class KissState
        {
            public Dialogue[] Current;
            public MarriageDialogueReference[] Marriage;
        }

        /// <summary>
        /// Vanilla only kisses the spouse when BOTH their dialogue stack and their daily marriage
        /// dialogue are empty, but this mod queues morning dialogue. When the player is positioned for a
        /// real kiss (10+ hearts, slept in bed, empty-handed, beside the spouse, before 10pm, not yet
        /// kissed today) we briefly set that dialogue aside so the genuine vanilla kiss runs — which keeps
        /// kiss-based mods working — then restore it so it's still readable on the next interaction.
        /// </summary>
        private static void CheckAction_Prefix(NPC __instance, Farmer who, ref KissState __state)
        {
            __state = null;
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod?.Config == null || !mod.Config.AllowSpouseKiss)
                    return;
                if (who == null || !who.IsLocalPlayer || __instance == null)
                    return;
                if (__instance.Name != who.spouse)                          // not the player's spouse
                    return;
                if (__instance.hasBeenKissedToday.Value)                    // already kissed — let dialogue show
                    return;
                if (who.ActiveObject != null)                               // holding an item — let gifting/talking happen
                    return;
                if (__instance.Sprite?.CurrentAnimation != null)            // mid-animation — vanilla skips the kiss
                    return;
                if (!__instance.sleptInBed.Value)                           // vanilla requires this for a kiss
                    return;
                if (who.getFriendshipHeartLevelForNPC(__instance.Name) <= 9) // below 10 hearts vanilla won't kiss anyway
                    return;
                if (Game1.timeOfDay >= 2200)
                    return;

                // The kiss only fires when the spouse faces left/right, i.e. the player is beside them.
                if ((int)who.Tile.Y != (int)__instance.Tile.Y || Math.Abs((int)who.Tile.X - (int)__instance.Tile.X) != 1)
                    return;

                bool hasCurrent = __instance.CurrentDialogue.Count > 0;
                bool hasMarriage = __instance.currentMarriageDialogue.Count > 0;
                if (!hasCurrent && !hasMarriage)
                    return; // nothing blocking — vanilla already handles the kiss

                // If the spouse is wandering, stop them so vanilla's own "not moving" kiss check passes
                // (normally the game halts them when you talk, but the queued dialogue pre-empts that).
                // Halt() clears the movement flags that isMoving() reads.
                if (__instance.isMoving())
                    __instance.Halt();

                // Set both dialogue sources aside so vanilla's kiss branch runs this interaction.
                __state = new KissState
                {
                    Current = __instance.CurrentDialogue.ToArray(),                 // top-first
                    Marriage = hasMarriage ? __instance.currentMarriageDialogue.ToArray() : null
                };
                __instance.CurrentDialogue.Clear();
                if (hasMarriage)
                    __instance.currentMarriageDialogue.Clear();
            }
            catch (Exception ex)
            {
                __state = null;
                Monitor?.Log($"checkAction prefix error: {ex.Message}", LogLevel.Trace);
            }
        }

        private static void CheckAction_Postfix(NPC __instance, KissState __state)
        {
            if (__state == null)
                return;
            try
            {
                // Restore the dialogue (the kiss consumed this interaction; the lines stay for the next talk).
                if (__state.Current != null && __instance.CurrentDialogue.Count == 0)
                {
                    for (int i = __state.Current.Length - 1; i >= 0; i--)
                        __instance.CurrentDialogue.Push(__state.Current[i]);
                }
                if (__state.Marriage != null && __instance.currentMarriageDialogue.Count == 0)
                {
                    foreach (MarriageDialogueReference r in __state.Marriage)
                        __instance.currentMarriageDialogue.Add(r);
                }
            }
            catch (Exception ex)
            {
                Monitor?.Log($"checkAction postfix error: {ex.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Draw the anniversary heart and/or birthday gift icons on the calendar (if enabled).</summary>
        private static void Billboard_Draw_Postfix(Billboard __instance, SpriteBatch b)
        {
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod == null || mod.Config == null)
                    return;

                // Only draw on the calendar page, not the daily-quest/special-orders board.
                bool? isQuestBoard = mod.Helper.Reflection.GetField<bool>(__instance, "dailyQuestBoard", required: false)?.GetValue();
                if (isQuestBoard == true)
                    return;

                List<ClickableTextureComponent> days = mod.GetCalendarDayComponents(__instance);
                if (days == null)
                    return;

                // Anniversary heart.
                if (mod.Config.AnniversaryCalendarMarker && mod.GetSpouse() != null)
                {
                    int annivDay = mod.AnniversaryDayThisSeason();
                    if (annivDay >= 1 && days.Count >= annivDay)
                    {
                        Rectangle bounds = days[annivDay - 1].bounds;
                        // A heart icon in the top-left of the anniversary day's tile.
                        b.Draw(
                            Game1.mouseCursors,
                            new Vector2(bounds.X + 6, bounds.Y + 6),
                            new Rectangle(211, 428, 7, 6),
                            Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                    }
                }

                // Player birthday marker.
                if (mod.Config.BirthdayCalendarMarker)
                {
                    int birthDay = mod.BirthdayDayThisSeason();
                    if (birthDay >= 1 && days.Count >= birthDay)
                    {
                        Rectangle bounds = days[birthDay - 1].bounds;
                        // The same heart shape, tinted gold and placed in the top-right corner so it
                        // can't be confused with the anniversary heart even on the same day.
                        b.Draw(
                            Game1.mouseCursors,
                            new Vector2(bounds.Right - 27, bounds.Y + 6),
                            new Rectangle(211, 428, 7, 6),
                            Color.Gold, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                    }
                }
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
