using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.TerribleSockets.Packets;

namespace Server.ECS.Systems
{
    public class MoveSystem : PixelSystem<PositionComponent, DestinationComponent, SpeedComponent>
    {
        public MoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name  = "Move System"; }
        
        public override void Update(float deltaTime, GCNeutralList<Entity> Entities)
        {
            for(int i =0; i< Entities.Count; i++)
            {
                ref var entity = ref Entities[i];
                if(!entity.Has<PositionComponent, DestinationComponent, SpeedComponent>())
                    continue;
                    
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();

                if (pos.Value != dst.Value)
                {
                    ref readonly var spd = ref entity.Get<SpeedComponent>();
                    ref var vel = ref entity.Add<VelocityComponent>();

                    var dir = dst.Value - pos.Value;
                    dir = Vector2.Normalize(dir);

                    vel.Value = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                    var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Value);

                    if (distanceToDest > moveDistance)
                        pos.Value += vel.Value;
                    else
                    {
                        pos.Value = dst.Value;
                        vel.Value = Vector2.Zero;
                    }
                }
                else
                {
                    dst.Value = new Vector2(dst.Value.X + (Global.Random.Next(0,5) * Global.TileSize), dst.Value.Y + (Global.Random.Next(0,5) * Global.TileSize));
                    foreach (var kvp2 in Collections.Players)
                    {
                        var player = kvp2.Value;

                        //if (pos.Value.X < player.ViewBounds.Left || pos.Value.X > player.ViewBounds.Right)
                        //    continue;
                        //if (pos.Value.Y < player.ViewBounds.Top || pos.Value.Y > player.ViewBounds.Bottom)
                        //    continue;

                        if (Global.Verbose)
                            Console.WriteLine($"Sending Walk/{entity.EntityId * 1000000} {(int)pos.Value.X},{(int)pos.Value.Y} to player {(int)kvp2.Value.Location.X},{(int)kvp2.Value.Location.Y}");
                        kvp2.Value.Socket.Send(MsgSpawn.Create(entity.EntityId * 1000000, (int)pos.Value.X, (int)pos.Value.Y, Global.Random.Next(0, 12), "sup"));
                        kvp2.Value.Socket.Send(MsgWalk.Create(entity.EntityId * 1000000, (int)pos.Value.X, (int)pos.Value.Y));
                    }
                }
            }
        }
    }
}