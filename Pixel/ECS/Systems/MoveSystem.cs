using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Shared.TerribleSockets.Packets;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : PixelSystem
    {
        public override string Name { get; set; } = "Move System";

        private const int THREADS = 4;
        private float deltaTime;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Initialize()
        {
            StartWorkerThreads(THREADS, true, ThreadPriority.Lowest);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AsyncUpdate(int threadId)
        {
            var chunkSize = Entities.Count / THREADS;
            var start = chunkSize * threadId;
            var end = start + chunkSize;
            for (int ei = start; ei < end; ei++)
            {
                var entity = Entities[ei];
                ref var vel = ref entity.Get<VelocityComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();
                ref var spd = ref entity.Get<SpeedComponent>();

                if (pos.Value != dst.Value)
                {
                    var dir = dst.Value - pos.Value;
                    dir.Normalize();

                    vel.Velocity = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                    var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Velocity);

                    if (distanceToDest > moveDistance)
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
            if (entity.Has<PositionComponent>() && entity.Has<VelocityComponent>()
             && entity.Has<DestinationComponent>() && entity.Has<SpeedComponent>())
                base.AddEntity(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            this.deltaTime = deltaTime;
            UnblockThreads();
        }
    }
}