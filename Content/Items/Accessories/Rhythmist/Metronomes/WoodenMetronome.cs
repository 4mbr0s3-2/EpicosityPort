using Epicosity;
using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.UserInterface.Rhythm.Metronome;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Metronomes
{
	public class WoodenMetronome : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 100);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ModPlayerRhythmistAccessories>().hasMetronome = true;
			MetronomeInterface.Instance.MetronomeUIStateInstance.UpdateMetronome(typeof(WoodenMetronomeVisual));
		}
	}
}
