using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Components
{
    public struct DialogComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public int Id;
        public int Stage;


        public DialogComponent(int ownerId,int id, int stage = 0)
        {
            UniqueId=ownerId;
            Id = id;
            Stage =stage;
        }
    }
}
