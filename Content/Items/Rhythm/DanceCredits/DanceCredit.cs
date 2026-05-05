using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public abstract class DanceCredit : ModItem
    {
        float cycleAnimation = 0;
        public virtual ChoreabilityTier GetTier() { return ChoreabilityTier.Copper; }
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 32;
            Item.height = 32;
        }
        public override bool OnPickup(Player player)
        {
            SoundEngine.PlaySound(SoundID.Coins, Item.position);
            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            cycleAnimation += (float)Math.PI * 2 / 60f;
            cycleAnimation %= (float)Math.PI * 2;
            Texture2D itemTexture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 itemDimensions = new Vector2(Item.width, Item.height);
            spriteBatch.Draw
            (
                itemTexture,
                new Rectangle((int)Item.position.X - (int)Main.screenPosition.X + (int)(itemDimensions.X / 2), (int)Item.position.Y - (int)Main.screenPosition.Y + (int)(itemDimensions.Y / 2) + (int)(Math.Cos(cycleAnimation) * 4), (int)(itemTexture.Width * (float)Math.Abs(Math.Sin(cycleAnimation))), itemTexture.Height),
                itemTexture.Frame(),
                lightColor,
                rotation,
                new Vector2(itemDimensions.X, itemDimensions.Y) / 2f,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}
