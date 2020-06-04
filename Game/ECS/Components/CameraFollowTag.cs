using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class CameraFollowTagComponent : IEntityComponent
    {
        public float Zoom { get; set; }
        public int PixelOwnerId { get; set; }

        public CameraFollowTagComponent(int ownerId, float zoom = 1)
        {
            PixelOwnerId = ownerId;
            Zoom = zoom;
        }
    }
}
