using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public struct DrawableComponent
    {
        public string TextureName ;
        public Rectangle SrcRect;
        public Rectangle DestRect;
        public int UniqueId;

        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect)
        {
            UniqueId = ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = Rectangle.Empty;
        }
        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect, Rectangle destRect)
        {
            UniqueId = ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect =destRect;
        }
    }
}