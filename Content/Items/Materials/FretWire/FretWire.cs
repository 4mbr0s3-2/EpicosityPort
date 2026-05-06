using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Materials.FretWire
{
	// RS: Y'know what's funny
	// You need fretwire but not guitar strings
	// This is accurate to Guitar Hero
	public class FretWire : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 12;
			Item.material = true;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 1, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddRecipeGroup("IronBar", 1);
			recipe.Register();
		}
	}
}