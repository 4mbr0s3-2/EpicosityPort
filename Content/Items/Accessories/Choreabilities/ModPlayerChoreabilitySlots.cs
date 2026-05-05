using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.UserInterface.ChoreabilitySlots;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Accessories.Choreabilities
{
    public class ModPlayerChoreabilitySlots : ModPlayer
    {
        public override void PostUpdateBuffs()
        {
            ChoreabilitySlotsPlayer choreabilitySlotsPlayer = Player.GetModPlayer<ChoreabilitySlotsPlayer>();
            foreach (Item item in choreabilitySlotsPlayer.choreabilitySlotsInventory)
            {
                if (item.ModItem is ChoreabilityBase)
                {
                    ModPlayerRhythmist rhythmicPlayer = Player.GetModPlayer<ModPlayerRhythmist>();
                    ChoreabilityBase choreabilityItem = item.ModItem as ChoreabilityBase;
                    if (choreabilityItem.choreability == null) item.ModItem.SetDefaults(); // For some reason, the item doesn't have a set choreability... sweep that under the rug and just fix it on the spot
                    choreabilityItem.choreability.playerModifier(Player, rhythmicPlayer.comboMeter, choreabilityItem.choreability.strength);
                }
            }
        }
    }
}
