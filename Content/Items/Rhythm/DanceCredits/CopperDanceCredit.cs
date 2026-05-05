using Terraria.ID;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public class CopperDanceCredit : DanceCredit
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            base.SetDefaults();
        }
        public override ChoreabilityTier GetTier()
        {
            return ChoreabilityTier.Copper;
        }
    }
}
