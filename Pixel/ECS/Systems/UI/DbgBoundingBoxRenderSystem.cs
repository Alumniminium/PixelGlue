using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared;
using Pixel.Entities;
using System.Runtime.CompilerServices;

namespace Pixel.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Debug Boundingbox System";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<DbgBoundingBoxComponent>())
                base.AddEntity(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            var camera = Scene.Camera;
            var rectangle = camera.SimulationRect;
            int lineWidth = 2;
            Color color = Color.LightGreen;

            sb.Draw(AssetManager.GetTexture("pixel"), new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            sb.Draw(AssetManager.GetTexture("pixel"), new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            sb.Draw(AssetManager.GetTexture("pixel"), new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            sb.Draw(AssetManager.GetTexture("pixel"), new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
                
            foreach (var entity in Entities)
            {
                if (entity.Has<DestinationComponent>())
                {
                    ref readonly var dst = ref entity.Get<DestinationComponent>();
                    var destRect = new Rectangle((int)dst.Value.X, (int)dst.Value.Y, Global.TileSize, Global.TileSize);
                    sb.Draw(AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName), destRect, DbgBoundingBoxComponent.SrcRect, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                if (entity.Has<DrawableComponent>() && entity.Has<PositionComponent>())
                {
                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    ref readonly var drw = ref entity.Get<DrawableComponent>();
                    var destRect = new Rectangle((int)pos.Value.X, (int)pos.Value.Y, drw.SrcRect.Width, drw.SrcRect.Height);
                    sb.Draw(AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName), destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
                 }
            }
        }
    }
}