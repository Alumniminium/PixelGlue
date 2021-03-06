using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : PixelSystem<DbgBoundingBoxComponent>
    {
        public DbgBoundingBoxRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name = "Debug Boundingbox System"; }

        public unsafe override void Draw(SpriteBatch sb)
        {
            var pxl = AssetManager.GetTexture("pixel");
            var dbg = AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName);
            foreach (var entityList in Entities)
            for(int i =0; i< entityList.Count; i++)
            {
                ref var entity = ref entityList[i];
                if (entity.Has<CameraComponent>())
                {
                    ref readonly var cam = ref entity.Get<CameraComponent>();
                    const int lineWidth = 4;
                    Color color = Color.LightGreen;
                    sb.Draw(pxl, new Rectangle(cam.ScreenRect.X, cam.ScreenRect.Y, lineWidth, cam.ScreenRect.Height), color);
                    sb.Draw(pxl, new Rectangle(cam.ScreenRect.X, cam.ScreenRect.Y, cam.ScreenRect.Width, lineWidth), color);
                    sb.Draw(pxl, new Rectangle(cam.ScreenRect.X + cam.ScreenRect.Width - lineWidth, cam.ScreenRect.Y, lineWidth, cam.ScreenRect.Height), color);
                    sb.Draw(pxl, new Rectangle(cam.ScreenRect.X, cam.ScreenRect.Y + cam.ScreenRect.Height - lineWidth, cam.ScreenRect.Width, lineWidth), color);
                }
                if (entity.Has<DestinationComponent>())
                {
                    ref readonly var dst = ref entity.Get<DestinationComponent>();
                    var destRect = new Rectangle((int)dst.Value.X, (int)dst.Value.Y, Global.TileSize, Global.TileSize);
                    sb.Draw(dbg, destRect, DbgBoundingBoxComponent.SrcRect, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                if (entity.Has<PositionComponent, DrawableComponent>())
                {
                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    ref readonly var drw = ref entity.Get<DrawableComponent>();
                    var destRect = new Rectangle((int)pos.Value.X, (int)pos.Value.Y, drw.SrcRect.Width, drw.SrcRect.Height);
                    sb.Draw(dbg, destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else if (entity.Has<PositionComponent>())
                {
                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    var destRect = new Rectangle((int)pos.Value.X, (int)pos.Value.Y, Global.TileSize, Global.TileSize);
                    sb.Draw(dbg, destRect, DbgBoundingBoxComponent.SrcRect, Color.SeaGreen, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}