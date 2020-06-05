using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Helpers;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.Scenes;

namespace PixelGlueCore.ECS.Systems
{
    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(double timeSinceLastFrame)
        {

        }
        public void Draw(Scene scene, SpriteBatch sb)
        {
            if (scene.Camera == null)
                return;

            int renderedObjectsCounter = 0;
            var overdraw = scene.Map.TileWidth * 2;

            renderedObjectsCounter += TmxMapRenderer.Draw(sb, scene.Map, 0, scene.Camera);
            renderedObjectsCounter += TmxMapRenderer.Draw(sb, scene.Map, 1, scene.Camera);
            foreach (var kvp in scene.Entities)
            {
                if (!scene.TryGetComponent<DrawableComponent>(kvp.Key, out var drawable))
                    continue;
                if (!scene.TryGetComponent<PositionComponent>(kvp.Key, out var pos))
                    continue;

                if (pos.Position.X < scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.Textures[drawable.TextureName], pos.Position.DrawablePosition(), drawable.SrcRect, Color.White, 0f, Vector2.Zero,Vector2.One, SpriteEffects.None, 0f);
                renderedObjectsCounter++;
            }
            renderedObjectsCounter += TmxMapRenderer.Draw(sb, scene.Map, 2, scene.Camera);

        }
    }
}