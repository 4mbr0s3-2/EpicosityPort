using EpicosityPort.Systems.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Weapons.Melee.ComicallyLargeSpoon
{
    public class ModPlayerVineBoomSFX : ModPlayer
    {
        bool heldSpoonPrev = false;
        public override void PostUpdate()
        {
            // RS: Observer? What's that? What's an observer pattern? Doesn't this work?
            if (Player.HeldItem.type == ModContent.ItemType<ComicallyLargeSpoon>() && !heldSpoonPrev)
            {
                SoundEngine.PlaySound(GetType().GetSound("VineBoom"), Player.Center);
            }
            heldSpoonPrev = Player.HeldItem.type == ModContent.ItemType<ComicallyLargeSpoon>();
        }
    }
}
