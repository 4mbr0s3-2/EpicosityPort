using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using static EpicosityPort.Systems.Rhythm.RhythmHandler;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	public class JudgementCounter : ISprite
	{
		public Dictionary<Judgement, int> counter = new Dictionary<Judgement, int>() 
		{
			[Judgement.Epic] = 0,
			[Judgement.Perfect] = 0,
			[Judgement.Great] = 0,
			[Judgement.Good] = 0,
			[Judgement.Decent] = 0,
			[Judgement.WayOff] = 0,
			[Judgement.Miss] = 0
		};
		public int judgementHeight = 20;
		public int width = 200;
		public int padding = 20;
		public int margin = 20;
		public Vector2 pos;
		public JudgementCounter(Vector2 pos)
		{
			this.pos = pos;
		}
		public void DrawBG(SpriteBatch sb)
		{
			int numOfJudgements = counter.Count;
			sb.Draw
			(
				TextureAssets.BlackTile.Value,
				new Rectangle((int)pos.X, (int)pos.Y, width + padding * 2, numOfJudgements * judgementHeight + padding * 2),
				Color.Black * 0.5f
			);
		}
		public void Tally(Judgement j)
		{
			counter[j]++;
		}
		public void DrawJudgementBars(SpriteBatch sb)
		{
			//int total = RhythmBeatmapHandler.inst.Beatmap.Count;
			//int judgementIndex = 0;
			//foreach (Judgement j in counter.Keys)
			//{
			//	if (!counter.TryGetValue(j, out int count)) continue;
			//	if (!JudgementColors.TryGetValue(j, out Color color)) continue;
			//	sb.Draw
			//	(
			//		TextureAssets.MagicPixel.Value, 
			//		new Rectangle
			//		(
			//			(int)pos.X + padding, 
			//			(int)pos.Y + padding + judgementHeight * judgementIndex, 
			//			(int)((width - padding) / (float)total * count), 
			//			judgementHeight
			//		),
			//		color
			//	);
			//	judgementIndex++;
			//}
		}
		public void Draw(SpriteBatch sb)
		{
			DrawBG(sb);
			DrawJudgementBars(sb);
		}
		public void Update(GameTime gt)
		{
		}
	}
}
