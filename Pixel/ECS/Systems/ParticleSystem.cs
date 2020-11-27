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
        public ParticleSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw,THREAD_COUNT) { }
        public override bool MatchesFilter(Entity entity) => entity.Has<PositionComponent, VelocityComponent, DrawableComponent, ParticleComponent>();
        
        public override void ThreadedUpdate(WorkerThread wt)
        {
            foreach (var entityId in wt.Entities)
            {
                ref var entity = ref World.GetEntity(entityId);
                ref var prtcl = ref entity.Get<ParticleComponent>();
                ref readonly var emitter = ref World.GetEntity(prtcl.EmitterId);
                ref var pem = ref emitter.Get<ParticleEmitterComponent>();

                if (prtcl.FramesLeftToLive == 0)
                {
                    pem.Particles--;
                    World.Destroy(entity.EntityId);
                    continue;
                }

                if (prtcl.FramesLeftToLive > 0)
                {
                    ref var drw = ref entity.Get<DrawableComponent>();
                    ref var pos = ref entity.Get<PositionComponent>();
                    ref readonly var vel = ref entity.Get<VelocityComponent>();

                    pos.Value += (vel.Value * prtcl.Energy);
                    drw.DestRect.Location = pos.Value.ToPoint();
                    prtcl.FramesLeftToLive--;
                    // add different energy decay calcs
                    prtcl.Energy -= prtcl.Energy * pem.DecayFactor;
                }
            }
            base.ThreadedUpdate(wt);
        }
    }
}