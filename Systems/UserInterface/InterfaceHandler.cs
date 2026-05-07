using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.UserInterface.ChoreabilitySlots;
using EpicosityPort.Systems.UserInterface.Defibrillator;
using EpicosityPort.Systems.UserInterface.Rhythm;
using EpicosityPort.Systems.UserInterface.Rhythm.ComboBar;
using EpicosityPort.Systems.UserInterface.Rhythm.Metronome;
using EpicosityPort.Systems.UserInterface.Rhythm.Setup;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
namespace EpicosityPort.Systems.UserInterface
{
	[Autoload(Side = ModSide.Client)]
	public class InterfaceHandler : ModSystem
    {
		public static InterfaceHandler Instance 
		{ 
			get
			{
				return ModContent.GetInstance<InterfaceHandler>();
			} 
		}
		/// <summary>
		/// Used for InterfaceItems. The state of the interface would be set to that of the InterfaceItem being held.
		/// </summary>
		public Terraria.UI.UserInterface interfaceItemCommonInterface;
		/// <summary>
		/// UserInterface instances in the list will be updated in UpdateUI().
		/// </summary>
		public List<UserInterfaceInfo> interfaceList;
        //[LoadManager(LoadManagerAttribute.ManageType.Load, LoadManagerAttribute.ManageNetmode.Client)]
        //public static void LoadInterfaceList()
        //{
        //	interfaceList = new List<UserInterfaceInfo>();
        //}
        //[LoadManager(LoadManagerAttribute.ManageType.Unload, LoadManagerAttribute.ManageNetmode.Client)]
        //public static void UnloadInterfaceList()
        //{
        //	interfaceList = null;
        //}
        public override void OnModLoad()
        {
            interfaceList = new List<UserInterfaceInfo>();
			LoadInterfaces();
        }

