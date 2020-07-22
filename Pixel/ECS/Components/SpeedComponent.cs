using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct SpeedComponent
    {
        public float SpeedMulti;
        public float Speed;
        public SpeedComponent(float speed)
        {
            Speed = speed;
            SpeedMulti = 1;
        }
    }
}