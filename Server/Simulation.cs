using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Numerics;
using Shared;
using Shared.TerribleSockets.Packets;
using Server.ECS.Systems;
using Shared.ECS;

namespace Server
{
    public static class Simulation
    {
        public const int SIMULATION_STEP_DURATION = 1000;

        public static void Step(float deltaTime)
        {
            foreach(var system in World.Systems)
                system.Update(deltaTime);
           
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