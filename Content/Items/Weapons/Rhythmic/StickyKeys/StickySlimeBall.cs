//using Epicosity.Buffs;
using EpicosityPort.Content.Buffs.StickySlimed;
using EpicosityPort.Content.Items.Weapons.Rhythmic.GoldDancePad;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.StickyKeys
{
	public class StickySlimeBall : GoldPellet 
	{
		public override void OnKill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int dust = 0; dust < 5; dust++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime, 0, 0, 0, new Color(0x52, 0x88, 0xFE, 128));
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Main.EntitySpriteDraw
			(
				new DrawData(
					ModContent.Request<Texture2D>(Texture).Value,
					new Rectangle((int)Projectile.position.X - (int)Main.screenPosition.X, (int)Projectile.position.Y - (int)Main.screenPosition.Y, 12, 10),
					null,
					lightColor,
					Projectile.rotation,
					new Vector2(6, 5),
					SpriteEffects.None,
					0)
			);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<StickySlimed>(), 180);
		}
	}
}
