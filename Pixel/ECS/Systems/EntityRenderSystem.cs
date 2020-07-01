using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
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
        public Scene Scene => SceneManager.ActiveScene;
        public List<Entity> Entities;
        public ProceduralEntityRenderSystem()
        {
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
            for (int x = Scene.Camera.ServerScreenRect.Left; x < Scene.Camera.ServerScreenRect.Right; x += Global.TileSize)
                for (int y = Scene.Camera.ServerScreenRect.Top ; y < Scene.Camera.ServerScreenRect.Bottom; y += Global.TileSize)
                {
                    var (terrainTile, riverTile) = WorldGen.GetTiles(x, y);

                    if (terrainTile.HasValue)
                    {
                        sb.Draw(terrainTile.Value.Texture, terrainTile.Value.DestRect, terrainTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                        Global.DrawCalls++;
                    }
                    if (riverTile.HasValue && terrainTile.HasValue && !(terrainTile.Value.TextureName == "water" || terrainTile.Value.TextureName == "shallow_water" || terrainTile.Value.TextureName == "deep_water"))
                    {
                        sb.Draw(riverTile.Value.Texture, riverTile.Value.DestRect, riverTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.8f);
                        Global.DrawCalls++;
                    }
                }
            RenderEntities(sb);
        }
        private void RenderEntities(SpriteBatch sb)
        {
            var origin = new Vector2(Global.TileSize/2);
            foreach (var entity in Entities)
            {
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ServerScreenRect.Left || pos.Position.X > Scene.Camera.ServerScreenRect.Right)
                    Scene.Destroy(entity);
                if (pos.Position.Y < Scene.Camera.ServerScreenRect.Top || pos.Position.Y > Scene.Camera.ServerScreenRect.Bottom)
                    Scene.Destroy(entity);

                sb.Draw(drawable.Texture, pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }
    }
}