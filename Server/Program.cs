using Microsoft.Xna.Framework;
using PixelShared;
using PixelShared.TerribleSockets.Packets;
using PixelShared.TerribleSockets.Queues;
using PixelShared.TerribleSockets.Server;
using System;
using System.Threading;

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
                for (int i = 0; i < BotCount; i++)
                    Collections.Npcs.TryAdd(100_000 + i, new Npc(100_000 + i));
                Console.WriteLine("NPCs Active: " + Collections.Npcs.Count);
                while (true)
                {
                    foreach (var kvp in Collections.Npcs)
                    {
                        var npc = kvp.Value;
                        if (DateTime.Now >= npc.LastMove.AddMilliseconds(550))
                        {
                            npc.LastMove = DateTime.Now;
                            npc.Position.X += Random.Next(-1, 2) * Global.TileSize;
                            npc.Position.Y += Random.Next(-1, 2) * Global.TileSize;

                            foreach (var kvp2 in Collections.Players)
                            {
                                var player = kvp2.Value;
                                if (npc.Position.X < player.ViewBounds.Left || npc.Position.X >  player.ViewBounds.Right)
                                    continue;
                                if (npc.Position.Y < player.ViewBounds.Top || npc.Position.Y >  player.ViewBounds.Bottom)
                                    continue;
                                
                                    //Console.WriteLine($"Sending Walk/{npc.UniqueId} {(int)npc.Position.X},{(int)npc.Position.Y} to player {(int)kvp2.Value.Location.X},{(int)kvp2.Value.Location.Y}");
                                    kvp2.Value.Socket.Send(MsgWalk.Create(npc.UniqueId, npc.Position));
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

                    Thread.Sleep(100);
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