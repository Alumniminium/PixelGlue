using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct DestinationComponent
    {
        public Vector2 Value;

        public DestinationComponent(Vector2 val) => Value = val;
        public DestinationComponent(float x, float y) => Value = new Vector2(x,y);
    }
}