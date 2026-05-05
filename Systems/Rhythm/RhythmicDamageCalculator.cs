using Terraria;

using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace EpicosityPort.Systems.Rhythm
{
	public static class RhythmicDamageCalculator
    {
        [Obsolete]
        public static int GetRhythmicDamage(Player player, Item item)
        {
            ModPlayerRhythmicPlayerStats rhythmicPlayerStats = player.GetModPlayer<ModPlayerRhythmicPlayerStats>();
            float damageMult = rhythmicPlayerStats.rhythmicDamageMult;
            return (int)(item.damage * damageMult) + rhythmicPlayerStats.flatRhythmicDamage;
        }
    }
    public class ModPlayerRhythmicPlayerStats : ModPlayer
    {
        protected override bool CloneNewInstances => true;
        /// <summary>
        /// Make accessories add to this to increase the player's choreographic damage.
        /// </summary>
        public int flatRhythmicDamage = 0;
        public float rhythmicDamageMult = 1;
        public float rhythmicComboDepletionRateMult = 1;
        public float rhythmicComboIncrementMult = 1;
        public float rhythmicNoteRateMult = 1;
        public double rhythmicTrackSpeedMult = 1;
        public int rhythmicCrit = 4;
		public override void PreUpdateBuffs()
        {
            if (Player.HasBuff(BuffID.Rage))
            {
                rhythmicCrit += 10;
            }
        }
        public bool GetRhythmicCrit()
		{
            return Main.rand.Next(1, 101) <= rhythmicCrit;
        }
		//public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)/* tModPorter Note: Removed. Use ModifyHurt on the receiving player and check modifiers.PvP. Use modifiers.DamageSource.SourcePlayerIndex to get the attacking player */
  //      {
  //          if (proj.ModProjectile is RhythmicProjectile)
  //          {
  //              crit = GetRhythmicCrit();
  //          }
  //      }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            // This will allow the crits to occur
            if (proj.ModProjectile is RhythmicProjectile)
            {
                if (GetRhythmicCrit()) modifiers.SetCrit();
            }
        }
        public override void ResetEffects()
        {
            // Resets the multiplier at the beginning
            // Accessories will then stack and add on to the multiplier
            flatRhythmicDamage = 0;
            rhythmicDamageMult = 1;
            rhythmicComboDepletionRateMult = 1;
            rhythmicComboIncrementMult = 1;
            rhythmicNoteRateMult = 1;
            rhythmicTrackSpeedMult = 1;
            rhythmicCrit = 4;
        }
    }
}
