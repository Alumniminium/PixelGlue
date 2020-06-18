using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Components
{
    public struct Networked: IEntityComponent
    {
        public int UniqueId {get;set;}
        public Networked(int uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}