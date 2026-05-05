using EpicosityPort.Utilities;
using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.DiamondSwordSword
{
    public class DiamondSwordSword : ModItem
    {
        public class DiamondSwordSwordProjectile : ModProjectile
        {
            public override string Texture => typeof(DiamondSwordSword).GetQualifiedNameAsPath();
            public int time = 0;
            public override void SetDefaults()
            {
                Projectile.friendly = true;
                Projectile.aiStyle = 0;
                Projectile.alpha = 255;
                Projectile.width = 80;
                Projectile.height = 80;
                Projectile.tileCollide = false;
                Projectile.penetrate = 5;
            }
            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                time = 60;
            }
            public override Color? GetAlpha(Color lightColor)
            {
                return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
            }
            public override void AI()
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (Main.rand.Next(0, 5) == 0)
                    {
                        int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt);
                        Main.dust[dustIndex].color = new Color(95, 104, 242);
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
                Projectile.rotation += 0.3f * Projectile.velocity.X / Math.Abs(Projectile.velocity.X);
                time++;
                if (time > 60)
                {
                    Projectile.alpha += 10;
                    if (Projectile.alpha >= 255)
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.alpha -= 20;
                    if (Projectile.alpha < 0)
                    {
                        Projectile.alpha = 0;
                    }
                }
            }
            public override void OnKill(int timeLeft)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, Projectile.velocity.X / 2, Projectile.velocity.Y / 2);
                        Main.dust[dustIndex].color = new Color(95, 104, 242);
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
            }
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Diamond Sword Sword");
            // Tooltip.SetDefault("'I have a patent!'");
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.Y = player.Center.Y;
            player.itemLocation.X = player.Center.X - 3f * player.direction;
        }
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.width = 84;
            Item.height = 84;
            Item.damage = 60;
            Item.scale = 1f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.shoot = ModContent.ProjectileType<DiamondSwordSwordProjectile>();
            Item.shootSpeed = 15;
            Item.UseSound = SoundID.Item1;
        }
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LargeDiamond, 3);
            recipe.AddIngredient(ItemID.Wood, 200);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
