using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.RhythmistEmblem
{
	public class RhythmistEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("15% increased rhythmic damage");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			//Item.rare = Items.Rarity.EpicosityItemRarity.WorkInProgress;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicDamageMult += 0.15f;
		}
	}
}
