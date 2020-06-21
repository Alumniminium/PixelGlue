using PixelGlueCore.ECS;
using Microsoft.Xna.Framework;

namespace PixelGlueCore.Entities
{
    public class Camera : PixelEntity
    {
        public Matrix Transform { get; set; }
        public Matrix InverseTransform => Matrix.Invert(Transform);
        public Rectangle ScreenRect { get; set; }
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(point, InverseTransform);
        public float Zoom{get;set;}

        public Camera()
        {
            EntityId = 1;
        }
    }
}
