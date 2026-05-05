using EpicosityPort.Systems.UserInterface.ChoreabilitySlots;
using EpicosityPort.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EpicosityPort.Content.Items.Tiles.DanceMachine
{
    public class DanceMachine : ModTile
    {
        public Asset<Texture2D> glowMask;
        public Asset<Texture2D> groundGlowMask;
        public Asset<Texture2D> screenGlowMask;
        public Asset<Texture2D> front;

        public static float animationCycle;
        public override void Load()
        {
            string className = GetType().GetQualifiedNameAsPath();
            glowMask = ModContent.Request<Texture2D>($"{className}_Glowmask");
            groundGlowMask = ModContent.Request<Texture2D>($"{className}_GroundGlowmask");
            screenGlowMask = ModContent.Request<Texture2D>($"{className}_ScreenGlowmask");
            front = ModContent.Request<Texture2D>($"{className}_Front");
        }
        public static void UpdateAnimationCycle()
        {
            animationCycle += 1f;
            animationCycle %= 360f;
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.Origin = new Point16(2, 3);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(57, 67, 77));
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Point16 point = new Point16(i + frameX / 2, j + frameY / 2); // Center
            Vector2 worldCoordinates = point.ToWorldCoordinates();
            // RS: Jank, I know
            Item.NewItem(Main.LocalPlayer.GetSource_FromThis(), new Rectangle((int)worldCoordinates.X, (int)worldCoordinates.Y, 1, 1), ModContent.ItemType<DanceMachineItem>());
        }
        public override bool RightClick(int i, int j)
        {
            DanceMachineUI.Instance.DanceMachineUIInterface.SetState(new DanceMachineUI.DanceMachineUIState(new Vector2(i, j)));
            Main.playerInventory = true;
            Main.hidePlayerCraftingMenu = true;
            return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            // Front handlebars
            spriteBatch.Draw
            (
                front.Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 + 2 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX / 18 * 16, tile.TileFrameY / 18 * 16, 16, 16),
                Lighting.GetColor(i, j)
            );
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null);
            // Glow mask
            spriteBatch.Draw
            (
                glowMask.Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 + 2 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX / 18 * 16, tile.TileFrameY / 18 * 16, 16, 16),
                Color.White * (float)Math.Abs(Math.Sin((45 + animationCycle) / 360f * MathHelper.TwoPi))
            );
            // Ground glow mask
            spriteBatch.Draw
            (
                groundGlowMask.Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 + 2 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX / 18 * 16, tile.TileFrameY / 18 * 16, 16, 16),
                Color.White * (float)Math.Abs(Math.Cos(animationCycle / 360f * MathHelper.TwoPi))
            );
            // Screen
            spriteBatch.Draw
            (
                screenGlowMask.Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 + 2 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX / 18 * 16, tile.TileFrameY / 18 * 16, 16, 16),
                Color.White * (float)Math.Abs(Math.Sin(animationCycle / 360f * MathHelper.TwoPi))
            );

            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
