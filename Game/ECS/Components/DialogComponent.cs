namespace PixelGlueCore.ECS.Components
{
    public class DialogComponent : IEntityComponent
    {
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

        public int PixelOwnerId { get; set; }

        public bool UpdateRequired=true;

        public DialogComponent(int ownerId,int id, int stage = 0)
        {
            PixelOwnerId=ownerId;
            Id = id;
            Stage = stage;
        }
    }
}
