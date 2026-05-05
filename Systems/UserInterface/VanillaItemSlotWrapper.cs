using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface
{
    public class VanillaItemSlotWrapper : UIElement
    {
        public static string Attribution = "Code derived from ExampleMod.UI.VanillaItemSlotWrapper";
        public Item Item;
        private string _hoverItemName = string.Empty;
        private readonly int _context;
        public float alpha = 0;
        /// <summary>
        /// Used for changing the color of the slot while having a different functional slot context.
        /// </summary>
        private readonly int _drawContext;
        private readonly float _scale;
        private Texture2D _slotIcon = null;
        private Texture2D _slotBackground = null;
        internal Func<Item, bool> ValidItemFunc;
        /// <summary>
        /// Used to construct inventory slot UI elements.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scale"></param>
        /// <param name="hoverItemName"></param>
        /// <param name="drawContext">Optional. Pass a different ItemSlot.Context to change the slot's appearance.</param>
        /// <param name="slotIcon">Optional. Pass a 34 x 34 texture to make it appear as a slot icon.</param>
        public VanillaItemSlotWrapper(int context = ItemSlot.Context.BankItem, float scale = 1f, string hoverItemName = "", int drawContext = -1, Texture2D slotIcon = null, Texture2D slotBackground = null)
        {
            _context = context;
            _scale = scale;
            _hoverItemName = hoverItemName;
            if (drawContext < 0)
            {
                _drawContext = context;
            }
            else
            {
                _drawContext = drawContext;
            }
            if (slotIcon != null)
            {
                _slotIcon = slotIcon;
            }
            if (slotBackground != null)
            {
                _slotBackground = slotBackground;
            }

            Item = new Item();
            Item.SetDefaults(ItemID.None);

            Width.Set(TextureAssets.InventoryBack9.Value.Width * scale, 0f);
            Height.Set(TextureAssets.InventoryBack9.Value.Height * scale, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (_hoverItemName != String.Empty)
                {
                    Main.hoverItemName = _hoverItemName;
                }
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }

            if (_slotBackground != null)
            {
                spriteBatch.Draw(_slotBackground, rectangle.TopLeft(), null, Main.inventoryBack * (1f - alpha), 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
            }

            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _drawContext, rectangle.TopLeft(), Color.White * (1f - alpha));

            Texture2D texture2D = TextureAssets.InventoryBack.Value;

            if (_slotIcon != null && (Item.type <= ItemID.None || Item.stack <= 0))
            {
                Texture2D texture2D2 = _slotIcon;
                Rectangle rectangleIcon = _slotIcon.Frame();
                rectangleIcon.Width -= 2;
                rectangleIcon.Height -= 2;
                spriteBatch.Draw(texture2D2, rectangle.TopLeft() + texture2D.Size() / 2f * Main.inventoryScale, rectangleIcon, Color.White * 0.35f, 0f, rectangleIcon.Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);
            }

            Main.inventoryScale = oldScale;
        }
    }
}
