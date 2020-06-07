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

        public void Draw(Scene scene, SpriteBatch sb)
        {
            if (scene.Camera == null)
                return;
            foreach (var kvp in scene.Entities)
            {
                if(!kvp.Value.HasPositionComponent() || !kvp.Value.HasDrawableComponent())
                    continue;

                ref var pos = ref scene.GetPositionComponentRef(kvp.Key);
                ref var drawable = ref scene.GetDrawableComponentRef(kvp.Key);
                
                var destRect = new Rectangle((int)pos.Position.X, (int)pos.Position.Y, drawable.SrcRect.Width, drawable.SrcRect.Height);
                sb.Draw(AssetManager.Textures[DbgBoundingBoxComponent.TextureName], destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);

            }
        }
    }
}