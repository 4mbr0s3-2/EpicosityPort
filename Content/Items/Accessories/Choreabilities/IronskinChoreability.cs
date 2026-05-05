using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public abstract class IronskinChoreability : ChoreabilityBase
    {
        public override string Texture => typeof(IronskinChoreability).GetQualifiedNameAsPath();
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Ironskin, level);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChoreabilityIronskin", $"Increases defense by a maximum of {choreability.strength}"));
        }
    }
}
