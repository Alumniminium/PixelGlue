using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public class DialogComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        private int stage;


        public int Id;
        public int Stage
        {
            get => stage;
            set
            {
                stage = value;
                UpdateRequired = true;
            }
        }

        public bool UpdateRequired=true;

        public DialogComponent(int ownerId,int id, int stage = 0)
        {
            UniqueId=ownerId;
            Id = id;
            Stage = stage;
        }
    }
}
