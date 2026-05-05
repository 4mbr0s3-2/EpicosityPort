using EpicosityPort.Systems.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using static EpicosityPort.Systems.Rhythm.RhythmHandler;

namespace EpicosityPort.Systems.Rhythm
{
	// RS: Wow, this is heavily coupled with RhythmHandler (and reasonably so)
	public class RhythmNote : Sprite
	{
		public bool beatmap = false;
		public double beatmappedTime = 0;
		public double beatmappedHoldTime = 0;
		/// <summary>Set to keep the world position of the arrow.</summary>
		public Vector2 hitPosition;
		public GameplayType gameplayType = GameplayType.Note;
		public float direction = 0;
		public int column = 0;
		public bool destroy = false;
		public bool hold = false;
		public double holdTime = 0;
		public double time = 0;
		public double trueTime = 0;
		/// <summary>Used for mouse-based gameplay for animations.</summary>
		public float animationTime = 0;
		public bool initialHit = false;
		public float timeSinceDestroyed = 0;
		public int bodyFrame = 0;
		public int bodyDirection = 0;
		//public LegacySoundStyle legacySoundStyle = null;
		public Texture2D associatedDanceTexture = null;
		public Rectangle associatedDanceFrame = default;
		// Note-based
		/// <summary>
		/// Construct a key-based note for both DDR and GH gameplay styles.
		/// </summary>
		/// <param name="column">The column the note is in (0-3).</param>
		/// <param name="time">The time (in track-speed units) of the note. This is separate from the stored time of the hit objects.</param>
		/// <param name="bodyFrame">The frame the player will pose after hitting the note. Set to -1 for no pose.</param>
		/// <param name="bodyDirection">The direction the player will face after hitting the note. Set to 0 for no direction change.</param>
		/// <param name="holdTime">If greater than 0, the note is a hold note. Dictates how long the note should be held (in track-speed units).</param>
		/// <param name="beatmap">Whether or not the note is part of a beatmap.</param>
		/// <param name="beatmappedTime">In seconds, the time of the note along the beatmap.</param>
		public RhythmNote(int column, double time, int bodyFrame = -1, int bodyDirection = 0, double holdTime = 0, bool beatmap = false, double beatmappedTime = 0, double beatmappedHoldTime = 0)
			: base(RhythmHandler.Instance.Interface.RhythmUIStateInstance.texPlayfieldArrow, new Vector2(0.5f), Vector2.Zero, default, default)
		{
			switch (column)
			{
				case 0:
					rotation = (float)-Math.PI / 2;
					break;
				case 1:
					rotation = (float)Math.PI;
					break;
				case 3:
					rotation = (float)Math.PI / 2;
					break;
			}
			
			gameplayType = GameplayType.Note;
			this.column = column;
			this.time = time;
			this.holdTime = holdTime;
			this.bodyFrame = bodyFrame;
			this.bodyDirection = bodyDirection;
			this.beatmap = beatmap;
			this.beatmappedTime = beatmappedTime;
			this.beatmappedHoldTime = beatmappedHoldTime;
			hold = holdTime > 0;
		}
		public Vector2 GetReceptorPos()
		{
			Vector2 displayLocation = new Vector2(90, RhythmNotes.Instance.notesUpscroll ? 70 : 340);
			Vector2 arrowLocation = displayLocation + new Vector2(RhythmHandler.Instance.Interface.RhythmUIStateInstance.ArrowXSpace, 0) * column;
			return arrowLocation;
		}
		public override void Draw(SpriteBatch sb)
		{
			if (hold)
			{
				// Stream
				for (float i = 0; i <= holdTime; i += 1)
				{
					var targetPos = GetArrowPos(time + i + 1);
					sb.Draw(
						tex,
						new Rectangle(
							(int)targetPos.X,
							(int)targetPos.Y,
							(int)(32 * scale),
							(int)(32 * scale)),
						null,
						Color.White * opacity * 0.3f,
						rotation,
						tex.Size() / 2,
						SpriteEffects.None,
						0
					);
				}
				var endPos = GetArrowPos(time + holdTime + 1);
				// End
				sb.Draw(
					tex,
					new Rectangle(
						(int)endPos.X,
						(int)endPos.Y,
						(int)(32 * scale),
						(int)(32 * scale)),
					null,
					Color.White * opacity,
					rotation,
					tex.Size() / 2,
					SpriteEffects.None,
					0
				);
			}

			base.Draw(sb);
		}
		public void Update(GameTime gt, ModPlayerRhythmist dancePlayer, bool[] justPressed, ref bool[] alreadyHitCols)
		{
			UpdateOpacity();
			UpdatePosition();

			int timingWindow = EpicosityConfigClient.Instance.TimingWindow;
			// Attempt to get the notes closest to the control arrows
			if (!initialHit)
			{
				//if (beatmap && RhythmBeatmapHandler.inst?.conductor != null)
				//{
				//	time = ((beatmappedTime - RhythmBeatmapHandler.inst.conductor.syncedSongPosition) * 60f + EpicosityConfigClient.Instance.FKBeatmapOffset) * dancePlayer.trackSpeed;
				//	trueTime = time / dancePlayer.trackSpeed;
				//	holdTime = beatmappedHoldTime * 60 * dancePlayer.trackSpeed;
				//}
				//else
				//{
					time -= dancePlayer.trackSpeed;
					trueTime = time;
				//}
			}
			if (destroy) goto DestroyChecks;
			bool notHit = false;
			// Mark 'initialHit'
			if (Math.Abs(time) <= timingWindow && !initialHit && justPressed[column] && !alreadyHitCols[column]) // Hit range
			{
				alreadyHitCols[column] = true;
				dancePlayer.bodyFramePose = bodyFrame;
				dancePlayer.bodyDirectionPose = bodyDirection;
				dancePlayer.poseAnimationTimer = 10;
				initialHit = true;
				if (!hold)
				{
					destroy = true;
					RhythmHandler.Instance.NoteHit((float)trueTime);
				}
				else
				{
					if (!beatmap) holdTime += time;
					time = 0; // Center note
				}
			}
			else
			{
				notHit = true;
			}
			bool disableAntiMash = EpicosityConfigClient.Instance.DisableAntiMashingSystem;
			if (disableAntiMash && notHit)
			{
				// If hit beyond the timing window, check if within range of penalty
				if (Math.Abs(time) <= timingWindow + EpicosityConfigClient.Instance.ExperimentalPenaltyRange && !initialHit && justPressed[column] && !alreadyHitCols[column])
				{
					alreadyHitCols[column] = true;
					initialHit = true;
					destroy = true;
                    RhythmHandler.Instance.MissPenalty(dancePlayer, beatmap, 15);
				}
			}
			// Check if hold note is still being held down, subtract from holdTime
			if (initialHit && hold && RhythmHandler.Instance.heldDown[column])
			{
				//if (beatmap)
				//{
				//	holdTime = ((beatmappedTime + beatmappedHoldTime - RhythmBeatmapHandler.inst.conductor.syncedSongPosition) * 60 + EpicosityConfigClient.Instance.FKBeatmapOffset) * dancePlayer.trackSpeed;
				//}
				//else
				//{
					holdTime -= dancePlayer.trackSpeed;
				//}
			}
			if (hold && initialHit)
			{
				// If the player has let go and the hold time is in the timing window, it's a hit
				if (RhythmHandler.Instance.prevHeldDown[column] && !RhythmHandler.Instance.heldDown[column])
				{
					destroy = true;
					if (Math.Abs(holdTime) <= timingWindow)
					{
						RhythmHandler.Instance.NoteHit((float)(holdTime / dancePlayer.trackSpeed), default, 1.2f);
					}
					else
					{
						RhythmHandler.Instance.MissPenalty(dancePlayer, beatmap, 15);
					}
				}
			}

			DestroyChecks:
			// Dispose of notes after destroy animation
			if (destroy && timeSinceDestroyed >= 20)
			{
				RhythmHandler.Instance.chart.Remove(this);
				Dispose();
				return;
			}
			// Offchart
			if (trueTime <= -30)
			{
                RhythmHandler.Instance.MissPenalty(dancePlayer, beatmap, 20);
                RhythmHandler.Instance.chart.Remove(this);
				Dispose();
				return;
			}
			if (column > 3)
			{
                RhythmHandler.Instance.chart.Remove(this);
				Dispose();
				return;
			}
		}

