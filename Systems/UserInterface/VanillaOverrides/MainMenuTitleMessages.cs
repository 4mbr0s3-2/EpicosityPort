using System;
using System.Linq;

namespace EpicosityPort.Systems.UserInterface.VanillaOverrides
{
    // RS: Read this if you're resistant to old Internet memes
    // If Epicosity was made in 2025 you can imagine a 6-7 reference somewhere here
    public class MainMenuTitleMessages
    {
        public static string[] titleMessages = {
            "Terraria: What's poppin'?",
            "Terraria: You think you're funny?",
            "Terraria: BROOMSTICK HANDLE",
            "Terraria: SCREWDRIVER HANDLE",
            "Terraria: Mrs. Obama, get down!",
            "Terraria: A big, chunky game!", // Big Chungus
			//"Terraria: Otherworld is kil...",
			//"Terraria: Red? More like... blue!",
			"Terraria: Party rockers in the house tonight!",
            "Terraria: Party rock is in the house tonight!",
            "Terraria: Redigit is cool!",
            "Terraria: Okay, now this is epic...",
            "Terraria: I'm here to see Impending Doom",
            "Terraria: Where's the funny?",
            "Terraria: What's happening, forum?",
            "Terraria: Easier than Back On Track!", // GD Reference
			"Terraria: Now everyone can be happy...", // DDLC Reference
			"Terraria: Water, Ice, Salt... Aye!",
            "Terraria: Doin' your mom!",
            "Terraria: LOL, Limewire!",
			//"Terraria: Capture Baby Yoda!",
			"Terraria: Everybody gangsta 'till the fish starts walking",
            "Terraria: You dang barrels!",
            "Terraria: Nobody expects the Spanish Inquisition!",
			//"Terraria: Get over it!",
			"Terraria: Welcome to Terraria. It's safer here.", // Half-Life 2
			"Terraria: Hallowed be thy world", // Reference to Lord's Prayer
			"Terraria: You know it's fun, right?", // PewDiePie Tuber Sim
			"Terraria: You seek the Holy Grail...",
            "Terraria: That's no ordinary rabbit...",
            "Terraria: 1... 2... 5!",

            // RS: First rule of Dab Club: you do not talk about Dab Club
            //"Terraria: DC died for this...", // Dab Club
			
            "Terraria: Also try Fortress Craft 3D!",
            "Terraria: Diggin' makes me feel good!",
            "Terraria: =3",
            "Terraria: Yeah boiiiiiiiiii!",
            "Terraria: Something came in the mail today...", // DEEZ NUTS!
			"Terraria: Enjoy your DOTA!",
			//"Terraria: Consume the chalice!",
			"Nobody: Terraria:",
            "Terraria: That's amore!",
            "Terraria: I'm just playing games!", // Plastic Love
			"Terraria: Mix drinks! Change lives!", // VA-11 Hall-A
			"Terraria: Special thanks to Al0n37 for all those outdated but still somehow useful tModLoader tutorials lol",
            "Terraria: [object Object]",
            "Terraria: Fun is infinite!",
			//"Terrorism: Now with more explosive traps!",
			//"Teriariar: Help me im aheving a stroing eke",
			//"Terraria Otherworld: It's finally here!",
			//"TERRAAAAAAAAAARIAAAAAAAAAAA: AAAAAAAAAAAAAAAAAAAAAAAA",
			"2D Minecraft",
            "Terraria: Now in Wynnic!",
			//""
			"Terraria: Can you r e a d", // https://forums.terraria.org/index.php?threads/white-main-menu.85142/
			
			// THE MAD LADS DID IT

			//"Terraria: Add Moon Lord's legs. Please Re-Logic. You gotta do it. ;-;",
			//"Terraria: Refusing to add Moon Lord's legs since 1.4!",

			"Terraria: Created by Neru!",
            "Terraria: Every pickaxe is overpowered!",
            "Terraria: I really need some title text, 4mbr0s3 2!",
			//"Terraria: Now with a message to Yoraiz0r in 7 different languages!", // Reference to BartenderSpecialText.Party       // RS: OK this was pretty funny
			"Terraria: You can't outpizza the hut!",
            "Terraria: QnJ1aCE=",
            "Terraria: ( ͡° ͜ʖ ͡°)",
            "Terraria: (╯°□°)╯︵ ┻━┻",
            "Terraria: Pickle chin ah boy!",
            "Terraria: \"If it's not fun, why bother?\" - Reggie Fils-Aimé",
            //"Terraria: I would call your code cancer, but cancer actually executes properly...", // Epic pun by the dude          // RS: THIS ISN'T A PUN????
			"Terraria: Have you seen Chef?",
            "Terraria: 19 to 21!",
            "Terraria: Help me Gordon!",
            "Terraria: \"I just simultaneously whipped and nae nae'd!\" - Matthew Patrick",
            "Terraria: \"I just sucked in the puff!\" - Matthew Patrick",
            "Terraria: Bing bing wahoo!",
            "Terraria: Spaghetti!",
            "Terraria: Do the Stanky Leg!",
            "Terraria: Wait... it's all (dead) memes?"
			// If a pickaxe is considered OP because it can break chests, every pickaxe is OP.
			//""
			//"Terraria: ¡Dale!",
			//"Terraria: My name is Jeff"
		};
        public static string GetRandomGameTitle()
        {
            string message = titleMessages[new Random().Next(titleMessages.Count())];
            //if (message == "Terraria: Wait... it's all (dead) memes?") // RS: Imagine if Epicosity was made in 2026; it would have the Triple T stick
            //{
            //    itsAllMemesText = true;
            //}
            return message;
        }
    }
}
