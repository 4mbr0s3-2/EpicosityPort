using Terraria.ID;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public class SilverDanceCredit : DanceCredit
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            base.SetDefaults();
        }
        public override ChoreabilityTier GetTier()
        {
            return ChoreabilityTier.Silver;
        }
    }
}
