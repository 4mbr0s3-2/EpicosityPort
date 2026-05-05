using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.Flashlight
{
    public class Flashlight : ModItem
    {
        // TODO: Fix HoldStyle
        public float itemRotation = 0;
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.holdStyle = ItemHoldStyleID.None;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.damage = 20;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override void HoldItemFrame(Player player)
        {
            if (Main.myPlayer != player.whoAmI)
            {
                return;
            }
            Vector2 playerToCursor = (Main.MouseWorld - player.Center);
            playerToCursor.Normalize();
            //player.bodyFrame.Y = Math.Max(Utilities.BodyFrameCalculator.GetBodyFrameFromRotation(playerToCursor.ToRotation()), 3) * player.bodyFrame.Height;
            if (Main.MouseWorld.X > player.Center.X)
            {
                player.ChangeDir(1);
            }
            else
            {
                player.ChangeDir(-1);
            }
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.myPlayer == player.whoAmI) // This client
            {
                if (Main.MouseWorld.X > player.Center.X)
                {
                    player.ChangeDir(1);
                }
                else
                {
                    player.ChangeDir(-1);
                }
            }
            player.itemLocation = player.MountedCenter + new Vector2(6 * player.direction, 4);
            if (Main.myPlayer == player.whoAmI) // This client
            {
                Vector2 playerToCursor = (Main.MouseWorld - player.Center);
                playerToCursor.Normalize();
                player.itemRotation = playerToCursor.ToRotation() + (float)Math.PI / 4.0f;

                // Mulitplayer support
                //if (Main.netMode == NetmodeID.MultiplayerClient)
                //{
                //    ModNetHandler.GetPacketHandler<FlashlightItemRotationPacketHandler>().SendTarget(-1, Main.myPlayer, player.itemRotation, player.direction);
                //}
                Vector2 handleOffset = (new Vector2(4, -4)).RotatedBy(player.itemRotation);
                player.itemLocation -= handleOffset;
            }
            else // For other clients
            {
                player.itemRotation = itemRotation;
                Vector2 handleOffset = (new Vector2(4, -4)).RotatedBy(player.itemRotation);
                player.itemLocation -= handleOffset;
            }
            if (player.direction == -1)
            {
                player.itemRotation += (float)Math.PI / 2.0f;
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CastLight(player, true);
        }
        public override void HoldItem(Player player)
        {
            CastLight(player);
        }

        public void CastLight(Player player, bool notHoldingItem = false)
        {
            int lightRange = 50;
            float lightPercent = 1;
            for (int x = 0; x < lightRange; x++)
            {
                //Vector2 playerToCursor = (Main.MouseWorld - player.Center);
                //playerToCursor.Normalize();
                float lightItemRotation; // For this player, directly calculate the mouse
                if (notHoldingItem)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Vector2 playerToCursor2 = (Main.MouseWorld - player.Center);
                        playerToCursor2.Normalize();
                        lightItemRotation = playerToCursor2.ToRotation() + (float)Math.PI / 4.0f;
                    }
                    else // For other players
                    {
                        lightItemRotation = itemRotation;
                    }
                }
                else // If holding item and itemRotation is already calculated and synced
                {
                    lightItemRotation = player.itemRotation;
                }
                Vector2 playerToCursor;
                if (notHoldingItem)
                {
                    playerToCursor = (lightItemRotation - (float)Math.PI / 4.0f).ToRotationVector2();
                }
                else
                {
                    playerToCursor = (lightItemRotation - (player.direction == -1 ? (float)Math.PI / 2.0f : 0) - (float)Math.PI / 4.0f).ToRotationVector2();
                }
                Vector2 position = player.Center + playerToCursor * 16 * x;
                if (Collision.SolidCollision(position, 10, 10))
                {
                    lightPercent *= 0.8f;
                }
                Lighting.AddLight(position, new Vector3(1, 1, 1) / (x * 0.1f + 1) * 0.9f * lightPercent);
            }
        }
    }
}
