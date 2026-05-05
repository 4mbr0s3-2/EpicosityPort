using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EpicosityPort.Systems.Network
{
    /// <summary>
    /// This class handles all network packets.
    /// </summary>
    internal class ModNetHandler
    {
        // When a lot of handlers are added, it might be wise to automate
        // creation of them
        // Haha I did exactly that

        public static byte currentPacketHandler = 0;

        // RS: Apparently, I didn't know anything about OOP and just relied on putting static fields everywhere...

        public static List<PacketHandler> PacketHandlers;

        // RS: Use an initializer!! Why load on mod load??

        //[LoadManager(LoadManagerAttribute.ManageType.Load)]
        //public static void Load()
        //{
        //	PacketHandlers = new List<PacketHandler>();
        //	InitializePacketHandlers();
        //}
        //[LoadManager(LoadManagerAttribute.ManageType.Unload)]
        //public static void Unload()
        //{
        //	PacketHandlers = null;
        //}

        // RS: This code is so bad, I'm actually (somewhat) fixing it
        public ModNetHandler()
        {
            PacketHandlers = new List<PacketHandler>();
        }

        public static ModNetHandler Instance
        {
            get;
        }

        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            byte packetHandlerType = r.ReadByte();
            bool success = false;
            foreach (PacketHandler packetHandler in PacketHandlers)
            {
                if (packetHandlerType == packetHandler.HandlerType)
                {
                    packetHandler.HandlePacket(r, fromWho);
                    success = true;
                    //Terraria.Main.NewText($"Successfully handled handleType {packetHandlerType} ({packetHandler.GetType().Name})!");
                    break;
                }
            }
            if (!success)
            {
                Terraria.Main.NewText($"Couldn't identify packet of handlerType {packetHandlerType}!");
            }
        }
        /// <summary>
        ///  Gets the specified packet handler from the PacketHandler list. Returns null if no such handler could be found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetPacketHandler<T>() where T : PacketHandler
        {
            foreach (PacketHandler packetHandler in PacketHandlers)
            {
                if (packetHandler.GetType().Equals(typeof(T)))
                {
                    return (T)packetHandler;
                }
            }
            return null;
        }
        public static void RegisterPacketHandler<T>() where T : PacketHandler
        {
            RegisterPacketHandler(typeof(T));
        }
        public static void RegisterPacketHandler(Type type)
        {
            if (type.IsSubclassOf(typeof(PacketHandler)))
            {
                PacketHandlers.Add((PacketHandler)Activator.CreateInstance(type, currentPacketHandler));
                currentPacketHandler++;
            }
        }
        public static void InitializePacketHandlers()
        {
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();

            // Order packet handlers by name in order to prevent any kind of desync between clients that load the types in a different order
            IEnumerable<Type> packetHandlers = allTypes.Where((type) => type.BaseType == typeof(PacketHandler)).OrderBy((type) => type.Name);

            foreach (Type packetHandler in packetHandlers)
            {
                EpicosityPort.Instance.Logger.Info($"Loaded {packetHandler.Name}");
                RegisterPacketHandler(packetHandler);
            }
        }
    }
}
