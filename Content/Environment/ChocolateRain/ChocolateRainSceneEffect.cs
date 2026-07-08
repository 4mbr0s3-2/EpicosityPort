using EpicosityPort.Effects.Shaders.Screen;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public class ChocolateRainSceneEffect : ModSceneEffect
    {
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals(ShaderNames.ChocolateRainFilter, isActive);
        }
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<ChocolateRainWaterStyle>();
        public override bool IsSceneEffectActive(Player player)
        {
            return ChocolateRainManager.Instance.IsRainingChocolate;
        }
    }
}
