using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class PositionComponent : IEntityComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 IntegerPosition => Vector2.Round(Position);
        public float Rotation { get; set; }
        public int PixelOwnerId { get; set; }

        public PositionComponent(int ownerId,int x, int y, int rotation)
        {
            PixelOwnerId = ownerId;
            Position = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}