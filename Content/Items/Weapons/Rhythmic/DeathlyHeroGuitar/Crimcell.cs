//using Epicosity.Projectiles.Weapons.Ranged;
using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;
// RS: WHY IS STEAMWORKS HERE
//using Steamworks;
using Terraria;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.DeathlyHeroGuitar
{
	public class Crimcell : RhythmicProjectile
	{
		bool shooting = false;
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
		}
		public override void AI()
		{
			// Slow down
			if (Projectile.velocity.Length() > 0.3f && !shooting)
			{
				Projectile.velocity *= 0.9f;
			}
			else
			{
				shooting = true;
				// Has target
				Vector2 diff;
				if (targetNPCWhoAmI >= 0)
				{
					diff = (Main.npc[targetNPCWhoAmI].Center - Projectile.Center);
				}
				else
				{
					diff = (targetPosition - Projectile.Center);
				}
				Projectile.velocity += diff.SafeNormalize(Vector2.One) * 0.3f;
				if (diff.Length() < 10)
				{
					Projectile.Kill();
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.SomethingRed, Projectile.velocity.X, Projectile.velocity.Y)];
				dust.noGravity = true;
			}
		}
	}
}
