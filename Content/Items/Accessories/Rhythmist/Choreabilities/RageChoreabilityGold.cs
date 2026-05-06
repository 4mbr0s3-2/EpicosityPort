using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class RageChoreabilityGold : RageChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Rage Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Gold);
            base.SetDefaults();
        }
    }
}
