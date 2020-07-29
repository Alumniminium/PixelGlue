using System;
using System.Numerics;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.TerribleSockets.Packets;

namespace Server.ECS.Systems
{
    public class MoveSystem : PixelSystem
    {
        public override string Name { get; set; } = "Move System";

        public MoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, VelocityComponent, DestinationComponent, SpeedComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();

                if (pos.Value != dst.Value)
                {
                    ref readonly var spd = ref entity.Get<SpeedComponent>();
                    ref var vel = ref entity.Get<VelocityComponent>();

                    var dir = dst.Value - pos.Value;
                    dir = Vector2.Normalize(dir);

                    vel.Velocity = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                    var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Velocity);

                    if (distanceToDest > moveDistance)
                        pos.Value += vel.Velocity;
                    else
                    {
                        pos.Value = dst.Value;
                        vel.Velocity = Vector2.Zero;
                    }
                }
                else
                {
                    dst.Value = new Vector2(dst.Value.X + (Global.Random.Next(0,5) * Global.TileSize), dst.Value.Y + (Global.Random.Next(0,5) * Global.TileSize));
                    foreach (var kvp2 in Collections.Players)
                    {
                        var player = kvp2.Value;

                        if (pos.Value.X < player.ViewBounds.Left || pos.Value.X > player.ViewBounds.Right)
                            continue;
                        if (pos.Value.Y < player.ViewBounds.Top || pos.Value.Y > player.ViewBounds.Bottom)
                            continue;

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