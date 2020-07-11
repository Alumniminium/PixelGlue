using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared.Enums;
using Shared.TerribleSockets.Packets;
using Shared;

using Shared.ECS;
using Pixel.Scenes;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : PixelSystem
    {
        public MoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Move System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            for (int ei = 0; ei < Entities.Count; ei++)
            {
                var entity = Entities[ei];
                ref var vel = ref entity.Get<VelocityComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();
                ref readonly var spd = ref entity.Get<SpeedComponent>();

                if (pos.Value != dst.Value)
                {
                    var dir = dst.Value - pos.Value;
                    dir.Normalize();

                    vel.Velocity = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                    var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Velocity);
                    var keepmoving = false;
                    if(distanceToDest <= moveDistance && entity.Has<InputComponent>())
                    {
                        ref readonly var inp = ref entity.Get<InputComponent>();
                        keepmoving = inp.OldButtons.Contains(PixelGlueButtons.Left) || inp.OldButtons.Contains(PixelGlueButtons.Right) || inp.OldButtons.Contains(PixelGlueButtons.Down) || inp.OldButtons.Contains(PixelGlueButtons.Up);
                        if (entity.Has<NetworkComponent>() && entity.EntityId == Scene.Player.EntityId)
                        {
                            ref readonly var net = ref entity.Get<NetworkComponent>();
                            NetworkSystem.Send(MsgWalk.Create(net.UniqueId, dst.Value));
                        }
                        dst.Value += inp.Axis * Global.TileSize;
                    }

                    if (distanceToDest > moveDistance || keepmoving)
                        pos.Value += vel.Velocity;
                    else
                    {
                        pos.Value = dst.Value;
                        vel.Velocity = Vector2.Zero;
                        if (entity.Has<NetworkComponent>() && entity.EntityId == Scene.Player.EntityId)
                        {
                            ref readonly var net = ref entity.Get<NetworkComponent>();
                            NetworkSystem.Send(MsgWalk.Create(net.UniqueId, pos.Value));
                        }
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>())
                if (entity.Has<VelocityComponent>())
                    if (entity.Has<DestinationComponent>())
                        if (entity.Has<SpeedComponent>())
                            base.AddEntity(entity);
        }
    }
}