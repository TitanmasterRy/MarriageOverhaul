using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        private const int DateCooldownDays = 14;
        private const double MovieDateChance = 0.5; // when the theater is unlocked, half of date offers are movie outings

        /// <summary>Morning: the spouse may ask to go out tonight if the relationship is strong.</summary>
        private void DateNight_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableDateNights || this.Data.DateOfferedToday)
                return;

            // Only when things are good: above 10 hearts.
            if (this.GetSpousePoints() <= this.HeartsToPoints(10))
                return;

            // Roughly every two weeks.
            if (this.AbsoluteDay - this.Data.LastDateNightDay < DateCooldownDays)
                return;

            // Not during festivals, and not in a week where an argument happened.
            if (this.IsFestivalToday())
                return;
            if (this.ArgumentHappenedThisWeek())
                return;

            // Once the movie theater is unlocked, some dates become a trip to the movies instead.
            bool movie = this.Config.EnableMovieDates
                && this.IsMovieTheaterUnlocked()
                && Game1.random.NextDouble() < MovieDateChance;

            // Queue the invitation; it's shown by DateNight_TryPresentPending once the player is free
            // (so it never interrupts the wedding cutscene or other morning events).
            this.pendingDateOffer = true;
            this.pendingDateOfferMovie = movie;
        }

        /// <summary>Present a queued date offer once the player is in control. Called every tick.</summary>
        internal void DateNight_TryPresentPending()
        {
            if (!this.pendingDateOffer)
                return;
            if (!Context.IsPlayerFree)
                return;
            // The spouse asks at home in the morning, not wherever you happen to be standing.
            if (Game1.currentLocation is not FarmHouse)
                return;

            NPC spouse = this.GetSpouse();
            this.pendingDateOffer = false;
            if (spouse == null)
                return;

            this.Data.LastDateNightDay = this.AbsoluteDay; // consume the cooldown whether or not accepted
            this.PresentDateOffer(spouse, this.pendingDateOfferMovie);
        }

        /// <summary>Show the accept/decline date invitation (also used by the debug command).</summary>
        internal void PresentDateOffer(NPC spouse, bool movie)
        {
            this.Data.DateOfferedToday = true;

            DateInfo info = movie ? SpouseContent.GetMovieDate(spouse.Name) : SpouseContent.GetDate(spouse.Name);
            var responses = new Response[]
            {
                new Response("MO_date_yes", "Yes, I'd love to."),
                new Response("MO_date_no", "Not tonight.")
            };

            GameLocation location = Game1.currentLocation;
            location.createQuestionDialogue(
                info.Ask,
                responses,
                (Farmer who, string answer) => this.ResolveDateOffer(spouse, answer, movie));
        }

        private void ResolveDateOffer(NPC spouse, string answer, bool movie)
        {
            if (answer == "MO_date_yes")
            {
                this.Data.DateAcceptedTonight = true;
                this.Data.MovieDateTonight = movie;
            }
            else
            {
                this.ChangeSpouseFriendship(-30);
                this.forceGrumpyToday = true;
                this.PushDialogue(spouse, "...Oh. Okay. Maybe another time, then.", "sad");
            }
        }

        /// <summary>Evening: at 8pm, play the date scene if the player accepted.</summary>
        private void DateNight_OnTimeChanged(NPC spouse, int time)
        {
            if (!this.Config.EnableDateNights || !this.Data.DateAcceptedTonight || this.dateSceneShownToday)
                return;
            if (time < 2000)
                return;
            if (Game1.activeClickableMenu != null || Game1.eventUp)
                return;

            this.dateSceneShownToday = true;
            this.Data.DateAcceptedTonight = false;
            this.Data.DatePlayedTonight = true;

            bool movie = this.Data.MovieDateTonight;

            // A real handcrafted cutscene plays when cutscenes are enabled and it isn't a movie night.
            if (!movie && this.Config.EnableDateCutscenes)
            {
                this.ChooseAndPlayDateScene(spouse);
                return;
            }

            // Otherwise fall back to a narration "moment".
            DateInfo info = movie ? SpouseContent.GetMovieDate(spouse.Name) : SpouseContent.GetDate(spouse.Name);
            this.ShowNarration(info.Scene);
            this.ChangeSpouseFriendship(movie ? 120 : 100); // a movie night is a slightly bigger treat
        }

        /// <summary>
        /// Select tonight's date scene and play it: a spouse's unseen unique scenes first (random order,
        /// weather-aware), then the generic pool (rotating, no repeat within 3), then a freeform scene.
        /// </summary>
        internal void ChooseAndPlayDateScene(NPC spouse)
        {
            this.ChangeSpouseFriendship(100);

            DateScene scene = this.SelectDateScene(spouse.Name);
            if (scene != null)
                this.StartDateScene(spouse, scene);
            else
                this.PlayFreeformDate(spouse);
        }

        private DateScene SelectDateScene(string name)
        {
            // 1) Unique handcrafted scenes first, in random order, respecting weather requirements.
            var unique = DateScenes.GetUnique(name);
            if (unique != null && unique.Count > 0)
            {
                List<int> seen = this.GetSeenList(this.Data.SeenUniqueDateScenes, name);
                var playable = Enumerable.Range(0, unique.Count)
                    .Where(i => !seen.Contains(i) && this.WeatherAllows(unique[i]))
                    .ToList();
                if (playable.Count > 0)
                {
                    int idx = playable[Game1.random.Next(playable.Count)];
                    seen.Add(idx);
                    return unique[idx];
                }
                // Remaining unique scenes exist but can't play tonight (e.g. waiting on rain) — use the
                // generic pool for now and leave the gated scene unseen so it can play another night.
            }

            // 2) Generic pool: rotate through unseen, avoiding anything used within the last 3 dates.
            int genCount = DateScenes.GenericCount;
            List<int> gseen = this.GetSeenList(this.Data.SeenGenericDateScenes, name);
            var gunseen = Enumerable.Range(0, genCount).Where(i => !gseen.Contains(i)).ToList();
            if (gunseen.Count > 0)
            {
                var candidates = gunseen.Where(i => !this.Data.RecentGenericDateScenes.Contains(i)).ToList();
                if (candidates.Count == 0)
                    candidates = gunseen;
                int idx = candidates[Game1.random.Next(candidates.Count)];
                gseen.Add(idx);
                this.PushRecentGeneric(idx);
                return DateScenes.GenericPool[idx];
            }

            // 3) Everything has been seen — assemble a freeform scene from the shared dialogue pool.
            return null;
        }

        private void PlayFreeformDate(NPC spouse)
        {
            var beats = DateDialoguePool.BuildFreeformBeats(Game1.random);
            var scene = new DateScene { Map = "FarmHouse", X = 6, Y = 8, Facing = 2, Beats = beats };
            this.StartDateScene(spouse, scene);
        }

        private List<int> GetSeenList(Dictionary<string, List<int>> map, string name)
        {
            if (map == null)
                return new List<int>();
            if (!map.TryGetValue(name, out var list))
            {
                list = new List<int>();
                map[name] = list;
            }
            return list;
        }

        private void PushRecentGeneric(int idx)
        {
            this.Data.RecentGenericDateScenes.Add(idx);
            while (this.Data.RecentGenericDateScenes.Count > 3)
                this.Data.RecentGenericDateScenes.RemoveAt(0);
        }

        private bool WeatherAllows(DateScene scene)
        {
            if (string.IsNullOrEmpty(scene.Weather))
                return true;
            if (scene.Weather == "Rain")
                return this.IsRainingNow();
            return true;
        }

        private bool IsRainingNow()
        {
            try
            {
                return Game1.getFarm()?.IsRainingHere() ?? false;
            }
            catch
            {
                return false;
            }
        }

        private void DateNight_OnDayEnding(NPC spouse)
        {
            // Reset the per-day date flags for next morning.
            this.Data.DateOfferedToday = false;
            this.Data.DateAcceptedTonight = false;
            this.Data.DatePlayedTonight = false;
            this.Data.MovieDateTonight = false;
        }

        /// <summary>Whether the movie theater has been built (Community Center or Joja route).</summary>
        private bool IsMovieTheaterUnlocked()
        {
            try
            {
                if (Game1.MasterPlayer != null
                    && (Game1.MasterPlayer.mailReceived.Contains("ccMovieTheater")
                        || Game1.MasterPlayer.mailReceived.Contains("ccMovieTheaterJoja")
                        || Game1.MasterPlayer.mailReceived.Contains("jojaMovieTheater")))
                    return true;
                return Game1.getLocationFromName("MovieTheater") != null;
            }
            catch
            {
                return false;
            }
        }

        private bool IsFestivalToday()
        {
            try
            {
                return Utility.isFestivalDay(Game1.dayOfMonth, Game1.season);
            }
            catch
            {
                return false;
            }
        }
    }
}
