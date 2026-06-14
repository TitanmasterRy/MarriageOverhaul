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

            // Spouse kiss/hug + dialogue-mod compatibility (both handled in the checkAction prefix).
            var checkAction = AccessTools.Method(typeof(NPC), nameof(NPC.checkAction));
            if (checkAction != null)
                harmony.Patch(checkAction,
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CheckAction_Prefix)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CheckAction_Postfix)),
                    finalizer: new HarmonyMethod(typeof(HarmonyPatches), nameof(CheckAction_Finalizer)));
        }

        /// <summary>Dialogue set aside during a kiss interaction so it can be restored afterward.</summary>
        private class KissState
        {
            public Dialogue[] Current;
            public MarriageDialogueReference[] Marriage;
        }

        /// <summary>Whether a genuine vanilla kiss would fire this interaction (10+ hearts, slept in bed, empty-handed, beside the spouse, before 10pm, not yet kissed).</summary>
        private static bool IsKissEligible(NPC npc, Farmer who, ModEntry mod)
        {
            if (!mod.Config.AllowSpouseKiss)
                return false;
            if (npc.hasBeenKissedToday.Value)
                return false;
            if (who.ActiveObject != null)                               // holding an item — gifting/talking instead
                return false;
            if (npc.Sprite == null || npc.Sprite.CurrentAnimation != null) // no sprite (transitional) or mid-animation — vanilla's kiss would null-ref
                return false;
            if (npc.currentLocation == null || npc.currentLocation != who.currentLocation) // spouse not in the player's location (e.g. warping)
                return false;
            if (!npc.sleptInBed.Value)
                return false;
            if (who.getFriendshipHeartLevelForNPC(npc.Name) <= 9)       // below 10 hearts vanilla won't kiss
                return false;
            if (Game1.timeOfDay >= 2200)
                return false;
            // The kiss only fires when the spouse faces left/right, i.e. the player is beside them.
            if ((int)who.Tile.Y != (int)npc.Tile.Y || Math.Abs((int)who.Tile.X - (int)npc.Tile.X) != 1)
                return false;
            return true;
        }

        /// <summary>
        /// Handles two things when the player interacts with their spouse:
        /// 1. The kiss/hug: vanilla only kisses when the dialogue is clear, so if a kiss is due we set the
        ///    queued dialogue aside (restored in the postfix) so the real vanilla kiss runs.
        /// 2. Dialogue-mod compatibility: marriage-dialogue mods (e.g. Haley Ever After) fill the spouse's
        ///    dialogue and the game draws it before our queued greeting can show. So when no kiss is due we
        ///    show this mod's deferred greeting ourselves and skip the original, so it always appears.
        /// Returns false to skip the original method (greeting shown), true to let it run.
        /// </summary>
        private static bool CheckAction_Prefix(NPC __instance, Farmer who, ref KissState __state, ref bool __result)
        {
            __state = null;
            try
            {
                ModEntry mod = ModEntry.Instance;
                if (mod?.Config == null || who == null || !who.IsLocalPlayer || __instance == null)
                    return true;
                if (__instance.Name != who.spouse)
                    return true;
                if (Game1.eventUp)   // festivals / events use their own dialogue — stay out of the way
                    return true;

                // 1. Kiss path — if a kiss is due, clear blocking dialogue so vanilla's kiss runs, then let original run.
                if (IsKissEligible(__instance, who, mod))
                {
                    if (__instance.isMoving())
                        __instance.Halt(); // clears the movement flags vanilla's "not moving" kiss check reads

                    bool hasCurrent = __instance.CurrentDialogue.Count > 0;
                    bool hasMarriage = __instance.currentMarriageDialogue.Count > 0;
                    if (hasCurrent || hasMarriage)
                    {
                        __state = new KissState
                        {
                            Current = __instance.CurrentDialogue.ToArray(),
                            Marriage = hasMarriage ? __instance.currentMarriageDialogue.ToArray() : null
                        };
                        __instance.CurrentDialogue.Clear();
                        if (hasMarriage)
                            __instance.currentMarriageDialogue.Clear();
                    }
                    return true; // let original run → vanilla kiss
                }

                // 2. Dialogue compat — no kiss this interaction, so show our deferred greeting (it survives
                //    marriage-dialogue mods because we draw it directly instead of leaving it on the stack).
                if (mod.Config.EnableDialogueCompat && who.ActiveObject == null && mod.HasPendingSpouseLines())
                {
                    List<string> lines = mod.TakePendingSpouseLines();
                    foreach (string line in lines)
                        __instance.CurrentDialogue.Push(new Dialogue(__instance, null, line));
                    Game1.drawDialogue(__instance);
                    __result = true;
                    return false; // we handled this interaction; the marriage line shows on the next talk
                }

                return true;
            }
            catch (Exception ex)
            {
                __state = null;
                Monitor?.Log($"checkAction prefix error: {ex.Message}", LogLevel.Trace);
                return true;
            }
        }

        private static void CheckAction_Postfix(NPC __instance, KissState __state)
        {
            if (__state == null)
                return;
            try { RestoreKissDialogue(__instance, __state); }
            catch (Exception ex) { Monitor?.Log($"checkAction postfix error: {ex.Message}", LogLevel.Trace); }
        }

        /// <summary>
        /// Safety net: if the original NPC.checkAction (or an inner patch) throws, swallow it so the game
        /// doesn't crash on an NPC interaction, and restore any dialogue the kiss path set aside (the
        /// postfix doesn't run when the original throws).
        /// </summary>
        private static Exception CheckAction_Finalizer(NPC __instance, KissState __state, Exception __exception)
        {
            if (__exception == null)
                return null;
            try { RestoreKissDialogue(__instance, __state); } catch { }
            Monitor?.Log($"Suppressed an error in NPC.checkAction to avoid a crash: {__exception.Message}", LogLevel.Warn);
            return null; // swallow the exception
        }

        /// <summary>Restore dialogue the kiss path set aside, if it hasn't already been repopulated.</summary>
        private static void RestoreKissDialogue(NPC npc, KissState state)
        {
            if (state == null || npc == null)
                return;
            if (state.Current != null && npc.CurrentDialogue.Count == 0)
            {
                for (int i = state.Current.Length - 1; i >= 0; i--)
                    npc.CurrentDialogue.Push(state.Current[i]);
            }
            if (state.Marriage != null && npc.currentMarriageDialogue.Count == 0)
            {
                foreach (MarriageDialogueReference r in state.Marriage)
                    npc.currentMarriageDialogue.Add(r);
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
