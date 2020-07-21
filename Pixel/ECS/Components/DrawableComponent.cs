using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct DrawableComponent
    {
        public string TextureName;
        public Rectangle SrcRect;
        public Color Color;
        public Rectangle DestRect;

        public DrawableComponent(string textureName, Rectangle srcRect)
        {
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = Rectangle.Empty;
            Color = Color.White;
        }
        public DrawableComponent(string textureName, Rectangle srcRect, Rectangle destRect)
        {
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = destRect;
            Color = Color.White;
        }
        public DrawableComponent(Color color, Rectangle destRect)
        {
            TextureName = "pixel";
            SrcRect = new Rectangle(0,0,1,1);
            DestRect = destRect;
            Color = color;
        }
    }
}