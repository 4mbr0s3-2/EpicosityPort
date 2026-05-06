using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class EnduranceChoreabilityPlatinum : EnduranceChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Platinum Endurance Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Platinum);
            base.SetDefaults();
        }
    }
}
