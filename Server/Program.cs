using System;
using System.Threading;
using TerribleSockets.Packets;
using TerribleSockets.Queues;
using TerribleSockets.Server;

namespace Server
{
    public class Program
    {
        public static Random Random = new Random(1337);
        public static int BotCount = 250;
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                BotCount = int.Parse(args[0]);

            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13338);
            Console.WriteLine($"Server running!");

            var t = new Thread(() =>
            {
                Console.WriteLine($"Heartbeat Thread started.");
                for (int i = 10; i < BotCount; i++)
                    Collections.Npcs.TryAdd(1 + i, new Npc(i));
                while (true)
                {
                    foreach (var kvp in Collections.Npcs)
                    {
                        var npc = kvp.Value;
                        if (DateTime.Now >= npc.LastMove.AddMilliseconds(1450))
                        {
                            npc.LastMove = DateTime.Now;
                            npc.X += Random.Next(-1, 2) * 16;
                            npc.Y += Random.Next(-1, 2) * 16;

                            foreach (var kvp2 in Collections.Players)
                            {
                                //Console.WriteLine($"Sending walk {npc.UniqueId}.");
                                kvp2.Value.Socket.Send(MsgWalk.Create(npc.UniqueId, npc.X, npc.Y));
                            }
                        }
                    }
                    foreach (var kvp in Collections.Players)
                    {
                        var player = kvp.Value;

                        if (DateTime.Now >= player.LastPing.AddSeconds(5))
                        {
                            //Console.WriteLine($"Sending Ping to {player.Name}/{player.Username}.");
                            player.Socket.Send(MsgPing.Create(player.UniqueId));
                            player.LastPing = DateTime.Now;
                        }
                    }

                    Thread.Sleep(33);
                }
            });
            t.Start();
            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}