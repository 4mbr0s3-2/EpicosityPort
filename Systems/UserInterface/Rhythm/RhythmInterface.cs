using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	public class RhythmInterface
	{
		public Terraria.UI.UserInterface RhythmUserInterface;
		public RhythmUIState RhythmUIStateInstance;

		public float chartAnimationTime = 0;
		public string fourKeySkinName = "";
		public void ShowDanceUI()
		{
			RhythmHandler.Instance.targetPosition = Main.MouseWorld;
			RhythmUserInterface?.SetState(RhythmUIStateInstance);
		}

		public void HideDanceUI()
		{
			RhythmUserInterface?.SetState(null);
		}
	}
}
