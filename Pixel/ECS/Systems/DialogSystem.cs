using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class DialogSystem : PixelSystem
    {
        public override string Name { get; set; } = "Dialog System";
        public DialogSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, DrawableComponent>() && entity.Children != null)
            {
                foreach (var childId in entity.Children)
                {
                    ref var child = ref World.GetEntity(childId);
                    if (child.Has<TextComponent>())
                    {
                        base.AddEntity(entityId);
                        return;
                    }
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
        }
    }
}