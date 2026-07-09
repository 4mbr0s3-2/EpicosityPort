using EpicosityPort.Effects.Shaders.Screen;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
    public class ChocolateRainSceneEffect : ModSceneEffect
    {
        public override void Load()
        {
            On_Main.CalculateWaterStyle += On_Main_CalculateWaterStyle;
        }
        public override void Unload()
        {
            On_Main.CalculateWaterStyle -= On_Main_CalculateWaterStyle;
        }

        private int On_Main_CalculateWaterStyle(On_Main.orig_CalculateWaterStyle orig, bool ignoreFountains)
        {
            // RS: We're still going to need to prioritize this over vanilla...
            // Sorry to all other styles
            int original = orig(ignoreFountains);
            if (original < LoaderManager.Get<WaterStylesLoader>().VanillaCount && ChocolateRainManager.Instance.IsRainingChocolate)
                return WaterStyle.Slot;
            return original;
        }

        public override float GetWeight(Player player)
        {
            return 1f;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
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
