using EpicosityPort.Content.Items.Tiles.Ores.MemeiteOre;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EpicosityPort.Systems.WorldGen
{
    public class EpicosityWorldGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            tasks.Insert(genIndex + 1, new PassLegacy("Epicosity Shinies", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Adding some epicness...";
                for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 9E-05); k++)
                {
                    // The inside of this for loop corresponds to one single splotch of our Ore.
                    // First, we randomly choose any coordinate in the world by choosing a random x and y value.
                    int x = Terraria.WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = Terraria.WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY); // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.

                    // Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place. Feel free to experiment with strength and step to see the shape they generate.
                    Terraria.WorldGen.TileRunner(x, y, Terraria.WorldGen.genRand.Next(3, 6), Terraria.WorldGen.genRand.Next(2, 6), ModContent.TileType<MemeiteOreTile>());

                    // Alternately, we could check the tile already present in the coordinate we are interested. Wrapping WorldGen.TileRunner in the following condition would make the ore only generate in Snow.
                    // Tile tile = Framing.GetTileSafely(x, y);
                    // if (tile.active() && tile.type == TileID.SnowBlock)
                    // {
                    // 	WorldGen.TileRunner(.....);
                    // }
                }

            }));
        }
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
        }
        public override void PostWorldGen()
        {
            void AddBuffer(List<int> accessories, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    accessories.Add(0);
                }
            }
            // Add rhythmist accessories
            List<int> rhythmistAccessories = new List<int>();
            List<Chest> chests = Main.chest.ToList();

            rhythmistAccessories.Add(0);

            //rhythmistAccessories.Add(ModContent.ItemType<Pacemaker>());
            //rhythmistAccessories.Add(ModContent.ItemType<TapCane>());
            //rhythmistAccessories.Add(ModContent.ItemType<MetallicShoeTaps>());
            //AddBuffer(rhythmistAccessories, 10);
            //rhythmistAccessories.Add(ModContent.ItemType<Flashlight>());
            //rhythmistAccessories.Add(ModContent.ItemType<Leek>());
            //AddBuffer(rhythmistAccessories, 2);
            //rhythmistAccessories.Add(ModContent.ItemType<PizzaSauce>());
            //rhythmistAccessories.Add(ModContent.ItemType<MagicSpiceMix>());
            //AddBuffer(rhythmistAccessories, 13);
            //rhythmistAccessories.Add(ModContent.ItemType<NoteBlock>());
            //AddBuffer(rhythmistAccessories, 23);
            //rhythmistAccessories.Add(ModContent.ItemType<BillyGun>());
            //rhythmistAccessories.Add(ModContent.ItemType<Calebkulele>());
            //AddBuffer(rhythmistAccessories, 10);
            //rhythmistAccessories.Add(ModContent.ItemType<WonderGuitar>());

            // Sort by closest to spawn, use enumerator to iterate
            IEnumerator<Chest> closestToSpawn = chests.Where((chest) => chest != null).OrderBy((chest) => (new Vector2(chest.x, chest.y) - new Vector2(Main.spawnTileX, Main.spawnTileY)).Length()).GetEnumerator();
            foreach (int type in rhythmistAccessories)
            {
                bool success = closestToSpawn.MoveNext();
                if (!success) break;
                Chest chest = closestToSpawn.Current;
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None) // Put item in first blank slot
                        {
                            chest.item[inventoryIndex].SetDefaults(type);
                            chest.item[inventoryIndex].stack = 1;
                            break;
                        }
                    }
                }
            }
        }

    }
}
