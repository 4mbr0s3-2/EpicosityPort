using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Systems.Rhythm
{
    public class ChoreabilityTierManager : ModSystem
    {
        public static ChoreabilityTierManager Instance { get; private set; }
        public override void OnModLoad()
        {
            Instance = new ChoreabilityTierManager();
        }
        public override void OnModUnload()
        {
            Instance = null;
        }
        private Dictionary<int, ChoreabilityType> potionToChoreabilityType = new Dictionary<int, ChoreabilityType>
        {
            [ItemID.RagePotion] = ChoreabilityType.Rage,
            [ItemID.IronskinPotion] = ChoreabilityType.Ironskin,
            [ItemID.SwiftnessPotion] = ChoreabilityType.Swiftness,
            //[ItemID.EndurancePotion] = ChoreabilityType.Endurance,
            //[ItemID.WrathPotion] = ChoreabilityType.Wrath
        };
        private Dictionary<ChoreabilityTier, int> numberOfCreditsNeeded = new Dictionary<ChoreabilityTier, int>()
        {
            [ChoreabilityTier.Copper] = 10,
            [ChoreabilityTier.Silver] = 15,
            [ChoreabilityTier.Gold] = 20,
            [ChoreabilityTier.Platinum] = 30,
            [ChoreabilityTier.Terra] = 45,
        };

        public static Dictionary<int, ChoreabilityType> PotionToChoreabilityType { get => Instance.potionToChoreabilityType; set => Instance.potionToChoreabilityType = value; }
        public static Dictionary<ChoreabilityTier, int> NumberOfCreditsNeeded { get => Instance.numberOfCreditsNeeded; set => Instance.numberOfCreditsNeeded = value; }

        public static int GetNumberOfCreditsNeeded(ChoreabilityTier choreabilityTier)
        {
            if (NumberOfCreditsNeeded.ContainsKey(choreabilityTier))
            {
                return NumberOfCreditsNeeded[choreabilityTier];
            }
            else
            {
                return 0;
            }
        }
        public static int GetNextTier(ChoreabilityTier choreabilityTier)
        {
            if ((int)choreabilityTier + 1 >= Enum.GetNames(typeof(ChoreabilityTier)).Count()) // Reached the end of the tier list
            {
                return -1;
            }
            else
            {
                return ((int)choreabilityTier + 1);
            }
        }
    }
}
