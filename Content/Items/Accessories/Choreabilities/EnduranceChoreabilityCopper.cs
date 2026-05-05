using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Utilities;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class EnduranceChoreabilityCopper : EnduranceChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copper Endurance Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Copper);
            base.SetDefaults();
        }
    }
}
