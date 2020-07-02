using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct ParticleEmitterComponent : IEntityComponent
    {
        public int EntityId { get; set; }

        public int SpawnFrequency;
    }
    public struct ParticleComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public int TTL;
    }
}