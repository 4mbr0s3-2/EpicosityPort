using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class SwiftnessChoreability : ChoreabilityBase
    {
        public override LocalizedText ChoreabilityTooltip => Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(SwiftnessChoreability)}.{nameof(ChoreabilityTooltip)}"));
        public override string Texture => typeof(SwiftnessChoreability).GetQualifiedNameAsPath();

        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Swiftness, level);
        }
        //public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
        //    tooltips.Add(new TooltipLine(Mod, "ChoreabilitySwiftness", $"Increases movement speed by a maximum of {choreability.strength * 100}%"));
        //}
    }
}
