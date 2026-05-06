using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.WoodenDancePad
{
    public class WoodenPellet : RhythmicProjectile
    {
        bool hasHit = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
        }
        public override void AI()
        {
            if (hasHit)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.aiStyle = -1;
                Projectile.scale += 0.02f;
                Projectile.alpha += 20;
                if (Projectile.alpha >= 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }
            if ((Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height) - Projectile.velocity) != Vector2.Zero && Projectile.timeLeft < 60)
            {
                hasHit = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hasHit = true;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hasHit = true;
        }
        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        //{
        //    hasHit = true;
        //}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            hasHit = true;
        }
    }
}
