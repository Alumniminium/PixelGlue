using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class CursorMoveSystem : PixelSystem
    {
        public override string Name { get; set; } = "Cursor Move System";

        public CursorMoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, MouseComponent>() && !entity.Has<VelocityComponent, SpeedComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var mos = ref entity.Get<MouseComponent>();
                ref var pos = ref entity.Get<PositionComponent>();

                pos.Value.X = mos.WorldX;
                pos.Value.Y = mos.WorldY;
            }
        }
    }
}