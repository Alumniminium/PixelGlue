using Pixel.ECS.Components;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : PixelSystem
    {
        public override string Name { get; set; } = "Particle System";
        public ParticleSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent>() && entity.Has<VelocityComponent>()&& entity.Has<DrawableComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var pos = ref entity.Get<PositionComponent>();
                ref readonly var vel = ref entity.Get<VelocityComponent>();
                ref readonly var drw = ref entity.Get<DrawableComponent>();
                
                pos.Value += vel.Velocity;
            }
        }
    }
}