using Microsoft.Xna.Framework;
using Shared;

namespace Pixel.Helpers
{
    public class Tile
    {
        public int X, Y;
        public Color Color;

        public Rectangle Dst;

        public Tile(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
            Dst = new Rectangle(X, Y, Global.TileSize, Global.TileSize);
        }
        public Tile(Vector2 position, Color color) => new Tile((int)position.X, (int)position.Y, color);
    }
}