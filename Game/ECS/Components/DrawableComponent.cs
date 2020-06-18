using Microsoft.Xna.Framework;
using PixelGlueCore.Enums;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct DrawableComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public string TextureName;
        public Rectangle SrcRect;
        public Rectangle DestRect;

        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect)
        {
            UniqueId=ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = Rectangle.Empty;
        }
        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect, Rectangle destRect)
        {
            UniqueId=ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect =destRect;
        }
    }
}