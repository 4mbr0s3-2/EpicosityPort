using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Network
{
    // RS: This was straight from the tModLoader wiki. Don't you think it's a bit clunky?
    internal abstract class PacketHandler
    {
        public byte HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected PacketHandler(byte handlerType)
        {
            HandlerType = handlerType;
        }

        protected ModPacket GetPacket(byte packetType, int fromWho)
        {
            var p = EpicosityPort.Instance.GetPacket();
            p.Write(HandlerType);
            p.Write(packetType);
            if (Main.netMode == NetmodeID.Server)
            {
                p.Write((byte)fromWho);
            }
            return p;
        }
    }

}
