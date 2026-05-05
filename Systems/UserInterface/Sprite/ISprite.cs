using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EpicosityPort.Systems.UserInterface
{
    public interface ISprite
    {
        void Update(GameTime gt);
        void Draw(SpriteBatch sb);
    }
}
