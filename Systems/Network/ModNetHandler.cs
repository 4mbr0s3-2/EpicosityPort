using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Network
{
    /// <summary>
    /// This class handles all network packets.
    /// </summary>
    //internal class ModNetHandler
    internal class ModNetHandler : ModSystem
    {
        // When a lot of handlers are added, it might be wise to automate
        // creation of them
        // Haha I did exactly that

        //public static byte currentPacketHandler = 0;

        // RS: Apparently, I didn't know anything about OOP and just relied on putting static fields everywhere...
        // Not to mention this great data structure choice

        //public static List<PacketHandler> PacketHandlers;
        private Dictionary<int, Type> PacketHandlerTypeByID;
        private Dictionary<Type, PacketHandler> PacketHandlersByType;

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
            PacketHandlerTypeByID = new Dictionary<int, Type>();
            PacketHandlersByType = new Dictionary<Type, PacketHandler>();
        }

        public static ModNetHandler Instance
        {
            get { return ModContent.GetInstance<ModNetHandler>(); }
        }

        public void HandlePacket(BinaryReader r, int fromWho)
        {
            int packetHandlerType = r.ReadInt32();
            //bool success = false;
            // RS: Gross!! Use a hashmap buddy
            //foreach (PacketHandler packetHandler in PacketHandlers)
            //{
            //    if (packetHandlerType == packetHandler.HandlerType)
            //    {
            //        packetHandler.HandlePacket(r, fromWho);
            //        success = true;
            //        //Terraria.Main.NewText($"Successfully handled handleType {packetHandlerType} ({packetHandler.GetType().Name})!");
            //        break;
            //    }
            //}

            if (!PacketHandlerTypeByID.ContainsKey(packetHandlerType)) {
                Mod.Logger.Error($"Couldn't identify packet of handlerType {packetHandlerType}!");
                return;
            }

            PacketHandlerTypeByID.TryGetValue(packetHandlerType, out var handlerType);
            int hashedPacketHandlerType = r.ReadInt32();
            if (handlerType.Name.GetHashCode() != hashedPacketHandlerType)
            {
                Mod.Logger.Error($"PacketHandler mismatch! Wrongly assumed handlerType {handlerType.Name}!");
                return;
            }
            PacketHandlersByType.TryGetValue(handlerType, out var packetHandler);

            packetHandler.HandlePacket(r, fromWho);

            //if (!success)
            //{
            //    //Terraria.Main.NewText($"Couldn't identify packet of handlerType {packetHandlerType}!");
            //    Mod.Logger.Error($"Couldn't identify packet of handlerType {packetHandlerType}!");
            //}
        }
        /// <summary>
        ///  Gets the specified packet handler from the PacketHandler list. Returns null if no such handler could be found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetPacketHandler<T>() where T : PacketHandler
        {
            //foreach (PacketHandler packetHandler in PacketHandlers)
            //{
            //    if (packetHandler.GetType().Equals(typeof(T)))
            //    {
            //        return (T)packetHandler;
            //    }
            //}
            bool success = Instance.PacketHandlersByType.TryGetValue(typeof(T), out PacketHandler value);
            if (!success) { return null; }
            return (T)value;
        }
        //public void RegisterPacketHandler<T>() where T : PacketHandler
        //{
        //    RegisterPacketHandler(typeof(T));
        //}

        // RS: Used by PacketHandler on load
        public int RegisterPacketHandler(PacketHandler p)
        {
            //if (type.IsSubclassOf(typeof(PacketHandler)))
            //{
            //    PacketHandlers.Add((PacketHandler)Activator.CreateInstance(type, currentPacketHandler));
            //    currentPacketHandler++;
            //}
            int id = PacketHandlerTypeByID.Count;
            PacketHandlerTypeByID.Add(id, p.GetType());
            PacketHandlersByType.Add(p.GetType(), p);
            return id;
        }
        //public override void OnModLoad()
        //{
        //    InitializePacketHandlers();
        //}
        //public void InitializePacketHandlers()
        //{
        //    Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();

        //    // Order packet handlers by name in order to prevent any kind of desync between clients that load the types in a different order
        //    // RS: ?? I thought it'd be deterministic? whatever
        //    IEnumerable<Type> packetHandlers = allTypes.Where((type) => type.BaseType == typeof(PacketHandler)).OrderBy((type) => type.Name);

        //    // RS: Y'know what'd be easier? Have PacketHandler implement ILoadable
        //    // then just have em load through that
        //    // or have em load ModSystem so OnModLoad can be called after ModNetHandler is ready
        //    //foreach (Type packetHandler in packetHandlers)
        //    //{
        //    //EpicosityPort.Instance.Logger.Info($"Loaded {packetHandler.Name}");
        //    //RegisterPacketHandler(packetHandler);
        //    //}
        //}
    }
}
