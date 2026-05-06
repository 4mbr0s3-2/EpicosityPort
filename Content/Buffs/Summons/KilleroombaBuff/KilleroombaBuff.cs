//using Epicosity.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Buffs.Summons.KilleroombaBuff
{
	public class KilleroombaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Killeroomba");
			// Description.SetDefault("The roomba with a knife duct-taped to it will somehow fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			//if (player.ownedProjectileCounts[ModContent.ProjectileType<KilleroombaMinion>()] > 0)
			//{
			//	player.buffTime[buffIndex] = 18000;
			//}
			//else
			//{
			//	player.DelBuff(buffIndex);
			//	buffIndex--;
			//}
		}
	}
}
