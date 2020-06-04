using PixelGlueCore.ECS;
using Microsoft.Xna.Framework;
using System;

namespace PixelGlueCore.Entities
{
    public class Camera : PixelEntity
    {
        public Matrix Transform { get; set; }
        public Rectangle ScreenRect { get; set; }

        public Vector2 ScreenToWorld(in Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(Transform);
            return Vector2.Transform(point, invertedMatrix);
        }
    }
}
