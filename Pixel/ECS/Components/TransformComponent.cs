using Microsoft.Xna.Framework;

namespace Pixel.ECS.Components
{
    public struct TransformComponent
    {
        public Matrix ViewMatrix { get; set; }
        public Matrix InverseTransform => Matrix.Invert(ViewMatrix);
    }
}
