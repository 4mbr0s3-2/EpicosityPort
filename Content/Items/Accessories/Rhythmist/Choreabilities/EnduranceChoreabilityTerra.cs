using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class EnduranceChoreabilityTerra : EnduranceChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Endurance Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Terra);
            base.SetDefaults();
        }
    }
}
