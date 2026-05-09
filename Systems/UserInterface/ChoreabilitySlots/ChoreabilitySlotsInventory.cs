using EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities;
using EpicosityPort.Content.Items.Accessories.Rhythmist.Fretboards;
using EpicosityPort.Systems.UserInterface;
using EpicosityPort.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static EpicosityPort.Systems.UserInterface.ChoreabilitySlots.ChoreabilitySlotsPlayer;

namespace EpicosityPort.Systems.UserInterface.ChoreabilitySlots
{
    public class ChoreabilitySlotsInventory : ModSystem
    {
        public static ChoreabilitySlotsInventory Instance { get { 
            return ModContent.GetInstance<ChoreabilitySlotsInventory>();    
        } }
        public Item ChoreabilitySwap(Item item, out bool success)
        {
            List<VanillaItemSlotWrapper> vanillaItemSlotWrappers;
            if (ChoreabilityInventoryUIStateSingleton != null)
            {
                vanillaItemSlotWrappers = ((ChoreabilityInventoryUI)ChoreabilityInventoryUIStateSingleton).vanillaItemSlotWrappers;
            }
            else
            {
                success = false;
                return item;
            }
            item.favorited = false;
            int swapSlotIndex = NUM_OF_CHOREABILITY_SLOTS - 1;
            // Get last empty slot, otherwise use last slot
            for (int slot = 0; slot < NUM_OF_CHOREABILITY_SLOTS; slot++)
            {
                if (!vanillaItemSlotWrappers[slot].Item.IsAir)
                {
                    continue;
                }
                else
                {
                    swapSlotIndex = slot;
                    break;
                }
            }

            Item result = vanillaItemSlotWrappers[swapSlotIndex].Item.Clone();
            vanillaItemSlotWrappers[swapSlotIndex].Item = item.Clone();
            SoundEngine.PlaySound(SoundID.Grab);
            Recipe.FindRecipes();
            success = true;
            return result;
        }

