using System;
using Microsoft.Xna.Framework;

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
            var x = Pixel.Pixel.Random.Next(1,50) * Pixel.Pixel.TileSize;
            var y = Pixel.Pixel.Random.Next(1,50) * Pixel.Pixel.TileSize;
            Location = new Vector2(x,y);
        }
    }
}