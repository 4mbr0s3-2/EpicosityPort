using EpicosityPort;
using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface.Rhythm.Setup
{
	public class RhythmistSetup : ModSystem
	{
		public static RhythmistSetup Instance { get
			{
				return ModContent.GetInstance<RhythmistSetup>();
			}
		}
		public bool dontShowAgainConfig { 
			get 
			{ 
				return EpicosityConfigClient.Instance.DisableRhythmistSetup;
			}
			set
			{
				EpicosityConfigClient.Instance.DisableRhythmistSetup = value;

            }
		}
		public bool hasSeenThisSession = false;
		public Terraria.UI.UserInterface RhythmistSetupInterface;
		public class UIRhythmistSetup : UIState
		{
			string mouseText = "";
			public enum ControlsChoice
			{
				None,
				ArrowKeys,
				HomeRowKeys,
				Custom
			}
			public enum TrackChoice
			{
				None,
				Crosshair,
				Player
			}
			ControlsChoice controlsChoice = ControlsChoice.None;
			TrackChoice trackChoice = TrackChoice.None;
			Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
			public override void OnInitialize()
			{
				string[] imageNames =
				{
					"Background",
					"ControlsArrowKeys",
					"ControlsCustom",
					"ControlsHomeRowKeys",
					"Done",
					"Skip",
					"Up",
					"Down",
					"Instructions",
					"TrackAtCrosshair",
					"TrackAtPlayer"
				};
				foreach (string name in imageNames)
				{
					textures.Add(name, ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/{name}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
				}
			}
			public override void Draw(SpriteBatch spriteBatch)
			{
				mouseText = "";
				Texture2D background = textures["Background"];
				float sizeScalar = Main.screenWidth / 1920f;
				Point size = new Point((int)(background.Width * sizeScalar), (int)(background.Height * sizeScalar));
				Rectangle backgroundRect = new Rectangle((int)(Main.screenWidth / 2f - background.Size().X / 2f), (int)(Main.screenHeight / 2f - background.Size().Y / 2f), size.X, size.Y);
				Main.LocalPlayer.mouseInterface = true;
				#region Background
				spriteBatch.Draw(background,
					 new Rectangle((int)(Main.screenWidth / 2f), (int)(Main.screenHeight / 2f), size.X, size.Y),
					 null,
					 Color.White,
					 0,
					 background.Size() / 2f,
					 SpriteEffects.None,
					 0);
				Texture2D instructions = textures["Instructions"];
				spriteBatch.Draw(instructions,
					 new Rectangle((int)(Main.screenWidth / 2f), (int)(Main.screenHeight / 2f), size.X, size.Y),
					 null,
					 Color.White,
					 0,
					 instructions.Size() / 2f,
					 SpriteEffects.None,
					 0);
				#endregion
				DrawControlsButtons(spriteBatch, background, sizeScalar);
				DrawTrackPositionButtons(spriteBatch, background, sizeScalar);
				DrawBottomButtons(spriteBatch, background, sizeScalar);
				Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow
				(
					spriteBatch,
					FontAssets.MouseText.Value,
					mouseText,
					Main.MouseScreen + new Vector2(16, 10),
					Color.White,
					0,
					Vector2.Zero,
					Vector2.One
				);
			}

			public void DrawBottomButtons(SpriteBatch spriteBatch, Texture2D background, float sizeScalar)
			{
				Rectangle Scale(Rectangle rect, float scale)
				{
					return new Rectangle(rect.X, rect.Y, (int)(rect.Width * scale), (int)(rect.Height * scale));
				}
				Point size = new Point((int)(background.Width * sizeScalar), (int)(background.Height * sizeScalar));
				Vector2 topLeft = new Vector2(Main.screenWidth - size.X, Main.screenHeight - size.Y) / 2f;
				Texture2D done = textures["Done"];
				Vector2 doneOffset = new Vector2(770, 525) * sizeScalar;
				Rectangle doneRect = new Rectangle((int)(topLeft.X + doneOffset.X), (int)(topLeft.Y + doneOffset.Y), (int)(done.Width * sizeScalar), (int)(done.Height * sizeScalar));
				Vector2 doneOrig = done.Size() / 2f;
				bool doneHover = doneRect.Contains((Main.MouseScreen + doneOrig * sizeScalar).ToPoint());
				bool doneVis = controlsChoice != ControlsChoice.None && trackChoice != TrackChoice.None;
				spriteBatch.Draw(done,
					 Scale(doneRect, doneHover ? 1.1f : 1f),
					 null,
					 Color.White * (doneVis ? 1 : 0.3f),
					 0,
					 doneOrig,
					 SpriteEffects.None,
					 0);
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (doneHover && doneVis)
					{
						try
						{
							Dictionary<string, List<string>> keyStatus = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus;
							var leftKey = keyStatus[$"{typeof(EpicosityPort).Name}/Rhythm Button 1"];
							var downKey = keyStatus[$"{typeof(EpicosityPort).Name}/Rhythm Button 2"];
							var upKey = keyStatus[$"{typeof(EpicosityPort).Name}/Rhythm Button 3"];
							var rightKey = keyStatus[$"{typeof(EpicosityPort).Name}/Rhythm Button 4"];
							var switchKey = keyStatus[$"{typeof(EpicosityPort).Name}/Rhythm Switch Target (Left)"];
							var switchKey2 = keyStatus[$"{typeof(EpicosityPort).Name}/Rhythm Switch Target (Right)"];
							switch (controlsChoice)
							{
								case ControlsChoice.ArrowKeys:
									leftKey.Clear();
									upKey.Clear();
									rightKey.Clear();
									downKey.Clear();
									leftKey.Add("Left");
									upKey.Add("Up");
									rightKey.Add("Right");
									downKey.Add("Down");
									break;
								case ControlsChoice.HomeRowKeys:
									leftKey.Clear();
									upKey.Clear();
									rightKey.Clear();
									downKey.Clear();
									leftKey.Add("K");
									downKey.Add("L");
									upKey.Add("OemSemicolon");
									rightKey.Add("OemQuotes");
									break;
							}
							switchKey.Clear();
							switchKey.Add("LeftAlt");
							switchKey2.Clear();
							switchKey2.Add("RightAlt");
							EpicosityConfigClient.Instance.FKGHPlayfieldAbovePlayer = trackChoice == TrackChoice.Player;
							EpicosityConfigClient.Instance.DisableRhythmistSetup = true;
							EpicosityConfigClient.Instance.OnChanged();
							EpicosityConfigClient.Instance.Save();
							Instance.hasSeenThisSession = true;
                            Instance.RhythmistSetupInterface.SetState(null);
							Main.NewText("Controls can be changed in the Settings Menu. Preferences can be changed in the Epicosity Client Config.");
						}
						catch { }
					}
				}
			}

			public void DrawControlsButtons(SpriteBatch spriteBatch, Texture2D background, float sizeScalar)
			{
				Rectangle Scale(Rectangle rect, float scale)
				{
					return new Rectangle(rect.X, rect.Y, (int)(rect.Width * scale), (int)(rect.Height * scale));
				}
				Point size = new Point((int)(background.Width * sizeScalar), (int)(background.Height * sizeScalar));
				Vector2 topLeft = new Vector2(Main.screenWidth - size.X, Main.screenHeight - size.Y) / 2f;
				Texture2D arrowKeys = textures["ControlsArrowKeys"];
				Vector2 arrowKeysOffset = new Vector2(196, 230) * sizeScalar;
				Vector2 arrowKeysOrigin = arrowKeys.Size() / 2f;
				Rectangle arrowKeysDestRectangle = new Rectangle((int)(topLeft.X + arrowKeysOffset.X), (int)(topLeft.Y + arrowKeysOffset.Y), (int)(arrowKeys.Width * sizeScalar), (int)(arrowKeys.Height * sizeScalar));
				bool arrowKeysHover = arrowKeysDestRectangle.Contains((Main.MouseScreen + arrowKeysOrigin * sizeScalar).ToPoint());
				bool arrowKeysVis = arrowKeysHover || controlsChoice == ControlsChoice.ArrowKeys;
				if (arrowKeysHover)
				{
					mouseText = "(Not recommended for Guitar Hero weapons!)";
				}
				spriteBatch.Draw(arrowKeys,
					 Scale(arrowKeysDestRectangle, arrowKeysHover ? 1.1f : 1f),
					 null,
					 Color.White * (arrowKeysVis ? 1: 0.3f),
					 0,
					 arrowKeysOrigin,
					 SpriteEffects.None,
					 0);
				Texture2D homeRowKeys = textures["ControlsHomeRowKeys"];
				Vector2 homeRowKeysOffset = new Vector2(440, 254) * sizeScalar;
				Vector2 homeRowKeysOrigin = homeRowKeys.Size() / 2f;
				Rectangle homeRowKeysDestRectangle = new Rectangle((int)(topLeft.X + homeRowKeysOffset.X), (int)(topLeft.Y + homeRowKeysOffset.Y), (int)(homeRowKeys.Width * sizeScalar), (int)(homeRowKeys.Height * sizeScalar));
				bool homeRowKeysHover = homeRowKeysDestRectangle.Contains((Main.MouseScreen + homeRowKeysOrigin * sizeScalar).ToPoint());
				bool homeRowKeysVis = homeRowKeysHover || controlsChoice == ControlsChoice.HomeRowKeys;
				spriteBatch.Draw(homeRowKeys,
					 Scale(homeRowKeysDestRectangle, homeRowKeysHover ? 1.1f : 1f),
					 null,
					 Color.White * (homeRowKeysVis ? 1 : 0.3f),
					 0,
					 homeRowKeysOrigin,
					 SpriteEffects.None,
					 0);
				Texture2D custom = textures["ControlsCustom"];
				Vector2 customOffset = new Vector2(680, 242) * sizeScalar;
				Vector2 customOrigin = custom.Size() / 2f;
				Rectangle customDestRectangle = new Rectangle((int)(topLeft.X + customOffset.X), (int)(topLeft.Y + customOffset.Y), (int)(custom.Width * sizeScalar), (int)(custom.Height * sizeScalar));
				bool customHover = customDestRectangle.Contains((Main.MouseScreen + customOrigin * sizeScalar).ToPoint());
				bool customVis = customHover || controlsChoice == ControlsChoice.Custom;
				if (customHover)
				{
					mouseText = "(Recommended!)";
				}
				spriteBatch.Draw(custom,
					 Scale(customDestRectangle, customHover ? 1.1f : 1f),
					 null,
					 Color.White * (customVis ? 1 : 0.3f),
					 0,
					 customOrigin,
					 SpriteEffects.None,
					 0);
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (arrowKeysHover)
					{
						controlsChoice = ControlsChoice.ArrowKeys;
					}
					else if (homeRowKeysHover)
					{
						controlsChoice = ControlsChoice.HomeRowKeys;
					}
					else if (customHover)
					{
						controlsChoice = ControlsChoice.Custom;
						IngameFancyUI.OpenKeybinds();
					}
				}
			}
			public void DrawTrackPositionButtons(SpriteBatch spriteBatch, Texture2D background, float sizeScalar)
			{
				Rectangle Scale(Rectangle rect, float scale)
				{
					return new Rectangle(rect.X, rect.Y, (int)(rect.Width * scale), (int)(rect.Height * scale));
				}
				Point size = new Point((int)(background.Width * sizeScalar), (int)(background.Height * sizeScalar));
				Vector2 topLeft = new Vector2(Main.screenWidth - size.X, Main.screenHeight - size.Y) / 2f;
				Texture2D crosshair = textures["TrackAtCrosshair"];
				Vector2 crosshairOffset = new Vector2(316, 418) * sizeScalar;
				Rectangle crosshairRect = new Rectangle((int)(topLeft.X + crosshairOffset.X), (int)(topLeft.Y + crosshairOffset.Y), (int)(crosshair.Width * sizeScalar), (int)(crosshair.Height * sizeScalar));
				Vector2 crosshairOrig = crosshair.Size() / 2f;
				bool crosshairHover = crosshairRect.Contains((Main.MouseScreen + crosshairOrig * sizeScalar).ToPoint());
				bool crosshairVis = crosshairHover || trackChoice == TrackChoice.Crosshair;
				spriteBatch.Draw(crosshair,
					 Scale(crosshairRect, crosshairHover ? 1.1f : 1f),
					 null,
					 Color.White * (crosshairVis ? 1 : 0.3f),
					 0,
					 crosshairOrig,
					 SpriteEffects.None,
					 0);
				Texture2D player = textures["TrackAtPlayer"];
				Vector2 playerOffset = new Vector2(534, 415) * sizeScalar;
				Rectangle playerRect = new Rectangle((int)(topLeft.X + playerOffset.X), (int)(topLeft.Y + playerOffset.Y), (int)(player.Width * sizeScalar), (int)(player.Height * sizeScalar));
				Vector2 playerOrig = player.Size() / 2f;
				bool playerHover = playerRect.Contains((Main.MouseScreen + playerOrig * sizeScalar).ToPoint());
				bool playerVis = playerHover || trackChoice == TrackChoice.Player;
				spriteBatch.Draw(player,
					 Scale(playerRect, playerHover ? 1.1f : 1f),
					 null,
					 Color.White * (playerVis ? 1 : 0.3f),
					 0,
					 playerOrig,
					 SpriteEffects.None,
					 0);
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (crosshairHover)
					{
						trackChoice = TrackChoice.Crosshair;
					}
					else if (playerHover)
					{
						trackChoice = TrackChoice.Player;
					}
				}
			}
			public override void Update(GameTime gameTime)
			{
			}
		}
	}
}
