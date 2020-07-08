using Microsoft.Xna.Framework;
using Pixel.ECS.Components;

namespace Pixel.Entities
{
    public class Camera
    {
        public Matrix ViewMatrix { get; set; }
        public Matrix InverseTransform => Matrix.Invert(ViewMatrix);
        public Rectangle ScreenRect { get; set; }
        public Rectangle ServerScreenRect { get; set; }
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(point, InverseTransform);
    }
}
