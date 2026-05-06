using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class WrathChoreabilitySilver : WrathChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Wrath Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Silver);
            base.SetDefaults();
        }
    }
}
