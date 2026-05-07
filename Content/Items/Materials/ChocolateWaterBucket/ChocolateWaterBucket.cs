using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Materials.ChocolateWaterBucket
{
	public class ChocolateWaterBucket : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.autoReuse = true;
            Item.material = true;
        }
    }
}
