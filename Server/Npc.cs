using Shared;
using Shared.ECS;
using System;
using System.Numerics;

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
            var x = 1024 + (Global.Random.Next(1, 3) * Global.TileSize);
            var y = 1024 + (Global.Random.Next(1, 3) * Global.TileSize);
            Position = new Vector2(x, y);
        }
    }
}