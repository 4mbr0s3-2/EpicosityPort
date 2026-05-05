using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace EpicosityPort
{
    public class EpicosityPortModSystem : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            //ILHooks.Main_OnTick();
            //InterfaceHandler.UpdateUI(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //InterfaceHandler.ModifyInterfaceLayers(layers);
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            //int vertigo = ModContent.BuffType<Vertigo>();
            //if (vertigo < Main.LocalPlayer.buffImmune.Length)
            //{
            //    bool hasVertigo = Main.LocalPlayer.HasBuff(vertigo);
            //    if (hasVertigo)
            //    {
            //        Vector2 originalZoom = Transform.Zoom;
            //        Transform.Zoom = originalZoom * (float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 0.1f + 1.1);
            //        Matrix modifiedMatrix = Transform.TransformationMatrix * Matrix.CreateRotationZ((float)(Math.Cos(Main.GlobalTimeWrappedHourly) * 0.1f));
            //        FieldInfo info = Transform.GetType().GetField("_transformationMatrix", BindingFlags.Instance | BindingFlags.NonPublic);
            //        info?.SetValue(Transform, modifiedMatrix);
            //    }
            //}

            //if (RhythmBeatmapHandler.inst?.TrackActive == true && RhythmBeatmapHandler.inst.conductor != null)
            //{
            //    Transform.Zoom *= (float)RhythmBeatmapHandler.inst.conductor.zoomPumper;
            //}
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            //ChocolateRainManager.ModifySunLightColor(ref tileColor, ref backgroundColor);
            //MetroCityBackground.ModifySunLightColor(ref tileColor, ref backgroundColor);
        }
    }
}
