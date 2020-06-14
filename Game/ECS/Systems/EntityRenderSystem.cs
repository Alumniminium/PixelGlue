using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Loaders.TiledSharp;

namespace PixelGlueCore.ECS.Systems
{
    public class NameTagRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Name Tag Render System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float deltaTime) { }
        public void Update(float deltaTime) { }

        public void Draw(Scene scene, SpriteBatch sb)
        {
            foreach (var (_, entity) in scene.Entities)
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
    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(float timeSinceLastFrame)
        {
        }
        public void FixedUpdate(float timeSinceLastFrame)
        {
        }
        public void Draw(Scene scene, SpriteBatch sb)
        {
            if (scene.Camera == null)
                return;

            var overdraw = scene.Map.TileWidth * 2;
            var origin = new Vector2(8, 8);
            PixelGlue.RenderedObjects = 0;
            PixelGlue.RenderedObjects += TmxMapRenderer.Draw(sb, scene.Map, 0, scene.Camera);
            PixelGlue.RenderedObjects += TmxMapRenderer.Draw(sb, scene.Map, 1, scene.Camera);
            foreach (var (_, entity) in scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.Textures[drawable.TextureName], pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.RenderedObjects++;
            }
            if (scene.Map?.Layers?.Count >= 2)
                PixelGlue.RenderedObjects += TmxMapRenderer.Draw(sb, scene.Map, 2, scene.Camera);
        }
    }
}