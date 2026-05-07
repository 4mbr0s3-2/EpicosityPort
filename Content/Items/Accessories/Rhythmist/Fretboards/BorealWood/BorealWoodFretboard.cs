using EpicosityPort.Content.Items.Materials.FretWire;
using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Fretboards.BorealWood
{
	public class BorealWoodFretboard : FretboardBase
	{
		public override string FretboardType()
		{
			return "Boreal";
		}
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("10% decreased track speed\n10% decreased rhythmic damage\nRhythmic projectiles inflict slowness in Tundra");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 34;
			Item.height = 34;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 15, 0);
		}
		public override void UpdateEquip(Player player)
		{
			ModPlayerRhythmicPlayerStats stats = player.GetModPlayer<ModPlayerRhythmicPlayerStats>();
			ModPlayerRhythmistAccessories acc = player.GetModPlayer<ModPlayerRhythmistAccessories>();
			stats.rhythmicTrackSpeedMult -= 0.10f;
			stats.rhythmicDamageMult -= 0.10f;
			acc.hasFreezingAccessory |= player.ZoneSnow;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BorealWood, 50);
			recipe.AddIngredient(ModContent.ItemType<FretWire>(), 19);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
