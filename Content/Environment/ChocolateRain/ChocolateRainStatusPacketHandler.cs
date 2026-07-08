using System;
using System.IO;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using EpicosityPort.Systems.Network;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public partial class ChocolateRainManager
    {
        internal class ChocolateRainStatusPacketHandler : PacketHandler
        {
            public const byte SyncRain = 1;

            public ChocolateRainStatusPacketHandler() : base()
            {
            }

            public override void HandlePacket(BinaryReader reader, int fromWho)
            {
                switch (reader.ReadByte())
                {
                    case (SyncRain):
                        ReceiveTarget(reader, fromWho);
                        break;
                }
            }

            public void SendTarget(int toWho, int fromWho, bool rainingChocolate)
            {
                ModPacket packet = GetPacket(SyncRain, fromWho);
                packet.Write(rainingChocolate);
                packet.Send(toWho, fromWho);
            }

            public void ReceiveTarget(BinaryReader reader, int fromWho)
            {
                if (Terraria.Main.netMode == NetmodeID.MultiplayerClient)
                {
                    reader.ReadByte(); // fromWho, which really isn't needed since it's always sent by the server
                }
                bool rainingChocolate = reader.ReadBoolean();
                if (Terraria.Main.netMode == NetmodeID.Server)
                {
                    SendTarget(-1, fromWho, rainingChocolate);
                }
                else
                {
                    Instance.RainingChocolate = rainingChocolate; // Update on the rest of the clients
                    if (rainingChocolate)
                    {
                        // RS TODO
                        //for (int x = 0; x < Terraria.Main.anglerQuestItemNetIDs.Count(); x++)
                        //{
                        //    if (Terraria.Main.anglerQuestItemNetIDs[x] == ModContent.ItemType<ChocolateFish>())
                        //    {
                        //        Terraria.Main.anglerQuest = x;
                        //        break;
                        //    }
                        //}
                    }
                    else
                    {
                        Terraria.Main.AnglerQuestSwap();
                    }
                }
            }
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
