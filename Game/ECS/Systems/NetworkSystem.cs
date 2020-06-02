using PixelGlueCore;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Enums;
using PixelGlueCore.Helpers;
using PixelGlueCore.Networking;
using System.Collections.Concurrent;
using TerribleSockets.Client;
using TerribleSockets.Packets;
using TerribleSockets.Queues;

namespace PixelGlueCore.ECS.Systems
{
    public class NetworkSystem : IEntitySystem
    {
        public string Name { get; set; } = "Network System";
        private static ClientSocket Socket { get; set; } = new ClientSocket(null);
        public static ConnectionState ConnectionState { get; set; } = ConnectionState.NotConnected;
        private static ConcurrentQueue<byte[]> PendingPackets { get; set; } = new ConcurrentQueue<byte[]>();
        private static ConcurrentQueue<byte[]> PendingSends { get; set; } = new ConcurrentQueue<byte[]>();
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Owner { get; set; }
        public NetworkSystem(Scene owner)
        {
            Owner = owner;
            ReceiveQueue.Start(Receive);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
        }
        public void FixedUpdate(double deltaTime)
        {
            switch (ConnectionState)
            {
                case ConnectionState.NotConnected:
                    ConnectAsync("127.0.0.1", 13338);
                    return;
                case ConnectionState.Connected:
                    ConnectionState = ConnectionState.Authenticating;
                    Socket.Send(MsgLogin.Create("Test", "pw" + PixelGlue.Random.Next(0, 1000)));
                    break;
                case ConnectionState.Authenticated:
                    SyncObjects();
                    break;
            }
        }
        public void Update(double deltaTime)
        {
            while (PendingPackets.TryDequeue(out var packet))
                PacketHandler.Handle(packet,Owner);

            while (PendingSends.TryDequeue(out var packet))
                Socket.Send(packet);
        }

        private void SyncObjects()
        {
            foreach (var scene in SceneManager.ActiveScenes)
                foreach (var kvp in scene.GameObjects)
                {
                    if (!kvp.Value.TryGetComponent<Networked>(out var networked))
                        continue;

                }
        }

        private void ConnectAsync(string ip, ushort port)
        {
            if (ConnectionState != ConnectionState.NotConnected)
                return;
            ConnectionState = ConnectionState.Connecting;
            FConsole.WriteLine("Connecting to Server...");
            Socket.ConnectAsync(ip, port);
        }

        private void Connected()
        {
            ConnectionState = ConnectionState.Connected;
            FConsole.WriteLine("[Player][Net] CONNECTED! :D");
        }

        private void Disconnected()
        {
            ConnectionState = ConnectionState.NotConnected;
            FConsole.WriteLine("[Player][Net] DISCONNECTED! Reconnecting...");
        }
        public static void Send(byte[] packet) => PendingSends.Enqueue(packet);
        private void Receive(ClientSocket client, byte[] packet) => PendingPackets.Enqueue(packet);
    }
}