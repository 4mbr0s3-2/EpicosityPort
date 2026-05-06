using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class IronskinChoreabilityGold : IronskinChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Ironskin Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Gold);
            base.SetDefaults();
        }
    }
}
