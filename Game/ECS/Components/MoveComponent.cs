using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public struct MoveComponent
    {
        public float Speed;
        public float SpeedMulti;
        public bool Moving;
        public Vector2 Destination;
        public int UniqueId ;

        public MoveComponent(int ownerId, float speed, int destX, int destY)
        {
            UniqueId = ownerId;
            Speed = speed;
            SpeedMulti=1;
            Moving = false;
            Destination = new Vector2(destX, destY);
        }
    }
}