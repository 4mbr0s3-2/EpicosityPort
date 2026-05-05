using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class RageChoreabilityPlatinum : RageChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Platinum Rage Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Platinum);
            base.SetDefaults();
        }
    }
}
