using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>F15: a pool of dreams the spouse had about the player, one tone per vanilla spouse.</summary>
    public static class DreamContent
    {
        private static bool IsVanilla(string name) => SpouseContent.IsVanilla(name);

        private static readonly Dictionary<string, List<string>> Dreams = new Dictionary<string, List<string>>
        {
            ["Sebastian"] = new List<string> {
                "I dreamt we were on the bike, no destination, just a black highway and a sky full of stars. You held on and I never wanted to stop.",
                "There was a city I've never seen, all rain and neon. You were the only warm thing in it. I woke up reaching for you.",
                "We were on a rooftop somewhere far away. Said nothing for hours. It was the most at peace I've ever felt, awake or asleep.",
                "I dreamt of an empty coast at the edge of the world. Just us, the cold, and the quiet. You belonged there. So did I.",
                "We were driving through a tunnel that never ended, headlights cutting the dark. You laughed once. I'd cross any distance to hear it again.",
                "There was snow falling in a place with no name. You caught a flake on your tongue and grinned at me. That's all. That was enough.",
                "I dreamt we lived somewhere nobody knew our names. It was small and ours and silent. I didn't want to wake up.",
                "We were stargazing from a hill that didn't exist. You pointed at nothing and I agreed it was beautiful, because you were." },
            ["Elliott"] = new List<string> {
                "I dreamt of a grand ballroom drowned beneath the sea, and we danced there among the corals while fish bore witness to our love.",
                "Last night I lived a chapter set in a lighthouse at the world's end. You climbed the stairs to me, and the lamp had nothing on your light.",
                "We sailed a ship of paper across an ink-black ocean, and every wave whispered a line of verse I had written about you.",
                "I dreamt I was a king who traded his entire kingdom for one more evening at your side, and I called it the wisest bargain in all the realm.",
                "There was a library that stretched to the horizon, every book about us. I read until dawn and woke certain ours is the finest tale of all.",
                "We wandered an endless autumn garden where the leaves fell upward, and you laughed, and I understood at last what poets mean by 'home.'",
                "I dreamt the tide carried a message in a bottle to my feet. I opened it. It simply read your name, a thousand times, in my own hand.",
                "We stood atop a cliff as a storm performed for us alone. You were unafraid, and so, beside you, was I." },
            ["Haley"] = new List<string> {
                "I dreamt we were on a magazine cover and honestly? We looked incredible. Power couple. I woke up smug about it.",
                "There was a beach made entirely of glitter and you carried all my bags without complaining. Truly the dream of dreams.",
                "I dreamt I won an award for 'Most Photogenic Marriage' and gave a tearful speech about you. ...Don't let it go to your head.",
                "We were models walking some runway and I tripped and you caught me and somehow that made it MORE glamorous. Iconic.",
                "I dreamt our whole house was made of sunlight and there was a camera that only took flattering pictures. Perfect. Like you.",
                "There was a party where everyone was boring except you, so we left and it was way better. Story of our actual life, honestly.",
                "I dreamt you proposed all over again but with way better lighting. I said yes louder this time.",
                "We were on a yacht and the sunset matched my outfit exactly and you said something sweet. ...Okay, fine, it was a good dream." },
            ["Harvey"] = new List<string> {
                "I dreamt I'd lost you in a crowded station, searching every platform — and then there you were, and the panic became pure relief.",
                "Last night I was giving a lecture and forgot every word, but you were in the front row mouthing them to me. I got through it. Because of you.",
                "I dreamt the clinic was flooding and I couldn't save everyone, and then you took my hand and the water just... stilled. I could breathe.",
                "There was a plane I was meant to fly and I was terrified, until I saw you in the copilot's seat. Then the sky felt safe.",
                "I dreamt I was late for something terribly important, running and running — and it turned out the important thing was just coming home to you.",
                "Last night the worry had me in its grip, as it does, and then I heard you humming in the next room and the whole dream went warm and gold.",
                "I dreamt I had to give a toast and my hands shook — until you squeezed them under the table. I've never spoken so steadily.",
                "There was a long dark corridor and I was afraid of what was at the end. It was you, holding tea, asking how my day was." },
            ["Penny"] = new List<string> {
                "I dreamt of a little garden behind our home, and we tended it together, and everything we planted grew. It felt like a promise.",
                "Last night I dreamed of a quiet rainy afternoon, just us and a stack of books and the kettle. I woke up wishing I could stay there.",
                "I dreamt our home was full of soft lamplight and laughter, and I understood that this — peace — was a thing I was allowed to have.",
                "There was a long table set for a holiday, and it was full, and warm, and ours. I've never felt so safe, even in sleep.",
                "I dreamt we walked a country road at dusk, your hand in mine, and neither of us hurried. There was nowhere we needed to be but together.",
                "Last night I dreamed of teaching a little classroom while you waved from the window. A small, ordinary, perfect life. I hope it's a glimpse.",
                "I dreamt the trailer years were just a story I read once, and this — you, this home — was the truth I'd been waiting to wake up into.",
                "There was a fireplace and a blanket and your steady heartbeat under my ear. I didn't want morning to come at all." },
            ["Abigail"] = new List<string> {
                "I dreamt we were spelunking in a cave made of candy and fought a marshmallow golem. We WON. Obviously. Best dream ever.",
                "There was a dungeon under our house — like, infinite levels — and we just kept going down, holding hands, beating everything. Couple goals.",
                "I dreamt I was a sword and you were the hero who wielded me and honestly that's the most romantic thing my brain has ever produced.",
                "We rode a giant purple bat over the whole valley and everyone was so jealous. The bat's name was Gerald. It's always Gerald.",
                "I dreamt the mines went all the way to another world and we moved there and opened a haunted snack shop. It was thriving.",
                "There was a quest where I had to find the 'most annoying noble' to save the realm and it was just... you, being cute. Quest complete.",
                "We were ghosts but the fun kind, haunting a castle and rearranging the furniture to spook people. Married goals, honestly.",
                "I dreamt we found a secret door behind the fridge and on the other side was just... more us. Infinite us. The best kind of horror." },
            ["Shane"] = new List<string> {
                "I dreamt it was just a normal morning. You, me, coffee, the chickens. Nothing happened. I woke up and I wasn't sad. That's the dream.",
                "There was a version of me in the dream that never got dark. Turns out he had you too. Figures. You're the difference.",
                "I dreamt we were old. Real old. Sitting on a porch saying nothing. I'd take that. I'd take every quiet year of it.",
                "Dreamed I was drowning, like the old days. Then I wasn't. You were just... there. Pulled me up without a word. Like always.",
                "I dreamt Jas grew up happy. We did that, you and me. Woke up with my chest tight in a good way for once.",
                "There was a dream where I deserved all this. The home, the life, you. For once I believed it. Wish I could've stayed there.",
                "I dreamt the sun came up over the ranch and you were already awake, watching it. I just watched you. Best part of any day, real or not.",
                "Simple one. You laughed at something I said. That's it. That's the whole dream. Woke up smiling like an idiot." },
            ["Emily"] = new List<string> {
                "I dreamt our souls were two threads of light weaving a tapestry across the whole night sky. The universe was the loom; we were the pattern.",
                "Last night I danced with you on the surface of a still black lake, the stars dancing with us. A memory from a future life, I think.",
                "I dreamt every crystal in the world chimed your name at once, and the sound was so beautiful I wept in my sleep with joy.",
                "There was a garden where flowers bloomed in colors that don't exist yet, and each one opened as you passed. You make the world unfold.",
                "I dreamt we were two comets who chose, against all physics, to share a single orbit forever. The cosmos approved.",
                "Last night the aurora came down and wrapped around us like a shawl, and I understood that love is just energy that refuses to fade.",
                "I dreamt of a temple of light where the only prayer was your name, and I prayed it gladly, endlessly, until dawn.",
                "There was a sea of stars and we waded in up to our hearts. You glowed. You always glow. Even my dreams know it." },
            ["Leah"] = new List<string> {
                "I dreamt of a meadow at golden hour, and I tried to paint it, but you kept stepping into frame, and the painting got better every time.",
                "Last night the whole forest was hung with paper lanterns and we walked beneath them, and the light pooled in your hair like honey.",
                "I dreamt my sculptures came alive and the only one that could speak just kept saying how much it loved you. I'd made it in your image, of course.",
                "There was a cabin in the dream, snowed in and warm, wine and woodsmoke and you. I'd have happily never been found.",
                "I dreamt of a river that ran with watercolor, and we floated down it on a raft of canvas, painting the banks as we passed.",
                "Last night autumn never ended. Leaves the color of fire, the smell of rain, and you, sketched into every scene I dreamed.",
                "I dreamt I gave a gallery show and every piece was just you, in a hundred lights, and the critics finally understood what beauty meant.",
                "There was a field of tall grass and we lay in it until the stars came out, and I didn't reach for a single brush. I just looked at you." },
            ["Maru"] = new List<string> {
                "I dreamt we built a rocket in the backyard and flew it to a little moon that was just for us. The fuel was, scientifically, love. Don't laugh.",
                "Last night I dreamed of a lab where every experiment kept concluding the same thing: that I'm completely in love with you. Reproducible results.",
                "I dreamt I invented a machine that could bottle a feeling, and the first one I caught was the way you look at me. I never opened it. Too precious.",
                "There was an observatory in the dream and we found a new star and I named it after you, and this time the committee approved.",
                "I dreamt the whole valley ran on a tiny engine I built, and it was powered entirely by how happy you make me. Endless energy. Checks out.",
                "Last night I dreamed in equations, and every one of them solved to you. Math has never been so romantic.",
                "I dreamt we were astronauts and I got tangled in the tether and you reeled me back in, laughing. You're always my way home.",
                "There was a dream where I cured something impossible, and the first person I told was you, and your face was better than any award." },
            ["Alex"] = new List<string> {
                "I dreamt I scored the winning goal and the whole crowd faded out, and the only one in the stands was you, cheering. That's the dream that matters.",
                "Last night we were just tossing a ball on the beach, no game, no score, golden light. I woke up wanting that more than any trophy.",
                "I dreamt I was old and gray, telling some kid about the best thing I ever did. Wasn't gridball. It was marrying you.",
                "There was a dream where I wasn't scared of anything, and I figured out it was 'cause you were standing next to me the whole time.",
                "I dreamt we had a whole house full of noise and dogs and good food and you in the middle of it. Best future I've ever seen.",
                "Last night I dreamed I could fly, and the first thing I did was come find you to show you. Some things don't change, sleeping or awake.",
                "I dreamt grandpa was proud of me. And you were holding my hand while he said it. Woke up with my eyes wet. Don't tell anyone.",
                "There was a championship in the dream, and I held up the trophy and pointed it right at you. You're the reason I win anything." },
            ["Sam"] = new List<string> {
                "I dreamt our band finally hit it big, and the encore was just me dedicating every single song to you. Crowd went wild. You went red. Cute.",
                "Last night we toured the whole valley in a van that ran on snacks and good vibes, and you were my roadie AND my favorite groupie.",
                "I dreamt I wrote the perfect song and the only lyric was your name, and somehow it went number one. The critics wept. So did I.",
                "There was a dream where we had a treehouse with a half-pipe and never had to grow up, but we were also married, so, best of both worlds.",
                "I dreamt I taught you to skate and you immediately did a trick I can't even do, and I was so proud I forgot to be jealous. Mostly.",
                "Last night we played a rooftop show as the sun came up and nobody was watching but you, and that was the best gig of my whole life.",
                "I dreamt our kitchen was a recording studio and we made a whole album just goofing around. Title track was the sound of you laughing.",
                "There was a dream where I finally felt like a real grown-up, and it was just because you looked at me like I already was one." }
        };
        private static readonly List<string> Generic = new List<string>
        {
            "I dreamt we walked a quiet road together with nowhere to be, and I woke up wishing the dream had been a little longer.",
            "Last night I dreamed our home was full of warm light and laughter. I woke reaching for you, and there you were. The best feeling.",
            "I dreamt of an ordinary morning with you — coffee, sunlight, your voice. Funny how the simplest dreams are the sweetest ones.",
            "There was a dream where we grew old together on a porch somewhere peaceful. I'd take that future in a heartbeat.",
            "I dreamt we were dancing slowly to no music at all, just the two of us. I didn't want the song that wasn't playing to end.",
            "Last night I dreamed I lost you in a crowd, and the relief of finding you again followed me right into morning. I'm so glad you're real."
        };
        public static List<string> GetDreams(string name)
            => IsVanilla(name) && Dreams.ContainsKey(name) ? Dreams[name] : Generic;
    }
}
