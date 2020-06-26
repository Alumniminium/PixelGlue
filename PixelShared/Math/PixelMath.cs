using System;
using Microsoft.Xna.Framework;

namespace PixelShared.Maths
{
    public static class PixelMath
    {
        public static float GetDistance(this Vector2 a, Vector2 b)
        {
            var x = Math.Pow(a.X - b.X, 2);
            var y = Math.Pow(a.Y - b.Y, 2);
            return (float)Math.Sqrt(x + y)/Pixel.TileSize;
        }
        public static float GetDistance(float aX, float aY, float bX, float bY)
        {
            var x = Math.Pow(aX - bX, 2);
            var y = Math.Pow(aY - bY, 2);
            return (float)Math.Sqrt(x + y)/Pixel.TileSize;
        }
        public static bool Colliding(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            return x1 < x2 + w2 &&
                   x2 < x1 + w1 &&
                   y1 < y2 + h2 &&
                   y2 < y1 + h1;
        }
        public static float Map(float input, float min_in, float max_in, float min_out, float max_out) => ((input - min_in) * (max_out - min_out) / (max_in - min_in)) + min_out;
        
    }
}