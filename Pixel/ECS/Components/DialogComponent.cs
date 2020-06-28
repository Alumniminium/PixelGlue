using Pixel.ECS.Systems;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct DialogComponent: IEntityComponent
    {
        public int EntityId {get;set;}
        public int Id;
        public int Stage;


        public DialogComponent(int ownerId,int id, int stage = 0)
        {
            EntityId=ownerId;
            Id = id;
            Stage =stage;
        }
    }
}
