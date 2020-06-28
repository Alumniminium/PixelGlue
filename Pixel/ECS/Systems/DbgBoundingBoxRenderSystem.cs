using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Debug Boundingbox System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Scene{get;set;}
        public DbgBoundingBoxRenderSystem(Scene scene)
        {
            Scene=scene;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;
            var origin = new Vector2(0, 0);
            foreach (var entity in CompIter.Get<PositionComponent,DrawableComponent>())
            {                
                ref readonly var pos  = ref entity.Get<PositionComponent>();
                ref readonly var drw = ref entity.Get<DrawableComponent>();
                var destRect = new Rectangle((int)pos.Position.X, (int)pos.Position.Y, drw.SrcRect.Width, drw.SrcRect.Height);
                sb.Draw(AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName), destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0,origin, SpriteEffects.None, 0);
                Global.DrawCalls++;
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