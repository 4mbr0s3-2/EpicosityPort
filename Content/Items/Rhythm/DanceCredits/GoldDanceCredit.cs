using Terraria.ID;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public class GoldDanceCredit : DanceCredit
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            base.SetDefaults();
        }
        public override ChoreabilityTier GetTier()
        {
            return ChoreabilityTier.Gold;
        }
    }
}
