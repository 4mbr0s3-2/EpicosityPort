using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class IronskinChoreabilityPlatinum : IronskinChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Platinum Ironskin Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Platinum);
            base.SetDefaults();
        }
    }
}
