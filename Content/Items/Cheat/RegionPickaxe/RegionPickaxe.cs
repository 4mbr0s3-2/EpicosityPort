using EpicosityPort.Systems.UserInterface;
using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static EpicosityPort.Content.Items.Cheat.RegionPickaxe.RegionPickaxe;

namespace EpicosityPort.Content.Items.Cheat.RegionPickaxe
{
	public class RegionPickaxe : SpecialItem
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.TinPickaxe}";
		public static RegionPickaxe GetLocalRegionPickaxe()
		{
			if (Main.LocalPlayer.HeldItem.type != ModContent.ItemType<RegionPickaxe>())
			{
				return null;
			}
			else
			{
				return (RegionPickaxe)Main.LocalPlayer.HeldItem.ModItem;
			}
		}
		public static int[] GetSelectionBoundaries()
		{
			ModPlayerRegionPickaxeUI regionPickaxeUIPlayer = Main.LocalPlayer.GetModPlayer<ModPlayerRegionPickaxeUI>();
			if (!regionPickaxeUIPlayer.cursor1Position.HasValue || !regionPickaxeUIPlayer.cursor2Position.HasValue)
			{
				return new int[4];
			}
			int cursor1X = (int)regionPickaxeUIPlayer.cursor1Position.Value.X / 16;
			int cursor1Y = (int)regionPickaxeUIPlayer.cursor1Position.Value.Y / 16;
			int cursor2X = (int)regionPickaxeUIPlayer.cursor2Position.Value.X / 16;
			int cursor2Y = (int)regionPickaxeUIPlayer.cursor2Position.Value.Y / 16;

			int minTileCursorX = Math.Min(cursor1X, cursor2X);
			int maxTileCursorX = Math.Max(cursor1X, cursor2X);
			int minTileCursorY = Math.Min(cursor1Y, cursor2Y);
			int maxTileCursorY = Math.Max(cursor1Y, cursor2Y);

			return new int[4] { minTileCursorX, maxTileCursorX, minTileCursorY, maxTileCursorY };
		}
		// RS: Yeah I'm (almost) not touching this
		public class SpecialItemUIState : UIState
		{
			private string param;
			private string paramValue;
			private bool isHovering = false;
			public override void Update(GameTime gameTime)
			{
				Main.LocalPlayer.cursorItemIconEnabled = isHovering;
				Main.LocalPlayer.cursorItemIconText = $"{param}: {paramValue}";
				Main.LocalPlayer.cursorItemIconID = -1;
			}
			private Asset<Texture2D>[] shapeButtonTextures;
            private Asset<Texture2D>[] bypassButtonTextures;
            private Asset<Texture2D>[] filterButtonTextures;
            private	Asset<Texture2D>[] inversionButtonTextures;
            public override void OnInitialize()
            {
				shapeButtonTextures = new Asset<Texture2D>[Enum.GetNames<PickaxeShape>().Length];
				bypassButtonTextures = new Asset<Texture2D>[Enum.GetNames<PickaxeBypass>().Length];
				filterButtonTextures = new Asset<Texture2D>[Enum.GetNames<PickaxeFilter>().Length];
				inversionButtonTextures = new Asset<Texture2D>[Enum.GetNames<PickaxeSelectionInversion>().Length];

				for (int i = 1; i <= Enum.GetNames<PickaxeShape>().Length; i++)
				{
					shapeButtonTextures[i - 1] = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/ShapeButton{i}");
				}
                for (int i = 1; i <= Enum.GetNames<PickaxeBypass>().Length; i++)
				{
                    bypassButtonTextures[i - 1] = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/BypassButton{i}");
				}
                for (int i = 1; i <= Enum.GetNames<PickaxeFilter>().Length; i++)
                {
					filterButtonTextures[i - 1] = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/FilterButton{i}");
                }
                for (int i = 1; i <= Enum.GetNames<PickaxeSelectionInversion>().Length; i++)
                {
					inversionButtonTextures[i - 1] = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InversionButton{i}");
                }
            }
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				isHovering = false;
				ModPlayerRegionPickaxeUI regionPickaxeUIPlayer = Main.LocalPlayer.GetModPlayer<ModPlayerRegionPickaxeUI>();
				RegionPickaxe regionPickaxe = GetLocalRegionPickaxe();
				if (regionPickaxe == null)
				{
					return;
				}
				#region Draw and process region pickaxe buttons
				if (regionPickaxeUIPlayer.guiAnimationProgress > 0)
				{
					//Texture2D shapeButtonTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/ShapeButton" + ((int)regionPickaxe.pickaxeShape + 1)).Value;
					//Texture2D bypassButtonTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/BypassButton" + ((int)regionPickaxe.pickaxeBypass + 1)).Value;
					//Texture2D filterButtonTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/FilterButton" + ((int)regionPickaxe.pickaxeFilter + 1)).Value;
					//Texture2D inversionButtonTexture = ModContent.Request<Texture2D>($"{GetType().GetNamespaceAsPath()}/InversionButton" + ((int)regionPickaxe.pickaxeSelectionInversion + 1)).Value;
					// RS: Wow, so much for localization
					string shapeString = EnumHelper.GetEnumValueNameAndSplitCamelCase(regionPickaxe.pickaxeShape);
					string bypassString = EnumHelper.GetEnumValueNameAndSplitCamelCase(regionPickaxe.pickaxeBypass);
					string filterString = EnumHelper.GetEnumValueNameAndSplitCamelCase(regionPickaxe.pickaxeFilter);
					string inversionString = EnumHelper.GetEnumValueNameAndSplitCamelCase(regionPickaxe.pickaxeSelectionInversion);
					float ease = Easings.EaseOutBack(regionPickaxeUIPlayer.guiAnimationProgress);
					// Button index indicates the number of the current button
					for (int buttonIndex = 0; buttonIndex < 4; buttonIndex++)
					{
						Vector2 buttonOffset = new Vector2(50 * ease, 0);
						buttonOffset = buttonOffset.RotatedBy((1f - ease) + (MathHelper.TwoPi / 4f) * buttonIndex);
                        // RS: Taking notes on matrices here
                        // x - Main.screenPosition
                        // PlayerInput.SetZoom_Unscaled()
                        // PlayerInput.SetZoom_MouseInWorld()
                        // Main.MouseScreen
                        // Main.MouseWorld
                        // Main.UIScaleMatrix
                        // Main.GameViewMatrix.TransformationMatrix
                        Vector2 menuScreenPosition = Vector2.Transform(regionPickaxeUIPlayer.menuScreenPosition, Main.GameViewMatrix.TransformationMatrix * Matrix.Invert(Main.UIScaleMatrix));
						
                        Rectangle currentButtonRectangle = new Rectangle((int)(menuScreenPosition.X + buttonOffset.X), (int)(menuScreenPosition.Y + buttonOffset.Y), 40, 40);
						Texture2D buttonTexture = null;
						switch (buttonIndex)
						{
							case 0:
								buttonTexture = shapeButtonTextures[(int)regionPickaxe.pickaxeShape].Value;
								break;
							case 1:
								buttonTexture = bypassButtonTextures[(int)regionPickaxe.pickaxeBypass].Value;
								break;
							case 2:
								buttonTexture = filterButtonTextures[(int)regionPickaxe.pickaxeFilter].Value;
								break;
							case 3:
								buttonTexture = inversionButtonTextures[(int)regionPickaxe.pickaxeSelectionInversion].Value;
								break;
							//default:
							//	buttonTexture = shapeButtonTextures[(int)regionPickaxe.pickaxeShape].Value; // RS: ???
							//	break;
						}
						#region Draw buttons
						if (currentButtonRectangle.Contains((Main.MouseScreen + new Vector2(20, 20)).ToPoint()))
						{
							isHovering = true;
							//Main.LocalPlayer.mouseInterface = true;
							currentButtonRectangle.Width += 5;
							currentButtonRectangle.Height += 5;
							// param is the name of the parameter that will be used for tooltip text
							switch (buttonIndex)
							{
								case 0:
									param = "Shape";
									break;
								case 1:
									param = "Bypass";
									break;
								case 2:
									param = "Filter";
									break;
								case 3:
									param = "Inverted Selection";
									break;
								default:
									param = "???";
									break;
							}
							switch (buttonIndex)
							{
								case 0:
									paramValue = shapeString;
									break;
								case 1:
									paramValue = bypassString;
									break;     
								case 2:
									paramValue = filterString;
									break;
								case 3:
									paramValue = inversionString;
									break;
								default:
									paramValue = "???";
									break;
							}
							//Main.LocalPlayer.cursorItemIconText = $"{param}: {paramValue}";
							if (regionPickaxeUIPlayer.guiAnimationProgress >= 1)
							{
								if (Main.mouseLeft && Main.mouseLeftRelease)
								{
									switch (buttonIndex)
									{
										case 0:
											regionPickaxe.pickaxeShape = (PickaxeShape)(((int)regionPickaxe.pickaxeShape + 1) % RegionPickaxe.PICKAXE_SHAPE_COUNT);
											break;
										case 1:
											regionPickaxe.pickaxeBypass = (PickaxeBypass)(((int)regionPickaxe.pickaxeBypass + 1) % Enum.GetNames(typeof(PickaxeBypass)).Length);
											break;
										case 2:
											regionPickaxe.pickaxeFilter = (PickaxeFilter)(((int)regionPickaxe.pickaxeFilter + 1) % RegionPickaxe.PICKAXE_FILTER_COUNT);
											break;
										case 3:
											regionPickaxe.pickaxeSelectionInversion = (PickaxeSelectionInversion)(((int)regionPickaxe.pickaxeSelectionInversion + 1) % Enum.GetNames(typeof(PickaxeSelectionInversion)).Length);
											break;
										default:
											break;
									}
								}
								else if (Main.mouseRight && Main.mouseRightRelease)
								{
									switch (buttonIndex)
									{
										case 0:
											regionPickaxe.pickaxeShape = (PickaxeShape)(((int)regionPickaxe.pickaxeShape - 1) % RegionPickaxe.PICKAXE_SHAPE_COUNT);
											if ((int)regionPickaxe.pickaxeShape < 0)
											{
												regionPickaxe.pickaxeShape = (PickaxeShape)(RegionPickaxe.PICKAXE_SHAPE_COUNT - 1);
											}
											break;
										case 1:
											regionPickaxe.pickaxeBypass = (PickaxeBypass)(((int)regionPickaxe.pickaxeBypass - 1) % Enum.GetNames(typeof(PickaxeBypass)).Length);
											if ((int)regionPickaxe.pickaxeBypass < 0)
											{
												regionPickaxe.pickaxeBypass = (PickaxeBypass)(Enum.GetNames(typeof(PickaxeBypass)).Length - 1);
											}
											break;
										case 2:
											regionPickaxe.pickaxeFilter = (PickaxeFilter)(((int)regionPickaxe.pickaxeFilter - 1) % RegionPickaxe.PICKAXE_FILTER_COUNT);
											if ((int)regionPickaxe.pickaxeFilter < 0)
											{
												regionPickaxe.pickaxeFilter = (PickaxeFilter)(RegionPickaxe.PICKAXE_FILTER_COUNT - 1);
											}
											break;
										case 3:
											regionPickaxe.pickaxeSelectionInversion = (PickaxeSelectionInversion)(((int)regionPickaxe.pickaxeSelectionInversion - 1) % Enum.GetNames(typeof(PickaxeSelectionInversion)).Length);
											if ((int)regionPickaxe.pickaxeSelectionInversion < 0)
											{
												regionPickaxe.pickaxeSelectionInversion = (PickaxeSelectionInversion)(Enum.GetNames(typeof(PickaxeSelectionInversion)).Length - 1);
											}
											break;
										default:
											break;
									}
								}
							}
						}
						spriteBatch.Draw
						(
							buttonTexture,
							currentButtonRectangle,
							new Rectangle(0, 0, 40, 40),
							Color.White * ease,
							0,
							new Vector2(20, 20),
							SpriteEffects.None,
							0
						);
						#endregion
					}
				}
				#endregion
				#region Draw selection boundaries
				if (regionPickaxeUIPlayer == null)
				{
					return;
				}
				if (regionPickaxeUIPlayer.menuOpen)
				{
					return;
				}
				// The player is holding the region pickaxe...
				if (!regionPickaxeUIPlayer.cursor1Position.HasValue || !regionPickaxeUIPlayer.cursor2Position.HasValue)
				{
					return;
				}
				// The player is selecting and has defined both cursors...
				if (Main.LocalPlayer.channel && !Main.LocalPlayer.controlUseTile) // Holding down left click
				{
					int[] selectionBoundaries = GetSelectionBoundaries();

					int minTileCursorX = selectionBoundaries[0];
					int maxTileCursorX = selectionBoundaries[1];
					int minTileCursorY = selectionBoundaries[2];
					int maxTileCursorY = selectionBoundaries[3];

					Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.White;

					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					// Draw outline
					if (regionPickaxe.pickaxeShape == PickaxeShape.Rectangle || regionPickaxe.pickaxeShape == PickaxeShape.Random || regionPickaxe.pickaxeShape == PickaxeShape.Checkerboard || regionPickaxe.pickaxeSelectionInversion == PickaxeSelectionInversion.On)
					{
						Utils.DrawLine(spriteBatch, new Vector2(minTileCursorX, minTileCursorY).ToWorldCoordinates(0, 0), new Vector2(minTileCursorX, maxTileCursorY).ToWorldCoordinates(0, 2), color, color, 2f);
						Utils.DrawLine(spriteBatch, new Vector2(minTileCursorX, minTileCursorY).ToWorldCoordinates(0, 0), new Vector2(maxTileCursorX, minTileCursorY).ToWorldCoordinates(0, 0), color, color, 2f);
						Utils.DrawLine(spriteBatch, new Vector2(minTileCursorX, maxTileCursorY).ToWorldCoordinates(0, 0), new Vector2(maxTileCursorX, maxTileCursorY).ToWorldCoordinates(0, 0), color, color, 2f);
						Utils.DrawLine(spriteBatch, new Vector2(maxTileCursorX, minTileCursorY).ToWorldCoordinates(0, 0), new Vector2(maxTileCursorX, maxTileCursorY).ToWorldCoordinates(0, 0), color, color, 2f);
					}
					if (regionPickaxe.pickaxeShape == PickaxeShape.Rhombus)
					{
						Utils.DrawLine(spriteBatch, new Vector2((minTileCursorX + maxTileCursorX) / 2f, minTileCursorY).ToWorldCoordinates(0, 0), new Vector2(minTileCursorX, (minTileCursorY + maxTileCursorY) / 2f).ToWorldCoordinates(0, 2), color, color, 2f);
						Utils.DrawLine(spriteBatch, new Vector2(minTileCursorX, (minTileCursorY + maxTileCursorY) / 2f).ToWorldCoordinates(0, 0), new Vector2((minTileCursorX + maxTileCursorX) / 2f, maxTileCursorY).ToWorldCoordinates(2, 0), color, color, 2f);
						Utils.DrawLine(spriteBatch, new Vector2((minTileCursorX + maxTileCursorX) / 2f, maxTileCursorY).ToWorldCoordinates(0, 0), new Vector2(maxTileCursorX, (minTileCursorY + maxTileCursorY) / 2f).ToWorldCoordinates(0, -2), color, color, 2f);
						Utils.DrawLine(spriteBatch, new Vector2(maxTileCursorX, (minTileCursorY + maxTileCursorY) / 2f).ToWorldCoordinates(0, 0), new Vector2((minTileCursorX + maxTileCursorX) / 2f, minTileCursorY).ToWorldCoordinates(-2, 0), color, color, 2f);
					}
					if (regionPickaxe.pickaxeShape == PickaxeShape.Ellipse)
					{
						for (int pointIndex = 0; pointIndex < 100; pointIndex++)
						{
							float radians = (float)MathHelper.TwoPi / 100 * pointIndex;
							float radiansPrev = (float)MathHelper.TwoPi / 100 * (pointIndex - 2f);
							float horizontalStretch = (maxTileCursorX - minTileCursorX) / 2f;
							float verticalStretch = (maxTileCursorY - minTileCursorY) / 2f;
							Vector2 pointPos = new Vector2(minTileCursorX + horizontalStretch + (float)Math.Cos(radians) * horizontalStretch, minTileCursorY + verticalStretch + (float)Math.Sin(radians) * verticalStretch);
							Vector2 pointPosPrev = new Vector2(minTileCursorX + horizontalStretch + (float)Math.Cos(radiansPrev) * horizontalStretch, minTileCursorY + verticalStretch + (float)Math.Sin(radiansPrev) * verticalStretch);
							Utils.DrawLine(spriteBatch, pointPosPrev * 16, pointPos * 16, color, color, 2f);
						}
					}

					// Draw what tiles will break
					for (int x = minTileCursorX; x < maxTileCursorX; x++)
					{
						for (int y = minTileCursorY; y < maxTileCursorY; y++)
						{
							if (ShouldDestroyTile(x, y, minTileCursorX, minTileCursorY, maxTileCursorX, maxTileCursorY, regionPickaxe.pickaxeShape, regionPickaxe.pickaxeSelectionInversion))
							{
								spriteBatch.Draw
								(
									ModContent.Request<Texture2D>("EpicosityPort/Misc/WhitePixel").Value,
									new Rectangle(x * 16 - (int)Main.screenPosition.X, y * 16 - (int)Main.screenPosition.Y, 16, 16),
									new Rectangle(0, 0, 1, 1),
									new Color(255, 255, 255, 20),
									0,
									Vector2.Zero,
									SpriteEffects.None,
									0
								);
							}
						}
					}
				}
				#endregion
			}
		}
		
		public static bool ShouldDestroyTile(int x, int y, int minTileX, int minTileY, int maxTileX, int maxTileY, PickaxeShape pickaxeShape, PickaxeSelectionInversion pickaxeSelectionInversion)
		{
			bool returnValue = false;

			// RS: Big if-elseif chain that Big Strategy and Deep State don't want you to see

			if (pickaxeShape == PickaxeShape.Rectangle)
			{
				returnValue = true;
			}

			// Elliptical mining
			else if (pickaxeShape == PickaxeShape.Ellipse)
			{
				int diameterX = Math.Abs(minTileX - maxTileX);
				int diameterY = Math.Abs(minTileY - maxTileY);
				float centerX = (minTileX + maxTileX - 1) / 2f;
				float centerY = (minTileY + maxTileY - 1) / 2f;
				returnValue = (Math.Pow(x - centerX, 2) / Math.Pow(diameterX / 2, 2)) + (Math.Pow(y - centerY, 2) / Math.Pow(diameterY / 2, 2)) <= 1;
			}

			// Random mining
			else if (pickaxeShape == PickaxeShape.Random)
			{
				returnValue = Main.rand.NextBool();
			}
			// Checkerboard mining
			else if (pickaxeShape == PickaxeShape.Checkerboard)
			{
				returnValue = (x + y) % 2 == 0;
			}

			// Rhombus mining
			else if (pickaxeShape == PickaxeShape.Rhombus)
			{
				float relativeX = (x - minTileX) / (float)(maxTileX - minTileX - 1);
				float relativeY = (y - minTileY) / (float)(maxTileY - minTileY - 1);
				returnValue = relativeY <= (1f - Math.Abs(relativeX - 0.5f)) && relativeY >= Math.Abs(relativeX - 0.5f);
			}

			// Selection inversion

			if (pickaxeSelectionInversion == PickaxeSelectionInversion.On)
			{
				returnValue = !returnValue;
			}

			return returnValue;
		}

		public static int PICKAXE_FILTER_COUNT = Enum.GetNames(typeof(PickaxeFilter)).Length;
		public static int PICKAXE_SHAPE_COUNT = Enum.GetNames(typeof(PickaxeShape)).Length;
		
		public PickaxeFilter pickaxeFilter = PickaxeFilter.TilesOnly;
		public PickaxeShape pickaxeShape = PickaxeShape.Rectangle;
		public PickaxeBypass pickaxeBypass = PickaxeBypass.Pickaxe;
		public PickaxeSelectionInversion pickaxeSelectionInversion = PickaxeSelectionInversion.Off;
		
		public int pickaxePower = 100;

		public enum PickaxeBypass
		{
			Pickaxe,
			WorldGen,
			BlackHole
		}
		public enum PickaxeFilter
		{
			TilesOnly,
			WallsOnly,
			TilesAndWalls,
			LiquidsOnly,
			Everything,
			PinkTilesOnly
		}
		public enum PickaxeShape
		{
			Rectangle,
			Ellipse,
			Rhombus,
			Random,
			Checkerboard
		}

		public enum PickaxeSelectionInversion
		{
			Off,
			On
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 1;
			Item.useAnimation = 1;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.channel = true;
		}
		public override void SetStaticSpecialDefaults()
		{
			// DisplayName.SetDefault("Region Pickaxe");
			// Tooltip.SetDefault("\nCheat Item\n'New and improved!'\nClick and drag to mine blocks");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (Main.myPlayer == player.whoAmI)
			{
				ModPlayerRegionPickaxeUI regionPickaxeUIPlayer = Main.LocalPlayer.GetModPlayer<ModPlayerRegionPickaxeUI>();
				regionPickaxeUIPlayer.cursor1Position = Main.MouseWorld;
			}
			return true;
		}
		public override bool AltFunctionUse(Player player)
		{
			return false;
		}
		public void KillTile(int x, int y)
		{
			if (pickaxeFilter == PickaxeFilter.TilesOnly || pickaxeFilter == PickaxeFilter.Everything || pickaxeFilter == PickaxeFilter.TilesAndWalls)
			{
				if (pickaxeBypass == PickaxeBypass.Pickaxe)
				{
					if (pickaxePower >= 0) Main.LocalPlayer.PickTile(x, y, pickaxePower);
					else { WorldGen.SquareTileFrame(x, y); }
				}
				else if (pickaxeBypass == PickaxeBypass.WorldGen)
				{
					WorldGen.KillTile(x, y, false, false, false);
				}
				else if (pickaxeBypass == PickaxeBypass.BlackHole)
				{
					Tile t = Main.tile[x, y];
					t.HasTile = false;
				}
			}
			else if (pickaxeFilter == PickaxeFilter.PinkTilesOnly)
			{
                // RS: tModLoader changed the way unloaded stuff works
				// It WOULD be useful to use VanillaFallbackOnModDeletion as a feature but... this is a pickaxe
                Tile t = Main.tile[x, y];
                ModTile modTile = ModContent.GetModTile(t.TileType);
                bool unloadedTile = modTile is UnloadedTile;
                if (t.HasTile && unloadedTile)
                {
                    //ushort type = TileIO.Tiles.unloadedEntryLookup.Lookup(i, j);
                    //var info = TileIO.Tiles.entries[type];
                    //if (modName == null || info.modName.Equals(modName, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    string key = $"{info.modName}/{info.name}";
                    //    purgedTiles.TryGetValue(key, out var currentCount);
                    //    purgedTiles[key] = currentCount + 1;
                    //    if (actuallyPurge)
                    //    {
                    //        t.TileType = info.vanillaReplacementType;
                    //    }
                    
                    if (pickaxeBypass == PickaxeBypass.Pickaxe)
                    {
                        if (pickaxePower >= 0) Main.LocalPlayer.PickTile(x, y, pickaxePower);
                        else { WorldGen.SquareTileFrame(x, y); }
                    }
                    else if (pickaxeBypass == PickaxeBypass.WorldGen)
                    {
                        WorldGen.KillTile(x, y, false, false, false);
                    }
                    else if (pickaxeBypass == PickaxeBypass.BlackHole)
                    {
                        t.HasTile = false;
                    }
                }
                var unloadedWallType = ModContent.WallType<UnloadedWall>();
				if (t.WallType == unloadedWallType)
				{
                    if (pickaxeBypass == PickaxeBypass.Pickaxe || pickaxeBypass == PickaxeBypass.WorldGen)
                    {
                        WorldGen.KillWall(x, y);
                    }
                    else if (pickaxeBypass == PickaxeBypass.BlackHole)
                    {
                        Main.tile[x, y].WallType = WallID.None;
                    }
                }
                //bool success = ModLoader.GetMod("ModLoader").TryFind<ModTile>("MysteryTile");
                //if (!success) return;

                //            ushort num = (ushort)ModLoader.GetMod("ModLoader").Find<ModTile>("MysteryTile").Type;
                //Tile tile = Framing.GetTileSafely(x, y);
                //bool flag = tile.TileType != num || !tile.HasTile;
                //if (!flag)
                //{
    //            	if (pickaxeBypass == PickaxeBypass.Pickaxe)
				//{
				//	if (pickaxePower >= 0) Main.LocalPlayer.PickTile(x, y, pickaxePower);
				//	else { WorldGen.SquareTileFrame(x, y); }
				//}
				//else if (pickaxeBypass == PickaxeBypass.WorldGen)
				//{
				//	WorldGen.KillTile(x, y, false, false, false);
				//}
				//else if (pickaxeBypass == PickaxeBypass.BlackHole)
				//{
				//	Tile t = Main.tile[x, y];
				//	t.HasTile = false;
				//}
				//}
			}
			if (pickaxeFilter == PickaxeFilter.WallsOnly || pickaxeFilter == PickaxeFilter.Everything || pickaxeFilter == PickaxeFilter.TilesAndWalls)
			{
				if (pickaxeBypass == PickaxeBypass.Pickaxe || pickaxeBypass == PickaxeBypass.WorldGen)
				{
					WorldGen.KillWall(x, y);
				}
				else if (pickaxeBypass == PickaxeBypass.BlackHole)
				{
                    Main.tile[x, y].WallType = WallID.None;
				}
			}
			if (pickaxeFilter == PickaxeFilter.LiquidsOnly || pickaxeFilter == PickaxeFilter.Everything)
			{
				Main.tile[x, y].LiquidAmount = 0;
				if (pickaxeBypass != PickaxeBypass.BlackHole)
				{
					WorldGen.SquareTileFrame(x, y);
				}
			}
		}
	}
	public class ModPlayerRegionPickaxeUI : ModPlayer
	{
		public bool lastControlUseTile = false;
		public bool lastControlUseItem = false;
		public Vector2? cursor1Position = null;
		public Vector2? cursor2Position = null;
		public Vector2 menuScreenPosition = Vector2.Zero;
		public bool menuOpen = false;
		public float guiAnimationProgress = 0;
		public float guiButtonAnimationProgress = 0;
		public override void PreUpdate()
		{
			if (menuOpen)
			{
				lastControlUseItem = Player.controlUseItem;
			}
		}
		public override void PostUpdate()
		{
			if (Main.LocalPlayer.HeldItem.type == Mod.Find<ModItem>("RegionPickaxe").Type && Main.myPlayer == Player.whoAmI)
			{
				RegionPickaxe regionPickaxe = GetLocalRegionPickaxe();
				ModPlayerRegionPickaxeUI regionPickaxeUIPlayer = Main.LocalPlayer.GetModPlayer<ModPlayerRegionPickaxeUI>();
				
				if (regionPickaxeUIPlayer == null)
				{
					return;
				}

				// Menu animation
				if (Player.controlUseTile && !lastControlUseTile && (guiAnimationProgress <= 0 || guiAnimationProgress >= 1))
				{
					menuOpen = !menuOpen;
					if (menuOpen)
					{
						//Main.NewText("Menu is on!");
						menuScreenPosition = Main.MouseScreen;
						// Prevents breaking upon exiting menu
						cursor1Position = null;
						cursor2Position = null;
					}
				}

				lastControlUseTile = Player.controlUseTile;
				
				
				// Prevents any selection while menu is open
				if (menuOpen)
				{
					if (guiAnimationProgress < 1)
					{
						// RS: No time. No deltaTime. We ball
						guiAnimationProgress += 0.05f;
					}

					return;
				}
				else
				{
					if (guiAnimationProgress > 0)
					{
						guiAnimationProgress -= 0.05f;
					}
				}

				// Selection logic
				if (Player.channel)
				{
					if (regionPickaxeUIPlayer.cursor1Position != null)
					{
						// Since cursor1Position is already defined, cursor2Position must be at the MouseWorld
						regionPickaxeUIPlayer.cursor2Position = Main.MouseWorld;
					}
				}

				if (!regionPickaxeUIPlayer.cursor1Position.HasValue || !regionPickaxeUIPlayer.cursor2Position.HasValue)
				{
					return;
				}
				#region Destroy tiles
				// Activate destruction since the pickaxe is no longer channeling and both boundaries are defined
				if (!Player.channel)
				{
					//Main.NewText("Destroying region!");
					
					// Both are defined. Later, we'll make both null so the mining wouldn't trigger again

					int[] selectionBoundaries = GetSelectionBoundaries();
					
					int minTileX = selectionBoundaries[0];
					int maxTileX = selectionBoundaries[1];
					int minTileY = selectionBoundaries[2];
					int maxTileY = selectionBoundaries[3];

					AchievementsHelper.CurrentlyMining = true;

					// All types of mining
					for (int x = minTileX; x < maxTileX; x++)
					{
						for (int y = minTileY; y < maxTileY; y++)
						{
							if (ShouldDestroyTile(x, y, minTileX, minTileY, maxTileX, maxTileY, regionPickaxe.pickaxeShape, regionPickaxe.pickaxeSelectionInversion))
							{
								regionPickaxe.KillTile(x, y);
							}
						}
					}

					AchievementsHelper.CurrentlyMining = false;

					if (regionPickaxe.pickaxeBypass != PickaxeBypass.Pickaxe) // If black hole bypass or world gen bypass, update world for rest of players
					{
						//NetMessage.SendTileRange(Main.myPlayer, minTileX - 1, minTileY - 1, Math.Abs(maxTileX - minTileX) + 1, Math.Abs(maxTileY - minTileY) + 1);
						NetMessage.SendTileSquare(Main.myPlayer, minTileX - 1, minTileY - 1, Math.Abs(maxTileX - minTileX) + 1, Math.Abs(maxTileY - minTileY) + 1);
					}

					regionPickaxeUIPlayer.cursor1Position = null;
					regionPickaxeUIPlayer.cursor2Position = null;
				}
			}
			#endregion
		}
	}
}
