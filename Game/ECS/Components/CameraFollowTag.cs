using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public struct CameraFollowTagComponent
    {
        public float Zoom { get; set; }

        public CameraFollowTagComponent(float zoom = 1)
        {
            Zoom = zoom;
        }
    }
}
