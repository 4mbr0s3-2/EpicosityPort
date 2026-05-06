using EpicosityPort.Content.Items.Weapons.Rhythmic.GoldDancePad;
using Terraria;
using Terraria.ID;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.PlatinumDancePad
{
	public class PlatinumPellet : GoldPellet 
	{
		public override void OnKill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int dust = 0; dust < 5; dust++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
				}
			}
		}
	}
}
