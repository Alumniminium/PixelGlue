using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS
{
    public class ComponentCollection
    {
        public int DrawablesCount, PositionsCount, MovesCount, DbgBoundingBoxCount, InputComponentsCount, NetworkedsCount, CameraFollowTagsCount;
        public DrawableComponent[] DrawableComponents;
        public PositionComponent[] PositionComponents;
        public InputComponent[] InputComponents;
        public Networked[] Networkeds;
        public CameraFollowTagComponent[] CameraFollowTags;
        public MoveComponent[] MoveComponents;
        public DbgBoundingBoxComponent[] DbgBoundingBoxComponents;

        public ComponentCollection()
        {
            DrawableComponents = new DrawableComponent[0];
            PositionComponents = new PositionComponent[0];
            DbgBoundingBoxComponents = new DbgBoundingBoxComponent[0];
            MoveComponents = new MoveComponent[0];
            Networkeds = new Networked[0];
            InputComponents = new InputComponent[0];
            CameraFollowTags = new CameraFollowTagComponent[0];
        }
    }
}