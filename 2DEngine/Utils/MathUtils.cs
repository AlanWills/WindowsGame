using System;
using System.Diagnostics;

namespace _2DEngine
{
    public static class MathUtils
    {
        private static Random Random = new Random();

        public static float GenerateFloat(float min, float max)
        {
            Debug.Assert(min < max);
            float randFloat = (float)Random.NextDouble();

            randFloat *= (max - min);
            randFloat += min;

            return randFloat;
        }
    }
}
