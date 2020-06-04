using System.Collections.Concurrent;

namespace Server
{
    public static class Collections
    {
        public static ConcurrentDictionary<int, Player> Players = new ConcurrentDictionary<int, Player>();
        public static ConcurrentDictionary<int, Npc> Npcs = new ConcurrentDictionary<int, Npc>();
    }
}