using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	// RS: Rhythm UI is definitely the correct namespace for this useful and related class
	public struct VertexPosTexQuad
	{
		public Texture2D texture;
		public Vector3[] pos;
		public Vector2[] texCoords;
		public float alpha;
		public VertexPosTexQuad(Vector3[] pos, Vector2[] texCoords, Texture2D tex, float alpha = 1)
		{
			texture = tex;
			this.pos = pos;
			this.texCoords = texCoords;
			this.alpha = alpha;
		}
	}
}
