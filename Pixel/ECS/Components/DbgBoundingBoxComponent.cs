using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct DbgBoundingBoxComponent 
    {
        public int EntityId { get; set; }
        public const string TextureName = "selectionrect4";
        public static readonly Rectangle SrcRect = new Rectangle(0, 0, 32, 32);
        public static Texture2D Texture => AssetManager.GetTexture(TextureName);

        public DbgBoundingBoxComponent(int ownerId)
        {
            EntityId = ownerId;
        }
    }
}