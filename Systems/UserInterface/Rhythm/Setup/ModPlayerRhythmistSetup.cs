using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.UserInterface.Rhythm.Setup
{
    public class ModPlayerRhythmistSetup : ModPlayer
	{
		public override void PostUpdate()
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				if (Player.HeldItem.ModItem is RhythmicWeaponBase
					&& !RhythmistSetup.Instance.hasSeenThisSession
					&& RhythmistSetup.Instance.RhythmistSetupInterface.CurrentState == null
					&& !RhythmistSetup.Instance.dontShowAgainConfig)
				{
					RhythmistSetup.Instance.RhythmistSetupInterface.SetState(new RhythmistSetup.UIRhythmistSetup());
				}
			}
		}
	}
}
