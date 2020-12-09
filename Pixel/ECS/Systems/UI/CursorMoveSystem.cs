using System.Collections.Generic;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class CursorMoveSystem : PixelSystem<PositionComponent, MouseComponent>
    {
        public CursorMoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name = "Cursor Move System";}
        public override void Update(float deltaTime, GCNeutralList<Entity> Entities)
        {
            for(int i =0; i< Entities.Count; i++)
            {
                ref var entity = ref Entities[i];
                if(entity.Has<VelocityComponent, SpeedComponent>())
                    continue;

                ref readonly var mos = ref entity.Get<MouseComponent>();
                ref var pos = ref entity.Get<PositionComponent>();

                pos.Value.X = mos.WorldX;
                pos.Value.Y = mos.WorldY;
            }
        }
    }
}