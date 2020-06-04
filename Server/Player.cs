using System;
using System.Numerics;
using TerribleSockets.Client;

namespace Server
{
    public class Player
    {
        public string Name;
        public int UniqueId;
        public Vector2 Location;
        public ClientSocket Socket;
        public string Username;
        public string Password;

        public Player(ClientSocket socket)
        {
            Socket = socket;
            Socket.OnDisconnect += OnDisconnected;
        }

        public DateTime LastPing { get; set; }

        private void OnDisconnected()
        {
            Console.WriteLine($"Player {Name}/{Username} disconnected.");
            Collections.Players.TryRemove(UniqueId, out _);
        }
    }
}