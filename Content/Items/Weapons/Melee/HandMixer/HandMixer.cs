using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.HandMixer
{
    class HandMixer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hand Mixer");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 10;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.knockBack = 2;
        }
        public override void UseItemHitbox(Player player, ref Microsoft.Xna.Framework.Rectangle hitbox, ref bool noHitbox)
        {
            hitbox.Location = new Microsoft.Xna.Framework.Vector2(player.itemLocation.X + player.itemWidth * player.direction, player.itemLocation.Y).ToPoint();
        }
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            return true;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Microsoft.Xna.Framework.Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
        }
    }
}
