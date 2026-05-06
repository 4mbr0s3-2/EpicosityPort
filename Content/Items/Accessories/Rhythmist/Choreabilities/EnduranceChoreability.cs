using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class EnduranceChoreability : ChoreabilityBase
    {
        public override string Texture => typeof(EnduranceChoreability).GetQualifiedNameAsPath();

        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Endurance, level);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChoreabilityEndurance", $"Reduces damage taken by a maximum of {choreability.strength * 100}%"));
        }
    }
}
