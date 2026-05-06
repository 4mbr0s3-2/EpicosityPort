using Epicosity.Items.Equipables.Accessories.Rhythmist.Fretboards;
using EpicosityPort.Content.Items.Materials.FretWire;
using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Fretboards.Ebonwood
{
	public class EbonwoodFretboard : FretboardBase
	{
		public override string FretboardType()
		{
			return "Ebonwood";
		}
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("5% decreased track speed\n15% increased rhythmic damage in Corruption");
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
			stats.rhythmicTrackSpeedMult -= 0.05f;
			stats.rhythmicDamageMult += player.ZoneCorrupt ? 0.15f : 0;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ebonwood, 50);
			recipe.AddIngredient(ModContent.ItemType<FretWire>(), 19);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
