using System.Threading;
using Shared.TerribleSockets.Queues;
using Shared.TerribleSockets.Server;
using System;
using Shared.ECS;
using Server.ECS.Systems;
using Shared.ECS.Components;
using Shared;
using Shared.TerribleSockets.Packets;
using Shared.ECS.Systems;

namespace Server
{
    public static class Program
    {
        private const int TickRate = 1000 / 30;
        public static int BotCount = 100;
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                BotCount = int.Parse(args[0]);

            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13338);
            Console.WriteLine("Server running!");

            World.Systems.Add(new GCMonitor(true,false));
            World.Systems.Add(new MoveSystem(true, false));

            for (int i = 0; i < BotCount; i++)
            {
                ref var npc = ref World.CreateEntity(100_000 + i);
                npc.Add(new NetworkComponent(100_000 + i));
                npc.Add<PositionComponent>();
                npc.Add<DestinationComponent>();
                npc.Add(new SpeedComponent(16));
                npc.Add<VelocityComponent>();
                Collections.Npcs.TryAdd(100_000 + i, new Npc(100_000 + i));
            }
            Console.WriteLine("NPCs Active: " + Collections.Npcs.Count);

            var preUpdateTicks = DateTime.UtcNow.Ticks;
            var dt = 0f;
            var prevTicks = DateTime.UtcNow.Ticks;

            while (true)
            {
                preUpdateTicks = DateTime.UtcNow.Ticks;
                dt = CalcDelta(preUpdateTicks, prevTicks);

                Update(dt);

                prevTicks = preUpdateTicks;
                var postUpdateTicks = DateTime.UtcNow.Ticks;
                Sleep(postUpdateTicks - preUpdateTicks);
            }
        }

        private static void Sleep(long timeSpent) => Thread.Sleep(Math.Max(0, TickRate - (int)(timeSpent / TimeSpan.TicksPerMillisecond)));
        private static float CalcDelta(long now, long last) => (now - last) / TimeSpan.TicksPerMillisecond / 1000f;

        private static void Update(float dt)
        {
            foreach (var system in World.Systems)
                system.PreUpdate();
            foreach (var system in World.Systems)
                system.Update(dt);

            foreach (var kvp in Collections.Players)
            {
                var player = kvp.Value;

                if (DateTime.Now >= player.LastPing.AddSeconds(5))
                {
                    if (Global.Verbose)
                        Console.WriteLine($"Sending Ping to {player.Name}/{player.Username}.");
                    player.Socket.Send(MsgPing.Create(player.UniqueId));
                    player.LastPing = DateTime.Now;
                }
            }
        }
    }
}