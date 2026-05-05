namespace EpicosityPort.Effects.Bloom
{
	// RS: This isn't really used

	//public class TerrariaBloom : ModSystem
	//{
	//	public bool active = false;
	//	public class ModPlayerBloom : ModPlayer
	//	{
	//		public override void UpdateBiomeVisuals()
	//		{
	//			if (Player != null)
	//			{
	//				Player.ManageSpecialBiomeVisuals(ShaderNames.Bloom, Instance.active);
	//			}
	//		}
	//	}
	//	public BloomComponent bloom;
	//	public static TerrariaBloom Instance;
	//	//[LoadManager(LoadManagerAttribute.ManageType.Load, LoadManagerAttribute.ManageNetmode.Client)]
	//	public static void Initialize()
	//	{
	//		Instance = new TerrariaBloom();
	//		Terraria.Graphics.Effects.On_FilterManager.EndCapture += FilterManager_EndCapture;
	//	}

	//	private static void FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self)
	//	{
	//		orig(self);
	//		if (Filters.Scene[ShaderNames.Bloom].Active)
	//		{
	//			Instance.bloom.Draw();
	//		}
	//	}

	//	//[LoadManager(LoadManagerAttribute.ManageType.Unload, LoadManagerAttribute.ManageNetmode.Client)]
	//	public static void Unload()
	//	{
	//		Instance = null;
	//	}
	//	public TerrariaBloom()
	//	{
	//		bloom = new BloomComponent(Main.instance);
	//		Main.instance.Components.Add(bloom);
	//		bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
	//	}
	//}
}
