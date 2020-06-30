using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.World;
using PixelShared;
using System.Collections.Generic;
using System.Threading;

namespace Pixel.ECS.Systems
{
    public class ProceduralEntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Procedural Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Scene { get; set; }
        public List<Entity> Entities;
        public ProceduralEntityRenderSystem(Scene scene)
        {
            Scene = scene;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }
        public void Update(float deltaTime) 
        {
            Entities = CompIter<DrawableComponent, PositionComponent>.Get(deltaTime);
        }
        public void FixedUpdate(float timeSinceLastFrame) { }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;
            var overdraw = Global.TileSize * 4;
            for (int x = Scene.Camera.ScreenRect.Left - overdraw; x < Scene.Camera.ScreenRect.Right + overdraw; x += Global.TileSize)
                for (int y = Scene.Camera.ScreenRect.Top - overdraw; y < Scene.Camera.ScreenRect.Bottom + overdraw; y += Global.TileSize)
                {
                    var (terrainTile, riverTile) = WorldGen.GetTiles(x, y);

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
            var origin = new Vector2(8, 8);
            foreach (var entity in Entities)
            {
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ServerScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ServerScreenRect.Right + overdraw)
                    Scene.Destroy(entity);
                if (pos.Position.Y < Scene.Camera.ServerScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ServerScreenRect.Bottom + overdraw)
                    Scene.Destroy(entity);

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }
    }
}