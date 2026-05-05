using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class IronskinChoreabilityTerra : IronskinChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Ironskin Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Terra);
            base.SetDefaults();
        }
    }
}
