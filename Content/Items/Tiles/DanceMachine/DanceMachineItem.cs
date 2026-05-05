using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Tiles.DanceMachine
{
    public class DanceMachineItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dance Machine");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.createTile = ModContent.TileType<DanceMachine>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.maxStack = 99;
        }
        public override void AddRecipes()
        {
            // TODO: Recipe group if necessary
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.GoldBar, 10);
            recipe.AddIngredient(ItemID.RedPressurePlate, 4);
            recipe.AddIngredient(ItemID.BluePressurePlate, 4);
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.Switch, 10);
            recipe.AddIngredient(ItemID.WirePipe, 10);
            recipe.AddIngredient(ItemID.WireBulb, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.PlatinumBar, 10);
            recipe.AddIngredient(ItemID.RedPressurePlate, 4);
            recipe.AddIngredient(ItemID.BluePressurePlate, 4);
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.Switch, 10);
            recipe.AddIngredient(ItemID.WirePipe, 10);
            recipe.AddIngredient(ItemID.WireBulb, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
