using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct PositionComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public Vector2 Position;
        public float Rotation;
        public PositionComponent(int ownerId, int x, int y, int rotation)
        {
            UniqueId=ownerId;
            Position = new Vector2(x, y);
            Rotation = rotation;
        }
    }
}