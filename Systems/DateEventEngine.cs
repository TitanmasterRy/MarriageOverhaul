using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // Where to return the player after the date cutscene ends.
        private string preDateLocation;
        private int preDateX, preDateY, preDateDir;
        // Tracks an in-progress date event so we can warp home when it finishes.
        private bool dateEventWatching;
        private bool dateEventSeenActive;

        /// <summary>Verbose step logging for the date cutscene path (only when debug commands are on).</summary>
        private void DateDebug(string message)
        {
            if (this.Config.EnableDebugCommands)
                this.Monitor.Log($"[date-debug] {message}", LogLevel.Info);
        }

        /// <summary>Pick a date-event index that isn't the same as the one used last time.</summary>
        private int PickDateEventIndex(string name, int count)
        {
            int last = this.Data.LastDateEventIndex.TryGetValue(name, out int v) ? v : -1;
            int idx = Game1.random.Next(count);
            if (count > 1 && idx == last)
                idx = (idx + 1) % count;
            this.Data.LastDateEventIndex[name] = idx;
            return idx;
        }

        // Small reactions woven between spoken lines so scenes feel alive instead of static.
        // Mix of the spouse (hop / happy face / flustered shake / heart) and the player reacting back.
        private static readonly string[] DateReactions =
        {
            "jump {n}", "emote {n} 32", "emote farmer 20", "jump farmer", "shake {n} 150", "emote {n} 20"
        };

        /// <summary>Build a vanilla event script string positioning the farmer and spouse with portrait dialogue.</summary>
        private string BuildDateEventScript(NPC spouse, DateEventScript def, DateSpot spot)
        {
            string n = spouse.Name;
            var parts = new List<string>();

            // music slot / viewport (centered on the staging tile) / actor positions.
            // Both actors start facing the same way; the spouse stands one tile east of the farmer.
            parts.Add("continue");
            parts.Add($"{spot.X} {spot.Y}");
            parts.Add($"farmer {spot.X} {spot.Y} {spot.Facing} {n} {spot.X + 1} {spot.Y} {spot.Facing}");
            parts.Add("skippable");
            parts.Add("pause 600");
            parts.Add("playMusic junimoStarSong"); // a warm cue sets the mood; invalid cues are simply silent

            if (def.Beats != null)
            {
                for (int i = 0; i < def.Beats.Count; i++)
                {
                    string raw = def.Beats[i];
                    parts.Add(raw.Replace("{n}", n));

                    // After a spoken line (but not the last beat), sometimes weave in a little life.
                    bool isSpeak = raw.StartsWith("speak {n}");
                    bool isLast = i >= def.Beats.Count - 1;
                    if (isSpeak && !isLast && Game1.random.NextDouble() < 0.5)
                    {
                        parts.Add(DateReactions[Game1.random.Next(DateReactions.Length)].Replace("{n}", n));
                        parts.Add("pause 250");
                    }
                }
            }

            parts.Add("globalFade");
            parts.Add("pause 1200");
            parts.Add("end");
            return string.Join("/", parts);
        }

        /// <summary>Warp to the spouse's date spot and play the positioned cutscene.</summary>
        internal void StartDateCutscene(NPC spouse, DateEventScript def)
        {
            this.StartDateCutscene(spouse, def, SpouseContent.GetDateSpot(spouse.Name));
        }

        /// <summary>Play a handcrafted date scene (its own location + choreography).</summary>
        internal void StartDateScene(NPC spouse, DateScene scene)
        {
            var def = new DateEventScript { Beats = scene.Beats };
            var spot = new DateSpot { Map = scene.Map, X = scene.X, Y = scene.Y, Facing = scene.Facing };
            this.StartDateCutscene(spouse, def, spot);
        }

        /// <summary>Warp to a specific spot and play the positioned cutscene.</summary>
        internal void StartDateCutscene(NPC spouse, DateEventScript def, DateSpot spot)
        {
            // Never start while the world isn't safely interactive (avoids warp-during-warp crashes).
            if (!Context.IsPlayerFree || Game1.eventUp || Game1.activeClickableMenu != null || Game1.isFestival())
            {
                this.ShowNarration(SpouseContent.GetDate(spouse.Name).Scene);
                return;
            }

            try
            {
                this.DateDebug($"StartDateCutscene: spouse='{spouse.Name}', map='{spot.Map}', tile=({spot.X},{spot.Y})");
                this.LogDateLocationDiagnostics(spouse, spot);

                // Remember where the player was so we can bring them back afterward.
                this.preDateLocation = Game1.currentLocation?.NameOrUniqueName;
                this.preDateX = Game1.player.TilePoint.X;
                this.preDateY = Game1.player.TilePoint.Y;
                this.preDateDir = Game1.player.FacingDirection;
                this.DateDebug($"captured return point: {this.preDateLocation} ({this.preDateX},{this.preDateY}) dir {this.preDateDir}");

                string script = this.BuildDateEventScript(spouse, def, spot);
                this.DateDebug($"built event script:\n{script}");

                // Let warpFarmer handle the fade itself (nesting a manual globalFadeToBlack around it
                // can desync the fade state machine and hard-crash). Start the event on arrival.
                LocationRequest request = Game1.getLocationRequest(spot.Map);
                request.OnWarp += () =>
                {
                    this.DateDebug($"OnWarp fired; now in '{Game1.currentLocation?.NameOrUniqueName}'. Calling startEvent...");
                    try
                    {
                        Game1.currentLocation.startEvent(new Event(script));
                        this.dateEventWatching = true;
                        this.dateEventSeenActive = false;
                        this.DateDebug("startEvent returned without throwing.");
                    }
                    catch (Exception ex)
                    {
                        // If the event can't start, never leave the player stranded — send them home.
                        this.Monitor.Log($"Date event failed to start; returning home: {ex}", LogLevel.Warn);
                        this.dateEventWatching = false;
                        this.ReturnFromDate();
                    }
                };
                this.DateDebug("requesting warp...");
                Game1.warpFarmer(request, spot.X, spot.Y, this.preDateDir);
                this.DateDebug("warpFarmer call returned (warp is queued).");
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Date cutscene failed; using narration instead: {ex.Message}", LogLevel.Warn);
                this.ShowNarration(SpouseContent.GetDate(spouse.Name).Scene);
            }
        }

        /// <summary>Log diagnostics about the target location, tile and spouse portrait before warping.</summary>
        internal void LogDateLocationDiagnostics(NPC spouse, DateSpot spot)
        {
            if (!this.Config.EnableDebugCommands)
                return;
            try
            {
                GameLocation loc = Game1.getLocationFromName(spot.Map);
                this.DateDebug($"location '{spot.Map}' exists: {loc != null}");
                if (loc != null)
                {
                    bool onMap = loc.isTileOnMap(new Microsoft.Xna.Framework.Vector2(spot.X, spot.Y));
                    this.DateDebug($"tile ({spot.X},{spot.Y}) on map: {onMap}");
                    try
                    {
                        bool passable = loc.isTilePassable(new xTile.Dimensions.Location(spot.X, spot.Y), Game1.viewport);
                        this.DateDebug($"tile ({spot.X},{spot.Y}) passable: {passable}");
                    }
                    catch (Exception ex) { this.DateDebug($"passability check threw: {ex.Message}"); }
                }
                this.DateDebug($"spouse portrait loaded: {spouse?.Portrait != null}");
            }
            catch (Exception ex)
            {
                this.DateDebug($"location diagnostics threw: {ex.Message}");
            }
        }

        /// <summary>Watch for the date cutscene to finish, then return the player to where they were.</summary>
        private void DateEvent_WatchForEnd()
        {
            if (!this.dateEventWatching)
                return;

            if (Game1.eventUp)
            {
                if (!this.dateEventSeenActive)
                    this.DateDebug("event is now active (eventUp = true).");
                this.dateEventSeenActive = true;
                return;
            }

            // Event was active and is now over.
            if (this.dateEventSeenActive)
            {
                this.dateEventWatching = false;
                this.dateEventSeenActive = false;
                this.DateDebug("event ended; returning player home.");
                this.ReturnFromDate();
            }
        }

        private void ReturnFromDate()
        {
            if (string.IsNullOrEmpty(this.preDateLocation))
                return;
            try
            {
                Game1.warpFarmer(this.preDateLocation, this.preDateX, this.preDateY, this.preDateDir);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Could not return player home after date: {ex.Message}", LogLevel.Trace);
            }
        }
    }
}
