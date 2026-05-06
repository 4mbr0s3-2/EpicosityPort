using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.MetallicShoeTaps
{
	public class MetallicShoeTaps : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("'Ready... and tap, tap, tap!'\n5% increased rhythmic damage");
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.accessory = true;
			//Item.rare = Rarity.EpicosityItemRarity.WorkInProgress;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicDamageMult += 0.05f;
		}
	}
}
