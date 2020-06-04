using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class PositionComponent : IEntityComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 IntegerPosition => Vector2.Round(Position);
        public float Rotation { get; set; }

        public PositionComponent(int x, int y, int rotation)
        {
            Position = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}