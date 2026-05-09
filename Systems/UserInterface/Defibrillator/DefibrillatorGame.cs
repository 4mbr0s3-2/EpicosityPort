using EpicosityPort.Content.Items.Accessories.Rhythmist.DefibrillatorPack;
using EpicosityPort.Effects.Shaders.Screen;
using EpicosityPort.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface.Defibrillator
{
	public class DefibrillatorGame : ModSystem
	{
		public static DefibrillatorGame Instance {  get {  return ModContent.GetInstance<DefibrillatorGame>(); }  }
		public Terraria.UI.UserInterface GameInterface { get; internal set; }
		public Asset<Texture2D> YellowPulseTexture;
        public Asset<Texture2D> GreenPulseTexture;

        public override void Load()
        {
            YellowPulseTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/YellowPulseIndicator");
            GreenPulseTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/GreenPulseIndicator");
        }
		public class GameUIStateInstance : UIState, IDisposable
		{
			RenderTarget2D defibrillatorRenderTarget;
			float shockwaveAnimation = 0;
			float bonusTextAnimation = 0;
			float tooEarlyTextAnimation = 0;
			float tooLateTextAnimation = 0;
			float easeInAnimation = 0;
			bool hasHitForThisCycle = false;
			bool hasSuccessfulHitForThisCycle = false;
			bool lastPressedSpace = false;
			double pulseScalar = 0;
			double seventhPulseScalar = 0;
			double lastBeatTime = 0;
			int currentBeat = 0;
			int bpm = 130;
			double timer = 0;
			int[] pulseFrame = new int[] { 0, 0, 0, 0, 0, 0, 0 };
			public override void OnInitialize()
			{
				defibrillatorRenderTarget = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight, mipMap: false, Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
				foreach (NPC npc in Main.npc)
				{
					if (npc.boss && npc.active)
					{
						bpm = 270;
						break;
					}
				}
			}

			public void DrawDefibrillatorGame(SpriteBatch spriteBatch)
			{
				// RS: Y'know maybe it's NOT a good idea to do that
				// The garbage collector won't be happy
				// Instead we should just watch for window resizes
				//defibrillatorRenderTarget = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight, mipMap: false, Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
				
				Main.instance.GraphicsDevice.SetRenderTarget(defibrillatorRenderTarget);
				Main.instance.GraphicsDevice.Clear(Color.Transparent);
				spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				// Pulse indicators
				Point screenPosition = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f + Main.screenHeight * (1 - Easings.EaseInOutCubic(easeInAnimation))).ToPoint();
				Point defibrillatorScreenPosition = new Point(screenPosition.X - 203, screenPosition.Y + 100);
				for (int pulseIndex = 0; pulseIndex < 7; pulseIndex++)
				{
					if (pulseIndex < 6)
					{
						Main.spriteBatch.Draw
						(
							Instance.GreenPulseTexture.Value,
							new Rectangle(defibrillatorScreenPosition.X + 58 * pulseIndex, defibrillatorScreenPosition.Y, 58, 58),
							new Rectangle(0, 58 * (pulseFrame[pulseIndex] / 2), 58, 58),
							Color.White
						);
					}
					else
					{
						Main.spriteBatch.Draw
						(
                            Instance.YellowPulseTexture.Value,
							new Rectangle(defibrillatorScreenPosition.X + 58 * pulseIndex, defibrillatorScreenPosition.Y, 58, 58),
							new Rectangle(0, 58 * (pulseFrame[pulseIndex] / 2), 58, 58),
							Color.White
						);
						Main.spriteBatch.Draw
						(
							TextureAssets.BlackTile.Value,
							new Rectangle(defibrillatorScreenPosition.X + (int)(58 * (pulseIndex + 0.5f)), 0, (int)(20 + 10 * seventhPulseScalar), Main.screenHeight),
							null,
							Color.Cyan * (float)seventhPulseScalar * 0.5f,
							0,
							new Vector2(TextureAssets.BlackTile.Value.Size().X / 2f, 0),
							SpriteEffects.None,
							0
						);
					}
				}
				string message = "Press on the 7th beat!";
				Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow
				(
					spriteBatch,
					FontAssets.MouseText.Value,
					message,
					screenPosition.ToVector2() + new Vector2(0, 100),
					Color.White,
					0,
					FontAssets.MouseText.Value.MeasureString(message) / 2f,
					Vector2.One
				);
				// I'm not going to add a text list because that's overengineering
				#region Bonus text
				Terraria.UI.Chat.ChatManager.DrawColorCodedString
				(
					spriteBatch,
					FontAssets.MouseText.Value,
					"-2 seconds!",
					screenPosition.ToVector2() + new Vector2(0, 180 + 20 * Utilities.Easings.EaseOutQuad(1 - bonusTextAnimation)),
					Color.LimeGreen * bonusTextAnimation,
					0,
					FontAssets.MouseText.Value.MeasureString("-2 seconds!") / 2f,
					Vector2.One
				);
				#endregion
				#region Miss texts
				Terraria.UI.Chat.ChatManager.DrawColorCodedString
				(
					spriteBatch,
					FontAssets.MouseText.Value,
					"Too early!",
					screenPosition.ToVector2() + new Vector2(0, 180 + 20 * Utilities.Easings.EaseOutQuad(1 - tooEarlyTextAnimation)),
					Color.Red * tooEarlyTextAnimation,
					0,
					FontAssets.MouseText.Value.MeasureString("Too early!") / 2f,
					Vector2.One
				);
				Terraria.UI.Chat.ChatManager.DrawColorCodedString
				(
					spriteBatch,
					FontAssets.MouseText.Value,
					"Too late!",
					screenPosition.ToVector2() + new Vector2(0, 180 + 20 * Utilities.Easings.EaseOutQuad(1 - tooLateTextAnimation)),
					Color.Red * tooLateTextAnimation,
					0,
					FontAssets.MouseText.Value.MeasureString("Too late!") / 2f,
					Vector2.One
				);
				#endregion
				spriteBatch.Draw
				(
                    ModContent.Request<Texture2D>(ModContent.GetInstance<DefibrillatorPack>().Texture).Value,
					new Rectangle(defibrillatorScreenPosition.X, defibrillatorScreenPosition.Y + 29, (int)(36 * (1 + pulseScalar * 0.1)), (int)(38 * (1 + pulseScalar * 0.1))),
					null,
					Color.White,
					(float)System.Math.Sin(timer) * 0.2f,
					new Vector2(16, 18),
					SpriteEffects.None,
					0
				);
				int heartSize = 16 + (int)(5 * pulseScalar) + (int)(5 * seventhPulseScalar) + (int)(15 * shockwaveAnimation);
				spriteBatch.Draw
				(
					ModContent.Request<Texture2D>($"Terraria/Images/Item_{ItemID.Heart}").Value,
					new Rectangle(defibrillatorScreenPosition.X + 406, defibrillatorScreenPosition.Y + 29, heartSize, heartSize),
					null,
					Color.White,
					(float)System.Math.Cos(timer) * 0.2f,
					new Vector2(8, 8),
					SpriteEffects.None,
					0
				);
				spriteBatch.End();
				Main.instance.GraphicsDevice.SetRenderTarget(null);
			}


            public override void Recalculate()
            {
                defibrillatorRenderTarget?.Dispose();
                defibrillatorRenderTarget = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight, mipMap: false, Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            }

            public override void Draw(SpriteBatch spriteBatch)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
				if (Filters.Scene[ShaderNames.Shockwave].IsActive())
				{
					Filters.Scene[ShaderNames.Shockwave].GetShader().Apply();
				}
				spriteBatch.Draw(defibrillatorRenderTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
				spriteBatch.End();

				bool pressedSpace = Main.mouseLeft;
                // For some reason this only works during drawing
                // RS: I'm not even gonna question this
                if (pressedSpace && currentBeat >= 5 && currentBeat <= 7 && !lastPressedSpace && !hasHitForThisCycle)
				{
					hasHitForThisCycle = true;
					double beatTime = 60d / bpm;
					// Check if current beat is within 0.2 seconds of the 7th beat
					if (currentBeat <= 5 || (currentBeat == 6 && (timer % beatTime) <= 0.9 * beatTime))
					{
						tooEarlyTextAnimation = 1;
					}
					if (currentBeat >= 8 || (currentBeat == 7 && (timer % beatTime) >= 0.1 * beatTime))
					{
						tooLateTextAnimation = 1;
					}
					if (currentBeat == 6 && (timer % beatTime) > 0.9 * beatTime || currentBeat == 7 && (timer % beatTime) < 0.1 * beatTime)
					{
						hasSuccessfulHitForThisCycle = true;
						SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/Kick"));
						SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/Snare"));
						Main.LocalPlayer.respawnTimer -= 120;
						seventhPulseScalar = 1;
						bonusTextAnimation = 1;
					}
					else
					{
						SoundEngine.PlaySound(SoundID.Item16);
					}
				}
				lastPressedSpace = pressedSpace;
				spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			}
			public GameUIStateInstance()
			{
				// RS: I'm not sure why this was put here
				// I think it has to do with the drawing possibly interfering with other SpriteBatch drawing?
				// We love raw spritebatch calls
				Main.OnPreDraw += PreRender;
			}

            public void PreRender(GameTime gameTime)
			{
				// Put in update instead of initialize so width and height are always updated
				DrawDefibrillatorGame(Main.spriteBatch);
				// Draw render target to back buffer again
			}
			public override void Update(GameTime gameTime)
			{


				//Main.NewText(timer);
				if (easeInAnimation < 1)
				{
					easeInAnimation += 0.02f;
				}
				timer += gameTime.ElapsedGameTime.TotalSeconds;
				double crotchet = 60d / bpm;
				// RS: Do we revive RhythmBeatmapHandler 
				//if (RhythmBeatmapHandler.inst.TrackActive)
				//{
				//	timer = Systems.Rhythm.RhythmBeatmapHandler.inst.conductor.normalizedSongPosition;
				//	crotchet = Systems.Rhythm.RhythmBeatmapHandler.inst.conductor.crotchetSeconds;
				//}
				double beatPos = (timer % crotchet) / crotchet;
				if (beatPos < lastBeatTime && Math.Abs(beatPos - lastBeatTime) > 0.7)
				{
					if (currentBeat < 6)
					{
						SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/Kick"));
					}
					else if (currentBeat == 7 && hasSuccessfulHitForThisCycle)
					{
						SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/Taiko"));
						shockwaveAnimation = 1;
					}
					//Main.PlaySoundInstance(ModContent.GetSound($"{typeof(Epicosity).Name}/Sounds/Minigames/Defibrillator/Kick").CreateInstance());
					if (currentBeat < pulseFrame.Length)
					{
						pulseFrame[currentBeat] = 1;
					}
					pulseScalar = 1;
					currentBeat++;
					currentBeat %= 8;
					if (currentBeat == 0)
					{
						hasHitForThisCycle = false;
						hasSuccessfulHitForThisCycle = false;
						// Reset shockwave
						if (Filters.Scene[ShaderNames.Shockwave].IsActive())
						{
							Filters.Scene[ShaderNames.Shockwave].Deactivate();
						}
					}
				}
				// Shockwave animation
				if (shockwaveAnimation > 0)
				{
					shockwaveAnimation -= 0.02f;
					float progress = Easings.EaseOutQuad(1 - shockwaveAnimation);
					if (!Filters.Scene[ShaderNames.Shockwave].IsActive())
					{
						Vector2 target = Main.screenPosition + new Vector2(Main.screenWidth / 2f - 203 + 406, Main.screenHeight / 2f + 100 + 29);
						Filters.Scene.Activate(ShaderNames.Shockwave, target).GetShader().UseColor(1, 10, 15).UseTargetPosition(target);
					}
					Filters.Scene[ShaderNames.Shockwave].GetShader().UseProgress(progress).UseOpacity(100 * (1 - progress));
				}

				if (pulseScalar > 0)
				{
					pulseScalar -= 0.1;
				}
				if (seventhPulseScalar > 0)
				{
					seventhPulseScalar -= 0.1;
				}
				#region Animations
				if (tooEarlyTextAnimation > 0)
				{
					tooEarlyTextAnimation -= 0.05f;
				}
				if (tooLateTextAnimation > 0)
				{
					tooLateTextAnimation -= 0.05f;
				}
				if (bonusTextAnimation > 0)
				{
					bonusTextAnimation -= 0.05f;
				}
				#endregion
				lastBeatTime = beatPos;
				for (int pulseIndex = 0; pulseIndex < 7; pulseIndex++)
				{
					if (pulseFrame[pulseIndex] > 0)
					{
						pulseFrame[pulseIndex]++;
						pulseFrame[pulseIndex] %= 12;
					}
				}
				if (Main.LocalPlayer.respawnTimer <= 0)
				{
					// Dispose
					Instance.GameInterface.SetState(null);
					Filters.Scene[ShaderNames.Shockwave].Deactivate();
					Dispose();
				}
			}

			public void Dispose()
			{
				Main.OnPreDraw -= PreRender;
				defibrillatorRenderTarget.Dispose();
			}
		}
	}
}
