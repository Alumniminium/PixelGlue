using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class DrawableComponent: IEntityComponent
    {
        public string TextureName { get; set; }
        public Rectangle SrcRect { get; set; }
        public DrawableComponent(string textureName, Rectangle srcRect)
        {
            TextureName = textureName;
            SrcRect = srcRect;
        }
    }
}