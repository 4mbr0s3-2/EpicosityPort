using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Tiles.Ores.MemeiteBar
{
    public class MemeiteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Memeite Bar");
            // Tooltip.SetDefault("'Although bland in color, this bar holds potential for everything'");
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<MemeiteBarTile>();
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<MemeiteOre.MemeiteOre>(), 10);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
    class MemeiteBarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileShine[Type] = 1100; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileFrameImportant[Type] = true;
            HitSound = SoundID.Tink;
            AddMapEntry(new Microsoft.Xna.Framework.Color(40, 40, 40));
        }
    }
}
