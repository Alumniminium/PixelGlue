using Microsoft.Xna.Framework;

namespace PixelGlueCore.ECS.Components
{
    public class DbgBoundingBoxComponent: IEntityComponent
    {
        public string TextureName = "selectionrect4x4";
        public Rectangle SrcRect = new Rectangle(0,0,32,32);
        public int PixelOwnerId { get; set; }

        public DbgBoundingBoxComponent(int ownerId)
        {
            PixelOwnerId = ownerId;
        }
    }
}