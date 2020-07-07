using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared;
using Pixel.Entities;

namespace Pixel.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Debug Boundingbox System";

        public override void AddEntity(Entity entity)
        {
            if (entity.Has<DbgBoundingBoxComponent>())
                base.AddEntity(entity);
        }
        public override void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;
            var origin = new Vector2(0, 0);
            foreach (var entity in Entities)
            {
                if(entity.Has<DestinationComponent>())
                {
                    ref readonly var dst = ref entity.Get<DestinationComponent>();
                    var destRect = new Rectangle((int)dst.Value.X, (int)dst.Value.Y, Global.TileSize, Global.TileSize);
                    sb.Draw(AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName), destRect, DbgBoundingBoxComponent.SrcRect, Color.Blue, 0, origin, SpriteEffects.None, 0);
                    Global.DrawCalls++;
                }
                if(entity.Has<DrawableComponent>() && entity.Has<PositionComponent>())
                {
                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    ref readonly var drw = ref entity.Get<DrawableComponent>();
                    var destRect = new Rectangle((int)pos.Value.X, (int)pos.Value.Y, drw.SrcRect.Width, drw.SrcRect.Height);
                    sb.Draw(AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName), destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, origin, SpriteEffects.None, 0);
                    Global.DrawCalls++;
                }
            }
        }
    }
}