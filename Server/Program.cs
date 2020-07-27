using System.Data;
using Microsoft.Xna.Framework;
using Shared;
using Shared.TerribleSockets.Packets;
using Shared.TerribleSockets.Queues;
using Shared.TerribleSockets.Server;
using System;
using System.Threading;

namespace Server
{
    public static class Program
    {
        private const int TickRate = 30;
        private const int SleepTime = 1000 / TickRate;
        public static int BotCount;
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                BotCount = int.Parse(args[0]);

            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13338);
            Console.WriteLine("Server running!");

            for (int i = 0; i < BotCount; i++)
                Collections.Npcs.TryAdd(100_000 + i, new Npc(100_000 + i));
            Console.WriteLine("NPCs Active: " + Collections.Npcs.Count);

            while (true)
            {
                var ticks = DateTime.UtcNow.Ticks;
                Simulation.Step();
                var postUpdate = DateTime.UtcNow.Ticks;

                var timeSpent = (postUpdate - ticks) / 10000;
                Thread.Sleep(SleepTime - (int)timeSpent);
            }
        }
    }
}