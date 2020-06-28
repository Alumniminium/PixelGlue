using Microsoft.Xna.Framework;
using Pixel.ECS.Systems;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct DbgBoundingBoxComponent: IEntityComponent
    {
        public int EntityId {get;set;}
        public const string TextureName = "selectionrect4";
        public static readonly Rectangle SrcRect = new Rectangle(0,0,PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize);

        public DbgBoundingBoxComponent(int ownerId)
        {
            EntityId = ownerId;
        }
    }
}