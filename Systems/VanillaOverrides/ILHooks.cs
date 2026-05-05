using EpicosityPort.Systems.UserInterface.VanillaOverrides;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;

namespace EpicosityPort.Systems.VanillaOverrides
{
    // RS: This was a godclass of IL hooks that, in porting to 1.4.4 5 years later, has been reduced to just code comments
    // Look on my works, ye Mighty, and despair!
    public class ILHooks
    {
        internal static void SetupILHooks()
        {
            Terraria.IL_Main.OnCharacterNamed += MainMenuOverride.ProcessCharacterName;
            //Terraria.IL_Main.DrawInterface_15_InvasionProgressBars += MainMenuUI.PreventVanillaProgressBars;
            // Chocolate Rain
            //Terraria.IL_Main.DrawInfoAccs += ChocolateRainManager.ChocolateRainForecast;
            //Terraria.IL_Player.ItemCheck += ChocolateRainManager.PickupChocolateWater;

            // NPC Dialogue Faces
            //Terraria.IL_Main.GUIChatDrawInner += TownNPCFaceIL;

            // Breakable Chests
            //Terraria.IL_Chest.DestroyChest += BreakableChest.DestroyChest;
            //Terraria.IL_Chest.CanDestroyChest += BreakableChest.CanDestroyChest;

            //Terraria.IL_Main.DoDraw += EpicosityItemRarity.HookItemTextDraw;

            //Main.OnTick += Main_OnTick; // IDK if this is necessary
        }
        //private static void SpriteViewMatrix_Rebuild(ILContext il)
        //{
        //	ILCursor iLCursor = new ILCursor(il);
        //	bool instructionsFound = iLCursor.TryGotoNext(MoveType.After, i => i.MatchLdloc(1), i => i.MatchLdarg(0), i => i.MatchLdfld(typeof(SpriteViewMatrix), "_zoomMatrix"));
        //	if (instructionsFound)
        //	{
        //		// Skip multiply instruction
        //		iLCursor.Index++;
        //		// Modify the transformation matrix
        //		iLCursor.EmitDelegate<Func<Matrix, Matrix>>((originalMatrix) =>
        //		{
        //			return Epicosity.Instance.ModifyTransformationMatrix(originalMatrix);
        //		});
        //	}
        //}

        internal static void SetupOnHooks()
        {
            // Death message
            Terraria.On_Main.DrawInterface_35_YouDied += MainMenuOverride.HookYouDiedGUI;
            // Title message
            Terraria.On_Lang.GetRandomGameTitle += MainMenuOverride.HookGetRandomGameTitle;
            //Main.chTitle = true;
            // Chocolate Rain
            //Terraria.On_Main.DrawRain += ChocolateRainRender.ChocolateRainHook;
            //         Terraria.On_Rain.Update += ChocolateRainRender.ChocolateRainUpdate;
            //Terraria.On_Main.StartRain += ChocolateRainManager.StartChocolateRain;
            //Terraria.On_Main.StopRain += ChocolateRainManager.StopChocolateRain;
            // Choreability equips
            //Terraria.UI.On_ItemSlot.EquipPage += ItemSlotChoreabilityEquip;
            //Terraria.UI.On_ItemSlot.SwapEquip_ItemArray_int_int += ItemSlot_SwapChoreability;
            // Rarity text
            //On.Terraria.ItemText.NewText += EpicosityItemRarity.ItemText_NewText;
            // Speedbridging automatic platform selection
            //Terraria.On_Player.PlaceThing += Player_PlaceThing;
            //Terraria.On_Main.DoUpdate += Main_DoUpdate;
        }

        // RS: This is fine 😂 Nothing can go wrong here 😂
        //private static void Main_DoUpdate(Terraria.On_Main.orig_DoUpdate orig, Main self, Microsoft.Xna.Framework.GameTime gameTime)
        //{
        //	if (CutsceneManager.inCutscene)
        //	{
        //		CutsceneManager.DoUpdateInCutscene(self, gameTime);
        //		return;
        //	}
        //	orig(self, gameTime);
        //}

        public static void Player_PlaceThing(Terraria.On_Player.orig_PlaceThing orig, Player self)
        {
            //SpeedyBridger.Player_PlaceThing(orig, self);
        }

