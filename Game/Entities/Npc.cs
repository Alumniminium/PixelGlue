using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore
{
    public class Npc : GameObject
    {
        public Npc(BaseEntity baseEntity, int x, int y)
        {
            AddComponent(new PositionComponent(x, y, 0));
            AddComponent(new MoveComponent(50, x, y));
            AddComponent(new DrawableComponent(baseEntity.TextureName, baseEntity.SrcRect));
        }
    }
}