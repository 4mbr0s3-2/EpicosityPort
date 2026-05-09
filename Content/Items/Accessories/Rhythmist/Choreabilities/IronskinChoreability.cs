using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class IronskinChoreability : ChoreabilityBase
    {
        public override LocalizedText ChoreabilityTooltip => Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(IronskinChoreability)}.{nameof(ChoreabilityTooltip)}"));
        public override string Texture => typeof(IronskinChoreability).GetQualifiedNameAsPath();
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Ironskin, level);
        }
        //public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
        //    tooltips.Add(new TooltipLine(Mod, "ChoreabilityIronskin", $"Increases defense by a maximum of {choreability.strength}"));
        //}
    }
}
