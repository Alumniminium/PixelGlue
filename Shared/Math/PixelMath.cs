using Microsoft.Xna.Framework;

namespace Shared.Maths
{
    public static class PixelMath
    {
        public static bool Colliding(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            return x1 < x2 + w2 &&
                   x2 < x1 + w1 &&
                   y1 < y2 + h2 &&
                   y2 < y1 + h1;
        }
        public static float Map(float input, float min_in, float max_in, float min_out, float max_out) => ((input - min_in) * (max_out - min_out) / (max_in - min_in)) + min_out;

        public static Point ToGridPosition(Vector2 pos)
        {
            var x = (int)pos.X;
            var y = (int)pos.Y;
            x /= Global.TileSize;
            x *= Global.TileSize;
            y /= Global.TileSize;
            y *= Global.TileSize;
            return new Point(x,y);
        }
        public static Vector2 ToGridPosition(float posX, float posY)
        {
            var x = (int)posX;
            var y = (int)posY;
            x /= Global.TileSize;
            x *= Global.TileSize;
            y /= Global.TileSize;
            y *= Global.TileSize;
            return new Vector2(x,y);
        }
    }
}