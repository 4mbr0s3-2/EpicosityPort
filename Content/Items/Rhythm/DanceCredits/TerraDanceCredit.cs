using Terraria.ID;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public class TerraDanceCredit : DanceCredit
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
            base.SetDefaults();
        }
        public override ChoreabilityTier GetTier()
        {
            return ChoreabilityTier.Terra;
        }
    }
}
