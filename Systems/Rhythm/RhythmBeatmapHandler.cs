using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NVorbis;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace EpicosityPort.Systems.Rhythm
{
	// RS: Yeah no I'm probably not revisiting this
	// This is someone else's problem now (if they're interested)
	// RS TODO

	//public class RhythmBeatmapHandler
	//{
	//	public static RhythmBeatmapHandler inst;
	//	//[LoadManager(LoadManagerAttribute.ManageType.Load)]
	//	public static void Initialize()
	//	{
	//		inst = new RhythmBeatmapHandler();
	//	}
	//	//[LoadManager(LoadManagerAttribute.ManageType.Unload)]
	//	public static void Uninitialize()
	//	{
	//		inst = null;
	//	}
	//	bool _trackActive;
	//	public bool TrackActive { get => _trackActive; set => _trackActive = value; }

	//	public ParsedBeatmap parsedBeatmap;

	//	//[Obsolete]
	//	Music _song;
	//	//[Obsolete]
	//	public Music Song { get => _song; set => _song = value; }

	//	public Conductor conductor;

	//	List<HitObject> _beatmap;
	//	public List<HitObject> Beatmap { get => _beatmap; set => _beatmap = value; }
	//	public List<Coroutine> beatmapCoroutines;
	//	public List<Coroutine> beatmapVisualCoroutines;
	//	public UserInterface BeatmapVisualInterface;
	//	public UIState beatmapVisualUIState;
	//	public double lastAdjustedTimePos = 0;
	//	public Action endTrackCallback;
	//	public float currentSliderVelocityMult = 1;
	//	public float lastSliderVelocityMult = 1;
	//	public float targetSliderVelocityMult = 1;
	//	public float sliderVelocityTweenTimer = 0;
	//	public Coroutine drawUiStateOverride;
	//	public Coroutine uiStateShader;
	//	public class BeatmapVisualUIState : UIState
	//	{
	//		public BeatmapVisualUIState()
	//		{
	//		}
	//		public override void Update(GameTime gameTime)
	//		{
	//			base.Update(gameTime);
	//		}
	//		protected override void DrawSelf(SpriteBatch spriteBatch)
	//		{
	//			foreach (Coroutine routine in inst?.beatmapVisualCoroutines)
	//			{
	//				routine.Update();
	//			}
	//		}
	//	}
	//	public void Update()
	//	{
	//		if (TrackActive)
	//		{
	//			conductor.songPosition += 1 / 60d;
	//			if (Main.netMode != NetmodeID.Server)
	//			{
	//				try
	//				{
	//					_song?.CheckBuffer();
	//					_song.SetVariable("Volume", Main.musicVolume);
	//				}
	//				catch { }
	//				if (_song is MusicStreamingMP3)
	//				{
	//				}
	//				else if (_song is MusicStreamingOGG)
	//				{
	//					VorbisReader reader = (VorbisReader)typeof(MusicStreamingOGG).GetField("reader", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_song as MusicStreamingOGG);
	//					double adjustedSongPos = reader.DecodedTime.TotalSeconds;
	//					if (lastAdjustedTimePos != adjustedSongPos)
	//					{
	//						conductor.songPosition = adjustedSongPos;
	//					}
	//					lastAdjustedTimePos = adjustedSongPos;
	//				}
	//				// Lag spike failsafe AND end of track
	//				if (Math.Abs(conductor.lastSongPosition - conductor.songPosition) > 10)
	//				{
	//					EndTrack();
	//					return;
	//				}
	//				conductor.lastSongPosition = conductor.songPosition;
	//			}
	//			//Main.NewText(stream.Position);
	//			conductor.Update();
	//			TrueUpdate(); // "Temporary" fix
	//		}
	//	}
	//	public void TrueUpdate()
	//	{
	//		if (TrackActive)
	//		{
	//			foreach (Coroutine routine in beatmapCoroutines)
	//			{
	//				routine?.Update();
	//			}
	//			foreach (TimingPointInfo timingPointInfo in parsedBeatmap.timingPoints)
	//			{
	//				if (conductor.songPosition >= timingPointInfo.Time / 1000f && !timingPointInfo.dirty)
	//				{
	//					timingPointInfo.dirty = true;
	//					conductor.bpm = timingPointInfo.BPM;
	//					conductor.offset = timingPointInfo.Time / 1000f;
	//				}
	//			}
	//			foreach (SliderVelocityInfo sliderVelocityInfo in parsedBeatmap.sliderVelocityPoints)
	//			{
	//				if (conductor.songPosition >= sliderVelocityInfo.Time / 1000f && !sliderVelocityInfo.dirty)
	//				{
	//					sliderVelocityInfo.dirty = true;
	//					targetSliderVelocityMult = sliderVelocityInfo.SliderVelocity;
	//				}
	//			}

	//			#region Slider velocity tweening
	//			if (targetSliderVelocityMult != currentSliderVelocityMult)
	//			{
	//				sliderVelocityTweenTimer += 0.01f; // Start tween
	//				lastSliderVelocityMult = currentSliderVelocityMult;
	//			}
	//			if (sliderVelocityTweenTimer > 0 && sliderVelocityTweenTimer < 1)
	//			{
	//				float trueTween = sliderVelocityTweenTimer;
	//				currentSliderVelocityMult = lastSliderVelocityMult * (1 - trueTween) + targetSliderVelocityMult * trueTween;
	//				sliderVelocityTweenTimer += 1 / 60f;
	//			}
	//			else
	//			{
	//				sliderVelocityTweenTimer = 0;
	//				currentSliderVelocityMult = targetSliderVelocityMult;
	//			}
	//			#endregion
	//		}
	//	}
	//	public Music GetMusic(string fileName, out string format, bool external = false)
	//	{
	//		Music music = null;
	//		format = null;
	//		try
	//		{
	//			if (!external)
	//			{
	//				music = ModContent.GetMusic($"{typeof(Epicosity).Name}/Systems/Rhythm/Beatmaps/Music/{fileName}");
	//				format = "ogg";
	//			}
	//			else
	//			{
	//				if (fileName.EndsWith(".mp3"))
	//				{
	//					format = "mp3";
	//					music = new MusicStreamingMP3(fileName);
	//					Main.NewText("Note: MP3s are deprecated. Use OGG to fix off-syncing!");
	//				}
	//				else if (fileName.EndsWith(".ogg"))
	//				{
	//					format = "ogg";
	//					music = new MusicStreamingOGG(fileName);
	//				}
	//			}
	//		}
	//		catch
	//		{
	//			return null;
	//		}
	//		return music;
	//	}
	//	/// <summary>
	//	///  Starts playing the osu!mania or Quaver beatmap. Returns true if successful.
	//	/// </summary>
	//	/// <param name="fileName">The file name (if internal) or file path (if external).</param>
	//	/// <param name="initialBpm"></param>
	//	/// <param name="initialOffset"></param>
	//	public bool StartTrack(string fileName, double initialBpm = 100, double initialOffset = 0, IEnumerator[] sequences = null, IEnumerator[] visualSequences = null, Action endCallback = null, BeatmapType beatmapType = BeatmapType.OsuMania, bool external = false, string externalPath = "", IEnumerator uiStateOverride = null, IEnumerator uiStateShader = null)
	//	{
	//		TrackActive = true;
	//		string format = null;
	//		if (Main.netMode != NetmodeID.Server)
	//		{
	//			RhythmHandler.RhythmUI.judgementCounter = new JudgementCounter(new Vector2(50));
	//			RhythmHandler.GetLocalRhythmPlayer().combo = 0;
	//			RhythmHandler.inst.chart.sprites.Clear();
	//			if (!external)
	//			{
	//				try
	//				{
	//					switch (beatmapType)
	//					{
	//						case BeatmapType.OsuMania:
	//							parsedBeatmap = ReadOsuFileFromModContent(fileName);
	//							break;
	//						case BeatmapType.Quaver:
	//							parsedBeatmap = ReadQuaverFileFromModContent(fileName);
	//							break;
	//					}
	//				}
	//				catch (Exception e)
	//				{
	//					Main.NewText("Internal beatmap unable to be parsed! Please report this bug ;-;");
	//					Main.NewText(e.Message);
	//					EndTrack();
	//					return false;
	//				}
	//			}
	//			else
	//			{
	//				try
	//				{
	//					switch (beatmapType)
	//					{
	//						case BeatmapType.OsuMania:
	//							parsedBeatmap = ReadOsuString(fileName);
	//							break;
	//						case BeatmapType.Quaver:
	//							parsedBeatmap = ReadQuaverString(fileName);
	//							break;
	//					}
	//				}
	//				catch (Exception e)
	//				{
	//					Main.NewText("Beatmap unable to be parsed...");
	//					Main.NewText(e.Message);
	//					EndTrack();
	//					return false;
	//				}
	//			}
	//			string externalAudioFileName = "";
	//			if (external)
	//			{
	//				switch (beatmapType)
	//				{
	//					case BeatmapType.OsuMania:
	//						parsedBeatmap.general.TryGetValue("AudioFilename", out externalAudioFileName);
	//						break;
	//					case BeatmapType.Quaver:
	//						parsedBeatmap.general.TryGetValue("AudioFile", out externalAudioFileName);
	//						break;
	//				}
	//			}
	//			// REMOVE
	//			foreach (var point in parsedBeatmap.timingPoints)
	//			{
	//				Main.NewText($"TIME: {point.Time}, BPM: {point.BPM}");
	//			}
	//			foreach (var point in parsedBeatmap.sliderVelocityPoints)
	//			{
	//				Main.NewText($"TIME: {point.Time}, SLIDEMULT: {point.SliderVelocity}");
	//			}
	//			if (externalAudioFileName == null)
	//			{
	//				Main.NewText("Audio file path is empty!");
	//				EndTrack();
	//				return false;
	//			}
	//			Music music = external ? GetMusic(externalPath + $"/{externalAudioFileName}", out format, true) : GetMusic(fileName, out format);
	//			if (music == null)
	//			{
	//				Main.NewText("Song unable to be loaded!");
	//				EndTrack();
	//				return false;
	//			}
	//			try
	//			{
	//				_song = music;
	//				_song.Reset();
	//				_song.Play();
	//				_song.SetVariable("Volume", Main.musicVolume);
	//			}
	//			catch (Exception e)
	//			{
	//				Main.NewText(e.Message);
	//				EndTrack();
	//				return false;
	//			}

	//			_beatmap = parsedBeatmap.hitObjects;

	//			beatmapVisualCoroutines = new List<Coroutine>();
	//			if (visualSequences != null)
	//			{
	//				foreach (IEnumerator enumerator in visualSequences)
	//				{
	//					beatmapVisualCoroutines.Add(new Coroutine(enumerator));
	//					BeatmapVisualInterface.SetState(new BeatmapVisualUIState());
	//				}
	//			}
	//		}

	//		if (drawUiStateOverride != null) drawUiStateOverride = new Coroutine(uiStateOverride);
	//		if (uiStateShader != null) this.uiStateShader = new Coroutine(uiStateShader);
	//		endTrackCallback = endCallback;

	//		beatmapCoroutines = new List<Coroutine>();

	//		// Different header offsets for different file formats

	//		float headerOffset = 0;
	//		if (format != null)
	//		{
	//			switch (format)
	//			{
	//				case "mp3":
	//					headerOffset = 0.1f;
	//					break;
	//				case "ogg":
	//					headerOffset = 0.12f;
	//					break;
	//			}
	//		}

	//		conductor = new Conductor(initialBpm, initialOffset, headerOffset);
	//		if (sequences != null)
	//		{
	//			foreach (IEnumerator enumerator in sequences)
	//			{
	//				beatmapCoroutines.Add(new Coroutine(enumerator));
	//			}
	//		}
	//		return true;
	//	}
	//	public void ResetTrack()
	//	{
	//		_song = null;
	//		conductor = null;
	//	}
	//	public void EndTrack()
	//	{
	//		drawUiStateOverride = null;
	//		uiStateShader = null;
	//		endTrackCallback?.Invoke();
	//		currentSliderVelocityMult = 1; // Reset
	//		TrackActive = false;
	//		beatmapCoroutines = null;
	//		beatmapVisualCoroutines = null;
	//		if (Main.netMode != NetmodeID.Server)
	//		{
	//			RhythmHandler.RhythmUI.judgementCounter = null;
	//			RhythmHandler.inst.chart.sprites.Clear();
	//			try
	//			{
	//				_song?.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.Immediate);
	//			}
	//			catch { }
	//			BeatmapVisualInterface.SetState(null);
	//		}
	//		ResetTrack();
	//	}
	//}
}
