﻿using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct DrawableComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public string TextureName;
        public Rectangle SrcRect;
        public Color Color;
        public Rectangle DestRect;

        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect)
        {
            EntityId = ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = Rectangle.Empty;
            Color = Color.White;
        }
        public DrawableComponent(int ownerId, string textureName, Rectangle srcRect, Rectangle destRect)
        {
            EntityId = ownerId;
            TextureName = textureName;
            SrcRect = srcRect;
            DestRect = destRect;
            Color = Color.White;
        }
    }
}