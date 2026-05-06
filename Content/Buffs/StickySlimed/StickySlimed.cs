using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Buffs.StickySlimed
{
	public class StickySlimed : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sticky Slime'd");
			// Description.SetDefault("You trudge in slime");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed *= 0.8f;
		}
	}
}
