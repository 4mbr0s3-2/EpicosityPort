using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EpicosityPort.Content.Buffs.Summons.BaldEagle
{
	public class BaldEagleBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bald Eagle Minion");
			// Description.SetDefault("The bald eagle will fight for you (and for 'Murica!)\nIt's totally not a red-tailed hawk in disguise...");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//if (player.ownedProjectileCounts[ProjectileType<BaldEagleMinion>()] > 0)
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
