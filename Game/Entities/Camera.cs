using PixelGlueCore.ECS;
using Microsoft.Xna.Framework;

namespace PixelGlueCore.Entities
{
    public class Camera : GameObject
    {
        public Matrix Transform { get; set; }
        public Rectangle ScreenRect { get; set; }
    }
}
