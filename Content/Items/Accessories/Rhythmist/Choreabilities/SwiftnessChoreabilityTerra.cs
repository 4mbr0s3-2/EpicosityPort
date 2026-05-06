using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class SwiftnessChoreabilityTerra : SwiftnessChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Swiftness Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Terra);
            base.SetDefaults();
        }
    }
}
