using PixelGlueCore.ECS.Components;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct CameraFollowTagComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public float Zoom { get; set; }

        public CameraFollowTagComponent(int ownerId, float zoom = 1)
        {
            UniqueId = ownerId;
            Zoom = zoom;
        }
    }
}
