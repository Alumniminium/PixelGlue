using Microsoft.Xna.Framework;
using Pixel.ECS.Systems;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct MoveComponent: IEntityComponent
    {
        public int EntityId {get;set;}
        public float Speed;
        public float SpeedMulti;
        public bool Moving;
        public Vector2 Destination;

        public MoveComponent(int ownerId, float speed, int destX, int destY)
        {
            EntityId=ownerId;
            Speed = speed;
            SpeedMulti=1;
            Moving = false;
            Destination = new Vector2(destX, destY);
        }
    }
}