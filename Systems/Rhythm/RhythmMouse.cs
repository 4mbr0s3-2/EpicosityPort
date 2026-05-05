using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Rhythm
{
    //public partial class RhythmHandler
    public class RhythmMouse : ModSystem
    {
        public static RhythmMouse Instance
        {
            get
            {
                return ModContent.GetInstance<RhythmMouse>();
            }
        }
        public bool mouseGuiOnPlayer = true;
        /// <summary>
        /// Used for mouse-based gameplay.
        /// </summary>
        public bool mouseFlipNoteDirection = false;
        public Vector2 mouseTargetScreenPosition = Vector2.Zero;
        public Vector2 mouseStartPositionScreen = Vector2.Zero;
        public Dictionary<int, MouseTrackPatternObject> prefixTypeToMouseTrackPattern = new Dictionary<int, MouseTrackPatternObject>()
        {
        };
        public enum MouseTrackPattern
        {
            Random,
            Clockwise,
            CounterClockwise,
            Rockefeller,
            Fukkireta,
            PrefixDependent,
            Funnybones
        }
    }
}
