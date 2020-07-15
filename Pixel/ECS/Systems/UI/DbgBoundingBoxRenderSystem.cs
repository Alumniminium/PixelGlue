using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared;
using System.Runtime.CompilerServices;
using Shared.ECS;
using Pixel.Scenes;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class DbgBoundingBoxRenderSystem : PixelSystem
    {
        public DbgBoundingBoxRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Debug Boundingbox System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<DbgBoundingBoxComponent>())
                base.AddEntity(entityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            var camera = Scene.Camera;
            //var rectangle = camera.SimulationRect;
            var rectangle = camera.WorldBounds();
            const int lineWidth = 4;
            Color color = Color.LightGreen;

            var pxl = AssetManager.GetTexture("pixel");
            sb.Draw(pxl, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height), color);
            sb.Draw(pxl, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, lineWidth), color);
            sb.Draw(pxl, new Rectangle(rectangle.X + rectangle.Width-lineWidth, rectangle.Y, lineWidth, rectangle.Height), color);
            sb.Draw(pxl, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height-lineWidth, rectangle.Width, lineWidth), color);
                
            var dbgTexture = AssetManager.GetTexture(DbgBoundingBoxComponent.TextureName);
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                if (entity.Has<DestinationComponent>())
                {
                    ref readonly var dst = ref entity.Get<DestinationComponent>();
                    var destRect = new Rectangle((int)dst.Value.X, (int)dst.Value.Y, Global.TileSize, Global.TileSize);
                    sb.Draw(dbgTexture, destRect, DbgBoundingBoxComponent.SrcRect, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                if (entity.Has<DrawableComponent>() && entity.Has<PositionComponent>())
                {
                    ref readonly var pos = ref entity.Get<PositionComponent>();
                    ref readonly var drw = ref entity.Get<DrawableComponent>();
                    var destRect = new Rectangle((int)pos.Value.X, (int)pos.Value.Y, drw.SrcRect.Width, drw.SrcRect.Height);
                    sb.Draw(dbgTexture, destRect, DbgBoundingBoxComponent.SrcRect, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
                 }
            }
        }
    }
}