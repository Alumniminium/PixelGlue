using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;

namespace Pixel.ECS.Systems
{
    public class NameTagRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Name Tag Render System";
        public override void AddEntity(Entity entity)
        {
            if(!entity.Has<PositionComponent>())
                return;
            foreach(var child in entity.Children)
            {
                if (child.Has<TextComponent>() || child.Has<PositionComponent>())
                {
                    base.AddEntity(entity);
                    break;
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            foreach (var entity in Entities)
            {                    
                foreach (var child in entity.Children)
                {
                    if (!child.Has<TextComponent>() || !child.Has<PositionComponent>())
                        continue;

                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    if (pos.Value.X < Scene.Camera.ServerScreenRect.Left || pos.Value.X > Scene.Camera.ServerScreenRect.Right)
                        continue;
                    if (pos.Value.Y < Scene.Camera.ServerScreenRect.Top || pos.Value.Y > Scene.Camera.ServerScreenRect.Bottom)
                        continue;

                    ref readonly var offset = ref child.Get<PositionComponent>();
                    ref readonly var text = ref child.Get<TextComponent>();

                    if (!string.IsNullOrEmpty(text.Text))
                    {
                        var p = pos.Value + offset.Value;
                        AssetManager.Fonts[text.FontName].DrawText(sb, (int)p.X, (int)p.Y, text.Text,1f);
                    }
                }
            }
        }
    }
}