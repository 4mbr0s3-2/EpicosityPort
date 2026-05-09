using Terraria;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using EpicosityPort.Systems.Network;
using ReLogic.Utilities;

namespace EpicosityPort.Utilities
{
	public static class SoundUtil
	{
        /// <summary>
        /// Can be used in situations where a method isn't called by other clients.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        //public static SoundEffectInstance PlaySoundOnAllClients(LegacySoundStyle type, Vector2 position)
        //{
        //	if (type == null)
        //		return null;

        //	return PlaySoundOnAllClients(type, (int)position.X, (int)position.Y);
        //}

        //public static SoundEffectInstance PlaySoundOnAllClients(LegacySoundStyle type, int x = -1, int y = -1)
        //{
        //	if (type == null)
        //		return null;

        //	return PlaySoundOnAllClients(type.SoundId, x, y, type.Style, type.Volume, type.GetRandomPitch());
        //}
        //public static SoundEffectInstance PlaySoundOnAllClients(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
        //{
        //	if (Main.netMode == NetmodeID.MultiplayerClient)
        //	{
        //		ModNetHandler.GetPacketHandler<SoundClientHandler>().SendTarget(-1, Main.myPlayer, type, x, y, Style, volumeScale, pitchOffset);
        //	}
        //	return SoundEngine.PlaySound(type, x, y, Style, volumeScale, pitchOffset);
        //}
        public static SlotId PlaySoundOnAllClients(SoundStyle soundStyle, Vector2? pos = null)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.GetPacketHandler<SoundClientHandler>().SendSound(-1, Main.myPlayer, soundStyle, pos);
            }
            return SoundEngine.PlaySound(soundStyle, pos);
        }

        internal static void SetSoundInstancePanAndVolume(SoundEffectInstance sound, float x, float y)
		{
			SetSoundInstancePanAndVolume(sound, (int)x, (int)y);
		}

		/// <summary>
		/// Sets the pan and volume of the given sound instance based on the given coordinates. Will also change volume based on local sound volume setting.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void SetSoundInstancePanAndVolume(SoundEffectInstance instance, int x = -1, int y = -1, bool multiplySoundVolume = true)
		{
			bool center = false;
			float volume = 1f;
			float pan = 0f;
			Rectangle value = new Rectangle((int)(Main.screenPosition.X - Main.screenWidth * 2), (int)(Main.screenPosition.Y - Main.screenHeight * 2), Main.screenWidth * 5, Main.screenHeight * 5);
			Rectangle rectangle = new Rectangle(x, y, 1, 1);
			Vector2 centerOfScreen = new Vector2(Main.screenPosition.X + Main.screenWidth * 0.5f, Main.screenPosition.Y + Main.screenHeight * 0.5f);
			if (rectangle.Intersects(value))
				center = true;

			if (center)
			{
				pan = (x - centerOfScreen.X) / (Main.screenWidth * 0.5f);
				float distFromCenterX = Math.Abs(x - centerOfScreen.X);
				float distFromCenterY = Math.Abs(y - centerOfScreen.Y);
				float distFromCenter = (float)Math.Sqrt(distFromCenterX * distFromCenterX + distFromCenterY * distFromCenterY);
				volume = 1f - distFromCenter / (Main.screenWidth * 1.5f);
			}

			if (pan < -1f)
				pan = -1f;

			if (pan > 1f)
				pan = 1f;

			if (volume > 1f)
				volume = 1f;

			if (volume < 0) volume = 0;

			instance.Pan = pan;
			instance.Volume = volume * (multiplySoundVolume ? Main.soundVolume : 1);
		}
		internal class SoundClientHandler : PacketHandler
		{
			// RS: Lmao I really copy-pasted from that wiki
			//public const byte SyncTarget = 1;
			public override void HandlePacket(BinaryReader reader, int fromWho)
			{
				reader.ReadByte();
				ReceiveSound(reader, fromWho);
			}
			public void SendSound(int toWho, int fromWho, SoundStyle soundStyle, Vector2? pos)
			{
				ModPacket packet = GetPacket(0, fromWho);
				packet.Write(soundStyle.SoundPath);
                packet.Write(soundStyle.Volume);
                packet.Write(soundStyle.Pitch);
				packet.Write(pos != null);
				if (pos != null)
					packet.WriteVector2((Vector2)pos);
				packet.Send(toWho, fromWho);
			}

			public void ReceiveSound(BinaryReader reader, int fromWho)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					fromWho = reader.ReadByte(); // Read fromWho as sent from server
				}
				string soundPath = reader.ReadString();
				float volume = reader.ReadSingle();
				float pitch = reader.ReadSingle();
				Vector2? pos = null;
				if (reader.ReadBoolean())
					pos = reader.ReadVector2();
				//int type = reader.ReadInt32();
				//int x = reader.ReadInt32();
				//int y = reader.ReadInt32();
				//int Style = reader.ReadInt32();
				//float volumeScale = reader.ReadSingle();
				//float pitchOffset = reader.ReadSingle();
				//Main.NewText($"Received request to play SoundId {type} from {fromWho}");
				if (Main.netMode == NetmodeID.Server)
				{
					SendSound(-1, fromWho, new SoundStyle(soundPath) { Pitch = pitch, Volume = volume }, pos);
				}
				else
				{
					SoundEngine.PlaySound(new SoundStyle(soundPath) { Pitch = pitch, Volume = volume }, pos);
				}
			}
		}
	}
}
