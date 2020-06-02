using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.Networking
{
    public class Networked : IEntityComponent
    {
        public int ServerX { get; }

        public int ServerY { get; }
    }
}