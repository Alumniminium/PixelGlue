using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class DrawableComponent
    {
        public string TextureName { get; set; }
        public Rectangle SrcRect { get; set; }
        public int PixelOwnerId { get; set; }

        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect)
        {
            PixelOwnerId = ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
        }
    }
}