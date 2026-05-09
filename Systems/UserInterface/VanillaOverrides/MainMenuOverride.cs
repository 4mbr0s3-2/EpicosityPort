using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Graphics;
using System;
using System.Windows.Forms;
using Terraria;
using Terraria.GameContent;
using static Mono.Cecil.Cil.OpCodes;
namespace EpicosityPort.Systems.UserInterface.VanillaOverrides
{
    public class MainMenuOverride
    {
        static bool itsAllMemesText = false;
        static float splashTextScaleTimer = 0;
        static string splashText = "missingno.";
        internal static void ProcessCharacterName(ILContext il)
        {
            var c = new ILCursor(il).Goto(0);
            var originalCode = c.DefineLabel();
            c = new ILCursor(il).Goto(0);
            c.Emit(Ldarg_1);
            c.EmitDelegate<Func<string, bool>>((text) =>
            {
                if (text.ToLower().Trim() == "jschlatt")
                {
                    MessageBoxShow("You think you're funny?");
                }
                else if (text.ToLower().Trim() == "character name")
                {
                    MessageBoxShow("Haha. Very funny.");
                }
                else if (text.ToLower().Trim() == "mf doom")
                {
                    System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=7ZrYFGigX8k");
                }
                else if (text.ToLower().Trim() == "gaster")
                {
                    // RS: I'm Hilarious
                    Main.instance.Exit();
                }
                else if (text.ToLower().Trim() == "gameraiders101")
                {
                    // RS: I'm not gonna port this but you already know what this does
                    //if (Epicosity.loadedCompatibilityMods.HasFlag(Epicosity.CompatibilityMods.JoostMod))
                    //{
                    // Mod joost = ModLoader.GetMod("JoostMod");
                    // Item hat = joost.GetItem("MooshroomMask").item;
                    // Item body = joost.GetItem("MooshroomBody").item;
                    // Item legs = joost.GetItem("MooshroomLegs").item;
                    // Main.PendingPlayer.armor[10] = hat;
                    // Main.PendingPlayer.armor[11] = body;
                    // Main.PendingPlayer.armor[12] = legs;
                    //}
                }
                return false;
            });
            c.Emit(Brfalse_S, originalCode); // If false, then skip to original code
            c.Emit(Ret);
            c.MarkLabel(originalCode); // Marker for the rest of the code where the cursor is at
        }

        internal static string HookGetRandomGameTitle(Terraria.On_Lang.orig_GetRandomGameTitle orig)
        {
            if (!EpicosityConfigClient.Instance.DisableModifiedGameWindowTitles)
            {
                string message = MainMenuTitleMessages.GetRandomGameTitle();
                if (message == "Terraria: Wait... it's all (dead) memes?")
                    itsAllMemesText = true;
                return message;
            }
            else
            {
                return orig();
            }
        }

        public static void HookYouDiedGUI(Terraria.On_Main.orig_DrawInterface_35_YouDied orig)
        {
            if (EpicosityConfigClient.Instance.DisableModifiedDeathMessages)
            {
                orig();
                return;
            }
            DeathInterfaceOverride.DrawInterface_YouDied();
        }

        public static string[] mainMenuSplashMessages;

        // RS: I think I spent more time thinking of adding Minecraft-style splash messages
        // for specific mod combinations than actually working on the mod

        //public static string[] macrocosmSplashMessages =
        //{
        //	// References to that one Moonbase Alpha video by Scorpy, Gir, and motdef
        //	"SPAAACCCCCE!",
        //	"Space? SPACE!",
        //	"aeiou",
        //	"John Madden!",
        //	"Football!",
        //	"holla holla get $",
        //	"999999999999",
        //	"Ground control to Major Tom...", // Space Oddity
        //	"I'm sorry, Dave...", // 2001: A Space Odyssey
        //};

        //public static string[] thoriumSplashMessages = {
        //	"DivermanSam is cool!",
        //	"AdipemDragon is cool!"
        //};

        //public static string[] overhaulSplashMessages = {
        //	"Compatible by default?!",
        //	"Console music? On PC? Like, 1.4?",
        //	"Mirsario is cool!",
        //};

        //public static string[] calamityMusicSplashMessages = {
        //	//"DOKUROOOOOOOOOOOOOO!",
        //	"Now with GODLIKE TRACKS!",
        //	"BANGER ALERT!",
        //	//"\"Um, I really liked Timepiece Phase II...\"", // RS: I'm pretty sure this is a reference to KFAD2 MOJO
        //	"Jared loves his hugs UwU",
        //	"Fabsol is cool!",
        //	"MountainDrew is cool!",
        //	"Hectique is cool!",
        //	"Now with Touhou's bells!",
        //	//"Acidic chocolate!",
        //};

