using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Update Rate Monitoring System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene{get;set;}
        public DbgBoundingBoxRenderSystem(GameScene scene)
        {
            Scene=scene;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;
            var origin = new Vector2(0, 8);
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DbgBoundingBoxComponent>() || !entity.Has<PositionComponent>() || !entity.Has<DrawableComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();
                var a = Vector2.Floor(pos.Position/ 16);
                var b =  Vector2.Round(a* 16)+origin;
                
                var destRect = new Rectangle((int)b.X, (int)b.Y, drawable.SrcRect.Width, drawable.SrcRect.Height);
                sb.Draw(AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName), destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0,Vector2.Zero, SpriteEffects.None, 0);
                PixelGlue.DrawCalls++;
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