using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.BeeBeats
{
	public class BeeBeats : RhythmicWeaponBase
	{
        //public override string ComboBarBonusText()
        //{
        //	return "Shoot multiple giant bees";
        //}
        public override LocalizedText ComboBarBonusText()
        {
            return this.GetLocalization(nameof(ComboBarBonusText));
        }
        public override void SetRhythmistDefaults()
		{
			Item.damage = 20;
			Item.UseSound = SoundID.Item97; // The Bee's Knees
			Item.shoot = ProjectileID.Bee;
			Item.shootSpeed = 10;
			//Item.holdStyle = ItemHoldStyleID.HoldingOut;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.rare = ItemRarityID.Blue;
		}
		public override void HoldStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation = player.MountedCenter + new Vector2(-6 * player.direction, 18) * new Vector2(1, player.gravDir);
		}
		public override RhythmHandler.GameplayType GetGameplayType() => RhythmHandler.GameplayType.Note;
		public override RhythmHandler.GameplayNoteDisplayStyle GetGameplayNoteDisplayStyle => RhythmHandler.GameplayNoteDisplayStyle.FourKey;
		public override int NoteRate => 24;
		public override float GetComboDepletionRate() => 0.002f;
		public override string Get4KSkinName() => ""; // TODO: Add

		public override bool RhythmShoot(Vector2 velocity, int calculatedDamage, float calculatedKnockback, float comboMeter, Vector2 targetPosition, int targetWhoAmI = -1)
		{
			// If comboMeter maxxed, shoot 3 instead of 1
			if (comboMeter < 1)
			{
				return false;
			}
			else
			{
				RhythmHandler.Instance.PlayComboHitSound();
				for (int x = 0; x < 5; x++)
				{
					// Simple projectile, we don't need to pass any special parameters
					RhythmHandler.Instance.CreateRhythmistProjectileWithEffects
					(
						Main.LocalPlayer.Center,
						velocity.RotatedByRandom(MathHelper.TwoPi),
						ProjectileID.Bee,
						(int)(calculatedDamage * 0.72f),
						calculatedKnockback,
						Main.LocalPlayer.whoAmI,
						0f,
						0f
					);
				}
				return true;
			}
		}
	}
}
