using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class EnduranceChoreability : ChoreabilityBase
    {
        public override LocalizedText ChoreabilityTooltip => Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(EnduranceChoreability)}.{nameof(ChoreabilityTooltip)}"));
        public override string Texture => typeof(EnduranceChoreability).GetQualifiedNameAsPath();

        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Endurance, level);
        }
        //public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
        //    tooltips.Add(new TooltipLine(Mod, "ChoreabilityEndurance", $"Reduces damage taken by a maximum of {choreability.strength * 100}%"));
        //}
    }
}
