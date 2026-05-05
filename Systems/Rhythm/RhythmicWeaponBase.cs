using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static EpicosityPort.Systems.Rhythm.RhythmHandler;

namespace EpicosityPort.Systems.Rhythm
{
	public abstract class RhythmicWeaponBase : ModItem
	{
		public abstract GameplayType GetGameplayType();
		public virtual MouseTrackPatternObject ForcedMouseTrackPattern
		{
			get
			{
				return null;
			}
		}
		public virtual FourKeyTrackPatternObject ForcedArrowTrackPattern
		{
			get
			{
				return null;
			}
		}
		public virtual string Get4KSkinName()
		{
			return null;
		}
		public virtual int NoteRate
		{
			get
			{
				return 20;
			}
		}
		/// <summary>
		/// Only used if gameplay style is note-based.
		/// </summary>
		public virtual GameplayNoteDisplayStyle GetGameplayNoteDisplayStyle
		{
			get
			{
				return GameplayNoteDisplayStyle.FourKey;
			}
		}
		public abstract float GetComboDepletionRate();
		/// <summary>
		/// By default, the combo increment is 0.
		/// </summary>
		/// <returns></returns>
		public virtual float GetComboIncrement()
		{
			return 0;
		}
		public sealed override void SetDefaults()
		{
			SetRhythmistDefaults();
			// These can be changed or made hybrid by alt-uses.
			//Item.thrown = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			//Item.melee = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			//Item.ranged = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			//Item.magic = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			//Item.summon = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			Item.DamageType = DamageClass.Generic;
			RhythmicItemInfo info = Item.GetGlobalItem<RhythmicItemInfo>();
			info.rhythmic = true;
			info.gameplayNoteDisplayStyle = GetGameplayNoteDisplayStyle;
			info.gameplayType = GetGameplayType();
			info.forcedArrowTrackPattern = ForcedArrowTrackPattern;
			info.forcedMouseTrackPattern = ForcedMouseTrackPattern;
			//info.defaultTrackSpeed = GetTrackSpeed();
			info.defaultNoteRate = NoteRate;
			info.defaultComboDepletionRate = GetComboDepletionRate();
			info.defaultComboIncrement = GetComboIncrement();
			info.fourKeySkin = Get4KSkinName();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			ModPlayerRhythmicPlayerStats stats = player.GetModPlayer<ModPlayerRhythmicPlayerStats>();
			damage = new StatModifier(stats.flatRhythmicDamage, stats.rhythmicDamageMult);
		}
		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit = player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicCrit;
		}
		public virtual void SetRhythmistDefaults() {}
		/// <summary>
		/// Return true to override default shooting function. Call netUpdate2 on all projectiles to sync targetPosition and targetWhoAmI.
		/// </summary>
		/// <param name="velocity">The calculated velocity from the player to the target.</param>
		/// <param name="calculatedDamage">The calculated damage that takes into account the combo meter.</param>
		/// <param name="calculatedKnockback">The calculated damage that takes into account the knockback.</param>
		/// <param name="comboMeter">The comboMeter value at the time the projectile is shot.</param>
		/// <param name="targetPosition"></param>
		/// <param name="targetWhoAmI"></param>
		/// <returns></returns>
		public virtual bool RhythmShoot(Vector2 velocity, int calculatedDamage, float calculatedKnockback, float comboMeter, Vector2 targetPosition, int targetWhoAmI = -1) 
		{
			return false;
		}
		/// <summary>
		/// Return a string that will be used as the tooltip for the full combo bar bonus.
		/// </summary>
		/// <returns></returns>
		public virtual string ComboBarBonusText() { return ""; }
	}
}
