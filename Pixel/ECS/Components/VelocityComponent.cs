using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct VelocityComponent
    {
        public Vector2 Velocity;
    }
}