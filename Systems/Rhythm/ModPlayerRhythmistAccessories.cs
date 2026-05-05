using Terraria.ModLoader;
namespace EpicosityPort.Systems.Rhythm;

//public partial class RhythmHandler
//	{
    public class ModPlayerRhythmistAccessories : ModPlayer
		{
			// There's probably a much better way of doing this
			public string fretboardType = "";
			public bool hasFlamingAccessory = false;
			public bool hasDefibrillatorPack = false;
			public bool hasPacemaker = false;
			public bool hasFreezingAccessory = false;
			public bool hasAccuracyTracker = false;
			public bool hasMetronome = false;
			public override void ResetEffects()
			{
				fretboardType = "";
				hasFlamingAccessory = false;
				hasDefibrillatorPack = false;
				hasPacemaker = false;
				hasFreezingAccessory = false;
				hasAccuracyTracker = false;
				hasMetronome = false;
			}
		}
	//}
