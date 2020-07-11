using Pixel.ECS.Components;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class ParticleEmitterSystem : PixelSystem
    {
        public ParticleEmitterSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Particle System";
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>() && entity.Has<ParticleEmitterComponent>()&& entity.Has<ParticleComponent>())
                base.AddEntity(entity);
        }
        public override void Update(float deltaTime)
        {
            for(int i = 0; i< Entities.Count; i++)
            {
                var entity = Entities[i];
                ref var pos = ref entity.Get<PositionComponent>();
                ref var pem = ref entity.Get<ParticleEmitterComponent>();

            }
        }
    }
}