using Microsoft.Xna.Framework;

namespace Pixel.ECS.Components
{
    public struct PositionComponent
    {
        public Vector2 Position;
        public Vector2 Destination;
        public float Rotation;
        public PositionComponent(int x, int y, int rotation)
        {
            Position = new Vector2(x, y);
            Destination = Position;
            Rotation = rotation;
        }
    }
}