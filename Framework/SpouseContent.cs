using System.Collections.Generic;

namespace MarriageOverhaul
{
    /// <summary>A single argument scenario: an opening line and three player responses.</summary>
    public class ArgumentScenario
    {
        public string Intro;
        public string GoodChoice;
        public string GoodReply;
        public string NeutralChoice;
        public string NeutralReply;
        public string BadChoice;
        public string BadReply;
    }

    /// <summary>Per-spouse makeup-gift configuration.</summary>
    public class MakeupInfo
    {
        public string Category;     // "sweet", "nature", or "homemade"
        public string Hint;         // dialogue hint at the category (not the item)
        public string Reconcile;    // shown when the right category is given
        public string Resigned;     // shown when the makeup window lapses
    }

    /// <summary>Per-spouse anniversary lines.</summary>
    public class AnniversaryInfo
    {
        public string Reminder;     // morning letter body
        public string Sweet;        // shown when gifted on the anniversary
        public string Disappointed; // shown the morning after a forgotten anniversary
    }

    /// <summary>
    /// Static library of personalized content for the twelve vanilla marriage candidates,
    /// with graceful generic fallbacks for modded spouses.
    /// </summary>
    public static partial class SpouseContent
    {
        public static readonly HashSet<string> VanillaSpouses = new HashSet<string>
        {
            "Abigail", "Penny", "Haley", "Emily", "Leah", "Maru",
            "Alex", "Elliott", "Harvey", "Sam", "Sebastian", "Shane"
        };

        public static bool IsVanilla(string name) => name != null && VanillaSpouses.Contains(name);

