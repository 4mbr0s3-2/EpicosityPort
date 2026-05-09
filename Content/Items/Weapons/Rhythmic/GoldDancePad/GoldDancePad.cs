using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.GoldDancePad
{
	public class GoldDancePad : RhythmicWeaponBase
    {
        //public override string ComboBarBonusText()
        //{
        //    return "Shoot three pellets in a line";
        //}
        public override LocalizedText ComboBarBonusText()
        {
            return this.GetLocalization(nameof(ComboBarBonusText));
        }
        public override void SetRhythmistDefaults()
        {
            Item.damage = 10;
            Item.shoot = ModContent.ProjectileType<GoldPellet>();
            Item.shootSpeed = 12;
        }
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 9);
            recipe.Register();
		}
		public override RhythmHandler.GameplayType GetGameplayType() => RhythmHandler.GameplayType.Note;
		public override int NoteRate => 25;
		public override float GetComboDepletionRate() => 0.0016f;
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
                        velocity * (1f + 0.1f * x),
                        Item.shoot,
                        (int)(calculatedDamage * 0.8f),
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
