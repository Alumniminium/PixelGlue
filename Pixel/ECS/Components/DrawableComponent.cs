using Microsoft.Xna.Framework;
using Pixel.Enums;
using Pixel.ECS.Systems;

namespace Pixel.ECS.Components
{
    public struct DrawableComponent: IEntityComponent
    {
        public int EntityId {get;set;}
        public string TextureName;
        public Rectangle SrcRect;
        public Rectangle DestRect;

        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect)
        {
            EntityId=ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = Rectangle.Empty;
        }
        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect, Rectangle destRect)
        {
            EntityId=ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect =destRect;
        }
    }
}