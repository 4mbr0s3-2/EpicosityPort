namespace EpicosityPort.Systems.Utilities
{
    public static class BodyFrameCalculator
    {
        public static int GetBodyFrameFromRotation(float rotation)
        {
            int bodyFrame = 3;
            if (rotation < -0.78539816339f && rotation > -2.3561944902f)
            {
                bodyFrame = 2;
            }
            if (rotation > 0.78539816339f && rotation < 2.35619449019f)
            {
                bodyFrame = 4;
            }
            return bodyFrame;
        }
    }
}
