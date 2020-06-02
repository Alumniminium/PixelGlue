using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class CameraFollowTagComponent : IEntityComponent
    {
        public float Zoom { get; set; }
        public CameraFollowTagComponent(float zoom = 1)
        {
            Zoom = zoom;
        }
    }
}
