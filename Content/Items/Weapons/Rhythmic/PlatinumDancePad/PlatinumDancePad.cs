using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.PlatinumDancePad
{
	public class PlatinumDancePad : RhythmicWeaponBase
    {
		public override string ComboBarBonusText()
		{
			return "Shoot three pellets in a line";
		}
		public override void SetRhythmistDefaults()
        {
            Item.damage = 12;
            Item.shoot = ModContent.ProjectileType<PlatinumPellet>();
            Item.shootSpeed = 15;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 9);
            recipe.Register();
        }
        public override RhythmHandler.GameplayType GetGameplayType() => RhythmHandler.GameplayType.Note;

		public override int NoteRate => 22;
		public override float GetComboDepletionRate() => 0.0018f;
        public override string Get4KSkinName() => "PlatinumGold";
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
                        velocity * (1f + 0.12f * x),
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
