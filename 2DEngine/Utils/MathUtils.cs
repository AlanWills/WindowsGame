using System;
using System.Diagnostics;

namespace _2DEngine
{
    public static class MathUtils
    {
        private static Random Random = new Random();

        /// <summary>
        /// Generates a random float between the min and max values
        /// </summary>
        /// <param name="min">The minimum value the float can attain</param>
        /// <param name="max">The maximum value the float can attain</param>
        /// <returns>A random number between min and max inclusive</returns>
        public static float GenerateFloat(float min, float max)
        {
            Debug.Assert(min < max);
            float randFloat = (float)Random.NextDouble();

            randFloat *= (max - min);
            randFloat += min;

            return randFloat;
        }

        /// <summary>
        /// Generates a random int between the min and max values
        /// </summary>
        /// <param name="inclusiveMin">The minimum value that can be attained (inclusive)</param>
        /// <param name="inclusiveMax">The maximum value that can be attained (inclusive)</param>
        /// <returns>A random int between the min and max values</returns>
        public static int GenerateInt(int inclusiveMin, int inclusiveMax)
        {
            Debug.Assert(inclusiveMin < inclusiveMax);
            Debug.Assert(inclusiveMax - inclusiveMin >= 2);

            return Random.Next(inclusiveMin, inclusiveMax + 1);
        }
    }
}