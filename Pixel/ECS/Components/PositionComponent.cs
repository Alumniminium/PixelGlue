using Microsoft.Xna.Framework;

namespace Pixel.ECS.Components
{
    public struct PositionComponent
    {
        public Vector2 Value;
        public float Rotation;
        public PositionComponent(int x, int y, int rotation)
        {
            Value = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}