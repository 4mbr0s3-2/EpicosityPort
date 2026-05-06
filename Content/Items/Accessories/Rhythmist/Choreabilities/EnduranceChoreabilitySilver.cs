using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class EnduranceChoreabilitySilver : EnduranceChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Endurance Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Silver);
            base.SetDefaults();
        }
    }
}
