using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Rhythm
{
	public abstract class RhythmicProjectile : ModProjectile
	{
		public Vector2 targetPosition = Vector2.Zero;
		public int targetNPCWhoAmI = -1;
		// We're using a bitmask here to lessen clutter and to make it easier to send data
		// Bitmasks are kinda epic ngl
		[Flags]
		public enum RhythmistProjectileEffects
		{
			None = 0,
			Flaming = 1,
			Freezing = 2
		}
		public RhythmistProjectileEffects effects = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(targetPosition);
			writer.Write(targetNPCWhoAmI);
			writer.Write((int)effects);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			targetPosition = reader.ReadVector2();
			targetNPCWhoAmI = reader.ReadInt32();
			effects = (RhythmistProjectileEffects)reader.ReadInt32();
		}
		//public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)/* tModPorter Note: Removed. Use ModifyHitPlayer and check modifiers.PvP */
		//{
		//	base.ModifyHitPvp(target, ref damage, ref crit);
		//}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			// Use bitwise & to isolate the bit
			// If returns anything other than zero, that bit is 1, which means it's true
			if (effects.HasFlag(RhythmistProjectileEffects.Flaming))
			{
				AddBuff(target, BuffID.OnFire, 300);
			}
            // RS: Huh? Does Terraria not have a slow debuff for enemies?
			//else if (effects.HasFlag(RhythmistProjectileEffects.Freezing))
			//{
			//	AddBuff(target, ModContent.BuffType<FreezingCold>(), 300);
			//}
            else if (effects.HasFlag(RhythmistProjectileEffects.Freezing))
			{
				AddBuff(target, BuffID.Frostburn, 300);
			}
		}
        public void AddBuff(Entity entity, int buffID, int time)
		{
			if (entity is Player)
			{
				Player player = entity as Player;
				player.AddBuff(buffID, time, false);
			}
			else if (entity is NPC)
			{
				NPC npc = entity as NPC;
				npc.AddBuff(buffID, time);
			}
		}
		public override void PostAI()
		{
			if (effects.HasFlag(RhythmistProjectileEffects.Flaming))
			{
				// TODO: (Through the) Fire and Flames
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
			}
		}
	}
}
