using Microsoft.Xna.Framework;

namespace PixelGlueCore.ECS.Components
{
    public struct PositionComponent
    {
        public Vector2 Position;
        public float Rotation;
        public PositionComponent(int x, int y, int rotation)
        {
            Position = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}