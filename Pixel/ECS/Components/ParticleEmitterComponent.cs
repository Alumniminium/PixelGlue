using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct ParticleEmitterComponent
    {
        public bool Active;
        public int Particles,MaxParticles,SpawnFrequency,LifetimeFrames;

        public EmitterType EmitterType;

        public ParticleEmitterComponent(int maxParticles=1000,int spawnPerFrameNum=10, int lifetimeFrames=5000, EmitterType type = EmitterType.Up)
        {
            Active=false;
            Particles=0;
            MaxParticles=maxParticles;
            SpawnFrequency=spawnPerFrameNum;
            LifetimeFrames=lifetimeFrames;
            EmitterType=type;
        }
    }

    public enum EmitterType
    {
        Sphere,
        Up,
        Down,
        Left,
        Right,
    }
}