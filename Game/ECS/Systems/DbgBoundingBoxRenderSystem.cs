using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Scenes;

namespace PixelGlueCore.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Update Rate Monitoring System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Draw(Scene scene, SpriteBatch sb)
        {
                if (scene.Camera == null)
                    return;
                foreach (var entity in scene.Entities)
                {
                    if (scene.TryGetComponent<DbgBoundingBoxComponent>(entity.Key, out var _))
                    {
                        if (scene.TryGetComponent<PositionComponent>(entity.Key, out var pos))
                        {
                            if (scene.TryGetDrawableComponent(entity.Key, out var drawable))
                            {
                                var destRect = new Rectangle((int)pos.IntegerPosition.X, (int)pos.IntegerPosition.Y, drawable.SrcRect.Width, drawable.SrcRect.Height);
                                sb.Draw(AssetManager.Textures[DbgBoundingBoxComponent.TextureName], destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
                            }
                        }
                }
            }
        }
    }
}