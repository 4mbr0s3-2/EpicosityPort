using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.WeaponizedChairs
{
    public class ChairWeaponGlobalItem : GlobalItem
    {
        class WoodenChairGore : ModGore
        {
            public override string Texture => $"EpicosityPort/Content/Gores/WoodenChair1";
        }
        class WoodenChairGore2 : ModGore
        {
            public override string Texture => $"EpicosityPort/Content/Gores/WoodenChair2";
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.WoodenChair)
            {
                item.damage = 20;
                item.consumable = true;
                item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
                item.knockBack = 5;
            }
        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.type == ItemID.WoodenChair)
            {
                item.stack--;
                Gore.NewGore(player.GetSource_FromThis(), player.position, player.velocity + new Vector2(player.direction, 0) * 2, ModContent.GoreType<WoodenChairGore>(), 1);
                Gore.NewGore(player.GetSource_FromThis(), player.position, player.velocity + new Vector2(player.direction, 0) * 2, ModContent.GoreType<WoodenChairGore2>(), 1);
                SoundEngine.PlaySound(new SoundStyle("EpicosityPort/SharedSounds/BarrelDestroy"), player.position);
                for (int x = 0; x <= 5; x++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.position,
                                player.velocity + new Vector2(player.direction, -0.5f).RotatedBy(Main.rand.NextFloat()) * 2,
                                ModContent.ProjectileType<Splinter>(),
                                3,
                                2,
                                player.whoAmI);
                }
            }
        }
    }
}
