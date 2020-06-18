using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Systems
{
    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene {get;set;}

        public EntityRenderSystem(GameScene scene)
        {
            Scene=scene;
        }
        public void Update(float timeSinceLastFrame)
        {
        }
        public void FixedUpdate(float timeSinceLastFrame)
        {
        }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;

            var overdraw = Scene.Map.TileWidth * 2;
            var origin = new Vector2(8, 8);
            PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 0, Scene.Camera);
            PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 1, Scene.Camera);
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;
            }
            if (Scene.Map?.Layers?.Count >= 2)
                PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 2, Scene.Camera);
        }
    }
}