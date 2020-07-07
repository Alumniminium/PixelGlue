using Microsoft.Xna.Framework;
using PixelShared;

namespace Pixel.ECS.Components
{
    public struct CameraFollowTagComponent
    {
        public float Zoom { get; set; }
        public Vector2 PositionOffset { get; set; }

        public CameraFollowTagComponent(int zoom)
        {
            Zoom = zoom;
            PositionOffset = new Vector2(Global.TileSize/2);
        }
    }
}
