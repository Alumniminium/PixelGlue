using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Components
{
    public struct Networked: IEntityComponent
    {
        public int UniqueId {get;set;}
        public Vector2 Position;
        public Networked(int uniqueId)
        {
            UniqueId = uniqueId;
            Position=Vector2.Zero;
        }
    }
}