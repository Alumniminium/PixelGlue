using System;

namespace Pixel.Helpers
{
    public static class PxlRng
    {
        private static Random Rng = new Random();
        public static float Get(double min, double max) => (float)(Rng.NextDouble() * (max - min) + min);
    }
}