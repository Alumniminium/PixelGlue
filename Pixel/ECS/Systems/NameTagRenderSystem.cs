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
        public List<Entity> Entities{ get; set; }

        public void FixedUpdate(float deltaTime) { }
        public void Update(float deltaTime) 
        {
            Entities = CompIter<PositionComponent>.Get(deltaTime);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var entity in Entities)
            {
                foreach (var child in entity.Children)
                {
                    if (!child.Has<TextComponent>() || !child.Has<PositionComponent>())
                        continue;

                    ref readonly var pos = ref entity.Get<PositionComponent>();
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