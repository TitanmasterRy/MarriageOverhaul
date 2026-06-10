using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        private const int WarningCooldownDays = 14;

        /// <summary>
        /// End of day: track consecutive low-heart days, send the warning letter, and queue
        /// the auto-divorce farewell when the relationship has been broken long enough.
        /// </summary>
        private void Divorce_OnDayEnding(NPC spouse)
        {
            if (!this.Config.EnableDivorceWarning && !this.Config.EnableAutoDivorce)
                return;

            int threshold = this.HeartsToPoints(this.Config.DivorceWarningThresholdHearts);
            int points = this.GetSpousePoints();

            if (points < threshold)
            {
                this.Data.ConsecutiveLowDays++;
            }
            else
            {
                this.Data.ConsecutiveLowDays = 0;
                this.Data.WarningActive = false;
                return;
            }

            // Send the warning letter (once per cooldown) the first time things dip too low.
            if (this.Config.EnableDivorceWarning
                && !this.Data.WarningActive
                && this.AbsoluteDay - this.Data.WarningLetterSentDay >= WarningCooldownDays)
            {
                this.SendWarningLetter(spouse);
            }
        }

        private void SendWarningLetter(NPC spouse)
        {
            this.Data.WarningActive = true;
            this.Data.WarningLetterSentDay = this.AbsoluteDay;
            this.Data.ConsecutiveLowDays = 0; // start counting the 7-day window from the warning

            this.Data.PendingWarningLetterText = this.BuildWarningLetter(spouse);
            this.Helper.GameContent.InvalidateCache("Data/mail");
            this.QueueRepeatableMail("MO.DivorceWarning");
        }

        private string BuildWarningLetter(NPC spouse)
        {
            string body = SpouseContent.IsVanilla(spouse.Name)
                ? this.GetVanillaWarningBody(spouse.Name)
                : "I don't know how else to say this, so I'm writing it down. I'm unhappy. Things between us have to change, or I don't think I can stay. Please show me you still care.";
            return $"{body}^^- {spouse.displayName}";
        }

        private string GetVanillaWarningBody(string name) => name switch
        {
            "Abigail" => "Hey. I'm not great with serious talks, so I'm writing instead. I feel invisible to you lately, and it's eating at me. I didn't sign up to be lonely in my own marriage. Please. Show me you still want this. Show me you still want ME.",
            "Penny" => "I've started and stopped this letter a dozen times. The truth is, I feel like I've faded into the background of your life. I gave up so much for a quiet, loving home. I still believe in us, but I need you to believe in me, too. Please don't let us slip away.",
            "Haley" => "I never thought I'd be the one writing a letter like this. But I feel taken for granted, and it hurts more than I'll admit out loud. I want to feel special again. I want US to feel special again. Don't make me give up on you.",
            "Emily" => "My spirit has been heavy, and I can no longer ignore it. The warmth between us has dimmed to almost nothing. I want to rekindle it — truly I do — but I cannot do it alone. Please, let's find our colors again before they fade for good.",
            "Leah" => "I left a whole life behind to chase something real out here, and I thought I'd found it with you. Lately, though, I feel just as alone as I did back then. I don't want history to repeat itself. Please meet me halfway, before it's too late.",
            "Maru" => "I've analyzed this from every angle and I keep reaching the same conclusion: we're failing, and I don't want us to. I feel unseen and overworked and very, very alone. I need a real change, not just a promise. Please. I don't want to lose this.",
            "Alex" => "Okay, this is hard for me, so bear with me. I feel like you stopped caring, and it's messing me up more than I want to say. I put on a tough face but inside I'm scared we're falling apart. Please don't let us. I need you to actually show up for me.",
            "Elliott" => "I have penned many things, but never a letter so difficult as this. Our shared verse has fallen silent, and the quiet wounds me. I do not wish to write our final chapter, my love. Help me find the words to begin again, before the page runs dry.",
            "Harvey" => "I've rewritten this several times, trying to be measured about it. But the plain truth is that I feel lonely and unwanted in our marriage, and it's affecting me deeply. I want to mend what's broken between us. Please, let's not let this go untreated.",
            "Sam" => "Man, writing this is so weird, but I didn't know how else to say it. I feel like you checked out on me, and it really sucks. I keep waiting for things to feel like they used to. Please don't let us fizzle out. I still believe in us, okay?",
            "Sebastian" => "I'm not good at this stuff, so a letter felt easier. The thing is, I feel alone even when you're right here, and it's pulling me back to a dark place. I don't want to lose what we have. But I need you to actually be present. Please.",
            "Shane" => "Look, I'm bad at talking about feelings, so here it is in writing. I feel like I'm slipping, and you're not catching me anymore. I know I'm not easy to love, but I'm trying. I need to know you're still trying too. Please don't give up on me.",
            _ => "I'm unhappy, and things between us need to change. Please show me you still care."
        };

        /// <summary>Morning: if the 7-day window has elapsed since the warning, play the farewell and divorce.</summary>
        private void Divorce_OnDayStarted(NPC spouse)
        {
            if (!this.Config.EnableAutoDivorce || !this.Data.WarningActive)
                return;

            if (this.Data.ConsecutiveLowDays < this.Config.ConsecutiveDaysBeforeAutoDivorce)
                return;

            // Relationship has stayed broken past the deadline — the spouse leaves.
            this.Data.WarningActive = false;
            this.Data.ConsecutiveLowDays = 0;

            this.ShowNarration(SpouseContent.GetFarewell(spouse.Name));
            this.TriggerDivorce();
        }

        private void TriggerDivorce()
        {
            try
            {
                Game1.player.divorceTonight.Value = true;
            }
            catch
            {
                // If the divorce flag can't be set, drop friendship to zero as a fallback.
                this.ChangeSpouseFriendship(-this.GetSpousePoints());
            }
        }
    }
}
