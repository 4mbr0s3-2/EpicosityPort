using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    // RS: We have technology now (ModWaterStyle)
    //public class ChocolateRainRender : ModSystem
    //{
    //    //public override void ChooseWaterStyle(ref int style)
    //    //{
    //    //    if (ChocolateRain.IsRainingChocolate)
    //    //    {
    //    //        style = 100;
    //    //    }
    //    //}

    //    internal static void ChocolateRainUpdate(Terraria.On_Rain.orig_Update orig, Rain self)
    //    {
    //        Rain rain = self;
    //        if (ChocolateRainManager.IsRainingChocolate)
    //        {
    //            rain.position += rain.velocity;
    //            bool playerCollisionFlag = false;
    //            foreach (Player player in Main.player)
    //            {
    //                if (player.HeldItem.type == ItemID.Umbrella)
    //                {
    //                    if (Collision.CheckAABBvAABBCollision(rain.position, new Vector2(2, 2), player.position - new Vector2(40, 20), new Vector2(80, 120)))
    //                    {
    //                        playerCollisionFlag = true;
    //                    }
    //                }
    //            }
    //            if (Collision.SolidCollision(rain.position, 2, 2) || playerCollisionFlag || rain.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f || Collision.WetCollision(rain.position, 2, 2))
    //            {
    //                rain.active = false;
    //                if ((float)Main.rand.Next(100) < Main.gfxQuality * 100f)
    //                {
    //                    int num = 154;
    //                    if (ChocolateRainManager.IsRainingChocolate) // Chocolate Rain
    //                    {
    //                        num = 7;
    //                    }
    //                    if (rain.type == 3 || rain.type == 4 || rain.type == 5) // Blood Moon Rain
    //                    {
    //                        num = 218;
    //                    }
    //                    int num2 = Dust.NewDust(rain.position - rain.velocity, 2, 2, num, 0f, 0f, 0, default, 1f);
    //                    Dust expr_E6_cp_0 = Terraria.Main.dust[num2];
    //                    expr_E6_cp_0.position.X -= 2f;
    //                    Main.dust[num2].alpha = 38;
    //                    Main.dust[num2].velocity *= 0.1f;
    //                    Main.dust[num2].velocity += -rain.velocity * 0.025f;
    //                    Main.dust[num2].scale = 0.75f;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            orig(self);
    //        }
    //    }

    //    internal static void ChocolateRainHook(Terraria.On_Main.orig_DrawRain orig, Terraria.Main self)
    //    {
    //        Texture2D original = Terraria.Main.instance.OurLoad<Texture2D>("Images" + Path.DirectorySeparatorChar + "Rain");
    //        if (ChocolateRainManager.IsRainingChocolate)
    //        {
    //            Terraria.Main.rainTexture = ModContent.GetTexture("Epicosity/Environment/ChocolateRain/Rain");
    //        }
    //        else if (Terraria.Main.rainTexture == ModContent.GetTexture("Epicosity/Environment/ChocolateRain/Rain")) // Does once so other mods can override rain texture
    //        {
    //            Terraria.Main.rainTexture = original;
    //        }
    //        orig(self);
    //    }
    //}
}
