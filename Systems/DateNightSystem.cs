using System.Collections.Generic;
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
                this.PushDialogue(spouse, "...Oh. Okay. Maybe another time, then.");
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

            // Prefer a real positioned cutscene when one exists — but only if the (experimental) option is on.
            List<DateEventScript> events = (movie || !this.Config.EnableDateCutscenes) ? null : SpouseContent.GetDateEvents(spouse.Name);
            if (events != null && events.Count > 0)
            {
                this.ChangeSpouseFriendship(100);
                int idx = this.PickDateEventIndex(spouse.Name, events.Count);
                this.StartDateCutscene(spouse, events[idx]);
                return;
            }

            // Otherwise fall back to a narration "moment".
            DateInfo info = movie ? SpouseContent.GetMovieDate(spouse.Name) : SpouseContent.GetDate(spouse.Name);
            this.ShowNarration(info.Scene);
            this.ChangeSpouseFriendship(movie ? 120 : 100); // a movie night is a slightly bigger treat
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
