using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class WrathChoreabilityGold : WrathChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Wrath Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Gold);
            base.SetDefaults();
        }
    }
}
