using EpicosityPort;
using EpicosityPort.Systems.UserInterface.VanillaOverrides;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Epicosity
{
    public class EpicosityPortModPlayer : ModPlayer
    {
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (EpicosityConfigClient.Instance.DisableModifiedDeathMessages)
            {
                return true;
            }
            DeathInterfaceOverride.RandomizeSpecialDeathMessageMessage(damageSource);
            return true;
        }
    }
}
