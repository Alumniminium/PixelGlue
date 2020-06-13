using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.Helpers
{
    public class ComponentCollection
    {
        public ComponentList<DrawableComponent> DrawableComponents;
        public ComponentList<PositionComponent> PositionComponents;
        public ComponentList<InputComponent> InputComponents;
        public ComponentList<Networked> Networkeds;
        public ComponentList<CameraFollowTagComponent> CameraFollowTags;
        public ComponentList<MoveComponent> MoveComponents;
        public ComponentList<DbgBoundingBoxComponent> DbgBoundingBoxComponents;
        public ComponentList<TextComponent> TextComponents;

        public ComponentCollection()
        {
            DrawableComponents = new ComponentList<DrawableComponent>(0);
            PositionComponents = new ComponentList<PositionComponent>(0);
            DbgBoundingBoxComponents = new ComponentList<DbgBoundingBoxComponent>(0);
            MoveComponents = new ComponentList<MoveComponent>(0);
            Networkeds = new ComponentList<Networked>(0);
            InputComponents = new ComponentList<InputComponent>(0);
            CameraFollowTags = new ComponentList<CameraFollowTagComponent>(0);
            TextComponents = new ComponentList<TextComponent>(0);
        }
    }
}