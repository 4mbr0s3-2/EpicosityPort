using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class SwiftnessChoreabilityCopper : SwiftnessChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copper Swiftness Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Copper);
            base.SetDefaults();
        }
    }
}
