using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class ParticleEmitterSystem : PixelSystem
    {
        public override string Name { get; set; } = "Particle System";
        public ParticleEmitterSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent>() && entity.Has<ParticleEmitterComponent>() && entity.Has<ParticleComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var pos = ref entity.Get<PositionComponent>();
                ref var pem = ref entity.Get<ParticleEmitterComponent>();
            }
        }
    }
}