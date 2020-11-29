using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct ParticleComponent
    {
        public int FramesLeftToLive;

        public float Energy { get; internal set; }
        public int EmitterId { get; internal set; }

        public ParticleComponent(int framesToLive, float energy, int emitterId)
        {
            FramesLeftToLive=framesToLive;
            Energy=energy;
            EmitterId=emitterId;
        }
    }
}