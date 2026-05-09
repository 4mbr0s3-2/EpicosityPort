using System;


// RS: Maybe I should've just added all of them?
namespace EpicosityPort.Systems.Utilities
{
    /// <summary>
    /// All functions derived from https://easings.net/.
    /// The progress parameter is always a float between 0.0 and 1.0.
    /// </summary>
    public static class Easings
    {
        public static float EaseInOutSine(float progress)
        {
            return (float)(-(Math.Cos(Math.PI * progress) - 1f) / 2f);
        }
        public static float EaseInOutCubic(float progress)
        {
            return (float)(progress < 0.5f ? 4f * progress * progress * progress : 1f - Math.Pow(-2f * progress + 2f, 3f) / 2f);
        }
        public static float EaseOutBack(float progress)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1f;

            return (float)(1f + c3 * Math.Pow(progress - 1f, 3f) + c1 * Math.Pow(progress - 1f, 2f));
        }
        public static float EaseOutQuad(float progress)
        {
            return 1 - (1 - progress) * (1 - progress);
        }
        public static float EaseInQuad(float progress)
        {
            return progress * progress;
        }
        public static float EaseOutBounce(float progress)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (progress < 1f / d1)
            {
                return n1 * progress * progress;
            }
            else if (progress < 2f / d1)
            {
                return n1 * (progress -= 1.5f / d1) * progress + 0.75f;
            }
            else if (progress < 2.5f / d1)
            {
                return n1 * (progress -= 2.25f / d1) * progress + 0.9375f;
            }
            else
            {
                return n1 * (progress -= 2.625f / d1) * progress + 0.984375f;
            }
        }

        public static float EaseInBounce(float progress)
        {
            return 1 - EaseOutBounce(progress);
        }
    }
}
