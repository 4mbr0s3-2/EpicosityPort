using EpicosityPort.Content.Items.Accessories.Rhythmist.Choreabilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Rhythm
{
    public class ChoreabilityHelper
    {
        /// <summary>
        /// Type names must be consistent with the texture names!
        /// </summary>
        public enum ChoreabilityType
        {
            Ironskin,
            Endurance,
            Swiftness,
            Rage,
            Wrath
        }
        public enum ChoreabilityTier
        {
            Custom,
            Copper,
            Silver,
            Gold,
            Platinum,
            Terra,
        }
        public static Choreability BuildChoreability(ChoreabilityType choreabilityType, ChoreabilityTier choreabilityLevel, float choreabilityStrength = -1)
        {
            Action<Player, float, float> playerModifier;
            // RS: Somethin' somethin' switch case abuse use
            // Choreability more like CHOREability 😂😂😂😂
            switch (choreabilityType)
            {
                case ChoreabilityType.Endurance:
                    playerModifier = (player, comboMeter, strength) =>
                    {
                        player.endurance += comboMeter * strength;
                    };
                    break;
                case ChoreabilityType.Ironskin:
                    playerModifier = (player, comboMeter, strength) =>
                    {
                        player.statDefense += (int)(comboMeter * strength);
                    };
                    break;
                case ChoreabilityType.Rage:
                    playerModifier = (player, comboMeter, strength) =>
                    {
                        // TODO: Add Rogue from Calamity and Bard and Healer from Thorium
                        //player.GetCritChance(DamageClass.Melee) += (int)(comboMeter * strength * 100);
                        //player.GetCritChance(DamageClass.Ranged) += (int)(comboMeter * strength * 100);
                        //player.GetCritChance(DamageClass.Magic) += (int)(comboMeter * strength * 100);
                        //player.GetCritChance(DamageClass.Throwing) += (int)(comboMeter * strength * 100);
                        // RS: No need for that now
                        player.GetCritChance(DamageClass.Generic) += (int)(comboMeter * strength * 100);
                        //player.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicCrit += (int)(comboMeter * strength * 100);
                    };
                    break;
                case ChoreabilityType.Swiftness:
                    playerModifier = (player, comboMeter, strength) =>
                    {
                        player.moveSpeed += comboMeter * strength;
                        player.maxRunSpeed += comboMeter * strength;
                    };
                    break;
                case ChoreabilityType.Wrath:
                    playerModifier = (player, comboMeter, strength) =>
                    {
                        player.GetDamage(DamageClass.Generic) += comboMeter * strength;
                    };
                    break;
                default:
                    playerModifier = (player, comboMeter, strength) =>
                    {
                        player.moveSpeed += comboMeter * strength;
                        player.maxRunSpeed += comboMeter * strength;
                    };
                    break;
            }
            if (choreabilityStrength < 0 && choreabilityLevel != ChoreabilityTier.Custom)
            {
                switch (choreabilityType)
                {
                    case ChoreabilityType.Endurance:
                        choreabilityStrength = 0.05f * (int)choreabilityLevel;
                        break;
                    case ChoreabilityType.Ironskin:
                        choreabilityStrength = 5f * (int)choreabilityLevel;
                        break;
                    case ChoreabilityType.Rage:
                        choreabilityStrength = 0.05f * (int)choreabilityLevel;
                        break;
                    case ChoreabilityType.Swiftness:
                        choreabilityStrength = 0.15f * (int)choreabilityLevel;
                        break;
                    case ChoreabilityType.Wrath:
                        choreabilityStrength = 0.05f * (int)choreabilityLevel;
                        break;
                    default:
                        break;
                }
            }
            return new Choreability(playerModifier, choreabilityStrength, choreabilityType, choreabilityLevel);
        }
    }
}
