using System.Globalization;
using Microsoft.Xna.Framework;
using Shared;

namespace Pixel.ECS.Components
{
    public struct CameraComponent
    {
        public Matrix Transform;
        public Matrix InverseTransform => Matrix.Invert(Transform);
        public Vector2 ScreenToWorld(Vector2 point) => Vector2.Transform(point, InverseTransform);
        public Rectangle ScreenRect;
        public float Zoom { get; set; }
        public Vector2 PositionOffset { get; set; }

        public CameraComponent(int zoom) : this()
        {
            Zoom = zoom;
            PositionOffset = new Vector2(Global.HalfVirtualScreenWidth - Global.TileSize / 2, Global.HalfVirtualScreenHeight);
        }
    }
}
