using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class RageChoreability : ChoreabilityBase
    {
        public override LocalizedText ChoreabilityTooltip => Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(RageChoreability)}.{nameof(ChoreabilityTooltip)}"));
        public override string Texture => typeof(RageChoreability).GetQualifiedNameAsPath();
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Rage, level);
        }
        //public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
        //    tooltips.Add(new TooltipLine(Mod, "ChoreabilityRage", $"Increases critical chance by a maximum of {choreability.strength * 100}%"));
        //}
    }
}
