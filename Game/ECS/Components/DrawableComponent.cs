using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public struct DrawableComponent
    {
        public string TextureName;
        public Rectangle SrcRect;
        public Rectangle DestRect;

        public DrawableComponent(string textureName, Rectangle srcRect)
        {
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = Rectangle.Empty;
        }
        public DrawableComponent(string textureName, Rectangle srcRect, Rectangle destRect)
        {
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect =destRect;
        }
    }
}