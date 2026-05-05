using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Tiles.Ores.MemeiteOre
{
    public class MemeiteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Memeite Ore");
            // Tooltip.SetDefault("'Although bland in color, this ore holds potential for everything'");
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<MemeiteOreTile>();
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
        }
    }
    class MemeiteOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            HitSound = SoundID.Tink;
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Memeite Ore");
            AddMapEntry(new Microsoft.Xna.Framework.Color(40, 40, 40), name);
        }
    }
}
