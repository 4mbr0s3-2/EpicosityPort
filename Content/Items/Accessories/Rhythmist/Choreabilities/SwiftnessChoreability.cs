using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class SwiftnessChoreability : ChoreabilityBase
    {
        public override string Texture => typeof(SwiftnessChoreability).GetQualifiedNameAsPath();
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Swiftness, level);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChoreabilitySwiftness", $"Increases movement speed by a maximum of {choreability.strength * 100}%"));
        }
    }
}
