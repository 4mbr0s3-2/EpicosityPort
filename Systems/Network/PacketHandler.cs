using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Network
{
    // RS: This was straight from the tModLoader wiki
    internal abstract class PacketHandler : ModSystem
    {
        public int HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        //protected PacketHandler(byte handlerType)
        //{
        //    HandlerType = handlerType;
        //}

        protected ModPacket GetPacket(byte packetType, int fromWho)
        {
            var p = EpicosityPort.Instance.GetPacket();
            p.Write(HandlerType);
            p.Write(GetType().Name.GetHashCode()); // RS: Just to be sure we have the right one
            p.Write(packetType);
            if (Main.netMode == NetmodeID.Server)
                p.Write((byte)fromWho);
            return p;
        }


        public override void OnModLoad()
        {
            // RS: I'm gonna assume that this will load in the exact same order between server and client
            HandlerType = ModNetHandler.Instance.RegisterPacketHandler(this);
        }
    }

}
