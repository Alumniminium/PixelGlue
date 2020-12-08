using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared.Enums;
using Shared.TerribleSockets.Packets;
using Shared;
using Shared.ECS;
using Pixel.Scenes;
using Shared.ECS.Components;
using System.Collections.Generic;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : PixelSystem<PositionComponent, DestinationComponent, SpeedComponent>
    {
        public MoveSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }

        public override void Update(float deltaTime, List<Entity> Entities)
        {
            foreach (var entity in Entities)
            {
                if(!entity.Has<PositionComponent, DestinationComponent, SpeedComponent>())
                    continue;

                ref readonly var spd = ref entity.Get<SpeedComponent>();

                ref var vel = ref entity.Add<VelocityComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();


                var dir = dst.Value - pos.Value;
                if (dir == Vector2.Zero)
                {
                    entity.Remove<DestinationComponent>();
                    continue;
                }
                dir.Normalize();

                vel.Value = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Value);
                var keepmoving = false;

                if (distanceToDest <= moveDistance && entity.Has<KeyboardComponent>())
                {
                    ref readonly var inp = ref entity.Get<KeyboardComponent>();
                    keepmoving = inp.OldButtons.Contains(PixelGlueButtons.Left) || inp.OldButtons.Contains(PixelGlueButtons.Right) || inp.OldButtons.Contains(PixelGlueButtons.Down) || inp.OldButtons.Contains(PixelGlueButtons.Up);
                    dst.Value += inp.Axis * Global.TileSize;
                }

                if (distanceToDest <= moveDistance && !keepmoving)
                {
                    pos.Value = dst.Value;
                    vel.Value = Vector2.Zero;
                    //RemoveEntity(entity.EntityId);
                }
                else
                    pos.Value += vel.Value;

                if (entity.Has<NetworkComponent>() && entity.EntityId == TestingScene.Player.Entity.EntityId)
                {
                    ref readonly var net = ref entity.Get<NetworkComponent>();
                    NetworkSystem.Send(MsgWalk.Create(net.UniqueId, pos.Value));
                }
            }
        }
    }
}