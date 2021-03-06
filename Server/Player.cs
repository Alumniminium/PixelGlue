using Microsoft.Xna.Framework;
using Shared;
using Shared.TerribleSockets.Client;
using System;

namespace Server
{
    public class Player
    {
        private Rectangle _viewRect = new Rectangle(0, 0, Global.VirtualScreenWidth + Global.HalfVirtualScreenWidth, Global.VirtualScreenHeight + Global.HalfVirtualScreenHeight);
        private Vector2 _location = new Vector2(1024,1024);
        public string Name;
        public int UniqueId;
        public Rectangle ViewBounds => _viewRect;
        public Vector2 Location
        {
            get => _location; set
            {
                _location = value;
                value.Round();
                var simRectX = (int)value.X - (Global.HalfVirtualScreenWidth + Global.HalfVirtualScreenWidth);
                var simRectY = (int)value.Y - (Global.HalfVirtualScreenHeight + Global.HalfVirtualScreenHeight);
                _viewRect.X = simRectX;
                _viewRect.Y = simRectY;
            }
        }
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