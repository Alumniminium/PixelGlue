using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : PixelSystem
    {
        public override string Name { get; set; } = "Particle System";
        public ParticleSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent,VelocityComponent,DrawableComponent,ParticleComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var pos = ref entity.Get<PositionComponent>();
                ref var drw = ref entity.Get<DrawableComponent>();
                ref readonly var vel = ref entity.Get<VelocityComponent>();
                ref var prtcl = ref entity.Get<ParticleComponent>();
                
                if(prtcl.FramesLeftToLive == 0)
                {
                    World.DestroyAsap(entity.EntityId);
                    ref readonly var emitter = ref World.GetEntity(prtcl.EmitterId);
                    ref var pem = ref emitter.Get<ParticleEmitterComponent>();
                    pem.Particles--;
                    return;
                }

                if(prtcl.FramesLeftToLive > 0)
                {                
                    pos.Value += (vel.Value * prtcl.Energy);
                    drw.DestRect.Location = pos.Value.ToPoint();
                    prtcl.FramesLeftToLive--;
                    if(prtcl.FramesLeftToLive == 3000)
                    prtcl.Energy += 0.6f;
                }
            }
        }
    }
}