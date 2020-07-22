using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct MouseComponent
    {
        public float Scroll;
        public float OldScroll;
        public float X,Y;
    }
}