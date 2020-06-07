using Microsoft.Xna.Framework;

namespace PixelGlueCore.ECS.Components
{
    public struct PositionComponent
    {
        public Vector2 Position;
        public float Rotation;
        public int UniqueId;
        public PositionComponent(int ownerId,int x, int y, int rotation)
        {
            UniqueId = ownerId;
            Position = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}