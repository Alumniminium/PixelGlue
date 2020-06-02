using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework;

namespace PixelGlueCore
{
    public class Tile : GameObject
    {
        public Tile(int x, int y, string textureName, Rectangle srcRect)
        {
            AddComponent(new PositionComponent(x, y, 0));
            AddComponent(new DrawableComponent(textureName, srcRect));
        }
    }
}