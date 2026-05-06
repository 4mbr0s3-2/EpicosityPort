using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.WoodenDancePad
{
	public class WoodenDancePad : RhythmicWeaponBase
	{
		public override string ComboBarBonusText()
		{
			return "Shoot three spreaded pellets";
		}
		public override void SetRhythmistDefaults()
		{
			Item.damage = 8;
			Item.shoot = ModContent.ProjectileType<WoodenPellet>();
			Item.shootSpeed = 13;
		}

		public override RhythmHandler.GameplayType GetGameplayType() => RhythmHandler.GameplayType.Note;
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 9);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override int NoteRate => 40;
		public override float GetComboDepletionRate() => 0.002f;
		public override string Get4KSkinName() => "Wooden";
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
				for (int x = 0; x < 3; x++)
				{
					// Simple projectile, we don't need to pass any special parameters
					RhythmHandler.Instance.CreateRhythmistProjectileWithEffects
					(
						Main.LocalPlayer.Center,
						velocity.RotatedBy((x - 1) * 0.05f),
						Item.shoot,
						calculatedDamage,
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
