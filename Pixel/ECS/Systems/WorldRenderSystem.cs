using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Helpers;
using Pixel.Scenes;
using Shared;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class WorldRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "World Rendering System";
        public Point Overdraw = new Point(Global.TileSize*4,Global.TileSize*2);

        public WorldRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
           for (int x = Scene.Camera.Bounds.Left - Overdraw.X; x < Scene.Camera.Bounds.Right + Overdraw.X; x += Global.TileSize)
                for (int y = Scene.Camera.Bounds.Top - Overdraw.Y; y < Scene.Camera.Bounds.Bottom + Overdraw.Y; y += Global.TileSize)
                {
                    var (terrainTile, riverTile) = WorldGen.GetTiles(x, y);

                    if (terrainTile.HasValue)
                        sb.Draw(AssetManager.GetTexture(terrainTile.Value.TextureName), terrainTile.Value.DestRect, terrainTile.Value.SrcRect, terrainTile.Value.Color, 0, Vector2.Zero, SpriteEffects.None,0);
                    if (riverTile.HasValue && terrainTile.HasValue && !(terrainTile.Value.TextureName == "water" || terrainTile.Value.TextureName == "shallow_water" || terrainTile.Value.TextureName == "deep_water"))
                        sb.Draw(AssetManager.GetTexture(riverTile.Value.TextureName), riverTile.Value.DestRect, riverTile.Value.SrcRect, riverTile.Value.Color, 0, Vector2.Zero, SpriteEffects.None,0);
                }
        }
    }
}