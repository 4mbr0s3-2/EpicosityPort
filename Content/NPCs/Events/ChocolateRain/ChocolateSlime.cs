using EpicosityPort.Content.Environment.ChocolateRain;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace EpicosityPort.Content.NPCs.Events.ChocolateRain
{
    public class ChocolateSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            ChocolateRainSpawnPool.ChocolateEnemyList.Add(NPC.type);
            // DisplayName.SetDefault("Chocoslime");
        }
        public override void SetDefaults()
        {
            Terraria.Main.npcFrameCount[NPC.type] = 2;
            NPC.width = 16;
            NPC.height = 13;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.lifeMax = Terraria.Main.hardMode ? 200 : 100;
            NPC.defense = 30;
            NPC.damage = 30;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.BlueSlime;
            NPC.value = Terraria.Item.buyPrice(0, 0, 5);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return ChocolateRainManager.Instance.IsRainingChocolate ? SpawnCondition.OverworldDay.Chance : 0;
        }
    }
}
