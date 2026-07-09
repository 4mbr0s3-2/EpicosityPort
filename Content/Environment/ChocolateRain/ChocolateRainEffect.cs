using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public class ChocolateRainEffect : GlobalNPC
    {
        public int rainCooldown = 0;
        public override bool InstancePerEntity => true;
        public override void AI(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            if (!ChocolateRainManager.Instance.IsRainingChocolate)
                return;
            if (ChocolateRainSpawnPool.ChocolateEnemyList.Contains(npc.type)) 
                return;
            // RS: Is this a good idea?
            foreach (Rain rain in Main.rain)
            {
                //if (rain.active && Collision.CheckAABBvAABBCollision(rain.position, new Microsoft.Xna.Framework.Vector2(6, 6), npc.position, npc.Size))
                if (rain.active
                    && rain.waterStyle == ModContent.GetInstance<ChocolateRainWaterStyle>().Slot
                    && Collision.CheckAABBvAABBCollision(rain.position, new Microsoft.Xna.Framework.Vector2(6, 6), npc.position, npc.Size))
                {
                    if (rainCooldown <= 0)
                    {
                        NPC.HitInfo hit = npc.CalculateHitInfo(3, 0, false);
                        npc.StrikeNPC(hit);
                        rainCooldown = 5;
                        // RS: Does rain even run on the server
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendStrikeNPC(npc, hit);
                    }
                    else
                    {
                        rainCooldown--;
                    }
                }
            }
        }
    }
}
