using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

// RS: OK maybe this should go in Systems but ehhhhhhhhh
namespace EpicosityPort.Effects.Shaders.Screen
{
	/// <summary>
	/// Just a container for loading shader stuff...
	/// </summary>
	public class FilterLoader : ModSystem
	{
        public override void OnModLoad()
        {
            LoadFilters();
        }
		public static void LoadFilters()
		{
			//Ref<Effect> marquee = new Ref<Effect>(EpicosityPort.GetEffect("Effects/Shaders/Misc/Marquee"));

			// Jevil Background
			//Filters.Scene[ShaderNames.JevilBackground] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.07142857142f, 0.07142857142f, 1f).UseOpacity(0.33f), EffectPriority.Medium);
			//SkyManager.Instance[ShaderNames.JevilBackground] = new JevilBackground();

			//Ref<Effect> jevilDistortionMap = new Ref<Effect>(Epicosity.inst.GetEffect("Effects/Shaders/Misc/JevilDistortionMap"));
			//GameShaders.Misc[ShaderNames.JevilBackgroundDistortion] = new JevilBackgroundShaderData(jevilDistortionMap, "JevilDistortionMap");
			//GameShaders.Misc[ShaderNames.JevilMarquee] = new MiscShaderData(marquee, "Marquee").UseColor(1f, 0f, 0f);

			// Combo meter shine
			Asset<Effect> comboMeterFullComboEffect = EpicosityPort.Instance.Assets.Request<Effect>("Effects/Shaders/Misc/ComboMeterFullComboEffect");
			GameShaders.Misc[ShaderNames.ComboMeterFullComboEffect] = new MiscShaderData(comboMeterFullComboEffect, "ComboMeterFullComboEffect");
			
			// --- Rarities ---
			
			//Ref<Effect> rarityWIP = new Ref<Effect>(Epicosity.inst.GetEffect("Effects/Shaders/Rarities/WorkInProgressRarity"));
			//GameShaders.Misc[ShaderNames.RarityWIP] = new MiscShaderData(rarityWIP, "WorkInProgressRarity");
			
			// --- End Rarities ---
			
			
			
			// Chocolate Rain			
			//Filters.Scene[ShaderNames.ChocolateRainFilter] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.36862745098f, 0.09803921568f, 0.15294117647f).UseOpacity(0.33f), EffectPriority.Medium);

			//// Shockwave effect by Kazzymodus https://forums.terraria.org/index.php?threads/tutorial-shockwave-effect-for-tmodloader.81685/
			//Ref<Effect> shockwave = new Ref<Effect>(Epicosity.inst.GetEffect("Effects/Shaders/Screen/Shockwave"));
			//Filters.Scene[ShaderNames.Shockwave] = new Filter(new ScreenShaderData(shockwave, "Shockwave"), EffectPriority.VeryHigh);

			//// Metro City
			//Filters.Scene[ShaderNames.MetroCitySky] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0, 0f, 0.18039215686f).UseOpacity(0.33f), EffectPriority.Medium);
			//SkyManager.Instance[ShaderNames.MetroCitySky] = new MetroCitySky();

			//// Tokyo City (A Big Playground)
			//Filters.Scene[ShaderNames.TokyoCitySky] = new Filter(new ScreenShaderData("FilterMiniTower").UseOpacity(0));
			//SkyManager.Instance[ShaderNames.TokyoCitySky] = new TokyoCitySky();

			//// VVVVVV Dimension
			////Filters.Scene[ShaderNames.VVVVVVDimensionSky] = new Filter(new ScreenShaderData("FilterMiniTower").UseOpacity(0));
			////SkyManager.Instance[ShaderNames.VVVVVVDimensionSky] = new VVVVVVDimensionSky();

			//Filters.Scene[ShaderNames.GlenForeground] = new Filter(new ScreenShaderData("FilterMiniTower").UseOpacity(0.2f).UseColor(0, 1, 0), EffectPriority.Medium);
			//Overlays.Scene[ShaderNames.GlenForeground] = new GlenForegroundOverlay("Images/Misc/noise", new ScreenShaderData(null), EffectPriority.Medium, RenderLayers.TilesAndNPCs);

			//// Bloom (placeholder shader to activate frame capturing)
			//Filters.Scene[ShaderNames.Bloom] = new Filter(new ScreenShaderData("FilterMiniTower").UseOpacity(0.0f).UseColor(0, 0, 0), EffectPriority.Medium);
		}
	}
}
