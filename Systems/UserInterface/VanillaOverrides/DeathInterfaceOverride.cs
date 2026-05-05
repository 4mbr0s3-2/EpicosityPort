using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
namespace EpicosityPort.Systems.UserInterface.VanillaOverrides
{
    public class DeathInterfaceOverride
    {
        public static string deathMessage = "";
        public static string[] deathMessages = {
            "Oof...",
            "Bruh...",
            "Damn that sucks, bro...",
            "Thou hast been slain...", // Shakespearean
            "You died!", // Minecraft
            "You've been knocked out!", // Slime Rancher
			"Game over!", // Also Minecraft but older
			"You've yee'd your last haw!",
            "You've been vibe checked!",
            "R.I.P. in peace...",
            "You were game-ended...",
            "Change the world...",
            "Did you dieded?",
            "you are kil",
            "F",
            "You got nae-nae'd!",
            "Adios...",
            "You just lost the game!", // You also just lost The Game.
            "Eliminated!", // Fall Guys
            "Fail!"
        };

        public static void DrawInterface_YouDied()
        {
            if (Main.player[Main.myPlayer].dead)
            {
                string value = deathMessage;
                //if (value == "Mortis")
                //{
                //    Texture2D mortis = ModContent.GetTexture("Epicosity/UI/EasterEggs/Mortis");
                //    Main.spriteBatch.Draw(mortis, new Vector2(Main.screenWidth / 2 - 2 - mortis.Size().X / 2f, Main.screenHeight / 2 - 110), Main.player[Main.myPlayer].GetDeathAlpha(Color.Transparent));
                //    return;
                //}

                bool extraText = false;
                string textValue;
                //if (value == "GAME OVER IWBTG")
                //{
                //    value = "GAME OVER";
                //    extraText = true;
                //    int time = Main.player[Main.myPlayer].respawnTimer / 60 + 1;
                //    textValue = "WAIT " + time + " SECOND" + (time == 1 ? "" : "S") + " TO TRY AGAIN";
                //    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, textValue, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(textValue).X / 2f, Main.screenHeight / 2 + 30), Main.player[Main.myPlayer].GetDeathAlpha(Color.Transparent), 0f, default, 1f, SpriteEffects.None, 0f);
                //}
                if (value == "GAME OVER YEAH")
                {
                    value = "GAME OVER";
                }

                Main.spriteBatch.DrawString(FontAssets.DeathText.Value, value, new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(value).X / 2f, Main.screenHeight / 2 - 20), Main.player[Main.myPlayer].GetDeathAlpha(Color.Transparent), 0f, default, 1f, SpriteEffects.None, 0f);
                if (value == "You've been knocked out!")
                {
                    extraText = true;
                    textValue = "But a day's rest should fix you right up...";
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, textValue, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(textValue).X / 2f, Main.screenHeight / 2 + 30), Main.player[Main.myPlayer].GetDeathAlpha(Color.Transparent), 0f, default, 1f, SpriteEffects.None, 0f);
                }
                if (Main.player[Main.myPlayer].lostCoins > 0)
                {
                    textValue = Language.GetTextValue("Game.DroppedCoins", Main.player[Main.myPlayer].lostCoinString);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, textValue, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(textValue).X / 2f, Main.screenHeight / 2 + (extraText ? 60 : 30)), Main.player[Main.myPlayer].GetDeathAlpha(Color.Transparent), 0f, default, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        public static void RandomizeSpecialDeathMessageMessage(PlayerDeathReason damageSource)
        {
            string message = deathMessages[new Random().Next(deathMessages.Count())];
            float easterEggChanceMult = EpicosityConfigClient.Instance.EasterEggChanceMult;
            if (Main.rand.NextFloat() < 0.003 * easterEggChanceMult)
            {
                message = "Mortis";
            }
            if (Main.rand.NextFloat() < 0.005 * easterEggChanceMult)
            {
                message = "GAME OVER IWBTG";
            }
            if (damageSource.SourceNPCIndex >= 0)
            {
                //if (BarrelInvasion.GetFullInvaderList().Contains(Main.npc[damageSource.SourceNPCIndex].type))
                //{
                //    message = "You were contained...";
                //}
                //if (Main.npc[damageSource.SourceNPCIndex].type == ModContent.NPCType<DespacitoSpider>())
                //{
                //    message = "Oof!";
                //}
            }
            //if (message == "Mortis")
            //{
            //    SoundEngine.PlaySound(Epicosity.inst.GetLegacySoundSlot(SoundType.Custom, "Sounds/EasterEggs/Mortis"));
            //}
            //if (message == "GAME OVER IWBTG")
            //{
            //    SoundEngine.PlaySound(Epicosity.inst.GetLegacySoundSlot(SoundType.Custom, "Sounds/EasterEggs/IWBTGDeath"));
            //}
            deathMessage = message;
        }
    }
}
