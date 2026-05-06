using EpicosityPort.Systems.Rhythm;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class WrathChoreabilityCopper : WrathChoreability
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Copper Wrath Choreability");
        }
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityHelper.ChoreabilityTier.Copper);
            base.SetDefaults();
        }
    }
}
