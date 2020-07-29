using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Shared.ECS.Components
{
    [Component]
    public struct PositionComponent
    {
        public Vector2 Value;
        public float Rotation;
        public PositionComponent(float x, float y, float rotation = 0)
        {
           Value= new Vector2(x,y);
            Rotation = rotation;
        }
    }
}