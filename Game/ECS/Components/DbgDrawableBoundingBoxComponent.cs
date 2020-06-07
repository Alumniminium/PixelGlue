using Microsoft.Xna.Framework;

namespace PixelGlueCore.ECS.Components
{
    public struct DbgBoundingBoxComponent
    {
        public const string TextureName = "selectionrect3";
        public static readonly Rectangle SrcRect = new Rectangle(0,0,32,32);
        public int PixelOwnerId { get; set; }

        public DbgBoundingBoxComponent(int ownerId)
        {
            PixelOwnerId = ownerId;
        }
    }
}