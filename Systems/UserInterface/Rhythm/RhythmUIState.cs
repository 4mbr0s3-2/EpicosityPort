using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using static EpicosityPort.Systems.Rhythm.RhythmHandler;
using static System.Net.Mime.MediaTypeNames;
namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	public class RhythmUIState : UIState, IDisposable
	{
		private IEnumerable<NPC> sortedTargetableNPCs;

        public SpriteScene receptors;
		public AccuracyDisplay currentAcc;
		public JudgementCounter judgementCounter;

		RenderTarget2D guitarHeroRenderTarget;
		RenderTarget2D fKRenderTarget;
		
		BasicEffect basicEffect;
		List<GuitarHeroNoteFireEffect> fireEffects;
		public int ArrowXSpace = 40;
		Vector2 ArrowRowOffset = new Vector2(-60, -70);
		
		Texture2D ArrowOutlineTexture;
		public Texture2D texPlayfieldArrow;
		
		Texture2D CenterTexture;
		Texture2D CrosshairTexture;
		Texture2D GuitarHeroNoteTexture;
		Texture2D GuitarHeroReceptorTexture;
		Texture2D GuitarHeroFireTexture;
		public void SetAccuracy(Judgement accuracy)
		{
			Color color = JudgementColors[accuracy];
			string text = "Bruh";
			switch (accuracy)
			{
				case Judgement.Miss:
					text = "Miss...";
					break;
				case Judgement.WayOff:
					text = "Way Off";
					break;
				case Judgement.Decent:
					text = "Decent";
					break;
				case Judgement.Good:
					text = "Good";
					break;
				case Judgement.Great:
					text = "Great";
					break;
				case Judgement.Perfect:
					text = "Perfect";
					break;
				case Judgement.Epic:
					text = "Epic";
					break;
			}
			currentAcc?.Dispose();
			currentAcc = new AccuracyDisplay(text, color);
			judgementCounter?.Tally(accuracy);
		}
		public void AddFireEffect(int column)
		{
			fireEffects.Add(new GuitarHeroNoteFireEffect()
			{
				column = column,
				time = 0
			});
		}
		// RS: Man these RenderTargets aren't managed at all
		// Let's uhhh try to fix that
		public void PreRender(GameTime gameTime)
		{
			if (RhythmHandler.Instance.Interface.RhythmUserInterface.CurrentState == null) return;
			if (gameplayNoteDisplayStyle == GameplayNoteDisplayStyle.GuitarHero)
			{
				//guitarHeroRenderTarget = RenderGuitarHero(Main.spriteBatch);
                RenderGuitarHero(Main.spriteBatch, guitarHeroRenderTarget);
            }
			else if (gameplayNoteDisplayStyle == GameplayNoteDisplayStyle.FourKey)
			{
				//fKRenderTarget = Render4K(Main.spriteBatch);
                Render4K(Main.spriteBatch, fKRenderTarget);
            }
		}

		public void Render4K(SpriteBatch spriteBatch, RenderTarget2D renderTarget2D)
		{
			//RenderTarget2D renderTarget2D = new RenderTarget2D(Main.graphics.GraphicsDevice, 300, 400);
			renderTarget2D.GraphicsDevice.Clear(Color.Transparent);
			RasterizerState rasterizerState = new RasterizerState
			{
				CullMode = CullMode.None
			};
			spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget2D);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin();
            //if (RhythmBeatmapHandler.inst?.TrackActive == true && RhythmBeatmapHandler.inst?.drawUiStateOverride != null)
            //{
            //	RhythmBeatmapHandler.inst.drawUiStateOverride.Update();
            //	
			//	RS: "Go To Statement Considered Beneficial"
            //	goto EndDrawing;
            //}
            receptors.Draw(spriteBatch);
			Instance.chart.Draw(spriteBatch);
			#region Draw playfield
			//foreach (RhythmNote note in inst.chart.sprites)
			//{
			//	//float scale = 1;
			//	note.Draw(spriteBatch);
			//	//Vector2 arrowLocation = displayLocation + new Vector2(ArrowXSpace, 0) * note.column;
			//}
			#endregion
			//EndDrawing:
			spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
			//return renderTarget2D;
		}
		public void RenderGuitarHero(SpriteBatch spriteBatch, RenderTarget2D renderTarget2D)
		{
            renderTarget2D.GraphicsDevice.Clear(Color.Transparent);
			List<VertexPosTexQuad> quads = new List<VertexPosTexQuad>();
            //RenderTarget2D renderTarget2D = new RenderTarget2D(Main.graphics.GraphicsDevice, 400, 400);
            #region Draw fretboard
            ModPlayerRhythmistAccessories accessories = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmistAccessories>();
			ModPlayerRhythmist rhythmist = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>();
			RasterizerState rasterizerState = new RasterizerState
			{
				CullMode = CullMode.None
			};
            spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;

			if (accessories.fretboardType != "")
			{
				// Draw fretboard

				Texture2D fretboardTexture = ModContent.Request<Texture2D>($"EpicosityPort/Systems/UserInterface/Rhythm/Fretboards/{accessories.fretboardType}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				basicEffect.Texture = fretboardTexture;
				basicEffect.TextureEnabled = true;

				float barLength = (float)(400 * rhythmist.noteRate / 60f * rhythmist.trackSpeed);

				int numOfBars = (int)(400 / barLength) * 2;

				float offset = RhythmHandler.Instance.Interface.chartAnimationTime * 6.67f;

				for (float i = -1f; i < numOfBars; i++)
				{
					quads.Add
					(
						new VertexPosTexQuad
						(
							new Vector3[]
							{
								new Vector3(50, 0, -barLength * (i + 1) - offset + 400),
								new Vector3(50, 0, -barLength * i - offset + 400),
								new Vector3(-50, 0, -barLength * (i + 1) - offset + 400),
								new Vector3(-50, 0, -barLength * i - offset + 400)
							},
							new Vector2[]
							{
								new Vector2(0, 0),
								new Vector2(0, 1),
								new Vector2(1, 0),
								new Vector2(1, 1)
							},
							fretboardTexture,
							0.5f
						)
					);
				}
			}
			#endregion
			#region Draw receptors
			ArrowXSpace = 40;
			for (int x = 0; x < 4; x++)
			{
				Vector3 noteLocation = new Vector3(ArrowXSpace * (-1.5f + x), 0, 0);
				Rectangle noteSourceRectangle = new Rectangle(0, 64 * x, 32, 32);
				int yOffset = 0;
				if (Instance.heldDown[x])
				{
					yOffset = -2;
				}
				float texWidth = GuitarHeroReceptorTexture.Width;
				float texHeight = GuitarHeroReceptorTexture.Height;
				quads.Add
				(
					new VertexPosTexQuad
					(
						new Vector3[]
						{
								new Vector3(50 - x * 25, 25 + yOffset, 0),
								new Vector3(50 - x * 25, 0 + yOffset, 0),
								new Vector3(25 - x * 25, 25 + yOffset, 0),
								new Vector3(25 - x * 25, 0 + yOffset, 0)
						},
						new Vector2[]
						{
								new Vector2(noteSourceRectangle.X / texWidth, noteSourceRectangle.Y / texHeight),
								new Vector2(noteSourceRectangle.X / texWidth, (noteSourceRectangle.Y + noteSourceRectangle.Height) / texHeight),
								new Vector2((noteSourceRectangle.X + noteSourceRectangle.Width) / texWidth, noteSourceRectangle.Y / texHeight),
								new Vector2((noteSourceRectangle.X + noteSourceRectangle.Width) / texWidth, (noteSourceRectangle.Y + noteSourceRectangle.Height) / texHeight),
						},
						GuitarHeroReceptorTexture
					)
				);
			}
			#endregion
			#region Draw charted notes
			IOrderedEnumerable<ISprite> chartOrderedByTime = Instance.chart.sprites.OrderByDescending((note) => (note as RhythmNote).time);
			foreach (RhythmNote note in chartOrderedByTime)
			{
				float scale = 1;
				//Vector2 arrowLocation = NoteRowLocation - Main.screenPosition + new Vector2(ArrowXSpace, 0) * note.column;
				Rectangle noteSourceRectangle = new Rectangle(0, 32 * note.column + 2, 32, 30); // Offset the source rectangle a bit to prevent other frames from interfering
				float opacity = 0;
				if (!note.destroy)
				{
					if (note.time <= 60 && note.time >= 0)
					{
						opacity = (float)(1 - (note.time / 60));
					}
					else if (note.time >= -30 && note.time < 0)
					{
						opacity = (float)(1 - (note.time / -30) * 2);
					}
				}
				else
				{
					note.timeSinceDestroyed++;
					if (!EpicosityConfigClient.Instance.ReduceNoteHitEffect)
					{
						scale = 1 + note.timeSinceDestroyed / 20.0f * 1.5f;
					}
					opacity = 1 - (note.timeSinceDestroyed / 20.0f);
				}

				// Legacy code for 2D note chart
				//spriteBatch.Draw(
				//	GuitarHeroNoteTexture,
				//	new Rectangle(
				//		(int)arrowLocation.X,
				//		(int)(arrowLocation.Y - (note.time / 60f) * topArrowChartSpawnSecondBefore),
				//		(int)(32 * scale),
				//		(int)(32 * scale)),
				//	noteSourceRectangle,
				//	Color.White * opacity,
				//	rotation,
				//	ArrowChartTexture.Size() / 2,
				//	SpriteEffects.None,
				//	0
				//);
				float noteTime = (float)(note.time / 60);
				float texWidth = GuitarHeroNoteTexture.Width;
				float texHeight = GuitarHeroNoteTexture.Height;
				float inflate = (scale - 1) * 5;
				if (note.hold)
				{
					for (float i = 0; i < note.holdTime; i += (float)GetLocalRhythmPlayer().trackSpeed)
					{
						quads.Add
						(
							new VertexPosTexQuad
							(
								new Vector3[]
								{
									new Vector3(50 - note.column * 25 + inflate, 25 + inflate, (float)(6.67 * (note.time + i))),
									new Vector3(50 - note.column * 25 + inflate, -inflate, (float)(6.67 * (note.time + i))),
									new Vector3(25 - note.column * 25 - inflate, 25 + inflate, (float)(6.67 * (note.time + i))),
									new Vector3(25 - note.column * 25 - inflate, -inflate, (float)(6.67 * (note.time + i)))
								},
								new Vector2[]
								{
									new Vector2(noteSourceRectangle.X / texWidth, noteSourceRectangle.Y / texHeight),
									new Vector2(noteSourceRectangle.X / texWidth, (noteSourceRectangle.Y + noteSourceRectangle.Height) / texHeight),
									new Vector2((noteSourceRectangle.X + noteSourceRectangle.Width) / texWidth, noteSourceRectangle.Y / texHeight),
									new Vector2((noteSourceRectangle.X + noteSourceRectangle.Width) / texWidth, (noteSourceRectangle.Y + noteSourceRectangle.Height) / texHeight),
								},
								GuitarHeroNoteTexture,
								opacity * 0.3f
							)
						);
					}
				}
				quads.Add
				(
					new VertexPosTexQuad
					(
						new Vector3[]
						{
							new Vector3(50 - note.column * 25 + inflate, 25 + inflate, (float)(6.67 * note.time)),
							new Vector3(50 - note.column * 25 + inflate, -inflate, (float)(6.67 * note.time)),
							new Vector3(25 - note.column * 25 - inflate, 25 + inflate, (float)(6.67 * note.time)),
							new Vector3(25 - note.column * 25 - inflate, -inflate, (float)(6.67 * note.time))
						},
						new Vector2[]
						{
							new Vector2(noteSourceRectangle.X / texWidth, noteSourceRectangle.Y / texHeight),
							new Vector2(noteSourceRectangle.X / texWidth, (noteSourceRectangle.Y + noteSourceRectangle.Height) / texHeight),
							new Vector2((noteSourceRectangle.X + noteSourceRectangle.Width) / texWidth, noteSourceRectangle.Y / texHeight),
							new Vector2((noteSourceRectangle.X + noteSourceRectangle.Width) / texWidth, (noteSourceRectangle.Y + noteSourceRectangle.Height) / texHeight),
						},
						GuitarHeroNoteTexture,
						opacity
					)
				);
			}
            #endregion
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget2D);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
			foreach (VertexPosTexQuad quad in quads)
			{
				VertexPositionTexture[] vertices = new VertexPositionTexture[4];
				for (int i = 0; i < 4; i++)
				{
					vertices[i].Position = quad.pos[i];
					vertices[i].TextureCoordinate = quad.texCoords[i];
				}
				basicEffect.Alpha = quad.alpha;
				basicEffect.Texture = quad.texture;
				foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
                    spriteBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
				}
			}
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
			//return renderTarget2D;
		}
		private void LoadAssets()
		{
            texPlayfieldArrow = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/NoteSkins/Note", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            ArrowOutlineTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/ArrowOutline", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            CrosshairTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/Crosshair", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            CenterTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/Center", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            GuitarHeroNoteTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/NoteSkins/GuitarHero/Notes", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            GuitarHeroReceptorTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/NoteSkins/GuitarHero/ControlNotes", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            GuitarHeroFireTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/NoteSkins/GuitarHero/HitFlame", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
		public RhythmUIState()
		{
			Main.OnPreDraw += PreRender;

            Main.RunOnMainThread(() =>
				{
					guitarHeroRenderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 400, 400);
					fKRenderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 300, 400);
				}
			);

			LoadAssets();
        }
		public void Dispose()
		{
			Main.OnPreDraw -= PreRender;

			Main.RunOnMainThread(() =>
			{
				guitarHeroRenderTarget.Dispose();
				fKRenderTarget.Dispose();
			});
		}

		public override void OnInitialize()
		{
            // RS: FNA forces us to run on the main thread (mod loading happens in a background .NET thread)
            // So we gotta do some STUFF

            Main.RunOnMainThread(() =>
                basicEffect = new BasicEffect(Main.graphics.GraphicsDevice)
                {
                    VertexColorEnabled = false,
                    View = Matrix.CreateLookAt(new Vector3(0, 100, -100), new Vector3(0, 0, 50), Vector3.Up),
                    Projection = Matrix.CreatePerspectiveFieldOfView(
                        (float)Math.PI / 4.0f,
                        640f / 480f,
                        0.1f,
                        100000),
                    World = Matrix.Identity
                }).Wait(); // RS: Just block the whole load thread and pray to the task scheduler aka "Main.Update"
						   // we got time
						   // it's not like there is any other concurrency happening

			receptors = new SpriteScene();
			fireEffects = new List<GuitarHeroNoteFireEffect>();
			//fireEffects = new List<GuitarHeroNoteFireEffect>();

			// RS: Wow! Asset preloading
			// It's like I knew I was going to have to port to 1.4

			//texPlayfieldArrow = ModContent.GetTexture($"Epicosity/UI/Rhythm/NoteSkins/Note");
			//ArrowOutlineTexture = ModContent.GetTexture("Epicosity/UI/Rhythm/ArrowOutline");
			//CrosshairTexture = ModContent.GetTexture("Epicosity/UI/Rhythm/Crosshair");
			//CenterTexture = ModContent.GetTexture("Epicosity/UI/Rhythm/Center");
			//GuitarHeroNoteTexture = ModContent.GetTexture("Epicosity/UI/Rhythm/NoteSkins/GuitarHero/Notes");
			//GuitarHeroReceptorTexture = ModContent.GetTexture("Epicosity/UI/Rhythm/NoteSkins/GuitarHero/ControlNotes");
			//GuitarHeroFireTexture = ModContent.GetTexture("Epicosity/UI/Rhythm/NoteSkins/GuitarHero/HitFlame");

            Instance.heldDown = new bool[]
			{
					false,
					false,
					false,
					false,
			};
			Instance.prevHeldDown = new bool[]
			{
					false,
					false,
					false,
					false,
			};

			#region Initialize receptors
			ArrowXSpace = 40;
			for (int x = 0; x < 4; x++)
			{
				Vector2 displayLocation = new Vector2(90, RhythmNotes.Instance.notesUpscroll ? 70 : 340);
				Vector2 controlsLocation = displayLocation + new Vector2(ArrowXSpace, 0) * x;
				float rotation = 0;
				switch (x)
				{
					case 0:
						rotation = (float)-Math.PI / 2;
						break;
					case 1:
						rotation = (float)Math.PI;
						break;
					case 3:
						rotation = (float)Math.PI / 2;
						break;
				}
				var rec = new SpriteReceptor(ArrowOutlineTexture, controlsLocation, rotation, x);
				receptors.Add(rec);
			}
			#endregion
		}
		public override void Update(GameTime gameTime)
		{
			if (Main.gamePaused) return;
			// TODO RS: Move this to a better place lol
            sortedTargetableNPCs = Instance.UpdateTargetPosition();
            
			receptors.Update(gameTime);
			texPlayfieldArrow = ModContent.Request<Texture2D>($"EpicosityPort/Systems/UserInterface/Rhythm/NoteSkins/Note{RhythmHandler.Instance.Interface.fourKeySkinName}").Value;
			Instance.UpdateRhythmGameplay(gameTime);
			if (EpicosityConfigClient.Instance.GHSpeen)
			{
				basicEffect.View = Matrix.CreateLookAt(new Vector3((float)(-100 * Math.Cos(Main.time * 0.02f)), 100, (float)(-100 * Math.Sin(Main.time * 0.02f))), new Vector3(0, 0, 50), Vector3.Up);
			}
			else
			{
				basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 100, -100), new Vector3(0, 0, 50), Vector3.Up);
			}
			currentAcc?.Update(gameTime);
			judgementCounter?.Update(gameTime);
		}
		public Vector2 CalculatePlayfieldLocation()
		{
			Vector2 PlayfieldLocation;
			if (RhythmNotes.Instance.playfieldAbovePlayer)
			{
				PlayfieldLocation = Main.LocalPlayer.MountedCenter + ArrowRowOffset;
			}
			else
			{
                PlayerInput.SetZoom_World();
                float centerOffset = 0;
				PlayfieldLocation = Instance.targetPosition + ArrowRowOffset + new Vector2(0, 110);
                float zoomxMarginAdjustment = (1 - (1 / Main.GameViewMatrix.Zoom.X)) / 2;
                PlayfieldLocation.X = Math.Max(PlayfieldLocation.X, Main.screenPosition.X + 40 - centerOffset + zoomxMarginAdjustment * Main.screenWidth);
                PlayfieldLocation.X = Math.Min(PlayfieldLocation.X, Main.screenPosition.X + (Main.screenWidth - 160) - zoomxMarginAdjustment * Main.screenWidth);
				PlayfieldLocation.Y = Math.Max(PlayfieldLocation.Y, Main.screenPosition.Y + 120 + zoomxMarginAdjustment * Main.screenHeight);
				PlayfieldLocation.Y = Math.Min(PlayfieldLocation.Y, Main.screenPosition.Y + (Main.screenHeight - 20) - zoomxMarginAdjustment * Main.screenHeight);
				PlayfieldLocation.X += centerOffset;
			}
			return PlayfieldLocation;
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
            //Terraria.GameInput.PlayerInput.SetZoom_MouseInWorld();
			#region Targeting code
			if (Instance.IsAutoAiming)
			{
				// RS: YIKES
				// I'm "fixing" this
				// Should prob extract this too
				//IEnumerable<NPC> sorted = Instance.UpdateTargetPosition();
				Vector2 halfScreen = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
				for (int npcIndex = 0; npcIndex < sortedTargetableNPCs.Count(); npcIndex++)
				{
					NPC npc = sortedTargetableNPCs.ElementAt(npcIndex);
					string text = npcIndex.ToString();
					Vector2 origin = FontAssets.MouseText.Value.MeasureString(text) / 2f;
					ChatManager.DrawColorCodedStringWithShadow(
						spriteBatch,
						FontAssets.MouseText.Value,
						text,
                        //((npc.Center - Main.screenPosition) - halfScreen) * Main.GameViewMatrix.Zoom.X + halfScreen,
                        // Put into zoomed screen space, then, since our spriteBatch is on Main.UIScaleMatrix, take it into actual screen space
                        Vector2.Transform((npc.Center - Main.screenPosition), Main.GameViewMatrix.TransformationMatrix * Matrix.Invert(Main.UIScaleMatrix)),
						Color.White,
						0,
						origin,
						Vector2.One);
				}
			}
			#endregion
			spriteBatch.End();
			// RS: RasterizerState needs to be CullNone so it doesn't cull when flipping gravity
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            DrawBackground(spriteBatch);
            if (gameplayType == GameplayType.Note)
			{
				DrawNoteGameplay(spriteBatch);
			}
			//else if (gameplayType == GameplayType.Swipe)
			//{
			//	DrawSwipeGameplay(spriteBatch);
			//}
			DrawCrosshair(spriteBatch);
			if (currentAcc != null) DrawAccuracyText(spriteBatch);
			if (judgementCounter != null) DrawJudgementCounter(spriteBatch);

			if (Main.GameViewMatrix.TransformationMatrix.M22 > 0) return;
            string gravityPotionNote = EpicosityPort.Instance.GetLocalization($"Rhythmic.GravityPotionNote").Value;
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                FontAssets.MouseText.Value,
                gravityPotionNote,
                //((npc.Center - Main.screenPosition) - halfScreen) * Main.GameViewMatrix.Zoom.X + halfScreen,
                CalculatePlayfieldLocation() - Main.screenPosition + new Vector2(60, 50),
                Color.White,
                0,
                FontAssets.MouseText.Value.MeasureString(gravityPotionNote) / 2f,
                new Vector2(1, -1));
		}
		public void DrawBackground(SpriteBatch sb)
		{
			Vector2 ArrowRowLocation = CalculatePlayfieldLocation() + new Vector2(10, -235) - new Vector2(100, 100);
			for (int i = 0; i < 10; i++)
			{
				sb.Draw(
					TextureAssets.BlackTile.Value,
					new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X + 150, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + 200, 200 - i * 3, 400 - i),
					null,
					Color.Black * 0.06f,
					0,
					new Vector2(8),
					SpriteEffects.None,
					0);
			}
		}
		public void DrawAccuracyText(SpriteBatch sb)
		{
			var loc = CalculatePlayfieldLocation() - Main.screenPosition + new Vector2(58, -82);
			currentAcc.Draw(sb, loc);
		}
		public void DrawJudgementCounter(SpriteBatch sb)
		{
			judgementCounter.Draw(sb);
		}
		public void DrawNoteGameplay(SpriteBatch spriteBatch)
		{
			ArrowRowOffset = new Vector2(-60 + EpicosityConfigClient.Instance.FKGHTrackXOffset, -70 + EpicosityConfigClient.Instance.FKGHTrackXOffset);

			switch (gameplayNoteDisplayStyle)
			{
				case GameplayNoteDisplayStyle.FourKey:
					DrawNote4KStyle(spriteBatch);
					break;
				case GameplayNoteDisplayStyle.GuitarHero:
					DrawNoteGuitarHeroStyle(spriteBatch);
					break;
			}
		}
		public void DrawNoteGuitarHeroStyle(SpriteBatch spriteBatch)
		{
			//if (guitarHeroRenderTarget != null)
			//{
			if (guitarHeroRenderTarget is null) return;
			Vector2 ArrowRowLocation = CalculatePlayfieldLocation() + new Vector2(60, -35) - new Vector2(100, 100);

			for (int i = 0; i < 5; i++)
			{
				spriteBatch.Draw(
					guitarHeroRenderTarget,
					//new Rectangle(Main.screenWidth / 2, Main.screenHeight / 2 - 100, 200, 200),
					new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + i, 200, 1),
					new Rectangle(0, 2 * i, 400, 2),
					Color.White * (i / 5f),
					0,
					Vector2.Zero,
					SpriteEffects.None,
					0);
			}
			spriteBatch.Draw(
				guitarHeroRenderTarget,
				//new Rectangle(Main.screenWidth / 2, Main.screenHeight / 2 - 100, 200, 200),
				new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + 5, 200, 185),
				new Rectangle(0, 10, 400, 370),
				Color.White,
				0,
				Vector2.Zero,
				SpriteEffects.None,
				0);
			for (int i = 0; i < 10; i++)
			{
				spriteBatch.Draw(
					guitarHeroRenderTarget,
					//new Rectangle(Main.screenWidth / 2, Main.screenHeight / 2 - 100, 200, 200),
					new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + i + 190, 200, 1),
					new Rectangle(0, 380 + 2 * i, 400, 2),
					Color.White * (1 - (i / 10f)),
					0,
					Vector2.Zero,
					SpriteEffects.None,
					0);
			}
			//}
		}
		public void DrawNote4KStyle(SpriteBatch spriteBatch)
		{
			if (fKRenderTarget != null)
			{
				Vector2 ArrowRowLocation = CalculatePlayfieldLocation() + new Vector2(10, -235) - new Vector2(100, 100);
				// Shader based on track coroutine
				//if (RhythmBeatmapHandler.inst?.TrackActive == true && RhythmBeatmapHandler.inst?.uiStateShader != null)
				//{
				//	RhythmBeatmapHandler.inst?.uiStateShader.Update();
				//	goto FullDraw;
				//}
				for (int i = 0; i < 5; i++)
				{
					spriteBatch.Draw(
						fKRenderTarget,
						new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + 2 * i, 300, 2),
						new Rectangle(0, i * 2, 300, 2),
						Color.White * (i / 5f),
						0,
						Vector2.Zero,
						SpriteEffects.None,
						0);
					spriteBatch.Draw(
						fKRenderTarget,
						new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + 390 + 2 * i, 300, 2),
						new Rectangle(0, 390 + i * 2, 300, 2),
						Color.White * (1 - (i / 5f)),
						0,
						Vector2.Zero,
						SpriteEffects.None,
						0);
				}
				FullDraw:
				spriteBatch.Draw(
					fKRenderTarget,
					new Rectangle((int)ArrowRowLocation.X - (int)Main.screenPosition.X, (int)ArrowRowLocation.Y - (int)Main.screenPosition.Y + 10, 300, 380),
					new Rectangle(0, 10, 300, 380),
					Color.White,
					0,
					Vector2.Zero,
					SpriteEffects.None,
					0);
			}
		}
		//[Obsolete]
		//public void DrawSwipeGameplay(SpriteBatch spriteBatch)
		//{
		//	Vector2 playerPosition = Main.LocalPlayer.MountedCenter;
		//	Vector2 guiPosition;
		//	if (mouseGuiOnPlayer)
		//	{
		//		guiPosition = playerPosition;
		//	}
		//	else
		//	{
		//		if (inst.lastMouseLeft)
		//		{
		//			guiPosition = mouseStartPositionScreen + Main.screenPosition;
		//		}
		//		else
		//		{
		//			// Update lags behind draw, so just set this directly instead of referring to targetPosition
		//			guiPosition = Main.MouseWorld;
		//		}
		//	}
		//	#region Draw control arrow and center when not held down
		//	Vector2 mouseStartToEnd = (Main.MouseScreen - mouseStartPositionScreen);
		//	if (!inst.lastMouseLeft)
		//	{
		//		spriteBatch.Draw
		//		(
		//			CenterTexture,
		//			new Rectangle
		//			(
		//				(int)guiPosition.X - (int)Main.screenPosition.X,
		//				(int)guiPosition.Y - (int)Main.screenPosition.Y,
		//				32,
		//				32
		//			),
		//			new Rectangle(0, 0, 32, 32),
		//			Color.White * (mouseStartToEnd.Length() / inst.MIN_DIST_FROM_START),
		//			0,
		//			ArrowChartTexture.Size() / 2,
		//			SpriteEffects.None,
		//			0
		//		);
		//	}
		//	foreach (RhythmNote arrow in inst.chart)
		//	{
		//		float scale = 1;
		//		float opacity = 0;
		//		#region Draw scrolling arrows
		//		if (!arrow.destroy)
		//		{
		//			if (arrow.time <= 60 && arrow.time >= 0)
		//			{
		//				opacity = (float)(1 - (arrow.time / 60));
		//			}
		//			else if (arrow.time >= -30 && arrow.time < 0)
		//			{
		//				opacity = (float)(1 - (arrow.time / -30) * 2);
		//			}
		//		}
		//		else
		//		{
		//			arrow.timeSinceDestroyed++;
		//			if (!EpicosityConfigClient.Instance.ReduceNoteHitEffect)
		//			{
		//				scale = 1 + arrow.timeSinceDestroyed / 20.0f * 1.5f;
		//			}
		//			opacity = 1 - (arrow.timeSinceDestroyed / 20.0f);
		//		}
		//		Vector2 chartArrowOffset = -((float)arrow.time / 60f) * playfieldHeight * (arrow.direction - MathHelper.Pi / 2f).ToRotationVector2();
		//		Vector2 arrowPosition = guiPosition;
		//		// Let hit arrow linger at same world position with set hit position
		//		if (arrow.initialHit)
		//		{
		//			arrowPosition = arrow.hitPosition;
		//		}
		//		spriteBatch.Draw
		//		(
		//			ArrowChartTexture,
		//			new Rectangle
		//			(
		//				(int)(arrowPosition.X + chartArrowOffset.X) - (int)Main.screenPosition.X,
		//				(int)(arrowPosition.Y + chartArrowOffset.Y) - (int)Main.screenPosition.Y,
		//				(int)(32 * scale),
		//				(int)(32 * scale)
		//			),
		//			new Rectangle(0, 0, 32, 32),
		//			Color.White * opacity,
		//			arrow.direction,
		//			ArrowChartTexture.Size() / 2,
		//			SpriteEffects.None,
		//			0
		//		);
		//		#endregion
		//		#region Draw associated dance frame
		//		if (arrow.associatedDanceTexture != null)
		//		{
		//			spriteBatch.Draw
		//			(
		//				arrow.associatedDanceTexture,
		//				new Rectangle
		//				(
		//					(int)(arrowPosition.X + chartArrowOffset.X) - (int)Main.screenPosition.X,
		//					(int)(arrowPosition.Y + chartArrowOffset.Y) - (int)Main.screenPosition.Y + 100,
		//					(int)(128 * scale),
		//					(int)(128 * scale)
		//				),
		//				arrow.associatedDanceFrame,
		//				Color.White * opacity,
		//				0,
		//				arrow.associatedDanceFrame.Size() / 2,
		//				SpriteEffects.None,
		//				0
		//			);
		//		}
		//		#endregion
		//	}
		//	if (inst.lastMouseLeft)
		//	{
		//		// Draw control arrow
		//		spriteBatch.Draw
		//		(
		//			ArrowOutlineTexture,
		//			new Rectangle
		//			(
		//				(int)guiPosition.X - (int)Main.screenPosition.X,
		//				(int)guiPosition.Y - (int)Main.screenPosition.Y,
		//				(int)(32),
		//				(int)(32)
		//			),
		//			new Rectangle(0, 0, 32, 32),
		//			Color.White * (mouseStartToEnd.Length() / inst.MIN_DIST_FROM_START),
		//			inst.rotationFromMouseStartPositionToCursor + MathHelper.Pi / 2,
		//			ArrowChartTexture.Size() / 2,
		//			SpriteEffects.None,
		//			0
		//		);
		//		spriteBatch.Draw
		//		(
		//			CenterTexture,
		//			new Rectangle
		//			(
		//				(int)mouseStartPositionScreen.X,
		//				(int)mouseStartPositionScreen.Y,
		//				(int)(32),
		//				(int)(32)
		//			),
		//			new Rectangle(0, 0, 32, 32),
		//			Color.White,
		//			0,
		//			ArrowChartTexture.Size() / 2,
		//			SpriteEffects.None,
		//			0
		//		);
		//	}
		//	#endregion
		//}
		public void DrawCrosshair(SpriteBatch sb)
		{
			Vector2 crosshairDisplacement = Instance.targetPosition - Main.screenPosition;


			//Main.NewText(crosshairDisplacement);
			sb.Draw(CrosshairTexture,
                new Rectangle((int)(crosshairDisplacement.X), (int)crosshairDisplacement.Y, 30, 30),
                new Rectangle(0, 0, 30, 30),
                RhythmHandler.Instance.lockedTargetIndex > -1 ? Color.Red : Color.White,
                Instance.crosshairRotation,
                CrosshairTexture.Size() / 2,
                SpriteEffects.None,
                0);
		}
	}
}
