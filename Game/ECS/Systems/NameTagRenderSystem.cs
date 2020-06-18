using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Systems
{
    public class NameTagRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Name Tag Render System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene {get;set;}
        public NameTagRenderSystem(GameScene scene)
        {
            Scene=scene;
        }

        public void FixedUpdate(float deltaTime) { }
        public void Update(float deltaTime) { }

        public void Draw(SpriteBatch sb)
        {
            foreach (var (_, entity) in Scene.Entities)
            {
                foreach (var child in entity.Children)
                {
                    if (!child.Has<TextComponent>() || !child.Has<PositionComponent>() || !entity.Has<PositionComponent>())
                        continue;

                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    ref readonly var offset = ref child.Get<PositionComponent>();
                    ref readonly var text = ref child.Get<TextComponent>();

                    if (!string.IsNullOrEmpty(text.Text))
                    {
                        AssetManager.Fonts[text.FontName].DrawText(sb, 
                            (int)(pos.Position.X+offset.Position.X), 
                            (int)(pos.Position.Y+offset.Position.Y), text.Text);
                    }
                }
            }
        }
    }
}