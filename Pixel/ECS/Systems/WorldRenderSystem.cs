using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.World;
using Shared;

namespace Pixel.ECS.Systems
{
    public class WorldRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "World Rendering System";
        public Point Overdraw = new Point(Global.TileSize*4,Global.TileSize*2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            for (int x = Scene.Camera.DrawRectZoomed.Left - Overdraw.X; x < Scene.Camera.DrawRectZoomed.Right + Overdraw.X; x += Global.TileSize)
                for (int y = Scene.Camera.DrawRectZoomed.Top - Overdraw.Y; y < Scene.Camera.DrawRectZoomed.Bottom + Overdraw.Y; y += Global.TileSize)
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