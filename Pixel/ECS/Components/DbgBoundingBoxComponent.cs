using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{    
    [Component]
    public struct DbgBoundingBoxComponent 
    {
        public const string TextureName = "selectionrect4";
        public static readonly Rectangle SrcRect = new Rectangle(0, 0, 32, 32);
    }
}