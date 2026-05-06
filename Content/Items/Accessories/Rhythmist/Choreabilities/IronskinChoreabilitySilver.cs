using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class IronskinChoreabilitySilver : IronskinChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Ironskin Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Silver);
            base.SetDefaults();
        }
    }
}
