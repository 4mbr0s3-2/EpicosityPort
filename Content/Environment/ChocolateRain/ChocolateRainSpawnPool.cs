using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public class ChocolateRainSpawnPool : GlobalNPC
    {
        // RS: Changed to hashset for easier lookup
        private static HashSet<int> chocolateEnemyList;

        public static HashSet<int> ChocolateEnemyList { get => chocolateEnemyList; set => chocolateEnemyList = value; }

        public override void Load()
        {
            ChocolateEnemyList = [];
        }

        public override void Unload()
        {
            ChocolateEnemyList.Clear();
            ChocolateEnemyList = null;
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (ChocolateRainManager.Instance.IsRainingChocolate)
            {
                pool.Clear();
                foreach (int npcID in ChocolateEnemyList)
                {
                    pool.Add(npcID, NPCLoader.GetNPC(npcID).SpawnChance(spawnInfo));
                }
            }
        }
    }
}
