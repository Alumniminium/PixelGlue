using Microsoft.Xna.Framework;
using Pixel.ECS.Components;

namespace Pixel.Entities
{
    public class Camera : Entity
    {
        public ref TransformComponent Transform => ref Get<TransformComponent>();
        public Rectangle ScreenRect { get; set; }
        public Rectangle ServerScreenRect { get; set; }
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(point, Transform.InverseTransform);
    }
}
