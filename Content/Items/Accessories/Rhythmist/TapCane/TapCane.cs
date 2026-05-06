using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.TapCane
{
	public class TapCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("5% increased rhythmic damage\n15% increased note rate");
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.accessory = true;
			//Item.rare = Items.Rarity.EpicosityItemRarity.WorkInProgress;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicDamageMult += 0.05f;
			player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicNoteRateMult += 0.15f;
		}
	}
}
