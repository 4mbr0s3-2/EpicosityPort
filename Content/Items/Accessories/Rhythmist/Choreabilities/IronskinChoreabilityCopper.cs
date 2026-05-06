using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class IronskinChoreabilityCopper : IronskinChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copper Ironskin Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Copper);
            base.SetDefaults();
        }
    }
}
