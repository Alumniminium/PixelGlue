using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using PixelShared;

namespace Pixel.ECS.Systems
{

    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Scene => SceneManager.ActiveScene;
        public void Update(float deltaTime) 
        {
        }
        public void FixedUpdate(float timeSinceLastFrame) { }
        public void Draw(SpriteBatch sb)
        {
            var overdraw = Global.TileSize * 4;
            var origin = new Vector2(8);
            foreach (var entityId in CompIter<DrawableComponent, PositionComponent>.Get())
            {
                ref readonly var pos = ref ComponentArray<PositionComponent>.Get(entityId);
                ref readonly var drawable = ref ComponentArray<DrawableComponent>.Get(entityId);

                if (pos.Position.X < Scene.Camera.ServerScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ServerScreenRect.Right + overdraw)
                    Scene.Destroy(entityId);
                if (pos.Position.Y < Scene.Camera.ServerScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ServerScreenRect.Bottom + overdraw)
                    Scene.Destroy(entityId);

                sb.Draw(drawable.Texture, pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }
    }
}