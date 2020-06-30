using Microsoft.Xna.Framework;

namespace Pixel.Entities
{
    public class Camera : Entity
    {
        public Matrix Transform { get; set; }
        public Matrix InverseTransform => Matrix.Invert(Transform);
        public Rectangle ScreenRect { get; set; }
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(Vector2.Floor(point), InverseTransform);
    }
}
