﻿using System;
using System.Threading;
using Pixel.TerribleSockets.Packets;
using Pixel.TerribleSockets.Queues;
using Pixel.TerribleSockets.Server;

namespace Server
{
    public static class Program
    {
        public static Random Random = new Random(1337);
        public static int BotCount = 250;
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                BotCount = int.Parse(args[0]);

            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13338);
            Console.WriteLine("Server running!");

            var t = new Thread(() =>
            {
                Console.WriteLine("Heartbeat Thread started.");
                for (int i = 10; i < BotCount; i++)
                    Collections.Npcs.TryAdd(1 + i, new Npc(i));
                while (true)
                {
                    foreach (var kvp in Collections.Npcs)
                    {
                        var npc = kvp.Value;
                        if (DateTime.Now >= npc.LastMove.AddMilliseconds(550))
                        {
                            npc.LastMove = DateTime.Now;
                            npc.Location.X += Random.Next(-1, 2) * Pixel.Pixel.TileSize;
                            npc.Location.Y += Random.Next(-1, 2) * Pixel.Pixel.TileSize;

                            foreach (var kvp2 in Collections.Players)
                            {
                                var player = kvp2.Value;
                                var distance = Pixel.Maths.PixelMath.GetDistance(player.Location,npc.Location);
                                
                                Console.WriteLine($"Sending Walk/{npc.UniqueId} to player/{kvp2.Value.Username}. Distance: {distance}`");
                                
                                if(distance < 10)
                                    kvp2.Value.Socket.Send(MsgWalk.Create(npc.UniqueId, npc.Location));
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