using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	public class SpriteReceptor : Sprite
	{
		public int column;
		public SpriteReceptor(Texture2D tex, Vector2 pos, float rot, int column) : base(tex, new Vector2(0.5f), pos, rot, 1) 
		{
			this.column = column;
		}
		public override void Update(GameTime gt)
		{
			scale = 1;
			if (RhythmHandler.Instance.heldDown[column])
			{
				scale = 1.1f;
			}
			Vector2 displayLocation = new Vector2(90, RhythmNotes.Instance.notesUpscroll ? 70 : 340);
			Vector2 controlsLocation = displayLocation + new Vector2(RhythmHandler.Instance.Interface.RhythmUIStateInstance.ArrowXSpace, 0) * column;
			position = controlsLocation;
		}
	}
}