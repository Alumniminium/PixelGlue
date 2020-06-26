using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Components
{
    public struct DbgBoundingBoxComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public const string TextureName = "selectionrect4";
        public static readonly Rectangle SrcRect = new Rectangle(0,0,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize);

        public DbgBoundingBoxComponent(int ownerId)
        {
            UniqueId = ownerId;
        }
    }
}