using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Fretboards
{
	public abstract class FretboardBase : ModItem
	{
		public override void SetDefaults()
		{
			Item.accessory = true;
		}
		public abstract string FretboardType();
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ModPlayerRhythmistAccessories acc = player.GetModPlayer<ModPlayerRhythmistAccessories>();
			acc.fretboardType = FretboardType();
		}
		//public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */
		//{
		//	ModPlayerRhythmistAccessories rhythm = player.GetModPlayer<ModPlayerRhythmistAccessories>();
		//	return rhythm.fretboardType == "";
		//}

		// RS: Thank you tModPorter
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
			return equippedItem.ModItem is not FretboardBase || incomingItem.ModItem is not FretboardBase;
        }
	}
}
