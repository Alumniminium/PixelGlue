using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Update Rate Monitoring System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Draw(Scene scene, SpriteBatch sb)
        {
            if (scene.Camera == null)
                return;
            foreach (var (_, entity) in scene.Entities)
            {
                if (!entity.Has<PositionComponent>() || !entity.Has<DrawableComponent>())
                    continue;

                ref var pos = ref entity.Get<PositionComponent>();
                ref var drawable = ref entity.Get<DrawableComponent>();

                var destRect = new Rectangle((int)pos.Position.X, (int)pos.Position.Y, drawable.SrcRect.Width, drawable.SrcRect.Height);
                sb.Draw(AssetManager.Textures[DbgBoundingBoxComponent.TextureName], destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }

        public void FixedUpdate(float deltaTime)
        {
        }

        public void Update(float deltaTime)
        {
        }
    }
}