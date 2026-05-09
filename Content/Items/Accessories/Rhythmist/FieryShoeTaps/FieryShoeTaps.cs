using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.FieryShoeTaps
{
	public class FieryShoeTaps : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("5% increased rhythmic damage\nRhythmic projectiles are on fire");
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
			// RS: I DON'T THINK THAT'S 5%
			//player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicDamageMult += 0.5f;
            player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicDamageMult += 0.05f;
            player.GetModPlayer<ModPlayerRhythmistAccessories>().hasFlamingAccessory = true;
		}
	}
}
