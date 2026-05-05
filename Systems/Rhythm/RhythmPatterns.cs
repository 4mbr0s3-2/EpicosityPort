using EpicosityPort.Systems.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static EpicosityPort.Systems.Rhythm.RhythmMouse;

namespace EpicosityPort.Systems.Rhythm
{
	//public partial class RhythmHandler
	public class RhythmPatterns : ModSystem
	{
		public static RhythmPatterns Instance { 
			get {
				return ModContent.GetInstance<RhythmPatterns>();
			}
		}
        public override void Load()
		{
			LoadTrackPatterns();
		}
		public List<FourKeyTrackPatternObject> arrowTrackPatterns = new List<FourKeyTrackPatternObject>();
		public List<MouseTrackPatternObject> mouseTrackPatterns = new List<MouseTrackPatternObject>();
		public MouseTrackPatternObject GetMouseTrackPattern<MousePattern>()
		{
			foreach (MouseTrackPatternObject mouseTrack in mouseTrackPatterns)
			{
				if (mouseTrack.GetType() == typeof(MousePattern))
				{
					return mouseTrack;
				}
			}
			throw new Exception("Can't find MousePattern in mouseTrackPatterns!");
		}
		public FourKeyTrackPatternObject GetArrowTrackPattern<ArrowPattern>()
		{
			foreach (FourKeyTrackPatternObject arrowTrack in arrowTrackPatterns)
			{
				if (arrowTrack.GetType() == typeof(ArrowPattern))
				{
					return arrowTrack;
				}
			}
			throw new Exception("Can't find ArrowPattern in arrowTrackPatterns!");
		}
		/// <summary>
		/// Track patterns are autoloaded based on nested classes in the RhythmHandler class.
		/// </summary>
		public void LoadTrackPatterns()
		{
			Type[] patterns = GetType().GetNestedTypes();
			foreach (Type info in patterns)
			{
				if (info.BaseType == typeof(FourKeyTrackPatternObject))
				{
					EpicosityPort.Instance.Logger.Info($"Auto-loaded arrow track {info.Name}");
					arrowTrackPatterns.Add((FourKeyTrackPatternObject)Activator.CreateInstance(info));
				}
				//if (info.BaseType == typeof(MouseTrackPatternObject))
				//{
				//	Epicosity.Instance.Logger.Info($"Auto-loaded mouse track {info.Name}");
				//	mouseTrackPatterns.Add((MouseTrackPatternObject)Activator.CreateInstance(info));
				//}
			}
			if (arrowTrackPatterns.Count() <= 0)
			{
				throw new Exception("Couldn't load any track patterns!");
			}
		}
		/// <summary>
		/// The duration alloted when a note is spawned. This should be the offset at the beginning of the song. The higher, the less noticable the notes are spawned since they're off chart.
		/// </summary>
		public float beatmapTicksDelayBeforeNote { get { return RhythmHandler.Instance.playfieldHeight; } }
		public void HandleSpawningBeatmap()
		{
			// RS: Nah.
			//ModPlayerRhythmist player = GetLocalRhythmPlayer();
			//if (RhythmBeatmapHandler.inst.Beatmap.Count <= 0) return;
			//float maxTime = RhythmBeatmapHandler.inst.Beatmap.OrderByDescending(o => o.time).First().time + 2000;
			////Main.NewText(beatmapTime);
			//foreach (BeatmapReader.HitObject hitObject in RhythmBeatmapHandler.inst.Beatmap)
			//{
			//	float advancedSec = 2;
			//	//int hitObjectTimeInTicks = (int)(hitObject.time / 1000f * 60);
			//	//if ((int)(RhythmBeatmapHandler.Instance.conductor.songPosition * 60 + 120 - EpicosityConfigClient.Instance.DDRBeatmapOffset) == hitObjectTimeInTicks)
			//	if (!hitObject.dirty && RhythmBeatmapHandler.inst.conductor.songPosition >= hitObject.time / 1000f - advancedSec)
			//	{
			//		hitObject.dirty = true;
			//		chart.Add(new RhythmNote(hitObject.column, beatmapTicksDelayBeforeNote * player.trackSpeed, default, default, player.trackSpeed * hitObject.holdTime * 60f / 1000f, true, hitObject.time / 1000f, hitObject.holdTime / 1000f));
			//	}
			//}
		}

		#region Arrow track patterns
		public class ArrowRandom : FourKeyTrackPatternObject
		{
			public override void GetNote(ref int patternArrowIndex, ref SpriteScene chart)
			{
				chart.Add(new RhythmNote(Main.rand.Next(4), 60, default, default));
			}
		}
		public class ArrowRightFacingStaircase : FourKeyTrackPatternObject
		{
			public override void GetNote(ref int patternArrowIndex, ref SpriteScene chart)
			{
				int[] lrstaircase = new int[] { 0, 1, 2, 3 };
				if (patternArrowIndex >= lrstaircase.Length)
				{
					patternArrowIndex = 0;
				}
				int bodyFrame = 0;
				switch (patternArrowIndex)
				{
					case 0:
						bodyFrame = 3;
						break;
					case 1:
						bodyFrame = 1;
						break;
					case 2:
						bodyFrame = 4;
						break;
					case 3:
						bodyFrame = 3;
						break;
				}
				chart.Add(new RhythmNote(lrstaircase[patternArrowIndex], 60, bodyFrame, patternArrowIndex >= 2 ? 1 : -1));
				patternArrowIndex++;
			}
		}
		public class ArrowPingPongStaircase : FourKeyTrackPatternObject
		{
			public override void GetNote(ref int patternArrowIndex, ref SpriteScene chart)
			{
				ModPlayerRhythmist player = RhythmHandler.GetLocalRhythmPlayer();
				int[] pingPongStaircase = new int[] { 0, 1, 2, 3, 2, 1 };
				if (patternArrowIndex >= pingPongStaircase.Length)
				{
					patternArrowIndex = 0;
				}
				chart.Add(new RhythmNote(pingPongStaircase[patternArrowIndex], 60));
				patternArrowIndex++;
			}
		}
		#endregion
	}
}
