using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class EnduranceChoreabilityGold : EnduranceChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Endurance Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Gold);
            base.SetDefaults();
        }
    }
}
