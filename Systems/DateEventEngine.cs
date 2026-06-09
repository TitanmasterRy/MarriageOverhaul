using System;
using System.Text;
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

        /// <summary>Build a vanilla event script string positioning the farmer and spouse with portrait dialogue.</summary>
        private string BuildDateEventScript(NPC spouse, DateEventScript def)
        {
            string n = spouse.Name;
            var sb = new StringBuilder();

            // music / viewport (centered on the staging tile) / actor positions.
            // Both actors start with the same facing (toward the ocean by default); the spouse stands
            // beside the farmer at the configured offset.
            sb.Append($"continue/{def.X} {def.Y}/");
            sb.Append($"farmer {def.X} {def.Y} {def.Facing} {n} {def.X + def.SpouseDX} {def.Y + def.SpouseDY} {def.Facing}/");
            sb.Append("skippable/");
            sb.Append("pause 1000/");

            if (def.Beats != null && def.Beats.Count > 0)
            {
                // Choreographed beats: raw event commands with "{n}" standing in for the spouse's name.
                foreach (string beat in def.Beats)
                {
                    sb.Append(beat.Replace("{n}", n));
                    sb.Append('/');
                }
            }
            else if (def.Lines != null)
            {
                // Simple fallback: each line becomes a spoken line, with a heart on the first.
                bool firstLine = true;
                foreach (string raw in def.Lines)
                {
                    sb.Append($"speak {n} \"{Sanitize(raw)}\"/");
                    if (firstLine)
                    {
                        sb.Append($"emote {n} 20/");
                        firstLine = false;
                    }
                    sb.Append("pause 400/");
                }
            }

            sb.Append("globalFade/pause 1200/end");
            return sb.ToString();
        }

        /// <summary>Strip characters that would break the event-script grammar.</summary>
        private static string Sanitize(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            return text.Replace("/", ",").Replace("\"", "'").Replace("^", " ").Replace("%", "");
        }

        /// <summary>Warp to the date location and play the positioned cutscene.</summary>
        internal void StartDateCutscene(NPC spouse, DateEventScript def)
        {
            // Never start while the world isn't safely interactive (avoids warp-during-warp crashes).
            if (!Context.IsPlayerFree || Game1.eventUp || Game1.activeClickableMenu != null || Game1.isFestival())
            {
                this.ShowNarration(SpouseContent.GetDate(spouse.Name).Scene);
                return;
            }

            try
            {
                this.DateDebug($"StartDateCutscene: spouse='{spouse.Name}', map='{def.Map}', tile=({def.X},{def.Y})");
                this.LogDateLocationDiagnostics(spouse, def);

                // Remember where the player was so we can bring them back afterward.
                this.preDateLocation = Game1.currentLocation?.NameOrUniqueName;
                this.preDateX = Game1.player.TilePoint.X;
                this.preDateY = Game1.player.TilePoint.Y;
                this.preDateDir = Game1.player.FacingDirection;
                this.DateDebug($"captured return point: {this.preDateLocation} ({this.preDateX},{this.preDateY}) dir {this.preDateDir}");

                string script = this.BuildDateEventScript(spouse, def);
                this.DateDebug($"built event script:\n{script}");

                // Let warpFarmer handle the fade itself (nesting a manual globalFadeToBlack around it
                // can desync the fade state machine and hard-crash). Start the event on arrival.
                LocationRequest request = Game1.getLocationRequest(def.Map);
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
                        this.Monitor.Log($"Date event failed to start: {ex}", LogLevel.Warn);
                    }
                };
                this.DateDebug("requesting warp...");
                Game1.warpFarmer(request, def.X, def.Y, this.preDateDir);
                this.DateDebug("warpFarmer call returned (warp is queued).");
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Date cutscene failed; using narration instead: {ex.Message}", LogLevel.Warn);
                this.ShowNarration(SpouseContent.GetDate(spouse.Name).Scene);
            }
        }

        /// <summary>Log diagnostics about the target location, tile and spouse portrait before warping.</summary>
        internal void LogDateLocationDiagnostics(NPC spouse, DateEventScript def)
        {
            if (!this.Config.EnableDebugCommands)
                return;
            try
            {
                GameLocation loc = Game1.getLocationFromName(def.Map);
                this.DateDebug($"location '{def.Map}' exists: {loc != null}");
                if (loc != null)
                {
                    bool onMap = loc.isTileOnMap(new Microsoft.Xna.Framework.Vector2(def.X, def.Y));
                    this.DateDebug($"tile ({def.X},{def.Y}) on map: {onMap}");
                    try
                    {
                        bool passable = loc.isTilePassable(new xTile.Dimensions.Location(def.X, def.Y), Game1.viewport);
                        this.DateDebug($"tile ({def.X},{def.Y}) passable: {passable}");
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
