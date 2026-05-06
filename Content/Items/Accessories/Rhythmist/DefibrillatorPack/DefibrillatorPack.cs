using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.DefibrillatorPack
{
	// RS: Talking about Rhythm Doctor
	// Shoutouts to huantian btw he's so cool

	//[GoogleSheetsAPI.EpicositySheet.SheetStats(GoogleSheetsAPI.EpicositySheet.Status.Done, "Homage to Rhythm Doctor by 7th Beat Games")]
	public class DefibrillatorPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rhythm Defibrillator");
			// Tooltip.SetDefault("Allows for a faster respawn on death\nPress space on the 7th beat\nCourtesy of Middlesea Hospital");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 36;
			Item.accessory = true;
			Item.rare = ItemRarityID.Orange;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ModPlayerRhythmistAccessories>().hasDefibrillatorPack = true;
		}
	}
}
