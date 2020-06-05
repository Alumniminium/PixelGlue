using System;

namespace Server
{
    public class Npc
    {
        public int UniqueId;
        public int X, Y = 256;
        public DateTime LastMove;
        public Npc(int uid)
        {
            UniqueId = uid;
            X = 256 + 128;
            Y = 256 + 128;
        }
    }
}