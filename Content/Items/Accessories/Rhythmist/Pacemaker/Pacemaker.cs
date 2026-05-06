using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Epicosity.Items.Equipables.Accessories.Rhythmist
{
	public class Pacemaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Increases health on successful hit object hits");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.accessory = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 2, 0, 0);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ModPlayerRhythmistAccessories>().hasPacemaker = true;
		}
	}
}
