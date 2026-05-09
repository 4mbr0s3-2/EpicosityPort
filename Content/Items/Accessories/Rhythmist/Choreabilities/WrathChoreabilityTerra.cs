using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities
{
    public class WrathChoreabilityTerra : WrathChoreability
    {
        public override void SetDefaults()
        {
            SetChoreability(ChoreabilityTier.Terra);
            base.SetDefaults();
        }
    }
}
