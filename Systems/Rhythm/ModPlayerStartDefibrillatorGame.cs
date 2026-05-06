using EpicosityPort.Systems.UserInterface.Defibrillator;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Rhythm
{
	public class ModPlayerStartDefibrillatorGame : ModPlayer
	{
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if (Main.myPlayer == Player.whoAmI) // Only call for the local player
			{
				ModPlayerRhythmistAccessories modPlayerRhythmistAccessories = Player.GetModPlayer<ModPlayerRhythmistAccessories>();
				if (modPlayerRhythmistAccessories.hasDefibrillatorPack)
				{
					DefibrillatorGame.GameInterface.SetState(new DefibrillatorGame.GameUIStateInstance());
				}
			}
		}
	}
}
