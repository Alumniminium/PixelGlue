using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class NameTagRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Name Tag Render System";

        public NameTagRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<TextComponent, PositionComponent>() && entity.Parent != 0)
                base.AddEntity(entityId);
        }
        public override void Draw(SpriteBatch sb)
        {
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
            foreach (var id in Entities)
            {
                ref readonly var entity = ref World.GetEntity(id);
                ref readonly var parent = ref World.GetEntity(entity.Parent);

                ref readonly var parentPos = ref parent.Get<PositionComponent>();
                ref readonly var txt = ref entity.Get<TextComponent>();
                ref readonly var posOff = ref entity.Get<PositionComponent>();

                var textPos = parentPos.Value + posOff.Value;
                AssetManager.Fonts[txt.FontName].DrawText(sb, textPos.X, textPos.Y, txt.Value, Color.Blue, 1f);
            }
        }
    }
}