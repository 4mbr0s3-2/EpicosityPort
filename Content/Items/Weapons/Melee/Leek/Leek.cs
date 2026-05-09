using EpicosityPort.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.Leek
{
    public class Leek : ModItem
    {
        public class SpinningLeek : ModProjectile
        {
            public override string Texture => typeof(Leek).GetQualifiedNameAsPath();
            //public override void AutoStaticDefaults()
            //{
            //    //TextureAssets.Projectile[Projectile.type].Value = ModContent.GetTexture("Epicosity/Items/Weapons/Melee/Leek");
            //    Main.projFrames[Projectile.type] = 1;
            //}
            public override void SetDefaults()
            {
                Projectile.width = 32;
                Projectile.height = 32;
                Projectile.friendly = true;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
            }
            public override void AI()
            {
                if (Projectile.owner < 0)
                {
                    return;
                }
                Player player = Main.player[Projectile.owner];
                if (Main.player[Projectile.owner].channel && Main.player[Projectile.owner].HeldItem.type == ModContent.ItemType<Leek>())
                {
                    Microsoft.Xna.Framework.Vector2 playerToMouse = Main.MouseWorld - player.MountedCenter;
                    player.bodyFrame.Y = BodyFrameCalculator.GetBodyFrameFromRotation(playerToMouse.ToRotation()) * player.bodyFrame.Height;
                    Projectile.Center = player.MountedCenter + playerToMouse.SafeNormalize(Microsoft.Xna.Framework.Vector2.Zero) * 10;
                    player.ChangeDir(playerToMouse.X < 0 ? -1 : 1);
                    Projectile.rotation += player.direction / 3.0f;
                    player.heldProj = Projectile.whoAmI;
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'Nuapurista kuulu se polokan tahti...'");
        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            //item.ranged = true;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 100;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<SpinningLeek>();
        }
    }
}
