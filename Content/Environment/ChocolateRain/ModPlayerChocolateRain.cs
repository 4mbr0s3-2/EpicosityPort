using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public class ModPlayerChocolateRain : ModPlayer
    {
        public override void PostUpdate()
        {
            // Local Player only
            if (Main.netMode != NetmodeID.Server && Player.whoAmI == Main.myPlayer)
            {
                foreach (Rain rain in Main.rain)
                {
                    if (rain.active && ChocolateRainManager.Instance.IsRainingChocolate && Collision.CheckAABBvAABBCollision(rain.position, new Microsoft.Xna.Framework.Vector2(6, 6), Player.position, Player.Size))
                    {
                        Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " didn't stay dry and felt the pain."), 3, 0);
                    }
                }
            }
        }
    }
}
