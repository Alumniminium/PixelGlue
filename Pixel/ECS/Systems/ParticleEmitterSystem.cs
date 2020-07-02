using System.Collections.Generic;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class ParticleEmitterSystem : IEntitySystem
    {
        public string Name { get; set; } = "Particle System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            foreach(var i in CompIter<PositionComponent, ParticleEmitterComponent>.Get())
            {
                ref var pos = ref ComponentArray<PositionComponent>.Get(i);
                ref var pem = ref ComponentArray<ParticleEmitterComponent>.Get(i);

            }
        }
    }
}