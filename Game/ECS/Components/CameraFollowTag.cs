using PixelGlueCore.Enums;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct CameraFollowTagComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public float Zoom { get; set; }

        public CameraFollowTagComponent(int ownerId, int zoom)
        {
            UniqueId = ownerId;
            Zoom = zoom;
        }
    }
}
