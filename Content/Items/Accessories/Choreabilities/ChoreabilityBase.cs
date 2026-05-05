using EpicosityPort.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    // RS: Why did I make this abstract when it doesn't have any abstract methods? Maybe it's to prevent direct instantiation.
    public abstract class ChoreabilityBase : ModItem
    {
        protected override bool CloneNewInstances => true;
        public Choreability choreability;
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            switch (choreability.tier)
            {
                case ChoreabilityTier.Copper:
                    Item.rare = Terraria.ID.ItemRarityID.Green;
                    break;
                case ChoreabilityTier.Silver:
                    Item.rare = Terraria.ID.ItemRarityID.Orange;
                    break;
                case ChoreabilityTier.Gold:
                    Item.rare = Terraria.ID.ItemRarityID.LightPurple;
                    break;
                case ChoreabilityTier.Platinum:
                    Item.rare = Terraria.ID.ItemRarityID.Lime;
                    break;
                case ChoreabilityTier.Terra:
                    Item.rare = Terraria.ID.ItemRarityID.Red;
                    break;
            }
        }
        public Texture2D GetChoreabilityTexture()
        {
            string choreabilityName = Enum.GetName(typeof(ChoreabilityType), choreability.type);
            return ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/{choreabilityName}Choreability").Value;
        }
        public Texture2D GetTierTexture()
        {
            string tierName = Enum.GetName(typeof(ChoreabilityTier), choreability.tier);
            return ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/TierCasings/{tierName}").Value;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            // Draw tier
            float newScale = 0.8f;
            spriteBatch.Draw
            (
                GetTierTexture(),
                new Rectangle((int)(Item.position.X + 20 * scale - (int)Main.screenPosition.X), (int)(Item.position.Y + 20 * scale) - (int)Main.screenPosition.Y, (int)(40 * newScale), (int)(40 * newScale)),
                new Rectangle(0, 0, 40, 40),
                lightColor,
                rotation,
                new Vector2(20, 20),
                SpriteEffects.None,
                0
            );
            // Draw choreability
            spriteBatch.Draw
            (
                GetChoreabilityTexture(),
                new Rectangle((int)(Item.position.X + 20 * scale - (int)Main.screenPosition.X), (int)(Item.position.Y + 20 * scale - (int)Main.screenPosition.Y), (int)(40 * newScale), (int)(40 * newScale)),
                new Rectangle(0, 0, 40, 40),
                lightColor,
                rotation,
                new Vector2(20, 20),
                SpriteEffects.None,
                0
            );
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // Draw tier
            float newScale = scale > 0.8f ? scale : 0.8f;
            //spriteBatch.Draw
            //(
            //    GetTierTexture(),
            //    new Rectangle((int)(position.X + 20 * scale), (int)(position.Y + 20 * scale), (int)(40 * newScale), (int)(40 * newScale)),
            //    new Rectangle(0, 0, 40, 40),
            //    drawColor,
            //    0,
            //    new Vector2(20, 20),
            //    SpriteEffects.None,
            //    0
            //);
            //// Draw choreability
            //spriteBatch.Draw
            //(
            //    GetChoreabilityTexture(),
            //    new Rectangle((int)(position.X + 20 * scale), (int)(position.Y + 20 * scale), (int)(40 * newScale), (int)(40 * newScale)),
            //    new Rectangle(0, 0, 40, 40),
            //    drawColor,
            //    0,
            //    new Vector2(20, 20),
            //    SpriteEffects.None,
            //    0
            //);
            // RS: Position is centered now
            Texture2D texture = GetTierTexture();
            spriteBatch.Draw
            (
                texture,
                new Rectangle((int)position.X, (int)position.Y, (int)(40 * newScale), (int)(40 * newScale)),
                new Rectangle(0, 0, 40, 40),
                drawColor,
                0,
                texture.Size()/2,
                SpriteEffects.None,
                0
            );
            // Draw choreability
            Texture2D texture2 = GetChoreabilityTexture();
            spriteBatch.Draw
            (
                texture2,
                new Rectangle((int)position.X, (int)position.Y, (int)(40 * newScale), (int)(40 * newScale)),
                new Rectangle(0, 0, 40, 40),
                drawColor,
                0,
                texture2.Size()/2,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}
