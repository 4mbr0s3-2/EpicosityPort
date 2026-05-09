using Terraria.GameContent;
using Terraria.UI;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System;
using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using EpicosityPort.Systems.UserInterface.ChoreabilitySlots;
using EpicosityPort.Effects.Shaders.Screen;
using EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities;

namespace EpicosityPort.Systems.UserInterface.Rhythm.ComboBar
{
	public class ComboBarInterface : ModSystem
	{
		public static ComboBarInterface Instance { get {
			return ModContent.GetInstance<ComboBarInterface>();
		} }

		public Terraria.UI.UserInterface ComboBarUserInterface;
		public ComboBarUIState comboBarUIState;
		public bool shouldShowComboBar = false;
		public int holdComboBarTimer = 0;
		public float arrowScale = 0;
		public float shake = 0;
		public float fullComboAnimation = 0;


		public class ComboBarUIState : UIState
		{
			List<AccuracyMarker> accuracyMarkers = new List<AccuracyMarker>();

			private float showAnimation = 0;
			
			public Texture2D ComboBarJuice;
			public Texture2D ComboBarBG;
			public Texture2D ComboBarArrow;
			
			public override void OnInitialize()
			{
				accuracyMarkers.Clear();
				ComboBarJuice = ModContent.Request<Texture2D>("EpicosityPort/Systems/UserInterface/Rhythm/ComboBar/ComboBarJuice", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ComboBarBG = ModContent.Request<Texture2D>("EpicosityPort/Systems/UserInterface/Rhythm/ComboBar/ComboBarBG", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				ComboBarArrow = ModContent.Request<Texture2D>("EpicosityPort/Systems/UserInterface/Rhythm/ComboBar/ComboBarArrow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			}
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				base.DrawSelf(spriteBatch);
                ModPlayerRhythmist modPlayerRhythmist = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>();
                ModPlayerRhythmistAccessories modPlayerRhythmistAccessories = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmistAccessories>();
                float comboMeter = modPlayerRhythmist.comboMeter;
				Vector2 barSize = new Vector2(186, 32);
				int shake = (int)ComboBarInterface.Instance.shake;
				float fullComboAnimation = ComboBarInterface.Instance.fullComboAnimation;
				float arrowScale = ComboBarInterface.Instance.arrowScale;
				Vector2 shakeOffset = new Vector2(Main.rand.Next(-shake, shake), Main.rand.Next(-shake, shake));
				Vector2 barPosition = new Vector2(Main.screenWidth / 2f - barSize.X / 2f, (-1.1f + 2 * Easings.EaseInOutCubic(showAnimation)) * barSize.Y) + shakeOffset;
				#region Draw choreabilities
				ChoreabilitySlotsPlayer choreabilityPlayer = Main.LocalPlayer.GetModPlayer<ChoreabilitySlotsPlayer>();
				IEnumerable<Item> validSlotList = choreabilityPlayer.choreabilitySlotsInventory.Where((item) => !item.IsAir);
				for (int itemIndex = 0; itemIndex < validSlotList.Count(); itemIndex++)
				{
					Item choreability = validSlotList.ToList()[itemIndex];
					Texture2D choreabilityTexture = (choreability.ModItem as ChoreabilityBase).GetChoreabilityTexture();
					Texture2D tierTexture = (choreability.ModItem as ChoreabilityBase).GetTierTexture();
					Rectangle choreabilityRect = choreabilityTexture.Frame();
					choreabilityRect.Y = 16;
					choreabilityRect.Height -= 16;
					Rectangle destinationRect = new Rectangle((int)barPosition.X + itemIndex * 20, (int)barPosition.Y + 14, choreabilityRect.Width, choreabilityRect.Height);
					spriteBatch.Draw
					(
						tierTexture,
						destinationRect,
						choreabilityRect,
						Color.White * showAnimation,
						0,
						Vector2.Zero,
						SpriteEffects.None,
						0
					);
					spriteBatch.Draw
					(
						choreabilityTexture,
						destinationRect,
						choreabilityRect,
						Color.White * showAnimation,
						0,
						Vector2.Zero,
						SpriteEffects.None,
						0
					);
					if (destinationRect.Contains(Main.MouseScreen.ToPoint()))
					{
						Main.LocalPlayer.cursorItemIconEnabled = true;
						Main.LocalPlayer.cursorItemIconID = -1;
						Main.LocalPlayer.cursorItemIconText = choreability.HoverName;
					}
				}
				#endregion
				#region Draw background of meter
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
				// RS TODO: Check how the game handles shaders now+

				MiscShaderData fCEffect = GameShaders.Misc[ShaderNames.ComboMeterFullComboEffect];
				DrawData meterBackground = new DrawData
				(
					ComboBarBG,
					new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)barSize.X, (int)barSize.Y),
					new Rectangle(0, 0, (int)barSize.X, (int)barSize.Y),
					Color.White,
					0,
					Vector2.Zero,
					SpriteEffects.None,
					0
				);
				fCEffect.UseColor(fullComboAnimation, 0, 0);
				fCEffect.Apply(meterBackground);
				meterBackground.Draw(spriteBatch);
				#endregion
				#region Draw juice on meter
				DrawJuice(spriteBatch, barPosition, comboMeter);
				#endregion
				#region Draw arrow on meter
				spriteBatch.Draw
				(
					ComboBarArrow,
					new Rectangle((int)barPosition.X + 22, (int)barPosition.Y + 16, (int)(barSize.X * (1 + arrowScale)), (int)(barSize.Y * (1 + arrowScale))),
					new Rectangle(0, 0, (int)barSize.X, (int)barSize.Y),
					Color.White,
					0,
					new Vector2(22, 16),
					SpriteEffects.None,
					0
				);
				#endregion
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				if (modPlayerRhythmistAccessories.hasAccuracyTracker)
					DrawAccuracyBar(barSize, shakeOffset, barPosition, spriteBatch);
			}
			public void AddMarker(float accuracy, Color color)
			{
				accuracyMarkers.Add(new AccuracyMarker(1, accuracy, color));
			}
			public void UpdateMarkers()
			{
				for (int i = 0; i < accuracyMarkers.Count; i++)
				{
					var marker = accuracyMarkers[i];
					marker.alpha -= 1f / 600f;
					if (marker.alpha <= 0)
					{
						accuracyMarkers.RemoveAt(i);
					}
				}
			}

			public void DrawAccuracyBar(Vector2 barSize, Vector2 shakeOffset, Vector2 barPosition, SpriteBatch spriteBatch)
			{
				float window = EpicosityConfigClient.Instance.TimingWindow;
				barPosition.Y += 40;
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)barSize.X, 16), Color.Black);
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)barPosition.X + (int)(barSize.X * 0.5f), (int)barPosition.Y - 2, 2, 20), Color.Red);
				foreach (var marker in accuracyMarkers)
				{
					spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)barPosition.X + (int)(barSize.X * (marker.accuracy / window / 2 + 1) / 2f), (int)barPosition.Y, 2, 16), marker.color * marker.alpha);
				}
			}

			public void DrawJuice(SpriteBatch spriteBatch, Vector2 barPosition, float comboMeter)
			{
				for (int row = 0; row < 4; row++)
				{
					Vector2 originOffset;
					int rowThickness = 2;
					switch (row)
					{
						case 0:
							originOffset = new Vector2(48, 10);
							break;
						case 1:
							originOffset = new Vector2(46, 12);
							rowThickness = 4;
							break;
						case 2:
							originOffset = new Vector2(44, 16);
							rowThickness = 4;
							break;
						case 3:
							originOffset = new Vector2(42, 20);
							break;
						default:
							originOffset = default;
							break;
					}
					spriteBatch.Draw
					(
						ComboBarJuice,
						new Rectangle((int)barPosition.X + (int)originOffset.X, (int)barPosition.Y + (int)originOffset.Y, (int)(122 * comboMeter), rowThickness),
						new Rectangle((int)originOffset.X, (int)originOffset.Y, (int)(122 * comboMeter), rowThickness),
						Color.White,
						0,
						Vector2.Zero,
						SpriteEffects.None,
						0
					);
				}
			}
			public override void Update(GameTime gameTime)
			{
				UpdateMarkers();
				// RS: Crazy how these are all outside of the actual UIState subclass
				// Cheap way of making an ease out animation
				Instance.arrowScale *= 0.9f;
				Instance.shake *= 0.9f;
				Instance.fullComboAnimation *= 0.9f;
				Instance.shouldShowComboBar = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>().comboMeter > 0;
				//Main.NewText(shouldShowComboBar);
				float constantRate = 1f / 30f;
				if (Instance.holdComboBarTimer > 0)
				{
					Instance.holdComboBarTimer--;
				}
				if (Instance.shouldShowComboBar || Instance.holdComboBarTimer > 0)
				{
					showAnimation += constantRate;
					if (showAnimation > 1)
					{
						showAnimation = 1;
					}
				}
				else if (showAnimation > 0)
				{
					showAnimation -= constantRate;
					if (showAnimation < 0)
					{
						showAnimation = 0;
					}
				}
			}
		}
	}
}
