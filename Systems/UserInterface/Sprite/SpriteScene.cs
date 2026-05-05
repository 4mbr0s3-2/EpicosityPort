using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EpicosityPort.Systems.UserInterface
{
    public partial class SpriteScene : ISprite

        //public partial class SpriteScene
    {
		//public interface ISprite
		//{
		//	void Update(GameTime gt);
		//	void Draw(SpriteBatch sb);
		//}
		public List<ISprite> sprites = new List<ISprite>();
		public void Draw(SpriteBatch sb)
		{
			DrawSprites(sb);	
		}
		public void DrawSprites(SpriteBatch sb)
		{
			foreach (ISprite s in sprites)
			{
				s.Draw(sb);
			}
		}
		public ISprite Add(ISprite s)
		{
			sprites.Add(s);
			return s;
		}
		public ISprite Remove(ISprite s)
		{
			sprites.Remove(s);
			return s;
		}
		public void Update(GameTime gt)
		{
			foreach (var sprite in sprites)
			{
				sprite.Update(gt);
			}
		}
	}
}
