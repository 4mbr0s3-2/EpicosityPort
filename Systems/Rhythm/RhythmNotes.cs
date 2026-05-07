using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Rhythm
{
    //public partial class RhythmHandler
    public class RhythmNotes : ModSystem
    {
        public static RhythmNotes Instance { get {
                return ModContent.GetInstance<RhythmNotes>();
            } }
        /// <summary>
        /// Used for arrow-based gameplay.
        /// </summary>
        public bool notesUpscroll {
            get {
                return EpicosityConfigClient.Instance.FKUpscroll;
            } 
            set {
                EpicosityConfigClient.Instance.FKUpscroll = value;
            } 
        }
        public bool playfieldAbovePlayer
        {
            get
            {
                return EpicosityConfigClient.Instance.FKGHPlayfieldAbovePlayer;
            }
            set
            {
                EpicosityConfigClient.Instance.FKGHPlayfieldAbovePlayer = value;
            }
        }
        [Obsolete]
        public enum FourKeyTrackPattern
        {
            Random,
            LRParadiddle,
            LRStaircase,
            PingPongStaircase,
            CounterClockwiseSpin,
            LRAlt,
            Konami,
            PrefixDependent,
        }
        public Dictionary<int, FourKeyTrackPatternObject> prefixTypeToFourKeyTrackPattern = new Dictionary<int, FourKeyTrackPatternObject>()
        {
        };
	}
}
