using Microsoft.Xna.Framework;

namespace PixelGlueCore.Helpers
{
    public static class Vector2Ext
    {
        public static Vector2 DrawablePosition(this Vector2 vector)
        {
            var floor = Vector2.Floor(vector);
            var delta = vector - floor;

            if (delta.X > 0.5f)
                delta.X = 1;
            else if (delta.X > 0 && delta.X < 0.5f)
                delta.X = 0;

            if (delta.Y > 0.5f)
                delta.Y = 1;
            else if (delta.Y > 0 && delta.Y < 0.5f)
                delta.Y = 0;
            
            return floor + delta;
        }
    }
}