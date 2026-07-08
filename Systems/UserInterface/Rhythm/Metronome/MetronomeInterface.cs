using EpicosityPort.Systems.Rhythm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace EpicosityPort.Systems.UserInterface.Rhythm.Metronome
{
	public class MetronomeInterface : ModSystem
	{
		public static MetronomeInterface Instance { get; private set; }
        public Terraria.UI.UserInterface MetronomeUserInterface { get => Instance.metronomeUserInterface; set => Instance.metronomeUserInterface = value; }
        public MetronomeUIState MetronomeUIStateInstance { get => Instance.metronomeUIState; set => Instance.metronomeUIState = value; }

        public override void Load()
        {
			Instance = new MetronomeInterface();
        }
        public override void Unload()
        {
            Instance = null;
        }

        private Terraria.UI.UserInterface metronomeUserInterface;
        private MetronomeUIState metronomeUIState;
        //[LoadManager(LoadManagerAttribute.ManageType.Unload, LoadManagerAttribute.ManageNetmode.Client)]
        //public static void Unload()
        //{
        //	metronomeUserInterface = null;
        //	metronomeUIState = null;
        //}
        public class MetronomeUIState : UIState
		{
			public MetronomeVisualBase metronome;
			public override void OnInitialize()
			{
				Main.OnPreDraw += Main_OnPreDraw;
			}
			~MetronomeUIState()
			{
				Main.OnPreDraw -= Main_OnPreDraw;
			}
			private void Main_OnPreDraw(GameTime obj)
			{
				if (metronome == null) return;
				var pos = GetPosition();
				float positionInMeasure = pos.Item1;
				float crotchet = pos.Item2;
				metronome?.PreDraw(Main.spriteBatch, obj, crotchet, positionInMeasure);
			}
			public void UpdateMetronome(Type metronomeType)
			{
				if (metronomeType == null)
				{
					metronome = null;
					return;
				}
				if (metronome?.GetType() == metronomeType) return;
				metronome?.Dispose();
				metronome = (MetronomeVisualBase)Activator.CreateInstance(metronomeType);
			}
			public override void Update(GameTime gameTime)
			{
				ModPlayerRhythmistAccessories accessories = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmistAccessories>(); ;
				if (!accessories.hasMetronome && metronome != null)
				{
					metronome = null;
				}
				metronome?.Update(gameTime);
			}
			public (float, float) GetPosition()
			{
				ModPlayerRhythmist dancePlayer = RhythmHandler.GetLocalRhythmPlayer();
				float positionInMeasure = 0;
				float crotchet = 0;
				// RS: Someone add this if they want to
				//if (!RhythmBeatmapHandler.inst.TrackActive)
				//{
				positionInMeasure = (int)(RhythmHandler.Instance.noteTimer - (60 / (float)dancePlayer.trackSpeed)) % (dancePlayer.noteRate * 4);
				crotchet = dancePlayer.noteRate;
				//}
				//else
				//{
					//positionInMeasure = (float)(RhythmBeatmapHandler.inst.conductor.normalizedSongPosition % (RhythmBeatmapHandler.inst.conductor.crotchetSeconds * RhythmBeatmapHandler.inst.conductor.timeSigNum));
					//if (positionInMeasure < 0) positionInMeasure = 0;
					//crotchet = (float)RhythmBeatmapHandler.inst.conductor.crotchetSeconds;
				//}
				return (positionInMeasure, crotchet);
			}
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				var pos = GetPosition();
				float positionInMeasure = pos.Item1;
				float crotchet = pos.Item2;
				metronome?.Draw(spriteBatch, false, crotchet, positionInMeasure);
				if (metronome?.Hovering(Main.MouseScreen) == true)
				{
					Main.LocalPlayer.cursorItemIconEnabled = true;
					Main.LocalPlayer.cursorItemIconID = -1;
					//if (!RhythmBeatmapHandler.inst.TrackActive)
					//{
					//	Main.LocalPlayer.cursorItemIconText = $"{decimal.Round((decimal)(3600 / crotchet), 2)} BPM";
					//}
					//else
					//{
						//Main.LocalPlayer.cursorItemIconText = $"{RhythmBeatmapHandler.inst.conductor.bpm} BPM / Offset: {RhythmBeatmapHandler.inst.conductor.offset} sec";
					//}
				}
			}
		}
	}
}
