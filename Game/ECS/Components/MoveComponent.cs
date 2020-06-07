using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class MoveComponent: IEntityComponent
    {
        public float Speed { get; set; }
        public float SpeedMulti { get; set; }
        public bool Moving { get; set; }
        public Vector2 Destination { get; set; }
        public int PixelOwnerId { get; set; }

        public MoveComponent(int ownerId, float speed, int destX, int destY)
        {
            PixelOwnerId = ownerId;
            Speed = speed;
            SpeedMulti=1;
            Moving = false;
            Destination = new Vector2(destX, destY);
        }
        public MoveComponent(int ownerId, float speed)
        {
            PixelOwnerId=ownerId;
            Speed = speed;
            SpeedMulti=1;
            Moving = false;
            Destination = Vector2.Zero;
        }
    }
}