		private void UpdatePosition()
		{
			position = GetArrowPos(time);
		}

		public Vector2 GetArrowPos(double time)
		{
			var recPos = GetReceptorPos();
			int negator = RhythmNotes.Instance.notesUpscroll ? -1 : 1;
			return recPos + new Vector2(0, (float)(-time / 60f * RhythmHandler.Instance.playfieldHeight * negator));
			//return recPos + new Vector2((float)Math.Pow(Math.Tan(time / 60 * 25), 9) * 30, (float)(-time / 60f * inst.playfieldHeight * negator));
		}

		private void UpdateOpacity()
		{
			scale = 1;
			if (!destroy)
			{
				float timeThresholdSize = 10;
				float timeThreshold = 60 - timeThresholdSize;
				if (time > timeThreshold)
				{
					opacity = (float)(1 - (time - timeThreshold) / timeThresholdSize);
				}
				if (time >= -30 && time < 0)
				{
					opacity = (float)(1 - time / -30 * 2);
				}
			}
			else
			{
				timeSinceDestroyed++;
				if (!EpicosityConfigClient.Instance.ReduceNoteHitEffect)
				{
					scale = 1 + timeSinceDestroyed / 20.0f * 1.5f;
				}
				opacity = 1 - (timeSinceDestroyed / 20.0f);
			}
		}
		//[Obsolete]
		//public RhythmNote(float direction, double time, int bodyFrame = -1, int bodyDirection = 0, Texture2D associatedDanceTexture = null, Rectangle associatedDanceFrame = default, LegacySoundStyle legacySoundStyle = null)
		//{
		//	gameplayType = GameplayType.Swipe;
		//	this.direction = direction;
		//	this.time = time;
		//	this.bodyFrame = bodyFrame;
		//	this.bodyDirection = bodyDirection;
		//	this.associatedDanceTexture = associatedDanceTexture;
		//	this.associatedDanceFrame = associatedDanceFrame;
		//	this.legacySoundStyle = legacySoundStyle;
		//}
	}
}
