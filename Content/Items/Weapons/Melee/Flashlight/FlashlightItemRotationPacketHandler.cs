using EpicosityPort.Systems.Network;
using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.Flashlight
{
    // RS: That's a lot just to replicate the rotation of the flashlight...
    internal class FlashlightItemRotationPacketHandler : PacketHandler
    {
        public const byte SyncTarget = 1;

        public FlashlightItemRotationPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncTarget):
                    ReceiveTarget(reader, fromWho);
                    break;
            }
        }

        public void SendTarget(int toWho, int fromWho, float itemRotation, int playerDirection)
        {
            ModPacket packet = GetPacket(SyncTarget, fromWho);
            packet.Write(itemRotation);
            packet.Write(playerDirection);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveTarget(BinaryReader reader, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                fromWho = reader.ReadByte(); // fromWho
            }
            float itemRotation = reader.ReadSingle();
            int playerDirection = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
            {
                SendTarget(-1, fromWho, itemRotation, playerDirection);
            }
            else
            {
                if (Main.player[fromWho].HeldItem.type == ModContent.ItemType<Flashlight>())
                {
                    ((Flashlight)Main.player[fromWho].HeldItem.ModItem).itemRotation = itemRotation;
                }
                Main.player[fromWho].ChangeDir(playerDirection);
            }
        }
    }
}
