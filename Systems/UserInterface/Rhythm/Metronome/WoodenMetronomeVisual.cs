using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.UserInterface.Rhythm.Metronome
{
	public class WoodenMetronomeVisual : MetronomeVisualBase
	{
		SoundEffectInstance sound;
		Texture2D woodenMetronome;
		RenderTarget2D pendulum;
		public WoodenMetronomeVisual()
		{
			woodenMetronome = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/Wooden", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            pendulum = new RenderTarget2D(Main.instance.GraphicsDevice, 10, 50, false, SurfaceFormat.Color, DepthFormat.None);
        }
		public override void Beat(bool strong)
		{
			string s = strong ? "Strong" : "Weak";
			//if (Systems.Rhythm.RhythmBeatmapHandler.inst?.conductor?.normalizedSongPosition < 0) return;
			//if (Systems.Rhythm.RhythmBeatmapHandler.Instance?.TrackActive == true)
			//{
			//	Main.NewText(Systems.Rhythm.RhythmBeatmapHandler.Instance?.conductor?.trueSongPosition + $" : {s}");
			//}
			if (Main.hasFocus)
			{
				SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/Sounds/{s}FL"));
			}
		}
		//public override void Update(GameTime gameTime)
		//{
			//base.Update(gameTime);
		//}
		public override void PreDraw(SpriteBatch spriteBatch, GameTime obj, float crotchet, float positionInMeasure)
		{
			// RS: Uhhhh yeah no
			// How did my PC even handle this back then
			// How did I let this happen
			// How did I not notice this
			//pendulum = new RenderTarget2D(Main.instance.GraphicsDevice, 10, 50, false, SurfaceFormat.Color, DepthFormat.None);
			
			// RS TODO: Make this pendulum NOT use a render target

			float bpm = 60f / crotchet;
			bpm /= 20f;
			Main.instance.GraphicsDevice.SetRenderTarget(pendulum);
			Main.instance.GraphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin();
			spriteBatch.Draw
			(
				woodenMetronome,
				new Rectangle(2, 0, 6, 50),
				new Rectangle(38, 0, 6, 50),
				Color.White
			);
			spriteBatch.Draw
			(
				woodenMetronome,
				new Rectangle(0, (int)(2 + bpm), 10, 10),
				new Rectangle(44, 0, 10, 10),
				Color.White
			);
			spriteBatch.End();
			Main.instance.GraphicsDevice.SetRenderTarget(null);
			Main.instance.GraphicsDevice.Clear(Color.Transparent);
		}
		public override void Draw(SpriteBatch spriteBatch, bool idle, float crotchet, float positionInMeasure)
		{
			//ChatManager.DrawColorCodedStringWithShadow(
			//	spriteBatch,
			//	Main.fontDeathText,
			//	positionInMeasure.ToString(),
			//	Vector2.Zero,
			//	Color.White,
			//	0,
			//	Vector2.Zero,
			//	Vector2.One
			//);
			double sine = System.Math.Cos(positionInMeasure / crotchet * MathHelper.Pi);
			int margin = 30;
			Vector2 topLeft = new Vector2(Main.screenWidth - 38 - margin, Main.screenHeight - 58 - margin);
			spriteBatch.Draw
			(
				woodenMetronome,
				new Rectangle((int)topLeft.X, (int)topLeft.Y, 38, 58),
				new Rectangle(0, 0, 38, 58),
				Color.White
			);
			if (pendulum != null)
			{
				spriteBatch.Draw
				(
					pendulum,
					new Rectangle((int)topLeft.X + 21, (int)topLeft.Y + 50, 10, 50),
					null,
					Color.White,
					(float)(sine * 0.8f),
					new Vector2(5, 50),
					SpriteEffects.None,
					0
				);
			}
			spriteBatch.Draw
			(
				woodenMetronome,
				new Rectangle((int)topLeft.X, (int)topLeft.Y + 38, 38, 20),
				new Rectangle(0, 38, 38, 20),
				Color.White
			);
		}
		public override bool Hovering(Vector2 screenMouse)
		{
			int margin = 30;
			Vector2 topLeft = new Vector2(Main.screenWidth - 38 - margin, Main.screenHeight - 58 - margin);
			return new Rectangle((int)topLeft.X, (int)topLeft.Y, 38, 58).Contains(screenMouse.ToPoint());
		}
        public override void Dispose()
        {
            pendulum.Dispose();
        }
    }
}
