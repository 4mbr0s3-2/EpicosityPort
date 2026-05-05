using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.WeaponizedChairs
{
    public class Splinter : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //projectile.damage = 20;
        }
        public override void AI()
        {
            //projectile.ranged = true;
            ////projectile.damage = 20;
            //projectile.friendly = true;
            //projectile.hostile = false;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
