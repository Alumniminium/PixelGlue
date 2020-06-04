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
        public bool UpdateRequired=true;

        public DialogComponent(int id, int stage = 0)
        {
            Id = id;
            Stage = stage;
        }
    }
}
