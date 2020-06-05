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

        public void Update(double timeSinceLastFrame)
        {

        }
        public void Draw(SpriteBatch sb)
        {
            foreach (var scene in SceneManager.ActiveScenes)
            {
                if(scene.Camera==null)continue;
                sb.Begin(transformMatrix: scene.Camera.Transform, samplerState: SamplerState.PointClamp);
                foreach (var entity in scene.Entities)
                {
                    if (scene.TryGetComponent<DbgBoundingBoxComponent>(entity.Key, out var box))
                    {
                        if (scene.TryGetComponent<PositionComponent>(entity.Key, out var pos))
                        {
                            if (scene.TryGetComponent<DrawableComponent>(entity.Key, out var drawable))
                            {
                                var destRect = new Rectangle((int)pos.IntegerPosition.X, (int)pos.IntegerPosition.Y, drawable.SrcRect.Width, drawable.SrcRect.Height);
                                sb.Draw(AssetManager.Textures[box.TextureName], destRect, box.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
                            }
                        }
                    }
                }
                sb.End();
            }
        }
    }
}