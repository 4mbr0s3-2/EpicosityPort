using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class WrathChoreability : ChoreabilityBase
    {
        public override LocalizedText ChoreabilityTooltip => Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(WrathChoreability)}.{nameof(ChoreabilityTooltip)}"));
        public override string Texture => typeof(WrathChoreability).GetQualifiedNameAsPath();
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Wrath, level);
        }
        //public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
        //    tooltips.Add(new TooltipLine(Mod, "ChoreabilityWrath", $"Increases damage by a maximum of {choreability.strength * 100}%"));
        //}
    }
}
