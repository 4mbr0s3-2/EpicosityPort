using EpicosityPort.Systems.Network;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EpicosityPort.Systems.Rhythm
{
    internal class RhythmPlayerSyncComboBarHandler : PacketHandler
	{
		public const byte SyncAll = 1;

		public override void HandlePacket(BinaryReader reader, int fromWho)
		{
			switch (reader.ReadByte())
			{
				case (SyncAll):
					ReceiveSyncAll(reader, fromWho);
					break;
			}
		}

		public void SendSyncAll
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
			ModPacket packet = GetPacket(SyncAll, fromWho);
			packet.Write(comboDepletionRate);
			packet.Write(comboMeter);
			packet.Write(bodyFramePose);
			packet.Write(poseAnimationTimer);
			packet.Write(bodyDirectionPose);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveSyncAll(BinaryReader reader, int fromWho)
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
				SendSyncAll(-1, fromWho, comboDepletionRate, comboMeter, bodyFramePose, poseAnimationTimer, bodyDirectionPose);
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
}
