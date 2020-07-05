using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class VelocitySystem : IEntitySystem
    {
        public string Name { get; set; } = "Velocity System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
         public void Update(float dt)
        {
            foreach (var entity in CompIter<InputComponent,SpeedComponent, VelocityComponent>.Get())
            {
                ref var inp = ref ComponentArray<InputComponent>.Get(entity);
                ref var vel = ref ComponentArray<VelocityComponent>.Get(entity);
                ref var spd = ref ComponentArray<SpeedComponent>.Get(entity);

                vel.Velocity = inp.Axis * spd.Speed * spd.SpeedMulti * dt;
            }
        }

    }
}