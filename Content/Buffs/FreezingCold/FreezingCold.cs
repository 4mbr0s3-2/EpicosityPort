using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Buffs.FreezingCold
{
	public class FreezingCold : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Freezing Cold");
			// Description.SetDefault("Movement speed is reduced");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed *= 0.8f;
		}
	}
}
