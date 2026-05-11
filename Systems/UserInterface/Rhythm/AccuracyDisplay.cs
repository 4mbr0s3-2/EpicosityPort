using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI.Chat;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	public class AccuracyDisplay : IDisposable
	{
		public string judgementText;
		public float time = 0;
		public float scaleAnim = 1;
		RenderTarget2D combo = null;
		RenderTarget2D text;
		RenderTarget2D shadow;
		public AccuracyDisplay(string text, Color color)
		{
			int combo = RhythmHandler.GetLocalRhythmPlayer().combo;
			string comboText = combo.ToString();
			judgementText = text;
			var tSize = FontAssets.DeathText.Value.MeasureString(text) * 0.5f;
			this.text = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)tSize.X, (int)tSize.Y);
			shadow = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)tSize.X, (int)tSize.Y);

			if (combo > 3)
			{
				var cSize = FontAssets.DeathText.Value.MeasureString(comboText) * 0.5f;
				this.combo = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)cSize.X, (int)cSize.Y);
				PreDrawCombo(Main.spriteBatch, comboText);
			}

			PreDraw(Main.spriteBatch, text);
			PreDrawShadow(Main.spriteBatch, text, color);
		}

		public void PreDrawCombo(SpriteBatch sb, string text)
		{
			sb.GraphicsDevice.SetRenderTarget(combo);
			sb.GraphicsDevice.Clear(Color.Transparent);
				
			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ChatManager.DrawColorCodedString(sb, FontAssets.DeathText.Value, text, Vector2.Zero, Color.White, 0, Vector2.Zero, Vector2.One * 0.5f);
			sb.End();
			sb.GraphicsDevice.SetRenderTarget(null);
		}

		public void PreDrawShadow(SpriteBatch sb, string text, Color color)
		{
			sb.GraphicsDevice.SetRenderTarget(shadow);
			sb.GraphicsDevice.Clear(Color.Transparent);
			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ChatManager.DrawColorCodedStringShadow(sb, FontAssets.DeathText.Value, text, Vector2.Zero, color * 0.7f, 0, Vector2.Zero, Vector2.One * 0.5f);
			sb.End();
			sb.GraphicsDevice.SetRenderTarget(null);
		}
		public void PreDraw(SpriteBatch sb, string text)
		{
			sb.GraphicsDevice.SetRenderTarget(this.text);
            sb.GraphicsDevice.Clear(Color.Transparent);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ChatManager.DrawColorCodedString(sb, FontAssets.DeathText.Value, text, Vector2.Zero, Color.White, 0, Vector2.Zero, Vector2.One * 0.5f);
			sb.End();
			sb.GraphicsDevice.SetRenderTarget(null);
		}
		public void Draw(SpriteBatch sb, Vector2 pos)
		{
            sb.End();
			// RS: Wow look at all this Ending and Beginning
			// If only there were some stack implementation of SpriteBatch states
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (judgementText == "Epic")
			{
				var shader = GameShaders.Armor.GetShaderFromItemId(ItemID.LivingRainbowDye);
				shader.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0, 0, 200, 200));
				shader.Shader.Parameters["uImageSize0"].SetValue(new Vector2(200, 200));
				shader.Apply(null);
			}
			sb.Draw(shadow, pos, null, Color.White, 0, shadow.Size() / 2f, 1 + 0.2f * scaleAnim, SpriteEffects.None, 0);
			sb.End();
			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			sb.Draw(text, pos, null, Color.White, 0, text.Size() / 2f, 1 + 0.2f * scaleAnim, SpriteEffects.None, 0);
			if (combo != null) sb.Draw(combo, pos + new Vector2(0, 40), null, Color.White, 0, combo.Size() / 2f, 1 + 0.2f * scaleAnim, SpriteEffects.None, 0);
			sb.End();

			sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		public void Update(GameTime gt)
		{
			scaleAnim *= 0.7f;
			time += (float)gt.ElapsedGameTime.TotalSeconds;
			if (time > 2)
			{
				RhythmHandler.Instance.Interface.RhythmUIStateInstance.currentAcc = null;
				Dispose();
			}
		}

        // RS: I wish I was as carefree about (V)RAM as back in the day
		// But 5 years later it's especially important to add these now
		public void Dispose()
		{
            combo?.Dispose();
			text?.Dispose();
			shadow?.Dispose();
        }
	}
}
