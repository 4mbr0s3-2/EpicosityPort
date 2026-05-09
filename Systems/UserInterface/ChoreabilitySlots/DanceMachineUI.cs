using EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities;
using EpicosityPort.Content.Items.Rhythm.DanceCredits;
using EpicosityPort.Content.Items.Tiles.DanceMachine;
using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Systems.UserInterface.ChoreabilitySlots
{
    public class DanceMachineUI : ModSystem
    {
        public static DanceMachineUI Instance { 
            get {
                return ModContent.GetInstance<DanceMachineUI>();    
            } 
        }
        public Terraria.UI.UserInterface DanceMachineUIInterface;
        /// <summary>
        /// This UIState is used to draw the crafting-like UI used to give quest items to the left of the screen.
        /// </summary>
        public class DanceMachineUIState : UIState
        {
            /// <summary>
            /// Used in order to determine whether or not a player has exited the NPC's dialogue.
            /// </summary>
            public Microsoft.Xna.Framework.Audio.SoundEffectInstance[] cachedSounds = new Microsoft.Xna.Framework.Audio.SoundEffectInstance[4];
            public int upgradeAnimationTimer = 0;
            public Texture2D upgradeAnimationTierTexture = null;
            public Vector2 tilePos = Vector2.Zero;
            private VanillaItemSlotWrapper _choreabilityItemSlot = new VanillaItemSlotWrapper();
            private VanillaItemSlotWrapper _danceCreditItemSlot = new VanillaItemSlotWrapper();
            public DanceMachineUIState(Vector2 tilePos)
            {
                this.tilePos = tilePos;
            }
            public override void OnInitialize()
            {
                // If we don't cache these the instances will get destroyed and the sound will get cut off
                // That took forever to debug since exceptions will also prevent spriteBatch from beginning/ending

                SoundEngine.PlaySound(Terraria.ID.SoundID.MenuOpen);
                Main.npcChatText = "";

                _choreabilityItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f, "Choreability", ItemSlot.Context.MouseItem, ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/ChoreabilityIcon").Value, TextureAssets.InventoryBack8.Value)
                {
                    Left = { Pixels = 50 },
                    Top = { Pixels = 270 },
                    ValidItemFunc = (item) => { return item.IsAir || item.ModItem is ChoreabilityBase || ChoreabilityTierManager.PotionToChoreabilityType.ContainsKey(item.type); }
                };
                _danceCreditItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f, "Dance Credit", ItemSlot.Context.MouseItem, ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/DanceCreditIcon").Value, TextureAssets.InventoryBack8.Value)
                {
                    Left = { Pixels = 50 },
                    Top = { Pixels = 320 },
                    ValidItemFunc = (item) => { return item.IsAir || item.ModItem is DanceCredit; }
                };
                Append(_choreabilityItemSlot);
                Append(_danceCreditItemSlot);
            }
            public override void OnDeactivate()
            {
                SoundEngine.PlaySound(Terraria.ID.SoundID.MenuClose);
                if (!_choreabilityItemSlot.Item.IsAir)
                {
                    Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), _choreabilityItemSlot.Item, _choreabilityItemSlot.Item.stack);
                    _choreabilityItemSlot.Item.TurnToAir();
                }
                if (!_danceCreditItemSlot.Item.IsAir)
                {
                    Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), _danceCreditItemSlot.Item, _danceCreditItemSlot.Item.stack);
                    _danceCreditItemSlot.Item.TurnToAir();
                }
            }

            public override void Update(GameTime gameTime)
            {
                // RS: Magic number 😍
                if ((Main.LocalPlayer.Center - tilePos * 16).Length() > 100 || Main.tile[(int)tilePos.X, (int)tilePos.Y].TileType != ModContent.TileType<DanceMachine>() || !Main.playerInventory)
                {
                    Instance.DanceMachineUIInterface.SetState(null);
                }
                // Don't delete this or the UIElements attached to this UIState will cease to function.
                base.Update(gameTime);

            }
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                base.DrawSelf(spriteBatch);
                Main.hidePlayerCraftingMenu = true;

                const int slotX = 50;
                const int slotY = 270;

                bool ableToUpgrade = false;

                string topText = "Place a choreability or convertible potion";
                if (_choreabilityItemSlot.Item.ModItem is ChoreabilityBase)
                {
                    ChoreabilityBase choreabilityItemSlot = _choreabilityItemSlot.Item.ModItem as ChoreabilityBase;
                    int nextTier = ChoreabilityTierManager.GetNextTier(choreabilityItemSlot.choreability.tier);
                    if (nextTier >= 0)
                    {
                        int numberNeeded = ChoreabilityTierManager.GetNumberOfCreditsNeeded((ChoreabilityTier)nextTier);
                        string nextTierName = Enum.GetName(typeof(ChoreabilityTier), nextTier);
                        if (_danceCreditItemSlot.Item.ModItem is DanceCredit)
                        {
                            DanceCredit danceCredit = _danceCreditItemSlot.Item.ModItem as DanceCredit;
                            if ((int)danceCredit.GetTier() != nextTier)
                            {
                                topText = $"{nextTierName} dance credits are needed to upgrade";
                            }
                            else
                            {
                                if (_danceCreditItemSlot.Item.stack >= numberNeeded)
                                {
                                    topText = $"Upgrade to {nextTierName}-tier";
                                    ableToUpgrade = true;
                                }
                                else
                                {
                                    topText = $"Required {nextTierName} dance credits: {numberNeeded} ({numberNeeded - _danceCreditItemSlot.Item.stack} more)";
                                }
                            }
                        }
                        else
                        {
                            topText = $"Required {nextTierName} dance credits: {numberNeeded}";
                        }
                    }
                    else
                    {
                        topText = $"Choreability maxed out!";
                    }
                }
                else if (ChoreabilityTierManager.PotionToChoreabilityType.ContainsKey(_choreabilityItemSlot.Item.type))
                {
                    int numberNeeded = ChoreabilityTierManager.GetNumberOfCreditsNeeded(ChoreabilityTier.Copper);
                    string copperTierName = Enum.GetName(typeof(ChoreabilityTier), ChoreabilityTier.Copper);
                    if (_danceCreditItemSlot.Item.ModItem is DanceCredit)
                    {
                        DanceCredit danceCredit = _danceCreditItemSlot.Item.ModItem as DanceCredit;
                        if (danceCredit.GetTier() != ChoreabilityTier.Copper)
                        {
                            topText = $"{copperTierName} dance credits are needed to upgrade";
                        }
                        else
                        {
                            if (_danceCreditItemSlot.Item.stack >= numberNeeded)
                            {
                                topText = $"Convert to {copperTierName}-tier choreability";
                                ableToUpgrade = true;
                            }
                            else
                            {
                                topText = $"Required {copperTierName} dance credits: {numberNeeded} ({numberNeeded - _danceCreditItemSlot.Item.stack} more)";
                            }
                        }
                    }
                    else
                    {
                        topText = $"Required {copperTierName} dance credits: {numberNeeded}";
                    }
                }
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, topText, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);

                if (ableToUpgrade)
                {
                    int serveX = slotX + 70;
                    int serveY = slotY + 40;
                    bool hoveringOverServeButton = Main.mouseX > serveX - 15 && Main.mouseX < serveX + 15 && Main.mouseY > serveY - 15 && Main.mouseY < serveY + 15 && !PlayerInput.IgnoreMouseInterface;
                    Texture2D serveTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InventoryButton3").Value;
                    if (hoveringOverServeButton)
                    {
                        // Prevents the player from using held item
                        Main.LocalPlayer.mouseInterface = true;
                        Main.hoverItemName = topText;
                        if (Main.mouseLeft)
                        {
                            UpgradeItemInChoreabilitySlot();
                        }
                        spriteBatch.Draw(ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InventoryButton2").Value, new Vector2(serveX, serveY), null, Color.White, 0f, serveTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
                    }
                    spriteBatch.Draw(serveTexture, new Vector2(serveX, serveY), null, Color.White, 0f, serveTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
                }

            }
            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                // RS: Maybe the timer should be in an update method instead
                // This will break with 60+ FPS mods
                base.DrawChildren(spriteBatch);
                if (upgradeAnimationTimer > 0)
                {
                    upgradeAnimationTimer--;
                    float size = Easings.EaseOutQuad(1f - (upgradeAnimationTimer / 30f));
                    #region Draw upgrade animation
                    if (upgradeAnimationTierTexture != null)
                    {
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix);
                        for (int i = 0; i < 3; i++)
                        {
                            spriteBatch.Draw
                            (
                                upgradeAnimationTierTexture,
                                new Rectangle((int)_choreabilityItemSlot.Left.Pixels + 22, (int)_choreabilityItemSlot.Top.Pixels + 22, (int)(40 * (0.5f + size * i)), (int)(40 * (0.5f + size * i))),
                                null,
                                Color.White * (1 - size),
                                0,
                                new Vector2(20, 20),
                                SpriteEffects.None,
                                0
                            );
                        }
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                    }
                    #endregion
                }
            }
            public void UpgradeItemInChoreabilitySlot()
            {
                // RS: For reason I haven't thought of this
                if (upgradeAnimationTimer > 0) return;
                
                if (_choreabilityItemSlot.Item.ModItem is ChoreabilityBase)
                {
                    ChoreabilityBase choreabilityBase = _choreabilityItemSlot.Item.ModItem as ChoreabilityBase;
                    ChoreabilityTier nextTier = (ChoreabilityTier)ChoreabilityTierManager.GetNextTier(choreabilityBase.choreability.tier);
                    //_danceCreditItemSlot.Item.stack -= ChoreabilityTierManager.GetNumberOfCreditsNeeded(nextTier);


                    upgradeAnimationTimer = 30;
                    string tierName = Enum.GetName(typeof(ChoreabilityTier), nextTier);
                    upgradeAnimationTierTexture = ModContent.Request<Texture2D>($"{typeof(ChoreabilityBase).GetNamespaceAsPath()}/TierCasings/{tierName}").Value;

                    
                    string choreabilityName = Enum.GetName(typeof(ChoreabilityType), choreabilityBase.choreability.type);
                    string choreabilityTier = Enum.GetName(typeof(ChoreabilityTier), nextTier);
                    Item newItem = new Item();
                    string itemName = $"{choreabilityName}Choreability{choreabilityTier}";
                    bool v = ModContent.TryFind(EpicosityPort.Instance.Name, itemName, out ModItem item);
                    if (v)
                    {
                        newItem.netDefaults(item.Type);
                        _choreabilityItemSlot.Item = newItem.Clone();
                        _danceCreditItemSlot.Item.stack -= ChoreabilityTierManager.GetNumberOfCreditsNeeded(nextTier);
                        if (_danceCreditItemSlot.Item.stack <= 0)
                            _danceCreditItemSlot.Item.TurnToAir();
                    }
                    else
                    {
                        EpicosityPort.Instance.Logger.Error($"Could not find {itemName}!");
                    }
                    try
                    {
                        switch (nextTier)
                        {
                            case ChoreabilityTier.Silver:
                                SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/DMUpgradeSilver"));
                                break;
                            case ChoreabilityTier.Gold:
                                SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/DMUpgradeCopperGold"));
                                break;
                            case ChoreabilityTier.Platinum:
                                SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/DMUpgradePlatinum"));
                                break;
                            case ChoreabilityTier.Terra:
                                SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/DMUpgradeTerra"));
                                break;
                        }
                    }
                    catch
                    {
                        // RS: Ah yes the 'do nothing'
                    }
                }
                // A potion is in the slot instead
                else if (ChoreabilityTierManager.PotionToChoreabilityType.ContainsKey(_choreabilityItemSlot.Item.type))
                {
                    // RS: Apparently I haven't considered the case where there is more than one potion stacked
                    // So let's make it a feature
                    ChoreabilityType choreabilityType = ChoreabilityTierManager.PotionToChoreabilityType[_choreabilityItemSlot.Item.type];
                    int creditsNeeded = ChoreabilityTierManager.GetNumberOfCreditsNeeded(ChoreabilityTier.Copper);
                    //_danceCreditItemSlot.Item.stack -= creditsNeeded;

                    upgradeAnimationTimer = 30;
                    string tierName = Enum.GetName(typeof(ChoreabilityTier), ChoreabilityTier.Copper);
                    upgradeAnimationTierTexture = ModContent.Request<Texture2D>($"{typeof(ChoreabilityBase).GetNamespaceAsPath()}/TierCasings/{tierName}").Value;
                    string choreabilityName = Enum.GetName(typeof(ChoreabilityType), choreabilityType);
                    string choreabilityTier = Enum.GetName(typeof(ChoreabilityTier), ChoreabilityTier.Copper);
                    Item newItem = new Item();
                    //newItem.netDefaults(EpicosityPort.Instance.ItemType($"{choreabilityName}Choreability{choreabilityTier}"));
                    string itemName = $"{choreabilityName}Choreability{choreabilityTier}";
                    bool v = ModContent.TryFind(EpicosityPort.Instance.Name, itemName, out ModItem item);
                    if (v)
                    {
                        if (_choreabilityItemSlot.Item.stack > 1)
                            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), _choreabilityItemSlot.Item.Clone(), _choreabilityItemSlot.Item.stack - 1);
                        newItem.netDefaults(item.Type);
                        _choreabilityItemSlot.Item = newItem.Clone();
                        _danceCreditItemSlot.Item.stack -= creditsNeeded;
                        if (_danceCreditItemSlot.Item.stack <= 0)
                            _danceCreditItemSlot.Item.TurnToAir();
                    }
                    else
                    {
                        EpicosityPort.Instance.Logger.Error($"Could not find {itemName}!");
                    }
                    _choreabilityItemSlot.Item = newItem.Clone();
                    SoundEngine.PlaySound(new SoundStyle($"{GetType().GetNamespaceAsPath()}/DMUpgradeCopperGold"));
                }
            }
        }
    }
}
