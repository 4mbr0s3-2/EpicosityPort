using EpicosityPort.Systems.UserInterface;

namespace EpicosityPort.Systems.Rhythm
{
    public abstract class MouseTrackPatternObject
	{
        public abstract void GetArrow(ref int patternArrowIndex, ref SpriteScene chart);
        public virtual bool CanBeRolled()
		{
            return true;
		}
	}
}
