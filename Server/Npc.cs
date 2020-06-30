using Microsoft.Xna.Framework;
using System;

namespace Server
{
    public class Npc
    {
        public int UniqueId;
        public Vector2 Location;
        public DateTime LastMove;
        public Npc(int uid)
        {
            UniqueId = uid;
            var x = 256 + (PixelShared.Pixel.Random.Next(1, 10) * PixelShared.Pixel.TileSize);
            var y = 256 + (PixelShared.Pixel.Random.Next(1, 10) * PixelShared.Pixel.TileSize);
            Location = new Vector2(x, y);
        }
    }
}