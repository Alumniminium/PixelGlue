using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.Entities
{
    public struct TransformComponent : IEntityComponent
    {
        public Matrix ViewMatrix { get; set; }
        public Matrix InverseTransform => Matrix.Invert(ViewMatrix);
        public int EntityId { get; set; }
    }
    public class Camera : Entity
    {
        public ref TransformComponent Transform => ref Get<TransformComponent>();
        public Rectangle ScreenRect { get; set; }
        public Rectangle ServerScreenRect { get; set; }
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(point, Transform.InverseTransform);
    }
}
