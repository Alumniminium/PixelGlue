using Microsoft.Xna.Framework;
using Pixel.Helpers;
using Pixel.ECS.Components;
using Pixel.Enums;

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
            foreach (var entity in CompIter.Get<VelocityComponent, PositionComponent>())
            {
                ref var vc = ref entity.Get<VelocityComponent>();
                ref var pc = ref entity.Get<PositionComponent>();

                MoveOneTile(deltaTime, ref vc, ref pc);
            }
        }

        private static void MoveOneTile(float dt, ref VelocityComponent vc, ref PositionComponent pc)
        {
            if (pc.Destination != pc.Position)
            {
                var dir = (pc.Destination - pc.Position);
                dir.Normalize();

                vc.Velocity = dir * vc.Speed * vc.SpeedMulti * dt;
                
                var distanceToDest = Vector2.Distance(pc.Position,pc.Destination);
                var moveDistance = Vector2.Distance(pc.Position, pc.Position + vc.Velocity);

                if (distanceToDest > moveDistance)
                    pc.Position += vc.Velocity;
                else
                    {
                        pc.Position = pc.Destination;
                        vc.Velocity = Vector2.Zero;
                    }
            }
        }
    }
}