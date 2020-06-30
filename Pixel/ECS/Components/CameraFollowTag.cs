using Microsoft.Xna.Framework;
using Pixel.Enums;
using PixelShared;

namespace Pixel.ECS.Components
{
    public struct CameraFollowTagComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public float Zoom { get; set; }
        public Vector2 PositionOffset { get; set; }

        public CameraFollowTagComponent(int ownerId, int zoom)
        {
            EntityId = ownerId;
            Zoom = zoom;
            PositionOffset = new Vector2(Global.TileSize/2);
        }
    }
}
