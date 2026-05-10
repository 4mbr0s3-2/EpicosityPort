using EpicosityPort.Content.Items.Tiles.Ores.MemeiteBar;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.ComicallyLargeSpoon
{
    public class ComicallyLargeSpoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Comically Large Spoon");
            // Tooltip.SetDefault("'Only a spoonful!'");
        }
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.width = 80;
            Item.height = 80;
            Item.damage = 28;
            Item.scale = 1f;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.MountedCenter + new Vector2(-10 * player.direction, 20) * new Vector2(1, player.gravDir);
            player.itemRotation = 0;
        }
        public override void HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 4;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.MountedCenter + new Vector2(-12 * player.direction, 12).RotatedBy(player.itemRotation);
            if (player.altFunctionUse == 2 && player.itemAnimation == 5 && player.whoAmI == Main.myPlayer)
            {
                Vector2 hitDirection = Main.MouseWorld - player.Center;
                hitDirection = hitDirection.SafeNormalize(Vector2.Zero);
                bool hit = false;
                player.ChangeDir(hitDirection.X < 0 ? -1 : 1);
                for (int i = 0; i < 5; i++)
                {
                    Point pos = ((player.Center + hitDirection * 16 * i) / 16).ToPoint();
                    Tile t = Framing.GetTileSafely(player.Center + hitDirection * 16 * i);
                    if (t.HasTile && Main.tileSolid[t.TileType])
                    {
                        hit = true;
                        Terraria.WorldGen.KillTile_MakeTileDust(pos.X, pos.Y, t);
                    }
                }
                if (hit)
                {
                    SoundEngine.PlaySound(SoundID.Dig);
                    player.velocity += -hitDirection * 7;
                }
            }
        }
        public override Vector2? HoldoutOrigin()
        {
            return null;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        // TODO: Add Recipe
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("IronBar", 20);
            recipe.AddIngredient(ModContent.ItemType<MemeiteBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
