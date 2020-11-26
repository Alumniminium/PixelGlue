using System;
using System.Threading;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.IO;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : AsyncPixelSystem
    {
        public const int THREAD_COUNT = 1;
        public override string Name { get; set; } = "Particle System";
        public ParticleSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void Initialize() => StartWorkerThreads(THREAD_COUNT, ThreadPriority.Highest);
        public override void Update(float gameTime) => UnblockThreads();

        public override bool MatchesFilter(Entity entity) => entity.Has<PositionComponent, VelocityComponent, DrawableComponent, ParticleComponent>();
        
        public override void ThreadedUpdate(WorkerThread wt)
        {
            foreach (var entityId in wt.Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var prtcl = ref entity.Get<ParticleComponent>();

                if (prtcl.FramesLeftToLive == 0)
                {
                    ref readonly var emitter = ref World.GetEntity(prtcl.EmitterId);
                    ref var pem = ref emitter.Get<ParticleEmitterComponent>();

                    pem.Particles--;
                    World.Destroy(entity.EntityId);
                    return;
                }

                if (prtcl.FramesLeftToLive > 0)
                {
                    ref var drw = ref entity.Get<DrawableComponent>();
                    ref var pos = ref entity.Get<PositionComponent>();
                    ref readonly var vel = ref entity.Get<VelocityComponent>();

                    pos.Value += (vel.Value * prtcl.Energy);
                    drw.DestRect.Location = pos.Value.ToPoint();
                    prtcl.FramesLeftToLive--;
                    if (prtcl.FramesLeftToLive == 3000)
                        prtcl.Energy += 0.6f;
                }
            }
            var dt = DateTime.UtcNow - wt.StartTime;
            if(wt.Entities.Count>0)
            FConsole.WriteLine($"Particle System WT {wt.Id} -> {wt.Entities.Count} in {dt.TotalMilliseconds.ToString("0.00")}ms");
            base.ThreadedUpdate(wt);
        }
    }
}