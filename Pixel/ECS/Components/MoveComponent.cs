using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct VelocityComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public Vector2 Velocity;
        public float SpeedMulti;
        public float Speed;

        public VelocityComponent(int ownerId, float speed)
        {
            EntityId = ownerId;
            Velocity = Vector2.Zero;
            Speed = speed;
            SpeedMulti = 1;
        }
    }
}