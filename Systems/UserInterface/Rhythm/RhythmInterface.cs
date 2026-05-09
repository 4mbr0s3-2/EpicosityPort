using EpicosityPort.Systems.Rhythm;
using System;
using Terraria;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
    public class RhythmInterface : IDisposable
	{
		public Terraria.UI.UserInterface RhythmUserInterface;
		public RhythmUIState RhythmUIStateInstance;

		public float chartAnimationTime = 0;
		public string fourKeySkinName = "";
		public void ShowDanceUI()
		{
			//RhythmHandler.Instance.targetPosition = Main.MouseWorld;
			RhythmUserInterface?.SetState(RhythmUIStateInstance);
		}

		public void HideDanceUI()
		{
			RhythmUserInterface?.SetState(null);
		}

        public void Dispose()
        {
            RhythmUIStateInstance.Dispose();
        }
    }
}
