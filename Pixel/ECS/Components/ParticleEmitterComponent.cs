using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct ParticleEmitterComponent
    {
        public bool Active;
        public int Particles,MaxParticles,SpawnFrequency,LifetimeFrames;
        public float ParticleStartingEnergy;
        public float DecayFactor;

        public EmitterType EmitterType;

        public ParticleEmitterComponent(int maxParticles=1000,int spawnPerFrameNum=1, int lifetimeFrames=5000,float startingEnergy = 2f, float decayFactor = 0.1f, EmitterType type = EmitterType.Up)
        {
            Active=false;
            Particles=0;
            MaxParticles=maxParticles;
            SpawnFrequency=spawnPerFrameNum;
            LifetimeFrames=lifetimeFrames;
            EmitterType=type;
            ParticleStartingEnergy=startingEnergy;
            DecayFactor=decayFactor;
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