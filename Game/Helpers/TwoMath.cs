using Microsoft.Xna.Framework;
using System;

namespace PixelGlueCore.Helpers
{
    public static class TwoMath
    {
        public static float GetDistance(this Vector2 a, Vector2 b)
        {
            var x = Math.Pow(a.X - b.X, 2);
            var y = Math.Pow(a.Y - b.Y, 2);
            return (float)Math.Sqrt(x + y);
        }
    }
}