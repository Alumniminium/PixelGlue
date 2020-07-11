using Pixel.ECS.Components;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : PixelSystem
    {
        public ParticleSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Particle System";

        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>() && entity.Has<VelocityComponent>()&& entity.Has<DrawableComponent>())
                base.AddEntity(entity);
        }
        public override void Update(float deltaTime)
        {
            for(int i = 0; i< Entities.Count; i++)
            {
                var entity = Entities[i];
                ref var pos = ref entity.Get<PositionComponent>();
                ref readonly var vel = ref entity.Get<VelocityComponent>();
                ref readonly var drw = ref entity.Get<DrawableComponent>();
                
                pos.Value += vel.Velocity;
            }
        }
    }
}