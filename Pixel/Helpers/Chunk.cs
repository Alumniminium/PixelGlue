namespace Pixel.Helpers
{
    public class Chunk
    {
        public int X, Y;
        public Tile[][] Tiles;
        public string Hash;

        public Chunk(int x, int y, int size)
        {
            X = x;
            Y = y;
            Tiles = new Tile[size][];
        }
    }
}