        //private static void ItemSlot_SwapChoreability(Terraria.UI.On_ItemSlot.orig_SwapEquip_ItemArray_int_int orig, Item[] inv, int context, int slot)
        //{
        //	bool success = false;
        //	if (inv[slot].ModItem is ChoreabilityBase)
        //	{
        //		inv[slot] = ChoreabilitySlotsInventory.ChoreabilitySwap(inv[slot], out success);
        //		if (success)
        //		{
        //			(ChoreabilitySlotsInventory.ChoreabilityInventoryUIStateSingleton as ChoreabilitySlotsInventory.ChoreabilityInventoryUI).choreabilitySelectedPage = 1;
        //		}
        //	}
        //	else if (inv[slot].ModItem is FretboardBase)
        //	{
        //		Item result;
        //		Player player = Main.player[Main.myPlayer];
        //		result = inv[slot];
        //		for (int i = 0; i < player.armor.Length; i++)
        //		{
        //			if (player.armor[i].ModItem is FretboardBase)
        //			{
        //				result = player.armor[i].Clone();
        //				player.armor[i] = inv[slot].Clone();
        //				success = true;
        //				SoundEngine.PlaySound(SoundID.Grab);
        //				Recipe.FindRecipes();
        //				break;
        //			}
        //		}
        //		inv[slot] = result;
        //		if (success)
        //		{
        //			Main.EquipPageSelected = 0;
        //		}
        //	}
        //	orig(inv, context, slot);
        //}

        //private static void ItemSlotChoreabilityEquip(Terraria.UI.On_ItemSlot.orig_EquipPage orig, Item item)
        //{
        //	orig(item);
        //	if (item.ModItem is ChoreabilityBase)
        //	{
        //		(ChoreabilitySlotsInventory.ChoreabilityInventoryUIStateSingleton as ChoreabilitySlotsInventory.ChoreabilityInventoryUI).choreabilityPage = 1;
        //	}
        //}

        //public static void Main_UpdateAudio_CalamityEasterEgg(ILContext il)
        //{
        //	ILCursor iLCursor = new ILCursor(il);
        //	// Go to where Main.curMusic = this.newMusic and replace this.newMusic with the new music
        //	// Calamity Music code only overrides if the music is default when loaded
        //	if (!iLCursor.TryGotoNext(MoveType.After, instruction => instruction.MatchLdfld<Main>("newMusic")))
        //	{
        //		Epicosity.inst.Logger.Info("Unable to override main menu music!");
        //		return;
        //	}
        //	iLCursor.EmitDelegate<Func<int, int>>((newMusic) =>
        //	{
        //		int tupacTitleMusic = Epicosity.inst.GetSoundSlot(SoundType.Music, "Sounds/Music/TheTaleOfASuckaForLove");
        //		if (newMusic == 6 && !Epicosity.loadedCompatibilityMods.HasFlag(Epicosity.CompatibilityMods.TerrariaOverhaul) && Main.gameMenu)
        //		{
        //			return tupacTitleMusic;
        //		}
        //		else
        //		{
        //			return newMusic;
        //		}
        //	});
        //}

        public static void Main_OnTick()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                //EpicosityItemRarity.Update();
                //DanceMachine.UpdateAnimationCycle();
            }
        }

        internal static void TownNPCFaceIL(ILContext il)
        {
            // RS: I'm pretty sure these are for displaying their original face sprites in-game
            // Yes, they added those to 1.4.5, but, since these are copyrighted characters,
            // I'm probably just not going to port this

            //var c = new ILCursor(il);
            //if (!c.TryGotoNext(i => i.MatchStloc((short)6)))
            //{
            //	Epicosity.inst.Logger.Info("Unable to patch!");
            //	return; // Patch unable to be applied
            //}
            //c.EmitDelegate<Action>(() => BirdTrader.BirdTraderInterfaceDraw());
            //c.EmitDelegate<Action>(() => TemShopkeeper.TemShopkeeperInterfaceDraw());
        }

        internal static void SetupHooks()
        {
            SetupILHooks();
            SetupOnHooks();
        }
    }
}