        // ─────────────────────────────────────────────────────────────
        //  ARGUMENTS
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, List<ArgumentScenario>> Arguments = new Dictionary<string, List<ArgumentScenario>>
        {
            ["Abigail"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "You're always running off into the mines without me. I can fight too, you know! It feels like you don't want me around for the fun stuff.",
                    GoodChoice = "You're right. Let's explore together from now on.",
                    GoodReply = "...Really? Heh. Okay. I'm holding you to that. Next time we go deep, side by side.",
                    NeutralChoice = "The mines are dangerous, Abby.",
                    NeutralReply = "Hmph. I can handle danger. But... fine. Whatever.",
                    BadChoice = "Adventuring is something I do alone.",
                    BadReply = "Alone? So that's how it is. I thought being married meant we were a team. Guess I was wrong."
                },
                new ArgumentScenario {
                    Intro = "Do you even notice me anymore? I've been playing my flute every night and you just walk right past.",
                    GoodChoice = "I love hearing you play. Play for me tonight?",
                    GoodReply = "You... actually like it? Okay. Sit down, then. This one's for you.",
                    NeutralChoice = "I've been busy with the farm.",
                    NeutralReply = "The farm, the farm. I get it. I guess I'll just play for the walls.",
                    BadChoice = "Honestly, the flute gets a little annoying.",
                    BadReply = "Annoying?! That's— wow. Forget I ever picked it up, then."
                },
                new ArgumentScenario {
                    Intro = "You treat this house like a hotel. In and out, never here. I didn't marry you to live alone.",
                    GoodChoice = "I'll make more time for us. Promise.",
                    GoodReply = "Good. Because I'd rather raid a dungeon with you than sit here bored. Don't forget me.",
                    NeutralChoice = "I'm working hard for our future.",
                    NeutralReply = "Our future. Sure. I just want a present, too.",
                    BadChoice = "Maybe you should get a hobby.",
                    BadReply = "A hobby?! You ARE my— ugh. Never mind."
                }
            },
            ["Penny"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "Sometimes I feel like I don't really matter to you. Like I'm just... here. I'm sorry, I don't mean to complain.",
                    GoodChoice = "You matter to me more than anything.",
                    GoodReply = "Oh... I— thank you. I needed to hear that. I'm sorry I doubted it.",
                    NeutralChoice = "Of course you matter.",
                    NeutralReply = "Okay. I'm sorry for bringing it up.",
                    BadChoice = "Don't be so needy, Penny.",
                    BadReply = "...Needy. Right. I'll try not to need anything from you, then."
                },
                new ArgumentScenario {
                    Intro = "I worked so hard to make this home nice, and you didn't even notice. I just wanted you to be happy here.",
                    GoodChoice = "It's beautiful. Thank you for caring so much.",
                    GoodReply = "Really? That means the world to me. I just want us to have something good.",
                    NeutralChoice = "It looks fine, Penny.",
                    NeutralReply = "Fine. I suppose fine is something.",
                    BadChoice = "I hadn't even noticed, honestly.",
                    BadReply = "Oh. ...Of course you hadn't. Why would you."
                },
                new ArgumentScenario {
                    Intro = "I grew up with so little. I just wanted our life to feel stable. Lately it feels like everything's slipping.",
                    GoodChoice = "We'll build something steady, together.",
                    GoodReply = "Together. Yes. That's all I ever wanted. Thank you for understanding.",
                    NeutralChoice = "Things are tough right now.",
                    NeutralReply = "I know. I just hoped we'd face them as a pair.",
                    BadChoice = "You worry too much about nothing.",
                    BadReply = "Nothing? It isn't nothing to me. ...It never was."
                }
            },
            ["Haley"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "Ugh, you came in covered in mud AGAIN and tracked it everywhere. Do you even care how this place looks?",
                    GoodChoice = "You're right, I'll clean up before coming in.",
                    GoodReply = "...Thank you. See, that wasn't hard. I just like things nice, okay?",
                    NeutralChoice = "It's a farm, Haley. Mud happens.",
                    NeutralReply = "Whatever. I'll just clean it up myself. Again.",
                    BadChoice = "Maybe stop obsessing over appearances.",
                    BadReply = "Obsessing?! At least I care about SOMETHING. Unbelievable."
                },
                new ArgumentScenario {
                    Intro = "You didn't even look at the photos I took. I worked really hard on them. I thought you'd be proud.",
                    GoodChoice = "Show me — I want to see every one.",
                    GoodReply = "Okay! Okay. This one's my favorite. ...I'm glad you actually care.",
                    NeutralChoice = "I'll look at them later.",
                    NeutralReply = "You always say later. Fine.",
                    BadChoice = "They're just photos, Haley.",
                    BadReply = "'Just photos.' That's MY art. Forget it."
                },
                new ArgumentScenario {
                    Intro = "Everyone in town thinks I married down. I don't, but... you could try a little harder sometimes, you know?",
                    GoodChoice = "I'll make you proud. You deserve that.",
                    GoodReply = "...You'd do that? For me? Maybe I did pick right after all.",
                    NeutralChoice = "I don't care what people think.",
                    NeutralReply = "Well, I do. A little. Ugh.",
                    BadChoice = "Then maybe you DID marry down.",
                    BadReply = "That's not— I didn't mean— now you're just being cruel."
                }
            },
            ["Emily"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "Your aura has been so dim and closed-off lately. I feel like you've stopped letting me in.",
                    GoodChoice = "Help me open up again. I want that.",
                    GoodReply = "Yes! Let's breathe together. I can already feel the colors warming. Thank you.",
                    NeutralChoice = "It's just stress, Emily.",
                    NeutralReply = "Stress clouds the spirit. Try to let it flow out, okay?",
                    BadChoice = "That aura stuff isn't real.",
                    BadReply = "Oh. It's real to me. I thought... you understood that part of me."
                },
                new ArgumentScenario {
                    Intro = "I had a dream that you walked away from me into the fog. I woke up and you were already gone for the day. It shook me.",
                    GoodChoice = "I'll always come back to you. Always.",
                    GoodReply = "I believe you. The fog lifts a little just hearing that.",
                    NeutralChoice = "It was only a dream.",
                    NeutralReply = "Maybe. But dreams speak, sometimes.",
                    BadChoice = "You read too much into things.",
                    BadReply = "...Maybe I do. Or maybe I just feel things you don't bother to."
                },
                new ArgumentScenario {
                    Intro = "I made you something and you barely glanced at it. I pour my heart into what I create for you.",
                    GoodChoice = "I'll treasure it. Everything you make is special.",
                    GoodReply = "Oh, that fills me right up. Thank you for seeing me.",
                    NeutralChoice = "I appreciate it, really.",
                    NeutralReply = "Okay. A little warmth is better than none.",
                    BadChoice = "I have enough of your stuff already.",
                    BadReply = "'Stuff.' I see. I'll keep my heart to myself next time."
                }
            },
            ["Leah"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "I gave up a whole life in the city to make art out here. Lately I wonder if you even respect that I'm an artist.",
                    GoodChoice = "Your art is the bravest thing I know.",
                    GoodReply = "...That actually means a lot, coming from you. Okay. I needed that.",
                    NeutralChoice = "Of course I respect it.",
                    NeutralReply = "Then show it a little, yeah?",
                    BadChoice = "Art doesn't pay the bills, Leah.",
                    BadReply = "Wow. My ex used to say exactly that. I didn't expect it from you."
                },
                new ArgumentScenario {
                    Intro = "You promised you'd come to my gallery showing and you forgot. I stood there looking for you the whole time.",
                    GoodChoice = "I'm so sorry. It'll never happen again.",
                    GoodReply = "...Thank you for owning it. That's all I wanted. Come to the next one?",
                    NeutralChoice = "I lost track of the day.",
                    NeutralReply = "Yeah. You do that a lot lately.",
                    BadChoice = "It's just a few sculptures.",
                    BadReply = "'Just'? That's my soul on display. Forget it."
                },
                new ArgumentScenario {
                    Intro = "I love the quiet out here, but I'm starting to feel lonely in it. When did we stop sharing the little things?",
                    GoodChoice = "Let's share a sunset tonight. Just us.",
                    GoodReply = "Heh. A salad, some wine, the sky. Yeah. That's the life I wanted with you.",
                    NeutralChoice = "We're both just busy.",
                    NeutralReply = "Busy. Sure. Busy people still drift, you know.",
                    BadChoice = "You wanted the quiet life, remember?",
                    BadReply = "Quiet, not empty. There's a difference. I thought you knew that."
                }
            },
            ["Maru"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "You unplugged my project to charge something and lost three days of work. Do you have any idea how that feels?",
                    GoodChoice = "I'm so sorry. Tell me how to help rebuild it.",
                    GoodReply = "...Okay. Hand me the soldering iron. And — thanks for not brushing it off.",
                    NeutralChoice = "It was an accident, Maru.",
                    NeutralReply = "I know. Accidents still cost, though.",
                    BadChoice = "It's just a gadget, you can redo it.",
                    BadReply = "'Just a gadget.' That gadget was months of my mind. Forget it."
                },
                new ArgumentScenario {
                    Intro = "I work nights at the clinic AND build in the lab, and I still come home to an empty table. I'm stretched so thin.",
                    GoodChoice = "Let me take some weight off you. We'll plan it together.",
                    GoodReply = "That's... a relief, honestly. A real plan. With you. Okay.",
                    NeutralChoice = "We both work hard.",
                    NeutralReply = "We do. I just need to not do it alone.",
                    BadChoice = "Then maybe drop one of the jobs.",
                    BadReply = "Drop the work that makes me ME? That's your solution? Wow."
                },
                new ArgumentScenario {
                    Intro = "You laughed when I told you my invention idea. I know it sounds far-fetched, but I needed you to believe in it.",
                    GoodChoice = "I believe in you. Show me the blueprints.",
                    GoodReply = "Really? Okay — okay! Look, if I route the power HERE... ahh, I love that you're listening.",
                    NeutralChoice = "I wasn't laughing AT you.",
                    NeutralReply = "It felt like it. But... okay. I'll take your word.",
                    BadChoice = "Come on, it's a little ridiculous.",
                    BadReply = "Ridiculous. Right. Edison heard that too. Never mind."
                }
            },
            ["Alex"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "You keep brushing off my training like it's nothing. Gridball's the one thing I'm actually good at, you know?",
                    GoodChoice = "You're great at it. I'll come watch you train.",
                    GoodReply = "Yeah? Heh, awesome. I'll show you my spiral. You're gonna be impressed.",
                    NeutralChoice = "I've just been busy.",
                    NeutralReply = "Yeah, yeah. Everybody's busy. Whatever.",
                    BadChoice = "Gridball isn't a real future, Alex.",
                    BadReply = "...Wow. That's exactly what I'm scared people think. Thanks a lot."
                },
                new ArgumentScenario {
                    Intro = "I act tough but... grandpa's getting older and it scares me. And you've been so distant I can't even talk to you about it.",
                    GoodChoice = "I'm here. Talk to me — always.",
                    GoodReply = "...Man. Okay. I don't say this stuff easy, but thanks. Really.",
                    NeutralChoice = "He'll be okay, Alex.",
                    NeutralReply = "Maybe. I just needed you to listen, not fix it.",
                    BadChoice = "Don't get all soft on me.",
                    BadReply = "Soft? I opened up and that's what I get? Forget I said anything."
                },
                new ArgumentScenario {
                    Intro = "You make me feel like the dumb jock everyone assumes I am. I thought you saw more in me than that.",
                    GoodChoice = "I see all of you. You're more than gridball.",
                    GoodReply = "...That hits different coming from you. Okay. I needed that. Thanks.",
                    NeutralChoice = "Nobody thinks that.",
                    NeutralReply = "You sure? 'Cause sometimes it feels like you do.",
                    BadChoice = "Well, you don't read much, do you?",
                    BadReply = "Real nice. Yeah. Dumb jock. Cool. Glad we cleared that up."
                }
            },
            ["Elliott"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "I read you a passage from my novel and you fell asleep. Words are how I love you — and you slept through them.",
                    GoodChoice = "Read it to me again. I'll hang on every word.",
                    GoodReply = "Ah... a second chance for the prose, and for us. Very well. Listen closely, my muse.",
                    NeutralChoice = "I was just tired, Elliott.",
                    NeutralReply = "Fatigue. The eternal critic of art. I shall forgive it. Once.",
                    BadChoice = "Maybe your book is just boring.",
                    BadReply = "...Boring. You plunge the dagger with such economy. Bravo."
                },
                new ArgumentScenario {
                    Intro = "You sold the crab cakes I made before I could share them with you. A small thing, perhaps, but it stung.",
                    GoodChoice = "Make them again — we'll feast together tonight.",
                    GoodReply = "Ha! A culinary reconciliation. I accept with great appetite, my dear.",
                    NeutralChoice = "I didn't know they were special.",
                    NeutralReply = "Now you do. Let us mind the small things, hmm?",
                    BadChoice = "It's just food, Elliott.",
                    BadReply = "Just food. Just words. Just art. Is anything sacred to you?"
                },
                new ArgumentScenario {
                    Intro = "I left the cabin by the sea to share a life with you, yet lately I dine alone with only the tide for company.",
                    GoodChoice = "Tonight, the tide and me both. I'll be there.",
                    GoodReply = "Then the evening is redeemed. The sea is lovely, but it is a poor conversationalist.",
                    NeutralChoice = "I'll try to be home more.",
                    NeutralReply = "'Try' is a fragile vessel. Still — I shall sail in it.",
                    BadChoice = "You chose this quiet life.",
                    BadReply = "I chose YOU, not solitude wearing your face. There is a difference."
                }
            },
            ["Harvey"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "I, ah... I worry about you out on that farm with all that machinery. And when you don't check in, I imagine the worst.",
                    GoodChoice = "I'll check in every day. I don't want you to worry.",
                    GoodReply = "Oh — that would put my mind so at ease. Thank you. I just care, is all.",
                    NeutralChoice = "I'm careful, Harvey.",
                    NeutralReply = "I know, I know. The doctor in me can't help it.",
                    BadChoice = "Stop hovering over me.",
                    BadReply = "Hovering. I see. I'll just... keep my concern to myself, then."
                },
                new ArgumentScenario {
                    Intro = "I overheard someone say I'm too dull for you. The awful thing is, lately I've started to believe them.",
                    GoodChoice = "You're the steadiest, kindest man I know.",
                    GoodReply = "I— goodness. That's the nicest thing anyone's said in ages. Thank you, truly.",
                    NeutralChoice = "Don't listen to gossip.",
                    NeutralReply = "You're right. Easier said than done, but you're right.",
                    BadChoice = "Well, you ARE a bit boring sometimes.",
                    BadReply = "Ah. ...Right. I suppose I walked into that one. Excuse me."
                },
                new ArgumentScenario {
                    Intro = "You skipped your check-up again. I'm your spouse AND your doctor, and you won't let me take care of you.",
                    GoodChoice = "You're right. Book me in — I trust you.",
                    GoodReply = "Thank you. Truly. It's hard to watch the person you love ignore their health.",
                    NeutralChoice = "I feel fine, Harvey.",
                    NeutralReply = "'Fine' isn't a diagnosis. But... alright.",
                    BadChoice = "Quit nagging me about my health.",
                    BadReply = "Nagging. It's called caring. But clearly that's unwelcome."
                }
            },
            ["Sam"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "You said you'd come to our band's show and you bailed. The guys were all there with someone. I felt like an idiot.",
                    GoodChoice = "I messed up. Front row at the next one, I swear.",
                    GoodReply = "For real? Okay, okay, that's awesome. I'll write you into a song or somethin'.",
                    NeutralChoice = "Something came up, Sam.",
                    NeutralReply = "Yeah... something always comes up. It's cool. I guess.",
                    BadChoice = "Your band isn't really my thing.",
                    BadReply = "Oh. Wow. The band's like, my whole heart, man. But okay."
                },
                new ArgumentScenario {
                    Intro = "With dad back and Vincent looking up to me, I'm trying to be more grown up. It'd help if you took me seriously too.",
                    GoodChoice = "I take you seriously. You're growing into someone great.",
                    GoodReply = "...Whoa. Thanks. That actually means a ton right now. Okay. Okay, cool.",
                    NeutralChoice = "You're doing fine, Sam.",
                    NeutralReply = "Fine. Heh. I'm shooting for better than fine, but thanks.",
                    BadChoice = "You still act like a kid, though.",
                    BadReply = "...Right. The kid. That's all anyone sees. Cool. Thanks."
                },
                new ArgumentScenario {
                    Intro = "You laughed off my skating and my music again. That stuff's not a phase, it's ME. Why's that so hard to get?",
                    GoodChoice = "It's who you are, and I love who you are.",
                    GoodReply = "Dude. Okay. That's the best thing you coulda said. We're good. We're so good.",
                    NeutralChoice = "I didn't mean anything by it.",
                    NeutralReply = "Okay. Just... think before you laugh next time, yeah?",
                    BadChoice = "Maybe it's time to grow out of it.",
                    BadReply = "Grow out of ME? That's— wow. Forget it, man."
                }
            },
            ["Sebastian"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "You keep planning our whole life out loud without asking me. I already feel boxed in by everyone. Not you too.",
                    GoodChoice = "My bad. Let's figure it out together, your pace.",
                    GoodReply = "...Yeah. Okay. That's all I needed, honestly. Thanks for getting it.",
                    NeutralChoice = "I was just thinking ahead.",
                    NeutralReply = "I know. Just... loop me in before you decide, alright?",
                    BadChoice = "Someone has to make decisions around here.",
                    BadReply = "Right. 'Cause my input's worthless. Cool. Noted."
                },
                new ArgumentScenario {
                    Intro = "I finally cleared a weekend for the bike trip we talked about and you forgot completely. I don't ask for much.",
                    GoodChoice = "I'll make it up to you — let's go this weekend.",
                    GoodReply = "...Seriously? Okay. Don't flake this time. The coast, just us and the engine.",
                    NeutralChoice = "It slipped my mind, sorry.",
                    NeutralReply = "Yeah. The one thing I looked forward to. It's whatever.",
                    BadChoice = "We don't have time for trips.",
                    BadReply = "Of course not. We never have time for the stuff I want. Forget it."
                },
                new ArgumentScenario {
                    Intro = "Everyone treats me like the gloomy basement guy. I thought you, of all people, saw past that.",
                    GoodChoice = "I see you — all of you. Always have.",
                    GoodReply = "...That's why it's you. Okay. I don't say this much, but I'm glad it's you.",
                    NeutralChoice = "Nobody really thinks that.",
                    NeutralReply = "They do. But maybe it matters less if you don't.",
                    BadChoice = "Well, you DO hide in the basement a lot.",
                    BadReply = "Cool. So you think it too. Good talk. I'm going back down."
                }
            },
            ["Shane"] = new List<ArgumentScenario>
            {
                new ArgumentScenario {
                    Intro = "I can feel myself slipping back into the dark place. And instead of reaching out, I just... pushed you away again. Sorry.",
                    GoodChoice = "Don't apologize. Let me be there with you.",
                    GoodReply = "...You'd do that. Even now. Okay. Yeah. Don't let me crawl back in that hole alone.",
                    NeutralChoice = "It's okay, just talk to me.",
                    NeutralReply = "Yeah. Talking. I'm bad at it, but... I'll try. For you.",
                    BadChoice = "I don't have time for your moods.",
                    BadReply = "...Right. My 'moods.' Should've known. Forget I said anything."
                },
                new ArgumentScenario {
                    Intro = "You found the empty cans again, didn't you. I'm trying, okay? But it feels like you're just waiting for me to fail.",
                    GoodChoice = "I'm not waiting for that. I'm rooting for you.",
                    GoodReply = "...Rooting for me. Heh. Nobody's ever really done that. Okay. I won't let you down.",
                    NeutralChoice = "I just want you to be healthy.",
                    NeutralReply = "I know. I know you do. It's hard. But I hear you.",
                    BadChoice = "I knew you'd fall off the wagon.",
                    BadReply = "...Yeah. There it is. Why even try if that's what you think."
                },
                new ArgumentScenario {
                    Intro = "Jas needs a real role model and I'm terrified I'm not it. And you joking around about it tonight really didn't help.",
                    GoodChoice = "You're already a great example for her.",
                    GoodReply = "...You think so? God, I hope you're right. Okay. Thanks. I needed that.",
                    NeutralChoice = "She adores you, Shane.",
                    NeutralReply = "Yeah. Hope I don't screw it up. Thanks, I guess.",
                    BadChoice = "Honestly, I worry about that too.",
                    BadReply = "...Wow. Even you. That's just great. Thanks for the vote of confidence."
                }
            }
        };

        private static readonly List<ArgumentScenario> GenericArguments = new List<ArgumentScenario>
        {
            new ArgumentScenario {
                Intro = "We need to talk. Lately it feels like we've been drifting apart, and it's been weighing on me.",
                GoodChoice = "You're right. Let's reconnect — starting tonight.",
                GoodReply = "Thank you for hearing me. That means more than you know.",
                NeutralChoice = "We've both just been busy.",
                NeutralReply = "I suppose so. I just don't want 'busy' to become 'distant.'",
                BadChoice = "I don't have time for this right now.",
                BadReply = "...Of course you don't. That's exactly the problem."
            },
            new ArgumentScenario {
                Intro = "I feel like I've been doing all the work to keep us close, and you haven't met me halfway.",
                GoodChoice = "I'll do better. We're a team.",
                GoodReply = "A team. Yes. That's all I wanted to hear.",
                NeutralChoice = "I didn't realize you felt that way.",
                NeutralReply = "Well, now you do. Let's not let it get worse.",
                BadChoice = "Maybe you're just too demanding.",
                BadReply = "Demanding? For wanting my spouse to care? Unbelievable."
            },
            new ArgumentScenario {
                Intro = "When was the last time we did something together, just the two of us? I honestly can't remember.",
                GoodChoice = "Too long. Let's fix that this week.",
                GoodReply = "I'd love that. Thank you for noticing before it was too late.",
                NeutralChoice = "Things have been hectic.",
                NeutralReply = "They always are. We have to make time anyway.",
                BadChoice = "Does it really matter that much?",
                BadReply = "It matters to me. I thought it mattered to you, too."
            }
        };

        public static List<ArgumentScenario> GetArguments(string name)
        {
            bool has = IsVanilla(name) && Arguments.ContainsKey(name);
            string p = $"argument.{(has ? name : "generic")}";
            List<ArgumentScenario> src = has ? Arguments[name] : GenericArguments;
            var outList = new List<ArgumentScenario>(src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                ArgumentScenario s = src[i];
                outList.Add(new ArgumentScenario
                {
                    Intro = I18n.Get($"{p}.{i}.intro", s.Intro),
                    GoodChoice = I18n.Get($"{p}.{i}.goodchoice", s.GoodChoice),
                    GoodReply = I18n.Get($"{p}.{i}.goodreply", s.GoodReply),
                    NeutralChoice = I18n.Get($"{p}.{i}.neutralchoice", s.NeutralChoice),
                    NeutralReply = I18n.Get($"{p}.{i}.neutralreply", s.NeutralReply),
                    BadChoice = I18n.Get($"{p}.{i}.badchoice", s.BadChoice),
                    BadReply = I18n.Get($"{p}.{i}.badreply", s.BadReply)
                });
            }
            return outList;
        }

        // ─────────────────────────────────────────────────────────────
        //  JEALOUSY
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, string> Jealousy = new Dictionary<string, string>
        {
            ["Abigail"] = "So... I heard you were giving gifts to someone else. Should I be worried, or just annoyed? Because right now I'm both.",
            ["Penny"] = "I saw you give them a present. I... I'm sure it's nothing. It's just hard not to wonder where I rank.",
            ["Haley"] = "Buying gifts for OTHER people now? Cute. I hope they appreciated it more than I'm appreciating finding out.",
            ["Emily"] = "Your aura had someone else's color on it today. I trust you, but... it gave me a strange feeling.",
            ["Leah"] = "Heard you were handing out gifts in town. I'm not the jealous type, usually. Usually.",
            ["Maru"] = "Statistically, gift-giving correlates with affection. So you'll forgive me for running some unpleasant numbers in my head.",
            ["Alex"] = "Hey, so, you got somebody else a present? I'm not insecure or anything, I just... yeah. It bugged me.",
            ["Elliott"] = "Word reached me that you bestowed a gift upon another. My heart is no stranger to drama, but I'd rather not star in one.",
            ["Harvey"] = "I, ah, heard you gave someone a gift today. I'm sure it's perfectly innocent. I'm... mostly sure.",
            ["Sam"] = "Wait, you got THEM a gift? Haha, okay, that's— no, it's fine. It's fine. Is it fine?",
            ["Sebastian"] = "So you're giving people gifts now. Cool. Didn't realize I had competition. ...Do I?",
            ["Shane"] = "You gave someone else a present, huh. Figures. Everyone's got someone better lined up eventually."
        };

        private const string GenericJealousy =
            "I heard you gave a gift to someone else today. I'm trying not to read into it, but it stung a little.";

        public static string GetJealousy(string name)
            => LocDict("jealousy", name, Jealousy, GenericJealousy);

        // ─────────────────────────────────────────────────────────────
        //  FAREWELL (auto-divorce scene)
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, string> Farewell = new Dictionary<string, string>
        {
            ["Abigail"] = "I wanted an adventure with you, not to be left behind in an empty house. I'm going home. Goodbye.",
            ["Penny"] = "I gave you all of me, and somewhere along the way I stopped mattering. I have to go. I'm sorry.",
            ["Haley"] = "I deserve to feel cherished, and I haven't in a long time. I'm leaving. Take care of yourself.",
            ["Emily"] = "Our spirits have pulled apart and I can't mend it alone. I have to walk my own path now. Be well.",
            ["Leah"] = "I left one life that made me feel small. I won't stay in another. Goodbye — I really did love you.",
            ["Maru"] = "I've run every scenario, and none of them work anymore. I have to go before there's nothing left of me. Goodbye.",
            ["Alex"] = "I opened up to you, and you let it all go cold. I can't keep doing this. I'm leaving.",
            ["Elliott"] = "Our story has reached its final, sorrowful page. I close the book now, with a heavy heart. Farewell.",
            ["Harvey"] = "I can't keep prescribing patience to a heart that isn't getting better. I have to leave. Please look after yourself.",
            ["Sam"] = "I tried to grow up for us, but I can't carry this by myself anymore. I'm going. Take it easy, okay?",
            ["Sebastian"] = "I felt trapped before I met you, and now I feel it again. I have to go. ...Sorry it ended like this.",
            ["Shane"] = "You were the one good thing, and I watched it slip away. Maybe I never deserved it. Goodbye."
        };

        private const string GenericFarewell =
            "I've thought about this for a long time, and I can't keep living like this. I'm leaving. Goodbye.";

        public static string GetFarewell(string name)
            => LocDict("farewell", name, Farewell, GenericFarewell);

        // ─────────────────────────────────────────────────────────────
        //  MOOD (greeting fragments)
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, List<string>> HappyMood = new Dictionary<string, List<string>>
        {
            ["Abigail"] = new List<string> {
                "Hey, you! I had the best dream about us raiding a dungeon together. Today's gonna be great, I can feel it!",
                "Morning! I'm in a stupidly good mood and I'm blaming you for it. In a good way.",
                "Hey! Today feels like an adventure waiting to happen. Let's go cause some trouble.",
                "Ugh, why are you so cute in the morning? It's distracting. Anyway, good morning!" },
            ["Penny"] = new List<string> {
                "Good morning, sweetheart. Waking up next to you makes everything feel right. I'm so lucky.",
                "Good morning! I woke up smiling and I couldn't tell you why. Then I saw you.",
                "Morning, dear. My heart just feels so full today. Thank you for that.",
                "Good morning. Everything feels gentle and right this morning. I hope your day is too." },
            ["Haley"] = new List<string> {
                "Morning, gorgeous. You know what? Life's actually pretty perfect right now. Don't tell anyone I said that.",
                "Morning! I'm having a great day already and it's barely started. Lucky you, getting to share it.",
                "Hey, gorgeous. I feel amazing today, which means YOU get to bask in it. You're welcome.",
                "Good morning! Ugh, I'm in such a good mood I might even be nice to people. Don't get used to it." },
            ["Emily"] = new List<string> {
                "Mmm, your aura is glowing this morning, and so is mine. The whole house feels warm. I love it.",
                "Good morning! The energy in here is just radiant today. I feel it shimmering all around us.",
                "Morning, love! My spirit's practically dancing. The colors are so vivid I could cry happy tears.",
                "Mmm, what a beautiful morning. The whole universe feels like it's smiling on us." },
            ["Leah"] = new List<string> {
                "Morning, you. I woke up inspired — might sculpt something just thinking about us. Sappy, I know.",
                "Morning! I'm buzzing with ideas today. Good mood, good light, good company. Perfect.",
                "Hey, you. I feel so light this morning. Like I could create anything. You do that to me.",
                "Good morning! The whole world looks paintable today. Especially you, half-asleep like that." },
            ["Maru"] = new List<string> {
                "Good morning! I ran the numbers and you're statistically the best part of my day. Every day. Don't question the data.",
                "Morning! All my readings say today's going to be great. Subjective ones, too. Mostly about you.",
                "Good morning! I woke up energized and ready to invent something brilliant. You're a great catalyst.",
                "Hey! My mood's running at peak efficiency today. Correlation with you is, frankly, undeniable." },
            ["Alex"] = new List<string> {
                "Hey, babe! I'm feelin' on top of the world today. Havin' you around does that, y'know?",
                "Morning, babe! I'm so pumped today, I could lift the whole barn. You give me that energy.",
                "Hey! Feelin' unstoppable this morning. Best part? Gettin' to share it with you.",
                "Good mornin', gorgeous! Today's gonna be a winner, I can feel it in my bones." },
            ["Elliott"] = new List<string> {
                "Ah, good morning, my muse. I woke with poetry in my heart and your name on my lips.",
                "Good morning! My spirits soar like gulls over the tide today. The words simply pour out.",
                "Ah, what a glorious morning. Inspiration courses through me — and you, my dear, are its source.",
                "Morning, my love! The day gleams with promise, and my pen can scarcely keep pace with my joy." },
            ["Harvey"] = new List<string> {
                "Good morning, dear! I, ah, slept wonderfully. There's something about waking up beside you. It's the best medicine.",
                "Good morning! I, ah, feel positively wonderful today. Quite the rare diagnosis for me.",
                "Morning, dear! My heart's light as a feather. I'd prescribe a day like this to everyone.",
                "Good morning! I woke up humming, if you can believe it. You bring that out in me." },
            ["Sam"] = new List<string> {
                "Yo, morning! I'm so stoked today. Honestly? Marrying you was the coolest thing I ever did.",
                "Morning! I'm in SUCH a good mood, I already wrote half a song before coffee. It's a bop.",
                "Yo! Today feels awesome and we haven't even done anything yet. That's the good stuff.",
                "Hey, hey! Feelin' great today. You've got a lot to do with that, just sayin'." },
            ["Sebastian"] = new List<string> {
                "Morning. ...I'm actually in a good mood, which is rare, so enjoy it. It's because of you, obviously.",
                "Morning. Weirdly good mood today. Don't make a thing of it. ...Okay, maybe make a small thing of it.",
                "Hey. I actually feel good today. Light, even. That's, uh, a you thing. Obviously.",
                "Morning. I caught myself smiling earlier. Out of nowhere. Then I remembered it was 'cause of you." },
            ["Shane"] = new List<string> {
                "Hey. You know, I woke up and actually felt... good. Hopeful, even. That's all you. Thanks.",
                "Hey. Today feels... lighter, somehow. Good lighter. Hadn't felt that in a while. It's you.",
                "Morning. Woke up in an actual good mood. Weird, right? The good kind of weird. Thanks to you.",
                "Hey, you. Feelin' alright today. Better than alright, honestly. You did that." }
        };

        private static readonly Dictionary<string, List<string>> GrumpyMood = new Dictionary<string, List<string>>
        {
            ["Abigail"] = new List<string> {
                "Oh. Morning. I'm not really in the mood to talk right now, okay?",
                "Ugh. Morning. Everything's annoying today and I don't know why. Just... bear with me.",
                "Hey. I'm in a mood. Not your fault. Probably. Just give me a little room.",
                "Morning. I feel all restless and prickly today. Don't take it personally." },
            ["Penny"] = new List<string> {
                "...Good morning. Sorry, I just didn't sleep well. I'd rather not get into it.",
                "Good morning. I'm feeling a little fragile today. Please be patient with me.",
                "Morning. My head's full of worries this morning. I'll try not to let it show.",
                "...Morning. I'm a bit out of sorts. It's nothing you did. I just need a quiet day." },
            ["Haley"] = new List<string> {
                "Hmph. Morning. Don't even look at me right now, I'm in a mood.",
                "Ugh. Today is already the worst and it just started. Don't test me.",
                "Morning. I'm cranky and my hair won't cooperate. Approach with caution.",
                "Hmph. I'm in a mood and I refuse to apologize for it. Morning, though." },
            ["Emily"] = new List<string> {
                "Morning. My aura's all tangled today. I need some space to sort myself out.",
                "Morning. The energy's all jagged and grey today. I need to sit quietly with it.",
                "Oh. Morning. My spirit feels heavy. I'll work through it, but go gently, okay?",
                "Morning. The colors are all muddled this morning. I just need a little space." },
            ["Leah"] = new List<string> {
                "Morning. ...Yeah, I'm a little off today. Just leave me to my work for now.",
                "Morning. Feeling blocked and cranky. Best to let me grumble at the clay alone.",
                "Hey. I'm in a mood today. It'll pass. Just give me some room 'til it does.",
                "Morning. ...Not my best day. Don't mind me. I'll come around." },
            ["Maru"] = new List<string> {
                "Oh, you're up. Morning. I've got a lot on my mind, so... we'll talk later.",
                "Morning. My focus is fried and my patience is thin. Bear with me, okay?",
                "Oh. Morning. Brain's running too many processes today. I need to defrag. Alone.",
                "Morning. I'm irritable and I can't quantify why. Just... give me a little space." },
            ["Alex"] = new List<string> {
                "Yeah, morning. Not really feelin' it today, so don't take it personal.",
                "Morning. I'm in a funk today. It's not you, I just woke up off. Gimme some room.",
                "Hey. Not feelin' great today. I'll shake it off, just... go easy on me.",
                "Yeah, mornin'. Kind of a rough head today. Don't mind me bein' quiet." },
            ["Elliott"] = new List<string> {
                "Good morning. Forgive my brevity — the words won't come and neither will my warmth, today.",
                "Good morning. A grey humor has me in its grip today. Forgive my poor company.",
                "Morning. The muse has fled and taken my cheer with her. I'll be poor conversation.",
                "Ah. Morning. My spirits are at low tide. Bear with me 'til they turn." },
            ["Harvey"] = new List<string> {
                "Oh. Good morning. I'm, ah, not quite myself today. Best give me a little room.",
                "Good morning. I, ah, woke up rather out of sorts. Best give me a bit of space.",
                "Morning. My nerves are a touch frayed today. It'll settle. Bear with me, won't you?",
                "Oh. Morning. I'm feeling a little low today. Nothing serious — just need some quiet." },
            ["Sam"] = new List<string> {
                "Hey. ...Yeah, I'm kinda in a funk this morning. Don't worry about it.",
                "Morning. The music in my head went quiet today. Kinda bummed. Don't mind me.",
                "Hey. I'm in a weird funk this morning. Not your fault. Just gimme a bit.",
                "Mornin'. Feelin' off today, can't shake it. I'll be alright. Probably." },
            ["Sebastian"] = new List<string> {
                "Morning. I'd rather not talk right now, if that's okay. Just one of those days.",
                "Morning. Pulling inward today. It's not you. Just one of those grey ones.",
                "Hey. Not in a talking place this morning. I'll be down in the basement.",
                "Morning. Everything feels far away today. Give me some space and I'll come back around." },
            ["Shane"] = new List<string> {
                "...Morning. I'm not in a great place today. Don't push it, alright?",
                "Morning. The dark kind of rolled back in today. Don't push it, okay? I'm tryin'.",
                "Hey. ...Rough one today. Nothin' happened, it just hit. Just let me be for now.",
                "...Morning. Feelin' low. It's not you. Just gotta get through it. Bear with me." }
        };

        private static readonly List<string> GenericHappy = new List<string> {
            "Good morning, my love! I woke up in such a wonderful mood. It's so good to see your face.",
            "Good morning! I'm in such high spirits today. Everything just feels right with you here.",
            "Morning, sweetheart! My heart's so light this morning. You have everything to do with that.",
            "Good morning, my love! Today feels full of promise — and it starts with seeing you." };
        private static readonly List<string> GenericGrumpy = new List<string> {
            "Oh. Morning. I'm not really feeling up to talking right now. Sorry.",
            "Morning. I'm a bit out of sorts today. It's nothing you did. Just bear with me.",
            "Oh. Morning. Not feeling my best today. I'd rather keep to myself for a while.",
            "...Morning. I woke up in a mood. It'll pass. Just go a little easy on me, okay?" };

        // ─────────────────────────────────────────────────────────────
        //  GENERAL POOL (everyday married flavor, used on neutral days)
        // ─────────────────────────────────────────────────────────────
        private static readonly List<string> GeneralPool = new List<string>
        {
            "Morning. I put the kettle on already, figured you'd want some before the work starts.",
            "I was just thinking about you. ...Well, I'm always thinking about you, but more than usual.",
            "Don't work yourself too hard out there today, okay? Come home to me in one piece.",
            "I had the strangest dream last night. You were in it, of course. You always are.",
            "It's a good life we've got here, isn't it? Quiet. Ours.",
            "I saved you the last of the breakfast. Don't tell anyone I'm that soft.",
            "Every time you walk back through that door, my whole day gets a little better.",
            "I keep catching myself smiling for no reason. Then I remember the reason. It's you.",
            "Whatever today throws at us, we handle it together. That's the deal, right?",
            "You looked so peaceful sleeping I almost didn't want to wake you.",
            "Come find me later, would you? Even five minutes with you beats a whole day without.",
            "I love the little ordinary moments most. Coffee, quiet, you across the room.",
            "Be careful in those mines if you head down there. I need you back tonight.",
            "I was humming a song all morning and only just realized it's the one from our wedding.",
            "Some days I still can't believe you picked me. Then I get over it and just feel lucky.",
            "The house feels right with you in it. Too empty when you're gone long.",
            "I'll keep dinner warm if you're out late. Just... don't be TOO late.",
            "You've got that look like you're already planning ten things. Don't forget to breathe.",
            "Give me a kiss before you head out? ...For luck. Obviously.",
            "Thanks for yesterday. And every day, really. I don't say it enough.",
            "There's nowhere I'd rather wake up than right here, next to you.",
            "If you find anything pretty out there today, think of me, yeah?",
            "I'm proud of you. Of us. Of whatever it is we're building here.",
            "Stay safe today. And hurry home. I get bored of missing you.",
            "Morning. The coffee's fresh and the day's wide open. Not bad, right?",
            "I fed the cat already. Don't worry, I told it you love it more.",
            "Sleep well? You were mumbling about crops again. It was kind of adorable.",
            "I'll be here when you get back. Same as always. I kind of love that about us.",
            "Don't forget to eat something out there. You always forget, and I always notice.",
            "The sun's barely up and you're already raring to go. I admire that. From bed, mostly.",
            "I dreamed we were old and grey and still doing exactly this. Wouldn't change a thing.",
            "Take a jacket. ...I know, I know. But humor me.",
            "Funny how a quiet morning with you beats anything fancy I ever wanted before.",
            "I left a little note in your bag. Don't read it 'til lunch. ...Okay, read it whenever.",
            "You make this old farmhouse feel like the best place in the valley.",
            "Whatever's been weighing on you lately — we'll figure it out. Together. Always together.",
            "I caught myself talking about you to the chickens this morning. They're very supportive.",
            "Go on, work your magic out there. I'll keep the home fires going.",
            "I love that the first face I see each day is yours. Even the bedhead. Especially the bedhead.",
            "Come back to me in one piece, alright? That's the only rule I've got."
        };

        public static string GetRandomGeneralLine(System.Random rng)
        {
            int i = rng.Next(GeneralPool.Count);
            return I18n.Get($"general.{i}", GeneralPool[i]);
        }

        public static string GetHappyMood(string name, System.Random rng)
            => LocPool("mood.happy", name, HappyMood, GenericHappy, rng);

        public static string GetGrumpyMood(string name, System.Random rng)
            => LocPool("mood.grumpy", name, GrumpyMood, GenericGrumpy, rng);

        // ─────────────────────────────────────────────────────────────
        //  FEEDING (morning dialogue hints)
        // ─────────────────────────────────────────────────────────────
        public static string GetCookingLine(string name)
            => I18n.Get($"feeding.cooking.{(IsVanilla(name) ? name : "generic")}", CookingEnglish(name));
        private static string CookingEnglish(string name) => name switch
        {
            "Abigail" => "I'm cooking tonight! Don't worry, it's not as scary as my mom's stuff. Probably.",
            "Penny" => "I'll have a nice dinner ready for you tonight. I love taking care of you.",
            "Haley" => "I'm making dinner tonight, so don't you dare fill up on snacks. It's going to look amazing.",
            "Emily" => "I feel inspired to cook tonight — something colorful for the both of us. The kitchen's mine!",
            "Leah" => "I'll handle dinner tonight. Something rustic and good, the way I like it.",
            "Maru" => "I've got dinner optimized for tonight — balanced macros and everything. You're welcome.",
            "Alex" => "I'm cookin' tonight! Grandma taught me a thing or two. You're gonna be impressed.",
            "Elliott" => "Allow me the honor of preparing tonight's meal. A dish worthy of the one I adore.",
            "Harvey" => "I'll make us a nice, wholesome dinner tonight. Good food is good medicine, after all.",
            "Sam" => "Yo, I'm makin' dinner tonight! It'll be awesome. Or at least edible. Definitely edible.",
            "Sebastian" => "I'll cook tonight. Nothing fancy, but it'll be good. Just leave the kitchen to me.",
            "Shane" => "I'll handle dinner tonight. I, uh, actually like cooking for you. Don't make it weird.",
            _ => "I'll take care of dinner tonight, so don't worry about the cooking."
        };

        public static string GetProvideLine(string name)
            => I18n.Get($"feeding.provide.{(IsVanilla(name) ? name : "generic")}", ProvideEnglish(name));
        private static string ProvideEnglish(string name) => name switch
        {
            "Abigail" => "I'm starving already. Think you could leave something tasty in the fridge for dinner?",
            "Penny" => "I was hoping you might leave a little something nice in the fridge for tonight's dinner.",
            "Haley" => "I'm NOT cooking today. Be a dear and put something good in the fridge, would you?",
            "Emily" => "My energy's all turned outward today — would you mind leaving us something for dinner?",
            "Leah" => "I'm buried in a project today. Mind tossing something in the fridge for dinner?",
            "Maru" => "I've got a double shift, so dinner's on you. Something with actual nutrients, please!",
            "Alex" => "Hey, mind grabbin' dinner today? Leave somethin' hearty in the fridge, I'll be hungry.",
            "Elliott" => "Might I impose upon you for tonight's repast? A morsel in the cold-box would delight me.",
            "Harvey" => "Would you mind providing dinner today? Something nourishing in the fridge would be lovely.",
            "Sam" => "Yo, can you handle food today? Just stick somethin' good in the fridge, I'll find it.",
            "Sebastian" => "Can you grab dinner today? Just leave it in the fridge, I'll heat it up later.",
            "Shane" => "Hey, uh, mind leaving something in the fridge for dinner? I'd appreciate it. Really.",
            _ => "Would you mind leaving something nice in the fridge for dinner tonight?"
        };

        public static string GetHungryLine(string name)
            => I18n.Get($"feeding.hungry.{(IsVanilla(name) ? name : "generic")}", HungryEnglish(name));
        private static string HungryEnglish(string name) => name switch
        {
            "Abigail" => "There was NOTHING in the fridge last night. I went to bed hungry. Not cool.",
            "Penny" => "The fridge was empty last night... I didn't want to say anything, but I went hungry.",
            "Haley" => "I had to go to bed STARVING because the fridge was empty. Do you know how that feels?",
            "Emily" => "My stomach growled all night — the fridge was bare. My aura's all out of sorts now.",
            "Leah" => "Empty fridge last night. I went hungry. A little notice next time, yeah?",
            "Maru" => "Blood sugar bottomed out overnight — the fridge was empty. That's genuinely bad for me, you know.",
            "Alex" => "Dude, the fridge was empty! I went to bed hungry. An athlete's gotta EAT.",
            "Elliott" => "I supped on naught but moonlight last eve — the cold-box was barren. A sorry state indeed.",
            "Harvey" => "The fridge was empty and I went to bed hungry. As your doctor and your spouse, I have to say — not good.",
            "Sam" => "Bro, there was zero food last night. I was so hungry I couldn't even sleep.",
            "Sebastian" => "Fridge was empty last night. Went to bed hungry. ...Thanks for that.",
            "Shane" => "Nothing in the fridge last night. Went to bed hungry. Felt about as low as usual, honestly.",
            _ => "The fridge was empty last night and I went to bed hungry. Please don't let that happen again."
        };

        // ─────────────────────────────────────────────────────────────
        //  ANNIVERSARY
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, AnniversaryInfo> Anniversaries = new Dictionary<string, AnniversaryInfo>
        {
            ["Abigail"] = new AnniversaryInfo {
                Reminder = "Hey adventurer,^Do you know what today is? It's OUR anniversary! Don't you dare forget. I expect something good. ;) ^- Abigail",
                Sweet = "You remembered! Of course you did. Best partner in crime a girl could ask for. Happy anniversary, you.",
                Disappointed = "You forgot our anniversary. ...Wow. I really thought you were different. I'm not okay right now."
            },
            ["Penny"] = new AnniversaryInfo {
                Reminder = "My dearest,^Today is the anniversary of the day we married. I treasure every single day with you. With all my love,^- Penny",
                Sweet = "Oh, you remembered our anniversary... I'm so happy I could cry. This is the life I always dreamed of. Thank you.",
                Disappointed = "You forgot our anniversary. I tried so hard not to mind, but... it really hurt. I just feel small today."
            },
            ["Haley"] = new AnniversaryInfo {
                Reminder = "Hey you,^It's our anniversary, in case it slipped that pretty little head of yours. I'll be expecting something nice. ^- Haley",
                Sweet = "You actually remembered! And it's perfect. Ugh, you're going to make me cry and ruin my makeup. Happy anniversary.",
                Disappointed = "You forgot our anniversary. I got all dressed up and everything. I've never felt so stupid."
            },
            ["Emily"] = new AnniversaryInfo {
                Reminder = "Beloved,^The stars aligned a year ago today to bind our spirits. I feel it humming in the air. Celebrate with me?^- Emily",
                Sweet = "Our anniversary, and you felt it too! The colors in this room are dancing. My spirit is so full. I love you.",
                Disappointed = "You forgot our anniversary. The whole house feels grey today. My aura just... dimmed. I need some quiet."
            },
            ["Leah"] = new AnniversaryInfo {
                Reminder = "Hey,^It's our anniversary today. Hard to believe it's been a year. I'm glad I took the leap with you.^- Leah",
                Sweet = "You remembered our anniversary. Heh. You old softie. This is exactly the life I wanted. Cheers to us.",
                Disappointed = "You forgot it was our anniversary. I'm trying not to make it a thing, but... yeah. It stung more than I expected."
            },
            ["Maru"] = new AnniversaryInfo {
                Reminder = "Hi!^My calendar flagged today: it's our anniversary! Statistically the best decision I ever made. Don't forget. ^- Maru",
                Sweet = "You remembered our anniversary! See, this is why the data loves you. Happy anniversary — you wonderful variable.",
                Disappointed = "You forgot our anniversary. I had it flagged for weeks. I know it's not 'logical' to be hurt, but I am."
            },
            ["Alex"] = new AnniversaryInfo {
                Reminder = "Hey babe,^It's our anniversary today!! Don't leave me hangin', okay? I got a good feeling about today.^- Alex",
                Sweet = "You remembered our anniversary! Aw man, you're the best. Seriously. Marryin' you was my best play ever.",
                Disappointed = "You forgot our anniversary. ...That one actually got me. I was really lookin' forward to today."
            },
            ["Elliott"] = new AnniversaryInfo {
                Reminder = "My beloved,^A year ago today, two souls became a single verse. Let us mark the occasion as it deserves.^- Elliott",
                Sweet = "You remembered our anniversary. My heart overflows like ink across the page. This day shall live in my finest chapter.",
                Disappointed = "You forgot our anniversary. The day reads like a torn-out page. I'll be by the window, with only the sea for solace."
            },
            ["Harvey"] = new AnniversaryInfo {
                Reminder = "My dear,^I just wanted to remind you — gently! — that today is our anniversary. It's been the best year of my life.^- Harvey",
                Sweet = "You remembered our anniversary! Oh, my heart's positively racing. You've made me the happiest man in the valley.",
                Disappointed = "You forgot our anniversary. I, ah... I won't make a fuss. But I'd be lying if I said it didn't ache a little."
            },
            ["Sam"] = new AnniversaryInfo {
                Reminder = "Yo!!^It's our ANNIVERSARY today, dude! A whole year! Don't forget, okay? I'm super pumped.^- Sam",
                Sweet = "You remembered our anniversary?! Awesome! Best. Year. Ever. I'm gonna write a song about today, I swear.",
                Disappointed = "You forgot our anniversary. Man. I was so hyped for today, too. That kinda bums me out, not gonna lie."
            },
            ["Sebastian"] = new AnniversaryInfo {
                Reminder = "Hey,^It's our anniversary today. One year. I'm not great with this stuff, but... it matters to me. Don't forget.^- Sebastian",
                Sweet = "You remembered our anniversary. ...Honestly didn't expect it to hit me like this. Thanks. Best year I've had.",
                Disappointed = "You forgot our anniversary. Figures. I let myself get my hopes up for once. ...Whatever. I'm heading downstairs."
            },
            ["Shane"] = new AnniversaryInfo {
                Reminder = "Hey,^So, uh, it's our anniversary today. A year with you. Honestly, never thought I'd get this. Don't forget, okay?^- Shane",
                Sweet = "You remembered our anniversary. ...A year ago I was a mess. Now I've got you. Thank you. For everything.",
                Disappointed = "You forgot our anniversary. ...Yeah. Of course. Why'd I think today would be different. I'm gonna go be alone."
            }
        };

        private static readonly AnniversaryInfo GenericAnniversary = new AnniversaryInfo
        {
            Reminder = "My love,^Today is the anniversary of the day we were married. I cherish every moment with you. Don't let today pass us by.^- Your spouse",
            Sweet = "You remembered our anniversary! It means the world to me. I love you more today than ever.",
            Disappointed = "You forgot our anniversary today. I tried not to let it bother me, but it really did hurt."
        };

        public static AnniversaryInfo GetAnniversary(string name)
        {
            bool has = IsVanilla(name) && Anniversaries.ContainsKey(name);
            string p = $"anniversary.{(has ? name : "generic")}";
            AnniversaryInfo a = has ? Anniversaries[name] : GenericAnniversary;
            return new AnniversaryInfo
            {
                Reminder = I18n.Get($"{p}.reminder", a.Reminder),
                Sweet = I18n.Get($"{p}.sweet", a.Sweet),
                Disappointed = I18n.Get($"{p}.disappointed", a.Disappointed)
            };
        }

        // ─────────────────────────────────────────────────────────────
        //  MAKEUP GIFTS
        // ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, MakeupInfo> Makeup = new Dictionary<string, MakeupInfo>
        {
            ["Abigail"] = new MakeupInfo { Category = "nature",
                Hint = "If you really want to make it up to me... something from deep underground or the wild would speak to me.",
                Reconcile = "You actually listened. A treasure from the wild. ...Okay. We're good. C'mhere, you.",
                Resigned = "I guess you weren't going to make it right. Fine. I'm over it. Mostly." },
            ["Penny"] = new MakeupInfo { Category = "homemade",
                Hint = "It's not about money. Something you made with your own hands would mean everything to me.",
                Reconcile = "You made this... for me? I— that's the kindest thing. All is forgiven. Truly.",
                Resigned = "I suppose the moment's passed. It's alright. I'm used to letting things go." },
            ["Haley"] = new MakeupInfo { Category = "sweet",
                Hint = "If you're sorry, prove it. Bring me something sweet. A girl has a weakness, okay?",
                Reconcile = "Something sweet, just for me? Ugh, fine, I forgive you. But only because it's adorable.",
                Resigned = "You never did make it up to me. Whatever. I've decided to be over it. You're lucky I'm generous." },
            ["Emily"] = new MakeupInfo { Category = "homemade",
                Hint = "Words won't mend this — but something crafted by your own hands carries real spirit. That would reach me.",
                Reconcile = "You poured yourself into this. I can feel it. My aura's mending already. We're whole again.",
                Resigned = "The rift just... faded on its own, I suppose. The spirit moves on. So will I." },
            ["Leah"] = new MakeupInfo { Category = "nature",
                Hint = "If you mean it, bring me something from nature. Something real, something that grew.",
                Reconcile = "Something straight from the earth. That's so you. Heh. Okay. We're good, you sweet thing.",
                Resigned = "Guess you weren't going to come around. That's fine. I've made my peace with it." },
            ["Maru"] = new MakeupInfo { Category = "homemade",
                Hint = "Don't just buy an apology. Something you built or made yourself — that's data I can trust.",
                Reconcile = "You MADE this? Okay, the numbers just flipped completely positive. Apology accepted. Warmly.",
                Resigned = "I've recalculated and decided to let it go. No sense running a grudge subroutine forever." },
            ["Alex"] = new MakeupInfo { Category = "sweet",
                Hint = "Look, the way to fix this is easy — bring me somethin' sweet and tasty. I'm a simple guy.",
                Reconcile = "Aw, somethin' sweet? For me? Okay, okay, you win. We're cool. C'mere, champ.",
                Resigned = "Guess the sorry never came. It's whatever. I'm not gonna hold it forever." },
            ["Elliott"] = new MakeupInfo { Category = "sweet",
                Hint = "To soften my wounded heart... something sweet upon the tongue would be a fitting overture.",
                Reconcile = "Ah, a sweet offering — poetry in edible form. The breach is healed. Come, my dear.",
                Resigned = "The apology never arrived. So the chapter closes quietly, as such chapters do." },
            ["Harvey"] = new MakeupInfo { Category = "homemade",
                Hint = "You needn't spend a thing. Something you made with care would mend this far better than coin.",
                Reconcile = "You made this yourself? Oh, my. That's... deeply touching. All is forgiven, truly.",
                Resigned = "I suppose the matter's simply faded. That's alright. I never could stay upset for long." },
            ["Sam"] = new MakeupInfo { Category = "sweet",
                Hint = "Easiest fix ever, dude — bring me somethin' sweet and we're square. I've got the biggest sweet tooth.",
                Reconcile = "Aw sweet, literally! Okay, we're totally good now. You're the best, you know that?",
                Resigned = "Eh, the sorry never showed, but it's cool. I can't stay mad, it's not my thing." },
            ["Sebastian"] = new MakeupInfo { Category = "nature",
                Hint = "If you actually care... bring me something from outside. Something wild. I'd get that.",
                Reconcile = "Something from out there in the wild. ...Yeah. That's the good stuff. Okay. We're fine.",
                Resigned = "Apology never came. Figures. I'm letting it go anyway. Not worth the headspace." },
            ["Shane"] = new MakeupInfo { Category = "sweet",
                Hint = "You wanna fix it? Bring me somethin' sweet. Don't laugh — it's the one thing that helps.",
                Reconcile = "Somethin' sweet, huh. ...Yeah. That actually helps. A lot. Okay. Thanks. We're okay.",
                Resigned = "The apology never came. Figures. Story of my life. I'll just let it go, like always." }
        };

        private static readonly MakeupInfo GenericMakeup = new MakeupInfo
        {
            Category = "sweet",
            Hint = "If you want to make things right between us, bring me something sweet. It would mean a lot.",
            Reconcile = "You brought me something sweet... thank you. That means more than you know. We're okay now.",
            Resigned = "I waited, but the apology never came. I'm letting it go on my own. Let's just move forward."
        };

        public static MakeupInfo GetMakeup(string name)
        {
            bool has = IsVanilla(name) && Makeup.ContainsKey(name);
            string p = $"makeup.{(has ? name : "generic")}";
            MakeupInfo m = has ? Makeup[name] : GenericMakeup;
            // Category is a logic identifier ("sweet"/"nature"/"homemade") — never translated.
            return new MakeupInfo
            {
                Category = m.Category,
                Hint = I18n.Get($"{p}.hint", m.Hint),
                Reconcile = I18n.Get($"{p}.reconcile", m.Reconcile),
                Resigned = I18n.Get($"{p}.resigned", m.Resigned)
            };
        }

    }
}
