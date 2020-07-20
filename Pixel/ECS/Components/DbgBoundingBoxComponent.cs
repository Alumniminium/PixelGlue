using Microsoft.Xna.Framework;

namespace Pixel.ECS.Components
{
    public struct DbgBoundingBoxComponent 
    {
        public const string TextureName = "selectionrect4";
        public static readonly Rectangle SrcRect = new Rectangle(0, 0, 32, 32);
    }
}