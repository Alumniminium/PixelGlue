using Microsoft.Xna.Framework;
using System;

namespace PixelGlueCore.Helpers
{
    public static class PixelMath
    {
        public static float GetDistance(this Vector2 a, Vector2 b)
        {
            var x = Math.Pow(a.X - b.X, 2);
            var y = Math.Pow(a.Y - b.Y, 2);
            return (float)Math.Sqrt(x + y);
        }
        public static float Map(float input, float min_in, float max_in, float min_out, float max_out) => ((input - min_in) * (max_out - min_out) / (max_in - min_in)) + min_out;
        
    }
}