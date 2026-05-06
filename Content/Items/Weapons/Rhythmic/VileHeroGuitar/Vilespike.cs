using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.VileHeroGuitar
{
	public class Vilespike : RhythmicProjectile
	{
		public override string Texture => $"Terraria/Projectile_{ProjectileID.VilethornTip}";
		public float Trailing
		{
			get
			{
				return Projectile.ai[0];
			}
			set
			{
				Projectile.ai[0] = value;
			}
		}
		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 3;
		}
		public override void AI()
		{
			if (Trailing != 1 && Projectile.owner == Main.myPlayer && Projectile.timeLeft % 2 == 0)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.1f, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.timeLeft < 30)
			{
				Projectile.alpha = (int)(255 * (1f - (Projectile.timeLeft / 30f)));
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			Vector2 origin = new Vector2(16, 16);
			Main.EntitySpriteDraw(
				new DrawData(
					texture,
					new Rectangle((int)Projectile.position.X - (int)Main.screenPosition.X + (int)origin.X, (int)Projectile.position.Y - (int)Main.screenPosition.Y + (int)origin.Y, (int)(32 * Projectile.timeLeft / 60f), (int)(32 * (1f - Projectile.timeLeft / 60f))),
					null,
					lightColor * (1f - Projectile.alpha / 255f),
					Projectile.rotation,
					origin,
					SpriteEffects.None,
					0
				));
			return false;
		}
	}
}
