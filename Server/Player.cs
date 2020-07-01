using Microsoft.Xna.Framework;
using PixelShared;
using PixelShared.TerribleSockets.Client;
using System;

namespace Server
{
    public class Player
    {
        private Rectangle _viewRect = new Rectangle(0,0,Global.VirtualScreenWidth + (Global.TileSize*8),Global.VirtualScreenHeight + (Global.TileSize*8));
        private Vector2 _location;
        public string Name;
        public int UniqueId;
        public Rectangle ViewBounds => _viewRect;
        public Vector2 Location { get => _location; set {
            _location = value;
            value.Round();
            _viewRect.X = (int)value.X - (Global.HalfVirtualScreenWidth + (Global.TileSize*8));
            _viewRect.Y = (int)value.Y - (Global.HalfVirtualScreenHeight + (Global.TileSize*8));
        }}
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