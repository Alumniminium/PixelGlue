using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct VelocityComponent
    {
        public Vector2 Velocity;
        public float SpeedMulti;
        public float Speed;

        public VelocityComponent(float speed)
        {
            Velocity = Vector2.Zero;
            Speed = speed;
            SpeedMulti = 1;
        }
    }
}