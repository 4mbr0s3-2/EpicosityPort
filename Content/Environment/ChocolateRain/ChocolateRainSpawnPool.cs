using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public class ChocolateRainSpawnPool : GlobalNPC
    {
        public static List<int> chocolateEnemyList = new List<int>();
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (ChocolateRainManager.Instance.IsRainingChocolate)
            {
                pool.Clear();
                foreach (int npcID in chocolateEnemyList)
                {
                    pool.Add(npcID, NPCLoader.GetNPC(npcID).SpawnChance(spawnInfo));
                }
            }
        }
    }
}
