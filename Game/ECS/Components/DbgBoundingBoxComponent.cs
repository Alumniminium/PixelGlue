using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Components
{
    public struct DbgBoundingBoxComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public const string TextureName = "selectionrect4";
        public static readonly Rectangle SrcRect = new Rectangle(0,0,PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize);

        public DbgBoundingBoxComponent(int ownerId)
        {
            UniqueId = ownerId;
        }
    }
}