        public static void SetMainMenuSplashText()
        {
            if (mainMenuSplashMessages == null)
            {
                byte[] b = EpicosityPort.Instance.GetFileBytes($"SplashMessages.txt");
                string s = System.Text.Encoding.UTF8.GetString(b);
                mainMenuSplashMessages = s.Split('\n');
            }

            string text = mainMenuSplashMessages[Main.rand.Next(mainMenuSplashMessages.Length)];

            // RS: Get me OUT of this code base
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            //{
            //	text = "Also play Friday Night Funkin!";
            //}
            // Mutually exclusive
            //if (Epicosity.CalamityTupacMenu)
            //{
            //	text = "Something's off...";
            //}
            //else if (itsAllMemesText)
            if (itsAllMemesText)
            {
                text = "Always has been...";
            }

            SetMainMenuSplashText(text);
        }
        public static void SetMainMenuSplashText(string text)
        {
            splashText = text;
        }
        internal static void HookMenuSplash(ILContext il)
        {
            var c = new ILCursor(il).Goto(0);
            if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Main).GetField("dayTime"))))
                return;
            c.Emit(Call, typeof(MainMenuOverride).GetMethod("DrawSplashText")); // Use reflection to pass the method
        }
        public static int splashTextAnimationTimer = 0;
        public static void DrawSplashText()
        {
            splashTextScaleTimer += 1f;
            splashTextScaleTimer %= 60;
            if (splashTextAnimationTimer < 120)
            {
                splashTextAnimationTimer += 1;
            }
            if (splashText == null) return;

            Vector2 stringSize = FontAssets.DeathText.Value.MeasureString(splashText);
            Vector2 position = new Vector2((float)(Main.screenWidth / 2) + 130, 170f - (220f * (1f - Easings.EaseOutBounce(splashTextAnimationTimer / 120f))));
            float scale = (splashText.Length > 20 ? 20.0f / splashText.Length : 1) * (0.75f + Math.Abs((float)Math.Sin(2 * Math.PI / 60f * splashTextScaleTimer) * 0.07f));
            for (int d = 0; d < 4; d++)
            {
                Main.spriteBatch.DrawString
                (
                    FontAssets.DeathText.Value,
                    splashText,
                    position - Vector2.UnitX.RotatedBy(d * MathHelper.PiOver2) * 2,
                    Color.Black * 0.5f,
                    -0.3f,
                    stringSize / 2,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }
            Main.spriteBatch.DrawString
            (
                FontAssets.DeathText.Value,
                splashText,
                position,
                Color.Yellow,
                -0.3f,
                stringSize / 2,
                scale,
                SpriteEffects.None,
                0
            );

            // Disclaimer
            for (int textIndex = 0; textIndex < 5; textIndex++)
            {
                Color color = Color.Black;
                if (textIndex == 4)
                {
                    byte b = (byte)((byte.MaxValue + Main.tileColor.R * 2) / 3);
                    color = new Color(b, b, b, 255);
                    color.R = (byte)((255 + color.R) / 2);
                    color.G = (byte)((255 + color.R) / 2);
                    color.B = (byte)((255 + color.R) / 2);
                }
                color.A = (byte)(color.A * 0.3f);
                double angle = Math.PI / 2 * textIndex;
                int xOffset = (int)Math.Cos(angle) * 2;
                int yOffset = (int)Math.Sin(angle) * 2;
                string text5 = "Notice: Epicosity is on hold and will be restarting development with tModLoader v0.12. This is the last pre-1.4 test build!";
                Vector2 origin2 = FontAssets.MouseText.Value.MeasureString(text5);
                origin2.X *= 0.5f;
                origin2.Y *= 0.5f;
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, text5, new Vector2(Main.screenWidth / 2f + xOffset, Main.screenHeight - origin2.Y + yOffset), color, 0f, origin2, 1f, SpriteEffects.None, 0f);
            }
        }
        internal static void MessageBoxShow(string text, string caption = null)
        {
            // MessageBox.Show fails on Mac, this method will open a text file to show a message.
            caption = caption ?? "Terraria";
#if !MAC
            MessageBox.Show(text, caption);
#else
			File.WriteAllText("fake-messagebox.txt", $"{caption}\n\n{text}");
			Process.Start("fake-messagebox.txt");
#endif
        }
    }
}
