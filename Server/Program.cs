using System;
using System.Threading;
using Microsoft.Xna.Framework;
using PixelShared.TerribleSockets.Packets;
using PixelShared.TerribleSockets.Queues;
using PixelShared.TerribleSockets.Server;

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
                    Collections.Npcs.TryAdd(100_000 + i, new Npc(100_000 +i));
                while (true)
                {
                    foreach (var kvp in Collections.Npcs)
                    {
                        var npc = kvp.Value;
                        if (DateTime.Now >= npc.LastMove.AddMilliseconds(550))
                        {
                            npc.LastMove = DateTime.Now;
                            npc.Location.X += Random.Next(-1, 2) * PixelShared.Pixel.TileSize;
                            npc.Location.Y += Random.Next(-1, 2) * PixelShared.Pixel.TileSize;

                            foreach (var kvp2 in Collections.Players)
                            {
                                var player = kvp2.Value;
                                var distance = Vector2.Distance(player.Location,npc.Location);
                                
                                Console.WriteLine($"Sending Walk/{npc.UniqueId} {(int)npc.Location.X},{(int)npc.Location.Y} to player {(int)kvp2.Value.Location.X}{(int)kvp2.Value.Location.Y}. Distance: {distance}`");
                                
                                if(distance < 100)
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