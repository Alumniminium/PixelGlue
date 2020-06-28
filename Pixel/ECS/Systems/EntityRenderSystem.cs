using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using System.Threading;

namespace Pixel.ECS.Systems
{
    public class ProceduralEntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Procedural Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Scene { get; set; }
        public ProceduralEntityRenderSystem(Scene scene)
        {
            Scene = scene;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }
        public void Update(float timeSinceLastFrame) { }
        public void FixedUpdate(float timeSinceLastFrame) { }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;
            var overdraw = PixelShared.Pixel.TileSize*2;
            for (int x = Scene.Camera.ScreenRect.Left - overdraw; x < Scene.Camera.ScreenRect.Right + overdraw; x += PixelShared.Pixel.TileSize)
            for (int y = Scene.Camera.ScreenRect.Top - overdraw; y < Scene.Camera.ScreenRect.Bottom + overdraw; y += PixelShared.Pixel.TileSize)
            {
                var (terrainTile,riverTile) = WorldGen.GetTiles(x,y);
                
                if (terrainTile.HasValue)
                {
                    sb.Draw(AssetManager.GetTexture(terrainTile.Value.TextureName), terrainTile.Value.DestRect, terrainTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                    Global.DrawCalls++;
                }        
                if (riverTile.HasValue && terrainTile.HasValue && !(terrainTile.Value.TextureName == "water" || terrainTile.Value.TextureName == "shallow_water" || terrainTile.Value.TextureName == "deep_water"))
                {
                    sb.Draw(AssetManager.GetTexture(riverTile.Value.TextureName), riverTile.Value.DestRect, riverTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.8f);
                    Global.DrawCalls++;
                }
            }
            RenderEntities(sb, overdraw);
        }
        private void RenderEntities(SpriteBatch sb, int overdraw)
        {
            var origin = new Vector2(-8, 8);
            foreach (var entity in CompIter.Get<DrawableComponent,PositionComponent>())
            {
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }
    }
}