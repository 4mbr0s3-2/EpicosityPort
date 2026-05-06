using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Buffs.Despacito
{
	public class Despacito : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Despacito");
            // Description.SetDefault("Se reduce la velocidad de movimiento");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed /= 2f;
        }
    }
}
