using System.Threading;
using Shared.TerribleSockets.Queues;
using Shared.TerribleSockets.Server;
using System;
using Shared.ECS;
using Pixel.ECS.Components;
using Server.ECS.Systems;

namespace Server
{
    public static class Program
    {
        public static int BotCount=100;
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                BotCount = int.Parse(args[0]);

            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13338);
            Console.WriteLine("Server running!");

            World.Systems.Add(new MoveSystem(true,false));

            for (int i = 0; i < BotCount; i++)
            {
                Collections.Npcs.TryAdd(100_000 + i, new Npc(100_000 + i));
                ref var npc = ref World.CreateEntity(100_000 + i);
                npc.Add<PositionComponent>();
                npc.Add<DestinationComponent>();
                npc.Add(new SpeedComponent(16));
                npc.Add<VelocityComponent>();
                World.Register(ref npc);
            }
            Console.WriteLine("NPCs Active: " + Collections.Npcs.Count);

            var now = DateTime.UtcNow.Ticks;
            var dt = 0f;
            var last = DateTime.UtcNow.Ticks;

            while (true)
            {
                now = DateTime.UtcNow.Ticks;
                dt = (now - last) / TimeSpan.TicksPerMillisecond / 1000f;
                Simulation.Step(dt);
                var postUpdateTicks = DateTime.UtcNow.Ticks;
                last = now;
                var timeTaken = postUpdateTicks-now;
                Thread.Sleep(Math.Max(0,33 - (int)(timeTaken / TimeSpan.TicksPerMillisecond)));
            }
        }
    }
}