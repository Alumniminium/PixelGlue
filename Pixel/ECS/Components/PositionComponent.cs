using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
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