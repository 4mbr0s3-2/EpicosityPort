using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class SwiftnessChoreabilitySilver : SwiftnessChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Swiftness Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Silver);
            base.SetDefaults();
        }
    }
}
