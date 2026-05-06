using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class RageChoreabilityCopper : RageChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copper Rage Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Copper);
            base.SetDefaults();
        }
    }
}