		private GameTime _lastUpdateUiGameTime;
		public class UserInterfaceInfo
        {
			public Terraria.UI.UserInterface userInterface;
			public string name;
			public string layerIndex;
			public int indexOffset;
			public InterfaceScaleType interfaceScaleType;
			public UserInterfaceInfo(Terraria.UI.UserInterface userInterface, string name, string layerIndex, int indexOffset, InterfaceScaleType interfaceScaleType)
            {
				this.userInterface = userInterface;
				this.name = name;
				this.layerIndex = layerIndex;
				this.indexOffset = indexOffset;
				this.interfaceScaleType = interfaceScaleType;
            }
        }
		public Terraria.UI.UserInterface CreateUserInterface(string name, string layerIndex, int indexOffset = 0, InterfaceScaleType interfaceScaleType = InterfaceScaleType.UI)
        {
			Terraria.UI.UserInterface userInterface = new Terraria.UI.UserInterface();
			interfaceList.Add(new UserInterfaceInfo(userInterface, name, layerIndex, indexOffset, interfaceScaleType));
			return userInterface;
        }
		public override void UpdateUI(GameTime gameTime)
        {
			_lastUpdateUiGameTime = gameTime;
			foreach (UserInterfaceInfo userInterfaceInfo in interfaceList)
            {
				if (userInterfaceInfo.userInterface?.CurrentState != null)
                {
					userInterfaceInfo.userInterface.Update(gameTime);
                }
            }
		}
		public void LoadInterfaces()
        {
			interfaceItemCommonInterface = CreateUserInterface("Common Item Interface", "Vanilla: Resource Bars");

			// Allows for Vacpack saving
			// TODO: Ok maybe this isn't the best place to put this
			//Terraria.ModLoader.IO.TagSerializer.AddSerializer(new Vacpack.VacpackStorageUnitSerializer());

			RhythmHandler.Instance.Interface.RhythmUserInterface = CreateUserInterface("Rhythm Interface", "Vanilla: Resource Bars");
			RhythmHandler.Instance.Interface.RhythmUIStateInstance = new RhythmUIState();
			RhythmHandler.Instance.Interface.RhythmUIStateInstance.Activate();

			//PizzaCutterUI.PizzaCutterInterface = CreateUserInterface("Pizza Cutter Interface", "Vanilla: Resource Bars");
			//PizzaCutterUI.CurrentPizzaCutterUIState = new PizzaCutterUI.PizzaCutterUIState();
			//PizzaCutterUI.CurrentPizzaCutterUIState.Activate();

			//NPCQuestGiveQuestItem.NPCConversationGiveQuestItemInterface = CreateUserInterface("NPC Conversation Interface", "Vanilla: Inventory", 1);
			// The UIState set for NPCDrinkServeInterface is disposable, so we don't need to make a singleton of it here

			DanceMachineUI.Instance.DanceMachineUIInterface = CreateUserInterface("Dance Machine Interface", "Vanilla: Inventory", 1);

			ChoreabilitySlotsInventory.Instance.ChoreabilitySlotsInventoryInterface = CreateUserInterface("Choreability Slots", "Vanilla: Inventory", 1);
			ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton = new ChoreabilitySlotsInventory.ChoreabilityInventoryUI();
			ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton.Activate();
			ChoreabilitySlotsInventory.Instance.ChoreabilitySlotsInventoryInterface.SetState(ChoreabilitySlotsInventory.Instance.ChoreabilityInventoryUIStateSingleton);

			// RS: Couldn't these be handled in the actual classes lmao
			ComboBarInterface.Instance.ComboBarUserInterface = CreateUserInterface("Combo Bar Interface", "Vanilla: Resource Bars");
			ComboBarInterface.Instance.comboBarUIState = new ComboBarInterface.ComboBarUIState();
			ComboBarInterface.Instance.ComboBarUserInterface.SetState(ComboBarInterface.Instance.comboBarUIState);
			ComboBarInterface.Instance.comboBarUIState.Activate();

			MetronomeInterface.Instance.MetronomeUserInterface = CreateUserInterface("Metronome Interface", "Vanilla: Resource Bars");
			MetronomeInterface.Instance.MetronomeUIStateInstance = new MetronomeInterface.MetronomeUIState();
			MetronomeInterface.Instance.MetronomeUserInterface.SetState(MetronomeInterface.Instance.MetronomeUIStateInstance);
			MetronomeInterface.Instance.MetronomeUIStateInstance.Activate();

			AlphaMessage.AlphaMessage.AlphaMessageInterface = CreateUserInterface("Alpha Message", "Vanilla: Resource Bars");
			
			//RhythmistSetup.Instance.RhythmistSetupInterface = CreateUserInterface("Alpha Message", "Vanilla: Resource Bars");
            RhythmistSetup.Instance.RhythmistSetupInterface = CreateUserInterface("Rhythmist Setup", "Vanilla: Resource Bars");

            //CutsceneManager.CutsceneInterface = CreateUserInterface("Cutscene", "Vanilla: Cursor");

            DefibrillatorGame.Instance.GameInterface = CreateUserInterface("Defibrillator Game", "Vanilla: Death Text");

            //RhythmBeatmapHandler.inst.BeatmapVisualInterface = CreateUserInterface("Beatmap Visuals", "Vanilla: Cursor");

            //SpellCard.SpellCardUIState.SpellCardInterface = CreateUserInterface("Touhou Spell Cards", "Vanilla: Cursor");
        }
		public void UnloadInterfaces()
		{

		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			foreach (UserInterfaceInfo userInterfaceInfo in interfaceList)
            {
				int layerIndex = layers.FindIndex(layer => layer.Name.Equals(userInterfaceInfo.layerIndex));
				if (layerIndex != -1)
				{
					layers.Insert
					(
						layerIndex + userInterfaceInfo.indexOffset, new LegacyGameInterfaceLayer
						(
							$"{EpicosityPort.Instance.Name}: {userInterfaceInfo.name}",
							delegate
							{
								if (_lastUpdateUiGameTime != null && userInterfaceInfo.userInterface?.CurrentState != null)
								{
									userInterfaceInfo.userInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
								}
								return true;
							},
							userInterfaceInfo.interfaceScaleType
						)
					);
				}
            }
		}
	}
}
