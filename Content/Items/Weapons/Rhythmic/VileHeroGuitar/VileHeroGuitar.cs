using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Rhythmic.VileHeroGuitar
{
	public class VileHeroGuitar : RhythmicWeaponBase
    {
        //public override string ComboBarBonusText()
        //{
        //    return "Shoot three spreaded vilespikes";
        //}
        public override LocalizedText ComboBarBonusText()
        {
            return this.GetLocalization(nameof(ComboBarBonusText));
        }
        public override void SetRhythmistDefaults()
        {
            Item.damage = 20;
            Item.shoot = ModContent.ProjectileType<Vilespike>();
            Item.shootSpeed = 10;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.rare = ItemRarityID.Blue;
		}
		public override void HoldStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation = player.MountedCenter + new Vector2(-6 * player.direction, 18);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 12);
			recipe.AddIngredient(ItemID.ShadowScale, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override RhythmHandler.GameplayType GetGameplayType() => RhythmHandler.GameplayType.Note;
		public override RhythmHandler.GameplayNoteDisplayStyle GetGameplayNoteDisplayStyle => RhythmHandler.GameplayNoteDisplayStyle.GuitarHero;
		public override int NoteRate => 25;
		public override float GetComboDepletionRate() => 0.0018f;
        //public override string GetArrowSkinName() => "PlatinumGold";

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
						velocity.RotatedBy((x - 1) * 0.1f),
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
			//return false;
        }
	}
}
