using System;
using Terraria;
using static EpicosityPort.Systems.Rhythm.ChoreabilityHelper;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class Choreability
    {
        public ChoreabilityTier tier;
        public ChoreabilityType type;
        public Choreability(Action<Player, float, float> playerModifier, float strength, ChoreabilityType type, ChoreabilityTier level)
        {
            this.type = type;
            this.tier = level;
            this.playerModifier = playerModifier;
            this.strength = strength;
        }
        public Action<Player, float, float> playerModifier;
        public float strength;
    }
}
