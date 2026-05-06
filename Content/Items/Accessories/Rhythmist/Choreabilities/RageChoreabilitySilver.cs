using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class RageChoreabilitySilver : RageChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Rage Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Silver);
            base.SetDefaults();
        }
    }
}
