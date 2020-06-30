using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct CameraFollowTagComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public float Zoom { get; set; }

        public CameraFollowTagComponent(int ownerId, int zoom)
        {
            EntityId = ownerId;
            Zoom = zoom;
        }
    }
}
