using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist
{
	public class AccuracyTracker : ModItem
	{
		public override string Texture => PathConstants.PlaceholderTexture;
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			var p = player.GetModPlayer<ModPlayerRhythmistAccessories>();
			p.hasAccuracyTracker = true;
		}
	}
}
