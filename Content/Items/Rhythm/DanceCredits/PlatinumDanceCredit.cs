using Terraria.ID;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public class PlatinumDanceCredit : DanceCredit
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            base.SetDefaults();
        }
        public override ChoreabilityTier GetTier()
        {
            return ChoreabilityTier.Platinum;
        }
    }
}
