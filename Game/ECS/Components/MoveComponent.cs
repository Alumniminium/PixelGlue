using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct MoveComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public float Speed;
        public float SpeedMulti;
        public bool Moving;
        public Vector2 Destination;

        public MoveComponent(int ownerId, float speed, int destX, int destY)
        {
            UniqueId=ownerId;
            Speed = speed;
            SpeedMulti=1;
            Moving = false;
            Destination = new Vector2(destX, destY);
        }
    }
}