using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using System.Collections.Generic;

namespace Pixel.ECS.Systems
{
    public class NameTagRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Name Tag Render System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Scene => SceneManager.ActiveScene;

        public void FixedUpdate(float deltaTime) { }
        public void Update(float deltaTime) 
        {
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var entity in CompIter<PositionComponent>.Get())
            {
                if(!Scene.Entities.TryGetValue(entity,out var sub))
                    continue;
                    
                foreach (var child in sub.Children)
                {
                    if (!child.Has<TextComponent>() || !child.Has<PositionComponent>())
                        continue;

                    ref readonly var pos = ref ComponentArray<PositionComponent>.Get(entity);
                    if (pos.Position.X < Scene.Camera.ServerScreenRect.Left || pos.Position.X > Scene.Camera.ServerScreenRect.Right)
                        continue;
                    if (pos.Position.Y < Scene.Camera.ServerScreenRect.Top || pos.Position.Y > Scene.Camera.ServerScreenRect.Bottom)
                        continue;

                    ref readonly var offset = ref child.Get<PositionComponent>();
                    ref readonly var text = ref child.Get<TextComponent>();

                    if (!string.IsNullOrEmpty(text.Text))
                    {
                        var p = pos.Position + offset.Position;
                        AssetManager.Fonts[text.FontName].DrawText(sb, (int)p.X, (int)p.Y, text.Text);
                    }
                }
            }
        }
    }
}