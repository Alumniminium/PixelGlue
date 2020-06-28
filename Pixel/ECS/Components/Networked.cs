using Microsoft.Xna.Framework;
using Pixel.ECS.Systems;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct Networked: IEntityComponent
    {
        public int EntityId {get;set;}
        public Vector2 Position;
        public Networked(int uniqueId)
        {
            EntityId = uniqueId;
            Position=Vector2.Zero;
        }
    }
}