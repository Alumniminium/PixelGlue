using System;
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

        public const int NUM_THREADS = 2;
        public Thread[] Threads = new Thread[NUM_THREADS];
        public AutoResetEvent[] Blocks = new AutoResetEvent[NUM_THREADS];
        private float deltaTime;

        public override void Initialize()
        {
            for (int i = 0; i < NUM_THREADS; i++)
            {
                Blocks[i] = new AutoResetEvent(false);
                Threads[i] = new Thread(WorkLoop)
                {
                    IsBackground = true
                };
                Threads[i].Start(i);
            }
        }
        private void WorkLoop(object idxObj)
        {
            int i = (int)idxObj;
            while (true)
            {
                Blocks[i].WaitOne();
                var chunkSize = Entities.Count / NUM_THREADS;
                var start = chunkSize*i;
                var end = start + chunkSize;
                for(int ei =start; ei < end; ei++)
                {
                    var entity = Entities[ei];
                    ref var vc = ref entity.Get<VelocityComponent>();
                    ref var pc = ref entity.Get<PositionComponent>();
                    ref var dc = ref entity.Get<DestinationComponent>();
                    ref var sp = ref entity.Get<SpeedComponent>();

                    if (pc.Value != dc.Value)
                    {
                        var dir = dc.Value - pc.Value;
                        dir.Normalize();

                        vc.Velocity = dir * sp.Speed * sp.SpeedMulti * deltaTime;

                        var distanceToDest = Vector2.Distance(pc.Value, dc.Value);
                        var moveDistance = Vector2.Distance(pc.Value, pc.Value + vc.Velocity);

                        if (distanceToDest > moveDistance)
                            pc.Value += vc.Velocity;
                        else
                        {
                            pc.Value = dc.Value;
                            vc.Velocity = Vector2.Zero;
                            if(entity.Has<NetworkComponent>() && entity.EntityId == Scene.Player.EntityId)
                            {
                                ref readonly var net = ref entity.Get<NetworkComponent>();
                                NetworkSystem.Send(MsgWalk.Create(net.UniqueId,pc.Value));
                            }
                        }
                    }
                }
            }
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>() && entity.Has<VelocityComponent>()
             && entity.Has<DestinationComponent>() && entity.Has<SpeedComponent>())
                base.AddEntity(entity);
        }

        public override void Update(float deltaTime)
        {
            this.deltaTime=deltaTime;
            for (int i = 0; i < NUM_THREADS; i++)
            {
                Blocks[i].Set();
            }
        }
    }
}