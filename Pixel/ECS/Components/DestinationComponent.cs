using Microsoft.Xna.Framework;

namespace Pixel.ECS.Components
{
    public struct DestinationComponent
    {
        public Vector2 Value;
        public DestinationComponent(int x, int y)
        {
            Value = new Vector2(x, y);
        }
    }
}