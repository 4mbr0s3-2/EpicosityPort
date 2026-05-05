using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using System;

namespace EpicosityPort.Systems.UserInterface
{
	public class ModPlayerSpecialItemHooks : ModPlayer
	{
		// RS: Is there a better way to check whether a player has equipped or deequipped an item
		// Instead of checking every frame
		// Say... an observer observing Player.HeldItem
		public Item lastHeldItem = null;
		public bool lastHeldItemWasSpecialItem = false;
		public int lastHeldItemType = -1;
		public override void PostUpdate()
		{
			// Call every frame
			if (Player.HeldItem.ModItem is SpecialItem)
			{
				SpecialItem specialItem = Player.HeldItem.ModItem as SpecialItem;
				specialItem.PostUpdatePlayer(Player);
			}
		}
		public override void PreUpdate()
		{
			// Call every frame
			if (Player.HeldItem.ModItem is SpecialItem)
			{
				SpecialItem specialItem = Player.HeldItem.ModItem as SpecialItem;
				// RS: I'm guessing this is for when an item needs an update method when held?
				// Given that ModItems don't have an update method...
				// Ok they do have UpdateInventory
				// maybe it would've been better to just check if it was held with that...
				specialItem.PreUpdatePlayer(Player);
			}
			// If the special item is held for the first frame
			if (Player.HeldItem.ModItem is SpecialItem && Player.HeldItem != lastHeldItem)
			{
				SpecialItem specialItem = Player.HeldItem.ModItem as SpecialItem;
				// Interfaces are local
				if (Main.myPlayer == Player.whoAmI)
				{
					// Get static UIState
					Type uiStateType = specialItem.GetType().GetNestedType("SpecialItemUIState");
					// Set interface state to held item
					if (uiStateType != null)
					{
						UIState uiState = (UIState)Activator.CreateInstance(uiStateType);
						uiState.Activate();
						InterfaceHandler.Instance.interfaceItemCommonInterface.SetState(uiState);
					}
				}
				specialItem.EquipItem(Player);
			}
			// If last held item was special item and current item is not that
			if (Player.HeldItem != lastHeldItem && lastHeldItemWasSpecialItem)
			{
				(lastHeldItem.ModItem as SpecialItem).UnequipItem(Player);
			}
			// Disable interface if not holding special item
			if (!(Player.HeldItem.ModItem is SpecialItem))
			{
				if (Main.myPlayer == Player.whoAmI)
				{
					InterfaceHandler.Instance.interfaceItemCommonInterface.SetState(null);
				}
			}
			lastHeldItemType = Player.HeldItem.type;
			lastHeldItemWasSpecialItem = Player.HeldItem.ModItem is SpecialItem;
			lastHeldItem = Player.HeldItem;
		}
	}
}
