using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class PositionComponent : IEntityComponent
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public PositionComponent(int x, int y, int rotation)
        {
            Position = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}