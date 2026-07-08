using EpicosityPort.Systems.Network;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Mono.Cecil.Cil.OpCodes;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public partial class ChocolateRainManager : ModSystem
    {
        public override void OnModLoad()
        {
            //On_Main.DrawRain += ChocolateRainRender.ChocolateRainHook;
            //On_Rain.Update += ChocolateRainRender.ChocolateRainUpdate;
            On_Main.StartRain += StartChocolateRain;
            On_Main.StopRain += StopChocolateRain;
        }
        public override void OnModUnload()
        {
            On_Main.StartRain -= StartChocolateRain;
            On_Main.StopRain -= StopChocolateRain;
        }
        public static ChocolateRainManager Instance { get { return ModContent.GetInstance<ChocolateRainManager>(); } }
        public bool RainingChocolate = false;
        public bool IsRainingChocolate
        {
            set
            {
                RainingChocolate = value;
            }
            get
            {
                return RainingChocolate && Main.raining;
            }
        }
        public float ChocolateRainEffectIntensity = 0;
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (IsRainingChocolate)
            {
                ChocolateRainEffectIntensity += 0.004f;
                if (ChocolateRainEffectIntensity > 1)
                {
                    ChocolateRainEffectIntensity = 1;
                }
            }
            else
            {
                ChocolateRainEffectIntensity -= 0.004f;
                if (ChocolateRainEffectIntensity < 0)
                {
                    ChocolateRainEffectIntensity = 0;
                }
            }
            if (ChocolateRainEffectIntensity > 0)
            {
                if (Main.dayTime)
                {
                    float num28 = (float)Math.Sin((Main.time / 54000.0) * 3.14159f);
                    //white.R = (byte)(num28 * 200f + 55f);
                    //white.G = (byte)(num28 * 180f + 75f);
                    //white.B = (byte)(num28 * 250f + 5f);
                    backgroundColor.R = (byte)(backgroundColor.R * (1 - ChocolateRainEffectIntensity) + (num28 * 40 + 5) * ChocolateRainEffectIntensity);
                    backgroundColor.G = (byte)(backgroundColor.G * (1 - ChocolateRainEffectIntensity) + (num28 * 12 + 2) * ChocolateRainEffectIntensity);
                    backgroundColor.B = (byte)(backgroundColor.B * (1 - ChocolateRainEffectIntensity) + (0) * ChocolateRainEffectIntensity);
                    tileColor.R = (byte)(tileColor.R * (1 - ChocolateRainEffectIntensity) + (num28 * 40 + 5) * ChocolateRainEffectIntensity);
                    tileColor.G = (byte)(tileColor.G * (1 - ChocolateRainEffectIntensity) + (num28 * 12 + 2) * ChocolateRainEffectIntensity);
                    tileColor.B = (byte)(tileColor.B * (1 - ChocolateRainEffectIntensity) + (0) * ChocolateRainEffectIntensity);
                }
                else
                {
                    float num28 = (float)Math.Sin((Main.time / 32400.0) * 3.14159f);
                    //white.R = (byte)(num28 * 200f + 55f);
                    //white.G = (byte)(num28 * 180f + 75f);
                    //white.B = (byte)(num28 * 250f + 5f);
                    backgroundColor.R = (byte)(backgroundColor.R * (1 - ChocolateRainEffectIntensity) + (num28 * 10 + 5) * ChocolateRainEffectIntensity);
                    backgroundColor.G = (byte)(backgroundColor.G * (1 - ChocolateRainEffectIntensity) + (num28 * 3 + 2) * ChocolateRainEffectIntensity);
                    backgroundColor.B = (byte)(backgroundColor.B * (1 - ChocolateRainEffectIntensity) + (0) * ChocolateRainEffectIntensity);
                    tileColor.R = (byte)(tileColor.R * (1 - ChocolateRainEffectIntensity) + (num28 * 10 + 5) * ChocolateRainEffectIntensity);
                    tileColor.G = (byte)(tileColor.G * (1 - ChocolateRainEffectIntensity) + (num28 * 3 + 2) * ChocolateRainEffectIntensity);
                    tileColor.B = (byte)(tileColor.B * (1 - ChocolateRainEffectIntensity) + (0) * ChocolateRainEffectIntensity);
                }
            }
        }

        internal static void StopChocolateRain(Terraria.On_Main.orig_StopRain orig)
        {
            if (Instance.RainingChocolate)
            {
                Instance.RainingChocolate = false;
                Main.AnglerQuestSwap();
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText("Chocolate has stopped raining.", new Color(138, 56, 41));
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    //Sync with net
                    ModNetHandler.GetPacketHandler<ChocolateRainStatusPacketHandler>().SendTarget(-1, 0, Instance.RainingChocolate);
                    //Terraria.NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Chocolate has stopped raining."), new Color(138, 56, 41));
                }
            }
            orig();
        }

        internal static void StartChocolateRain(Terraria.On_Main.orig_StartRain orig)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    Instance.RainingChocolate = true;
                    // RS TODO: Angler stuff
                    //for (int x = 0; x < Terraria.Main.anglerQuestItemNetIDs.Count(); x++)
                    //{
                    //    if (Terraria.Main.anglerQuestItemNetIDs[x] == ModContent.ItemType<ChocolateFish>())
                    //    {
                    //        Terraria.Main.anglerQuest = x;
                    //        break;
                    //    }
                    //}
                    // RS TODO: Localization
                    if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText("It's raining chocolate!", new Color(138, 56, 41));
                    if (Main.netMode == NetmodeID.Server)
                    {
                        //Sync with net
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("It's raining chocolate!"), new Color(138, 56, 41));
                        ModNetHandler.GetPacketHandler<ChocolateRainStatusPacketHandler>().SendTarget(-1, 256, Instance.RainingChocolate);
                    }
                }
            }
            orig();
        }

        // RS TODO: Chocolate water bucket
        public static void PickupChocolateWater(ILContext il)
        {
            var c = new ILCursor(il);
            // Try to find where 566 is placed onto the stack
            if (!c.TryGotoNext(i => i.MatchLdcI4(206)))
            {
                Instance.Mod.Logger.Info("Unable to patch!");
                return; // Patch unable to be applied
            }
            c.Index++;
            c.EmitDelegate<Func<int, int>>((originalValue) =>
            {
                //if (IsRainingChocolate)
                //    return ModContent.ItemType<ChocolateWaterBucket>();
                return originalValue;
            });
        }

        public static void ChocolateRainForecast(ILContext il)
        {
            // RS TODO: Make this localization friendly lol
            var c = new ILCursor(il);
            // Try to find where 566 is placed onto the stack
            if (!c.TryGotoNext(i => i.MatchLdstr("GameUI.HeavyRain")))
            {
                Instance.Mod.Logger.Info("Unable to patch!");
                return; // Patch unable to be applied
            }
            c.Index += 3;
            c.Emit(Ldloc, 19);
            c.EmitDelegate<Func<string, string>>((originalText) =>
            {
                if (Instance.IsRainingChocolate)
                {
                    return originalText.Replace("Rain", "Chocolate Rain");
                }
                else
                {
                    return originalText;
                }
            });
            c.Emit(Stloc, 19);
            //c.EmitDelegate<Func<string, string>>((original) =>
            //{
            //    return "Chocolate Rain";
            //});
            //c.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, (byte)27);
        }
    }
 //   public class ChocolateRainModPlayer : ModPlayer
	//{
	//	public override void UpdateBiomeVisuals()
	//	{
	//		Player.ManageSpecialBiomeVisuals(ShaderNames.ChocolateRainFilter, ChocolateRainManager.Instance.IsRainingChocolate);
	//	}
	//}
}
