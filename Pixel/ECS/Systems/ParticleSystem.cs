using System.Collections.Generic;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.IO;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : PixelSystem<PositionComponent, VelocityComponent, DrawableComponent, ParticleComponent>
    {
        public ParticleSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name= "Particle System"; }
        
        public override void Update(float deltaTime, List<Entity> Entities)
        {
            foreach (var entity in Entities)
            {
                ref var prtcl = ref entity.Get<ParticleComponent>();
                ref readonly var emitter = ref World.GetEntity(prtcl.EmitterId);
                ref var pem = ref emitter.Get<ParticleEmitterComponent>();

                if (prtcl.FramesLeftToLive == 0)
                {
                    pem.Particles--;
                    World.Destroy(entity.EntityId);
                    continue;
                }

                ref var drw = ref entity.Get<DrawableComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                ref readonly var vel = ref entity.Get<VelocityComponent>();

                pos.Value += ((vel.Value * 1.0f) * prtcl.Energy);
                drw.DestRect.Location = pos.Value.ToPoint();
                prtcl.FramesLeftToLive--;
                // add different energy decay calcs
                prtcl.Energy -= prtcl.Energy * pem.DecayFactor;
            }
        }
    }
}