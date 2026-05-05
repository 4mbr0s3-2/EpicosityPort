using EpicosityPort.Systems.UserInterface;

namespace EpicosityPort.Systems.Rhythm
{
    //public partial class RhythmHandler
    //{
    public abstract class FourKeyTrackPatternObject
    {
        public abstract void GetNote(ref int patternArrowIndex, ref SpriteScene chart);
        public virtual bool CanBeRolled()
        {
            return true;
        }
    }
	//}
}