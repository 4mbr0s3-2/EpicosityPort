using EpicosityPort.Systems.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EpicosityPort.Systems.UserInterface.ChoreabilitySlots
{
    public class ChoreabilitySlotsPlayer : ModPlayer
    {
        public static readonly int NUM_OF_CHOREABILITY_SLOTS = 5;
        public List<Item> choreabilitySlotsInventory = new List<Item>();
        internal class ChoreabilitySlotsSyncHandler : PacketHandler
        {
            public const byte SyncAllSlots = 1;

            public override void HandlePacket(BinaryReader reader, int fromWho)
            {
                switch (reader.ReadByte())
                {
                    case (SyncAllSlots):
                        ReceiveSlots(reader, fromWho);
                        break;
                }
            }

            public void SendSlots
            (
                int toWho,
                int fromWho,
                List<Item> choreabilitySlotsInventory
            )
            {
                //ModPacket packet = GetPacket(SyncTarget, fromWho);
                //for (int x = 0; x < NUM_OF_CHOREABILITY_SLOTS; x++)
                //{
                //    Item slot = choreabilitySlotsInventory.ElementAt(x);
                //    Main.NewText($"Wrote {slot.Name}");
                //packet.WriteItem(slot);
                //}
                //packet.Send(toWho, fromWho);
            }

            public void ReceiveSlots(BinaryReader reader, int fromWho)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    fromWho = reader.ReadByte(); // fromWho sent from server
                }
                ChoreabilitySlotsPlayer choreabilitySlotsPlayer = Main.player[fromWho].GetModPlayer<ChoreabilitySlotsPlayer>();
                List<Item> choreabilitySlotsInventory = new List<Item>();
                for (int x = 0; x < NUM_OF_CHOREABILITY_SLOTS; x++)
                {
                    //Item slotItem = reader.ReadItem();
                    //if (slotItem != null)
                    //{
                    //    choreabilitySlotsInventory.Add(slotItem);
                    //}
                    //else
                    //{
                    //    Main.NewText($"Unable to read slot");
                    //}
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    //SendTarget(-1, fromWho, choreabilitySlotsInventory);
                }
                else
                {
                    Main.NewText($"Received {fromWho}'s changed slots!");
                    choreabilitySlotsPlayer.choreabilitySlotsInventory = choreabilitySlotsInventory;
                }
            }
        }
        public override void PreUpdate()
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                choreabilitySlotsInventory.Clear();
                foreach (VanillaItemSlotWrapper slot in ((ChoreabilitySlotsInventory.ChoreabilityInventoryUI)ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton).vanillaItemSlotWrappers)
                {
                    choreabilitySlotsInventory.Add(slot.Item);
                }
            }
        }
        public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
        {
            ChoreabilitySlotsPlayer clone = clientClone as ChoreabilitySlotsPlayer;
            List<Item> clonedSlots = new List<Item>();
            foreach (Item slot in choreabilitySlotsInventory)
            {
                clonedSlots.Add(slot);
            }
            clone.choreabilitySlotsInventory = clonedSlots; // Clone list to prevent equality from passing by reference
        }
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ChoreabilitySlotsPlayer clone = clientPlayer as ChoreabilitySlotsPlayer;
            bool inequal = false;
            for (int slot = 0; slot < clone.choreabilitySlotsInventory.Count; slot++)
            {
                if (clone.choreabilitySlotsInventory[slot].type != choreabilitySlotsInventory[slot].type)
                {
                    inequal = true;
                    break;
                }
            }
            if (inequal)
            {
                Main.NewText($"Syncing {Player.whoAmI}'s changed choreability slots!");
                ModNetHandler.GetPacketHandler<ChoreabilitySlotsSyncHandler>().SendSlots(-1, Player.whoAmI, choreabilitySlotsInventory);
            }
        }
        public override void SaveData(TagCompound tag)/* tModPorter Suggestion: Edit tag parameter instead of returning new TagCompound */
        {
            for (int slotIndex = 0; slotIndex < choreabilitySlotsInventory.Count(); slotIndex++)
            {
                tag.Add($"ChoreabilitySlot{slotIndex}", choreabilitySlotsInventory[slotIndex]);
            }
            //return save;
        }
        public override void LoadData(TagCompound tag)
        {
            for (int slotIndex = 0; slotIndex < NUM_OF_CHOREABILITY_SLOTS; slotIndex++)
            {
                ((ChoreabilitySlotsInventory.ChoreabilityInventoryUI)ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton).vanillaItemSlotWrappers[slotIndex].Item = tag.Get<Item>($"ChoreabilitySlot{slotIndex}");
            }
        }
    }
}
