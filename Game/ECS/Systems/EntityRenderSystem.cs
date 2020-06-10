using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Helpers;
using PixelGlueCore.Loaders.TiledSharp;

namespace PixelGlueCore.ECS.Systems
{
    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(float timeSinceLastFrame)
        {
        }public void FixedUpdate(float timeSinceLastFrame)
        {
        }
        public void Draw(Scene scene, SpriteBatch sb)
        {
            if (scene.Camera == null)
                return;

            var overdraw = scene.Map.TileWidth * 2;
            PixelGlue.RenderedObjects = 0;
            PixelGlue.RenderedObjects += TmxMapRenderer.Draw(sb, scene.Map, 0, scene.Camera);
            PixelGlue.RenderedObjects += TmxMapRenderer.Draw(sb, scene.Map, 1, scene.Camera);
            foreach (var (_, entity) in scene.Entities)
            {
                if (!entity.HasDrawableComponent() || !entity.HasPositionComponent())
                    continue;

                ref var pos = ref entity.GetPositionComponentRef();
                ref var drawable = ref entity.GetDrawableComponentRef();

                if (pos.Position.X < scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.Textures[drawable.TextureName], Vector2.Round(pos.Position), drawable.SrcRect, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.RenderedObjects++;
            }
            if (scene.Map?.Layers?.Count >= 2)
                PixelGlue.RenderedObjects += TmxMapRenderer.Draw(sb, scene.Map, 2, scene.Camera);
        }
    }
}