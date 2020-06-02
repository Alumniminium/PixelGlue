using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class MoveComponent: IEntityComponent
    {
        public float Speed { get; set; }
        public bool Moving { get; set; }
        public Vector2 Destination { get; set; }

        public MoveComponent(float speed, int destX, int destY)
        {
            Speed = speed;
            Moving = false;
            Destination = new Vector2(destX, destY);
        }
        public MoveComponent(float speed)
        {
            Speed = speed;
            Moving = false;
            Destination = Vector2.Zero;
        }
    }
}