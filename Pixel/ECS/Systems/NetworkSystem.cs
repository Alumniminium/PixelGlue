using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Networking;
using Pixel.Scenes;
using System.Collections.Concurrent;
using PixelShared.TerribleSockets.Client;
using PixelShared.TerribleSockets.Packets;
using PixelShared.TerribleSockets.Queues;
using PixelShared.IO;
using PixelShared;
using System;

namespace Pixel.ECS.Systems
{
    public class NetworkSystem : IEntitySystem
    {
        public string Name { get; set; } = "Network System";
        private static DateTime LastConnect;
        private static ClientSocket Socket { get; } = new ClientSocket(null);
        public static ConnectionState ConnectionState { get; set; }
        private static ConcurrentQueue<byte[]> PendingPackets { get; } = new ConcurrentQueue<byte[]>();
        private static ConcurrentQueue<byte[]> PendingSends { get; } = new ConcurrentQueue<byte[]>();
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public NetworkSystem()
        {
            ReceiveQueue.Start(Receive);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
        }
        public void FixedUpdate(float _){}
        public void Update(float deltaTime)
        {
            switch (ConnectionState)
            {
                case ConnectionState.NotConnected:
                    Connect("127.0.0.1", 13338);
                    return;
                case ConnectionState.Connected:
                    ConnectionState = ConnectionState.Authenticating;
                    Socket.Send(MsgLogin.Create("Test", "pw" + Global.Random.Next(0, 1000)));
                    break;
                case ConnectionState.Authenticating:
                case ConnectionState.Authenticated:
                    SyncObjects();
                    break;
            }
        }

        private void SyncObjects()
        {
            while (PendingPackets.TryDequeue(out var packet))
                PacketHandler.Handle(packet);

            while (PendingSends.TryDequeue(out var packet))
                Socket.Send(packet);
        }

        private void Connect(string ip, ushort port)
        {
            if (ConnectionState != ConnectionState.NotConnected)
                return;
            if(LastConnect.AddSeconds(5) > DateTime.UtcNow)
                return;
            LastConnect=DateTime.UtcNow;
            ConnectionState = ConnectionState.Connecting;
            FConsole.WriteLine("[NetworkSystem] Connecting to Server...");
            Socket.ConnectAsync(ip, port);
        }

        private void Connected()
        {
            ConnectionState = ConnectionState.Connected;
            FConsole.WriteLine("[NetworkSystem] CONNECTED! :D");
        }

        private void Disconnected()
        {
            ConnectionState = ConnectionState.NotConnected;
            FConsole.WriteLine("[NetworkSystem] DISCONNECTED! Reconnecting...");
        }
        public static void Send(byte[] packet) => PendingSends.Enqueue(packet);
        private void Receive(ClientSocket client, byte[] packet) => PendingPackets.Enqueue(packet);
    }
}