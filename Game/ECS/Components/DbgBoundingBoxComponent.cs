using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct DbgBoundingBoxComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public const string TextureName = "selectionrect3";
        public static readonly Rectangle SrcRect = new Rectangle(0,0,32,32);

        public DbgBoundingBoxComponent(int ownerId)
        {
            UniqueId = ownerId;
        }
    }
}