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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            var overdraw = Global.TileSize * 4;

            for (int x = Scene.Camera.DrawRectZoomed.Left - overdraw; x < Scene.Camera.DrawRectZoomed.Right + overdraw; x += Global.TileSize)
                for (int y = Scene.Camera.DrawRectZoomed.Top - overdraw; y < Scene.Camera.DrawRectZoomed.Bottom + overdraw; y += Global.TileSize)
                {
                    var (terrainTile, riverTile) = WorldGen.GetTiles(x, y);

                    if (terrainTile.HasValue)
                    {
                        sb.Draw(AssetManager.GetTexture(terrainTile.Value.TextureName), terrainTile.Value.DestRect, terrainTile.Value.SrcRect, terrainTile.Value.Color, 0, Vector2.Zero, SpriteEffects.None,0);
                    }
                    if (riverTile.HasValue && terrainTile.HasValue && !(terrainTile.Value.TextureName == "water" || terrainTile.Value.TextureName == "shallow_water" || terrainTile.Value.TextureName == "deep_water"))
                    {
                        sb.Draw(AssetManager.GetTexture(riverTile.Value.TextureName), riverTile.Value.DestRect, riverTile.Value.SrcRect, riverTile.Value.Color, 0, Vector2.Zero, SpriteEffects.None,0);
                   }
                }
        }
    }
}