using Microsoft.Xna.Framework;
using Pixel.ECS.Components;

namespace Pixel.Entities
{
    public struct Camera
    {
        public Matrix ViewMatrix { get; set; }
        public Rectangle DrawRect { get; set; }
        public Rectangle DrawRectZoomed { get; set; }
        public Rectangle SimulationRect { get; set; }
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(point, Matrix.Invert(ViewMatrix));
        public Vector2 WorldToScreen(Vector2 point) => Vector2.Transform(point, ViewMatrix);
    }
}
