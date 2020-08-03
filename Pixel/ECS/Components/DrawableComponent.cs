using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct DrawableComponent
    {
        public string TextureName;
        public Rectangle SrcRect,DestRect;
        public Color Color;
        public Vector2 Origin;
        public float Scale;

        public DrawableComponent(string textureName, Rectangle srcRect,float scale = 1f)
        {
            TextureName = textureName;
            SrcRect = srcRect;
            Color = Color.White;
            Origin = new Vector2(srcRect.Width / 2, srcRect.Height / 2);
            Scale=scale;
            DestRect = Rectangle.Empty;
        }
        public DrawableComponent(Color color,float scale = 1f)
        {
            TextureName = "pixel";
            SrcRect = new Rectangle(0, 0, 1, 1);
            Color = color;
            Origin = new Vector2(SrcRect.Width / 2, SrcRect.Height / 2);
            Scale=scale;
            DestRect = Rectangle.Empty;
        }
        public DrawableComponent(Color color, Rectangle destination)
        {
            TextureName = "pixel";
            SrcRect = new Rectangle(0, 0, 1, 1);
            Color = color;
            Origin = new Vector2(SrcRect.Width / 2, SrcRect.Height / 2);
            Scale=1f;
            DestRect=destination;
        }
    }
}