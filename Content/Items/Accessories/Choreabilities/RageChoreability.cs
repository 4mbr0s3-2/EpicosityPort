using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public abstract class RageChoreability : ChoreabilityBase
    {
        public override string Texture => typeof(RageChoreability).GetQualifiedNameAsPath();
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Rage, level);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChoreabilityRage", $"Increases critical chance by a maximum of {choreability.strength * 100}%"));
        }
    }
}
