using EpicosityPort.Systems.Network;
using EpicosityPort.Systems.UserInterface.Rhythm;
using EpicosityPort.Utilities;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace EpicosityPort.Systems.Rhythm
{
	public class ModPlayerRhythmist : ModPlayer
	{
		internal class RhythmPlayerSyncComboBarHandler : PacketHandler
		{
			public const byte SyncTarget = 1;

			public RhythmPlayerSyncComboBarHandler(byte handlerType) : base(handlerType)
			{
			}

			public override void HandlePacket(BinaryReader reader, int fromWho)
			{
				switch (reader.ReadByte())
				{
					case (SyncTarget):
						ReceiveTarget(reader, fromWho);
						break;
				}
			}

			public void SendTarget
			(
				int toWho,
				int fromWho,
				float comboDepletionRate,
				float comboMeter,
				int bodyFramePose,
				int poseAnimationTimer,
				int bodyDirectionPose
			)
			{
				ModPacket packet = GetPacket(SyncTarget, fromWho);
				packet.Write(comboDepletionRate);
				packet.Write(comboMeter);
				packet.Write(bodyFramePose);
				packet.Write(poseAnimationTimer);
				packet.Write(bodyDirectionPose);
				packet.Send(toWho, fromWho);
			}

			public void ReceiveTarget(BinaryReader reader, int fromWho)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					fromWho = reader.ReadByte(); // fromWho sent from server
				}
				float comboDepletionRate = reader.ReadSingle();
				float comboMeter = reader.ReadSingle();
				int bodyFramePose = reader.ReadInt32();
				int poseAnimationTimer = reader.ReadInt32();
				int bodyDirectionPose = reader.ReadInt32();
				if (Main.netMode == NetmodeID.Server)
				{
					SendTarget(-1, fromWho, comboDepletionRate, comboMeter, bodyFramePose, poseAnimationTimer, bodyDirectionPose);
				}
				else
				{
					ModPlayerRhythmist dancePlayer = Main.player[fromWho].GetModPlayer<ModPlayerRhythmist>();
					dancePlayer.comboDepletionRate = comboDepletionRate;
					dancePlayer.comboMeter = comboMeter;
					dancePlayer.bodyFramePose = bodyFramePose;
					dancePlayer.poseAnimationTimer = poseAnimationTimer;
					dancePlayer.bodyDirectionPose = bodyDirectionPose;
				}
			}
		}
		/// <summary>The value removed from each arrow's time every tick.</summary>
		public double trackSpeed = 0.5;

		/// <summary>The number of ticks between each note.</summary>
		public int noteRate = 60;

		/// <summary>The value removed from the combo meter every tick.</summary>
		public float comboDepletionRate = 0.003f * 0.5f;

		public int bodyFramePose = 0;
		public int poseAnimationTimer = 0;
		public int bodyDirectionPose = 0;

		public float comboMeter = 0;
		public int combo = 0;

		public override void PostUpdate()
		{
			if (Main.LocalPlayer.HeldItem.type != ItemID.None)
			{
				if (RhythmHandler.Instance.toggleDoubleTapCooldown > 0)
				{
                    RhythmHandler.Instance.toggleDoubleTapCooldown -= 1;
				}
				if (Main.LocalPlayer.HeldItem.GetGlobalItem<RhythmicItemInfo>().rhythmic && Main.mouseRight && !RhythmHandler.Instance.lastMouseRight && !Main.LocalPlayer.mouseInterface)
				{
                    // Toggle targeting
                    RhythmHandler.Instance.targeting = !RhythmHandler.Instance.targeting;
					bool hasLocked = false;
					if (RhythmHandler.Instance.toggleDoubleTapCooldown > 0)
					{
						hasLocked = true;
                        RhythmHandler.Instance.LockTarget(RhythmHandler.Instance.targetWhoAmI);
					}
                    RhythmHandler.Instance.toggleDoubleTapCooldown = 10;
					if (!hasLocked)
					{
						if (RhythmHandler.Instance.targeting)
						{
                            SoundEngine.PlaySound(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/TargetEnable"));
						}
						else
						{
                            SoundEngine.PlaySound(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/TargetDisable"));
                            RhythmHandler.Instance.lockedTargetIndex = -1;
                            RhythmHandler.Instance.uiSound.Volume = Main.soundVolume;
                            RhythmHandler.Instance.uiSound.Play();
						}
					}
				}
                RhythmHandler.Instance.lastMouseRight = Main.mouseRight; // TODO: move these somewhere more appropriate probably
			}

			// Combo meter called outside update function of UI so it can drain in the background
			comboMeter -= comboDepletionRate;

			if (comboMeter < 0)
			{
				comboMeter = 0;
			}
			if (comboMeter > 1)
			{
				comboMeter = 1;
			}
			if (poseAnimationTimer > 0)
			{
				poseAnimationTimer--;
				if (bodyFramePose >= 0)
				{
					Player.bodyFrame.Y = Player.bodyFrame.Height * bodyFramePose;
				}
				if (bodyDirectionPose != 0)
				{
					Player.ChangeDir(bodyDirectionPose);
				}
				if (poseAnimationTimer == 0)
				{
					bodyDirectionPose = 0;
				}
			}
		}
	}
}
