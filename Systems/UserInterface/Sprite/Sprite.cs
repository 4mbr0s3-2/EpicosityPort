using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EpicosityPort.Systems.UserInterface
{
    /// <summary>
    /// Classes to help organize on-screen sprites.
    /// </summary>
    // RS: Wow, this code is actually somewhat decent
    // If this class interface'd ISprite, it would've used the composite pattern
    // Y'know what let's fix that
	// Let's make it not nested in SpriteScene too
    public class Sprite : ISprite, IDisposable
	{
        //public interface ISprite
        //{
        //    void Update(GameTime gt);
        //    void Draw(SpriteBatch sb);
        //}
        public float opacity = 1;
		public Color color = Color.White;
		public Texture2D tex;
		public Vector2 anchor = Vector2.Zero; // Top left by default
		public float rotation = 0;
		public float scale = 1;
		public Vector2 position = Vector2.Zero;
		public Rectangle? destRect = null;
		public Rectangle? sourceRect = null;
		public Vector2 destRectSize => new Vector2(destRect.Value.Width, destRect.Value.Height);

		public Sprite(Texture2D texture, Vector2? anc = null, Vector2? pos = null, float rot = 0, float sc = 1)
		{
			tex = texture;
			if (anc.HasValue)
			{
				anchor = anc.Value;
			}
			if (pos.HasValue)
			{
				position = pos.Value;
			}
			rotation = rot;
			scale = sc;
		}
		public Sprite(Texture2D texture, Vector2? anc = null, Rectangle? source = null, Rectangle? dest = null, float rot = 0, float sc = 1)
		{
			tex = texture;
			if (anc.HasValue)
			{
				anchor = anc.Value;
			}
			if (source.HasValue)
			{
				sourceRect = source.Value;
			}
			if (dest.HasValue)
			{
				destRect = dest.Value;
			}
			rotation = rot;
			scale = sc;
		}

		public void Dispose()
		{

		}

		public virtual void Draw(SpriteBatch sb)
		{
			if (destRect.HasValue)
			{
				DrawWithRectangle(sb);
			}
			else
			{
				DrawWithPoint(sb);
			}
		}

		public void DrawWithRectangle(SpriteBatch sb)
		{
			sb.Draw(tex, destRect.Value, sourceRect, color * opacity, rotation, anchor * destRectSize, SpriteEffects.None, 0); 
		}

		public void DrawWithPoint(SpriteBatch sb)
		{
			sb.Draw(tex, position, sourceRect, color * opacity, rotation, new Vector2(tex.Width, tex.Height) * anchor, new Vector2(scale), SpriteEffects.None, 0);
		}

		public virtual void Update(GameTime gt)
		{
				
		}
	}
}
