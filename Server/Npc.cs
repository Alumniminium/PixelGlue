using Microsoft.Xna.Framework;
using Shared;
using System;

namespace Server
{
    public class Npc
    {
        public int UniqueId;
        public Vector2 Position;
        public DateTime LastMove;
        public Npc(int uid)
        {
            UniqueId = uid;
            var x = (Global.Random.Next(1, 3) * Global.TileSize);
            var y = (Global.Random.Next(1, 3) * Global.TileSize);
            Position = new Vector2(x, y);
        }
    }
}