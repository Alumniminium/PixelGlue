using Microsoft.Xna.Framework;
using Pixel.ECS.Systems;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct PositionComponent: IEntityComponent
    {
        public int EntityId {get;set;}
        public Vector2 Position;
        public Vector2 Destination;
        public float Rotation;
        public PositionComponent(int ownerId, int x, int y, int rotation)
        {
            EntityId=ownerId;
            Position = new Vector2(x, y);
            Destination = Position;
            Rotation = rotation;
        }
    }
}