using Epicosity.Items.Equipables.Accessories.Rhythmist.Fretboards;
using EpicosityPort.Content.Items.Materials.FretWire;
using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Fretboards.RichMahogany
{
	public class RichMahoganyFretboard : FretboardBase
	{
		public override string FretboardType()
		{
			return "RichMahogany";
		}
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("5% decreased track speed\n15% increased rhythmic damage in Jungle");
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
			stats.rhythmicDamageMult += player.ZoneJungle ? 0.15f : 0;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.RichMahogany, 50);
			recipe.AddIngredient(ModContent.ItemType<FretWire>(), 19);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
