using EpicosityPort;
using EpicosityPort.Systems.Network;
using EpicosityPort.Systems.Rhythm;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EpicosityPort.Systems.Rhythm.ModPlayerRhythmist;
using static EpicosityPort.Systems.Rhythm.RhythmHandler;
using static EpicosityPort.Systems.Rhythm.RhythmPatterns;

namespace EpicosityPort.Systems.UserInterface.Rhythm
{
	public class ModPlayerShowRhythmOverlay : ModPlayer
	{
		public int lastRhythmItemHeldType = -1;
		public override void PostUpdate()
		{
			if (Main.netMode == NetmodeID.Server) return; // You don't want any of this loading on the server
			if (Main.LocalPlayer.HeldItem.type != ItemID.None)
			{
				ModPlayerRhythmist dancePlayer = GetLocalRhythmPlayer();
				ModPlayerRhythmicPlayerStats rhythmistPlayerStats = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmicPlayerStats>();
				RhythmicItemInfo dancerItemInfo = Main.LocalPlayer.HeldItem.GetGlobalItem<RhythmicItemInfo>();
				if (dancerItemInfo.rhythmic && RhythmHandler.Instance.Interface.RhythmUserInterface.CurrentState == null && dancerItemInfo.gameplayType != GameplayType.Miscellaneous)
				{
					// Initialize
					RhythmHandler.Instance.Interface.ShowDanceUI();
                    RhythmHandler.Instance.patternArrowIndex = 0;
				}
				else if ((!dancerItemInfo.rhythmic || dancerItemInfo.gameplayType == GameplayType.Miscellaneous) && RhythmHandler.Instance.Interface.RhythmUserInterface.CurrentState != null)
				{
                    RhythmHandler.Instance.Interface.HideDanceUI();
				}
				// Update function
				if (dancerItemInfo.rhythmic)
				{
					dancePlayer.noteRate = (int)(dancerItemInfo.noteRate / rhythmistPlayerStats.rhythmicNoteRateMult);
					//dancePlayer.trackSpeed = dancerItemInfo.trackSpeed * rhythmistPlayerStats.rhythmicTrackSpeedMult;
					dancePlayer.trackSpeed = EpicosityConfigClient.Instance.ScrollSpeed;
					//if (RhythmBeatmapHandler.inst?.TrackActive == true) dancePlayer.trackSpeed *= RhythmBeatmapHandler.inst.currentSliderVelocityMult;
					dancePlayer.comboDepletionRate = dancerItemInfo.comboDepletionRate * rhythmistPlayerStats.rhythmicComboDepletionRateMult;
					gameplayType = dancerItemInfo.gameplayType;
					if (gameplayType == GameplayType.Note)
					{
						gameplayNoteDisplayStyle = dancerItemInfo.gameplayNoteDisplayStyle; // Used by note-based weapons that have different styles
						RhythmHandler.Instance.fKTrackPattern = RhythmPatterns.Instance.GetArrowTrackPattern<ArrowRandom>(); // Default
						if (dancerItemInfo.forcedArrowTrackPattern != null)
						{
                            RhythmHandler.Instance.fKTrackPattern = dancerItemInfo.forcedArrowTrackPattern;
						}
						else if (RhythmNotes.Instance.prefixTypeToFourKeyTrackPattern.ContainsKey(Main.LocalPlayer.HeldItem.prefix))
						{
                            RhythmHandler.Instance.fKTrackPattern = RhythmNotes.Instance.prefixTypeToFourKeyTrackPattern[Main.LocalPlayer.HeldItem.prefix];
						}
					}
					//else if (gameplayType == GameplayType.Swipe)
					//{
					//	mouseTrackPattern = GetMouseTrackPattern<MouseRandom>(); // Default
					//	if (dancerItemInfo.forcedMouseTrackPattern != null)
					//	{
					//		mouseTrackPattern = dancerItemInfo.forcedMouseTrackPattern;
					//	}
					//	else if (prefixTypeToFourKeyTrackPattern.ContainsKey(Main.LocalPlayer.HeldItem.prefix))
					//	{
					//		mouseTrackPattern = prefixTypeToMouseTrackPattern[Main.LocalPlayer.HeldItem.prefix];
					//	}
					//}
				}
				if (dancerItemInfo.rhythmic && lastRhythmItemHeldType != Main.LocalPlayer.HeldItem.type)
				{
                    // Reset note spawn timer
                    RhythmHandler.Instance.noteTimer = 0;

                    // Reset fretboard animation
                    RhythmHandler.Instance.Interface.chartAnimationTime = 0;

                    // Set arrow skin
                    RhythmHandler.Instance.Interface.fourKeySkinName = dancerItemInfo.fourKeySkin != null ? dancerItemInfo.fourKeySkin : "";

                    // Reset other stats
                    RhythmHandler.Instance.patternArrowIndex = 0;
					dancePlayer.comboMeter = 0;
                    RhythmHandler.Instance.chart.sprites.Clear();

					// Sync up combo meter and noteRate
					if (Main.netMode == NetmodeID.MultiplayerClient)
						ModNetHandler.GetPacketHandler<RhythmPlayerSyncComboBarHandler>().SendSyncAll(-1, Main.myPlayer, dancePlayer.comboDepletionRate, dancePlayer.comboMeter, dancePlayer.bodyFramePose, dancePlayer.poseAnimationTimer, dancePlayer.bodyDirectionPose);

					// Set to HeldItem for one-shot
					lastRhythmItemHeldType = Main.LocalPlayer.HeldItem.type;
				}
			}
			else if (RhythmHandler.Instance.Interface.RhythmUserInterface.CurrentState != null)
			{
                RhythmHandler.Instance.Interface.HideDanceUI();
			}
		}
	}
}
