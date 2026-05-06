using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class SwiftnessChoreabilityGold : SwiftnessChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Swiftness Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Gold);
            base.SetDefaults();
        }
    }
}
