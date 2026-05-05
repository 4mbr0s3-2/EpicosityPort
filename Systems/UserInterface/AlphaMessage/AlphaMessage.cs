using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface.AlphaMessage
{
	public class AlphaMessage
	{
		public static bool dontShowAgainConfig = false;
		public static bool hasSeenThisSession = false;
		public static Terraria.UI.UserInterface AlphaMessageInterface;
		public class AlphaMessageUI : UIState
		{
			float topOffset = 20;
			float messagePanelHeight = Main.screenHeight - 200;
			public UIText AppendText(string text, float textScale = -1, bool large = false)
			{
				UIText uIText = new UIText(text, textScale, large);
				uIText.PaddingTop = topOffset;
				topOffset += (messagePanelHeight - 30) / 9f;
				uIText.HAlign = 0.5f;
				return uIText;
			}
			public override void OnInitialize()
			{
				UIPanel panel = new UIPanel();
				panel.HAlign = 0.5f;
				panel.VAlign = 0.5f;
				panel.Width.Set(Main.screenWidth - 200, 0);
				panel.Height.Set(messagePanelHeight, 0);
				Append(panel);

				panel.Append(AppendText("Welcome to Epicosity!", 1, true));
				panel.Append(AppendText("This is a sandbox build for testing and debugging.", 1));
				panel.Append(AppendText("This is not an official demo.", 1));
				panel.Append(AppendText("Some sprites and music are placeholders.", 1));
				panel.Append(AppendText("There is no way to win yet.", 1));
				panel.Append(AppendText("A great number of features are missing or unfinished.", 1));
				panel.Append(AppendText("Yes, this message is a reference to Yandere Simulator.", 1));
				panel.Append(AppendText("Enable this message again in the mod configuration.", 1));
				panel.Append(AppendText("Double-click to close this message", 0.7f, true));

				panel.OnLeftDoubleClick += AlphaMessageUI_OnDoubleClick;
			}

			public override void Update(GameTime gameTime)
			{
				base.Update(gameTime);
			}
			private void AlphaMessageUI_OnDoubleClick(UIMouseEvent evt, UIElement listeningElement)
			{
				EpicosityConfigClient.Instance.DisableDisclaimer = true;
				EpicosityConfigClient.Instance.Save();
				hasSeenThisSession = true;
				AlphaMessageInterface.SetState(null);
			}
		}
	}
	public class ModPlayerAlphaMessageWorld : ModPlayer
	{
		public override void PostUpdate()
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				if (!AlphaMessage.hasSeenThisSession && AlphaMessage.AlphaMessageInterface.CurrentState == null && !AlphaMessage.dontShowAgainConfig)
				{
					AlphaMessage.AlphaMessageInterface.SetState(new AlphaMessage.AlphaMessageUI());
				}
			}
		}
	}
}
