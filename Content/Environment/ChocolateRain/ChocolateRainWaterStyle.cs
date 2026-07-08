using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Environment.ChocolateRain
{
	public class ChocolateRainWaterStyle : ModWaterStyle
	{
		public override int GetSplashDust()
			=> 7;

		public override int GetDropletGore()
			=> 706;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor()
			=> new Color(64, 19, 13);

        public override int ChooseWaterfallStyle()
        {
			return ModContent.GetInstance<ChocolateRainWaterfallStyle>().Slot;
        }
    }
}
