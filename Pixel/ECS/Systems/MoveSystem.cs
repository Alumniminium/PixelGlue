using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : IEntitySystem
    {
        public string Name { get; set; } = "Move System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            foreach (var entity in CompIter<VelocityComponent, DestinationComponent,SpeedComponent, PositionComponent>.Get())
            {
                ref var vc = ref ComponentArray<VelocityComponent>.Get(entity);
                ref var pc = ref ComponentArray<PositionComponent>.Get(entity);
                ref var dc = ref ComponentArray<DestinationComponent>.Get(entity);
                ref var sp = ref ComponentArray<SpeedComponent>.Get(entity);
                
                if (pc.Value != dc.Value)
                {
                    var dir = dc.Value - pc.Value;
                    dir.Normalize();

                    vc.Velocity = dir * sp.Speed * sp.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pc.Value, dc.Value);
                    var moveDistance = Vector2.Distance(pc.Value, pc.Value + vc.Velocity);

                    if (distanceToDest > moveDistance)
                        pc.Value += vc.Velocity;
                    else
                    {
                        pc.Value = dc.Value;
                        vc.Velocity = Vector2.Zero;
                    }
                }
            }
        }
    }
}