        public Terraria.UI.UserInterface ChoreabilitySlotsInventoryInterface;
        public UIState ChoreabilityInventoryUIStateSingleton;
        public class ChoreabilityInventoryUI : UIState
        {
            public Texture2D choreabilityButton;
            public Texture2D choreabilityButtonHover;
            public Texture2D choreabilityButtonSelected;
            public int choreabilityRevealAnimationTimer = 0;
            public int choreabilityPage = 0;
            public int choreabilitySelectedPage = 0;
            public List<VanillaItemSlotWrapper> vanillaItemSlotWrappers = new List<VanillaItemSlotWrapper>();
            public override void OnInitialize()
            {
                choreabilityButton = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InventoryButton1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                choreabilityButtonHover = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InventoryButton2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                choreabilityButtonSelected = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InventoryButton3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                int spaceBetweenSlots = 0;
                for (int itemSlotIndex = 0; itemSlotIndex < ChoreabilitySlotsPlayer.NUM_OF_CHOREABILITY_SLOTS; itemSlotIndex++)
                {
                    vanillaItemSlotWrappers.Add
                    (
                        new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f, "Choreability", ItemSlot.Context.MouseItem, ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/ChoreabilityIcon").Value, TextureAssets.InventoryBack8.Value)
                        {
                            Left = { Pixels = 50 + (TextureAssets.InventoryBack9.Value.Width + spaceBetweenSlots) * itemSlotIndex },
                            Top = { Pixels = 270 },
                            ValidItemFunc = (item) => { return item.ModItem is ChoreabilityBase || item.IsAir; }
                        }
                    );
                }
                foreach (VanillaItemSlotWrapper wrapper in vanillaItemSlotWrappers)
                {
                    Append(wrapper);
                }
            }
            public override void Update(GameTime gameTime)
            {
                int miniMapOffset = Main.mapStyle == 1 ? 256 : 0;
                int extraOffset = Main.screenHeight > 856 ? 0 : 856 - Main.screenHeight;
                if (miniMapOffset == 0) // No map
                {
                    extraOffset = 0;
                } // This is supposed to replicate private member Main.mH, which is actually used in inventory code

                if (choreabilityPage == 0 && choreabilityRevealAnimationTimer > 0)
                {
                    choreabilityRevealAnimationTimer--;
                }
                else if (choreabilityPage == 1 && choreabilityRevealAnimationTimer < 15)
                {
                    choreabilityRevealAnimationTimer++;
                }

                for (int slotIndex = 0; slotIndex < vanillaItemSlotWrappers.Count(); slotIndex++)
                {
                    VanillaItemSlotWrapper slot = vanillaItemSlotWrappers[slotIndex];
                    slot.Left.Pixels = Main.screenWidth - 138 - 47 * slotIndex * Utilities.Easings.EaseInOutCubic(choreabilityRevealAnimationTimer / 15f);
                    slot.Top.Pixels = 92 + miniMapOffset - extraOffset;
                    slot.alpha = 1f - Utilities.Easings.EaseInOutCubic(choreabilityRevealAnimationTimer / 15f);
                }
                if (Main.mouseItem.IsAir)
                {
                    choreabilityPage = choreabilitySelectedPage;
                }
            }
            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                if (Main.playerInventory && choreabilityRevealAnimationTimer > 1)
                {
                    base.DrawChildren(spriteBatch);
                }
            }
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                if (Main.playerInventory)
                {
                    Texture2D buttonTexture;
                    int miniMapOffset = Main.mapStyle == 1 ? 256 : 0;
                    int extraOffset = Main.screenHeight > 856 ? 0 : 856 - Main.screenHeight;
                    if (miniMapOffset == 0) // No map
                    {
                        extraOffset = 0;
                    }
                    Rectangle destination = new Rectangle(Main.screenWidth - 86, 96 + miniMapOffset - extraOffset, 36, 36);
                    if (destination.Contains(Main.MouseScreen.ToPoint()))
                    {
                        Main.LocalPlayer.mouseInterface = true;
                        Main.hoverItemName = "Choreabilities";
                        buttonTexture = choreabilityButtonHover;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            choreabilitySelectedPage++;
                            choreabilitySelectedPage %= 2;
                        }
                    }
                    else
                    {
                        buttonTexture = choreabilityButton;
                    }
                    spriteBatch.Draw
                    (
                        buttonTexture,
                        destination,
                        null,
                        Color.White,
                        0,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0
                    );
                    if (choreabilityPage == 1)
                    {
                        spriteBatch.Draw
                        (
                            choreabilityButtonSelected,
                            destination,
                            null,
                            Color.White,
                            0,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0
                        );
                    }
                }
            }
        }
        private static void ItemSlot_SwapChoreability(Terraria.UI.On_ItemSlot.orig_SwapEquip_ItemArray_int_int orig, Item[] inv, int context, int slot)
        {
            bool success = false;
            if (inv[slot].ModItem is ChoreabilityBase)
            {
                inv[slot] = ChoreabilitySlotsInventory.Instance.ChoreabilitySwap(inv[slot], out success);
                if (success)
                {
                    (ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton as ChoreabilitySlotsInventory.ChoreabilityInventoryUI).choreabilitySelectedPage = 1;
                }
            }
            else if (inv[slot].ModItem is FretboardBase)
            {
                Item result;
                Player player = Main.player[Main.myPlayer];
                result = inv[slot];
                for (int i = 0; i < player.armor.Length; i++)
                {
                    if (player.armor[i].ModItem is FretboardBase)
                    {
                        result = player.armor[i].Clone();
                        player.armor[i] = inv[slot].Clone();
                        success = true;
                        SoundEngine.PlaySound(SoundID.Grab);
                        Recipe.FindRecipes();
                        break;
                    }
                }
                inv[slot] = result;
                if (success)
                {
                    Main.EquipPageSelected = 0;
                }
            }
            orig(inv, context, slot);
        }
        private static void ItemSlotChoreabilityEquip(Terraria.UI.On_ItemSlot.orig_SelectEquipPage orig, Item item)
        {
            orig(item);
            if (item.ModItem is ChoreabilityBase)
            {
                (ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton as ChoreabilitySlotsInventory.ChoreabilityInventoryUI).choreabilityPage = 1;
            }
        }
        public override void OnModLoad()
        {
            Terraria.UI.On_ItemSlot.SelectEquipPage += ItemSlotChoreabilityEquip;
            Terraria.UI.On_ItemSlot.SwapEquip_ItemArray_int_int += ItemSlot_SwapChoreability;
        }
        public override void OnModUnload()
        {
            Terraria.UI.On_ItemSlot.SelectEquipPage -= ItemSlotChoreabilityEquip;
            Terraria.UI.On_ItemSlot.SwapEquip_ItemArray_int_int -= ItemSlot_SwapChoreability;
        }
    }
}
