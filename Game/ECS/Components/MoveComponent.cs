using Microsoft.Xna.Framework;

namespace PixelGlueCore.ECS.Components
{
    public struct MoveComponent
    {
        public float Speed;
        public float SpeedMulti;
        public bool Moving;
        public Vector2 Destination;

        public MoveComponent(float speed, int destX, int destY)
        {
            Speed = speed;
            SpeedMulti=1;
            Moving = false;
            Destination = new Vector2(destX, destY);
        }
    }
}