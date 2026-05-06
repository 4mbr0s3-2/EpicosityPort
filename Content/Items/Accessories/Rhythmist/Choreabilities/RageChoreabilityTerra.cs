using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class RageChoreabilityTerra : RageChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Rage Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Terra);
            base.SetDefaults();
        }
    }
}
