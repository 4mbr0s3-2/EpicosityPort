using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public abstract class WrathChoreability : ChoreabilityBase
    {
        public override string Texture => typeof(WrathChoreability).GetQualifiedNameAsPath();
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public void SetChoreability(ChoreabilityHelper.ChoreabilityTier level)
        {
            choreability = ChoreabilityHelper.BuildChoreability(ChoreabilityHelper.ChoreabilityType.Wrath, level);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChoreabilityWrath", $"Increases damage by a maximum of {choreability.strength * 100}%"));
        }
    }
}
