using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EpicosityPort.Systems.UserInterface.Rhythm.Metronome
{
	public class MetronomeVisualBase
	{
		public virtual void Draw(SpriteBatch spriteBatch, bool idle, float crotchet, float positionInMeasure)
		{

		}
		public virtual void Beat(bool strong)
		{

		}
		/// <summary>
		/// Used for dragging.
		/// </summary>
		/// <param name="screenMouse"></param>
		/// <returns></returns>
		public virtual bool Hovering(Vector2 screenMouse)
		{
			return false;
		}
		public virtual void Update(GameTime gameTime)
		{

		}

		public virtual void PreDraw(SpriteBatch spriteBatch, GameTime obj, float crotchet, float positionInMeasure)
		{

		}
	}
}
