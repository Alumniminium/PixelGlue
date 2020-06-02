using System.Collections.Concurrent;

namespace Server
{
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Player> Players = new ConcurrentDictionary<uint, Player>();
        public static ConcurrentDictionary<uint, Npc> Npcs = new ConcurrentDictionary<uint, Npc>();
    }
}