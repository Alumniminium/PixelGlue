using System;
using System.Numerics;
using Shared;
using Shared.TerribleSockets.Packets;

namespace Server
{
    public static class Simulation
    {
        public const int SIMULATION_STEP_DURATION = 1000;

        public static void Step()
        {
            foreach (var kvp in Collections.Npcs)
            {
                var npc = kvp.Value;
                if (DateTime.Now >= npc.LastMove.AddMilliseconds(850))
                {
                    npc.LastMove = DateTime.Now;
                    var dir = Vector2.Zero;

                    dir.X += Global.Random.Next(-1, 2);
                    if (dir.X == 0)
                        dir.Y += Global.Random.Next(-1, 2);

                    npc.Position = npc.Position + (dir * 16);
                    foreach (var kvp2 in Collections.Players)
                    {
                        var player = kvp2.Value;

                        if (npc.Position.X < player.ViewBounds.Left || npc.Position.X > player.ViewBounds.Right)
                            continue;
                        if (npc.Position.Y < player.ViewBounds.Top || npc.Position.Y > player.ViewBounds.Bottom)
                            continue;

                        if (Global.Verbose)
                            Console.WriteLine($"Sending Walk/{npc.UniqueId} {(int)npc.Position.X},{(int)npc.Position.Y} to player {(int)kvp2.Value.Location.X},{(int)kvp2.Value.Location.Y}");
                        kvp2.Value.Socket.Send(MsgSpawn.Create(npc.UniqueId, (int)npc.Position.X, (int)npc.Position.Y, Global.Random.Next(0, 12), "sup"));
                        kvp2.Value.Socket.Send(MsgWalk.Create(npc.UniqueId, (int)npc.Position.X,(int)npc.Position.Y));
                    }
                }
            }
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