using Pixel.ECS.Components;
using Pixel.Entities;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : PixelSystem
    {
        public override string Name { get; set; } = "Particle System";

        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>() && entity.Has<VelocityComponent>()&& entity.Has<DrawableComponent>())
                base.AddEntity(entity);
        }
        public override void Update(float deltaTime)
        {
            foreach(var entity in Entities)
            {
                ref var pos = ref entity.Get<PositionComponent>();
                ref readonly var vel = ref entity.Get<VelocityComponent>();
                ref readonly var drw = ref entity.Get<DrawableComponent>();
                
                pos.Value += vel.Velocity;
            }
        }
    }
}