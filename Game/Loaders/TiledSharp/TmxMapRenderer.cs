using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;
using PixelGlueCore.ECS;
using Microsoft.Xna.Framework.Content;

namespace PixelGlueCore.Loaders.TiledSharp
{
    public static class TmxMapRenderer
    {
        public static TmxMap Load(string path,ContentManager _contentManager)
        {
            var map = new TmxMap(path);
            var tileSize = map.Tilesets[0].TileWidth;
            for (int i = 0; i < map.Tilesets.Count; i++)
                AssetManager.LoadTexture(map.Tilesets[i].Name, _contentManager);

            map.TileArray = new DrawableComponent[map.TileLayers.Count][];
            for (int i = 0; i < map.TileArray.Length; i++)
                map.TileArray[i] = new DrawableComponent[map.Width * map.Height * 10];

            var tilesetTilesWide = AssetManager.Textures[map.Tilesets[0].Name].Width / tileSize;
            var tilesetTilesHigh = AssetManager.Textures[map.Tilesets[0].Name].Height / tileSize;

            for (int c = 0; c < map.TileLayers.Count; c++)
            {
                for (var i = 0; i < map.TileLayers[c].Tiles.Count; i++)
                {
                    int gid = map.TileLayers[c].Tiles[i].Gid;
                    if (gid == 0)
                        continue;

                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor(tileFrame / (double)tilesetTilesWide);

                    int x = (i % map.Width);
                    int y = (int)Math.Floor(i / (double)map.Width);

                    Rectangle tilesetRec = new Rectangle(tileSize * column, tileSize * row, tileSize, tileSize);
                   // FConsole.WriteLine($"{x * map.Width + y}/{map.TileArray[c].Length}");
                    map.TileArray[c][x * map.Width + y] = new DrawableComponent(map.Tilesets[0].Name, tilesetRec);
                }
            }
            map.Layers = null;
            map.ImageLayers=null;
            map.TileLayers=null;
            return map;
        }
        public static int Draw(SpriteBatch sb, TmxMap map, int l, Camera cam)
        {
            int counter = 0;
            var origin = new Vector2(0, map.TileHeight / 2);
            var overdraw = map.TileWidth * 2;

            for (int x = cam.ScreenRect.Left - overdraw; x < cam.ScreenRect.Right + overdraw; x += map.TileWidth)
            {
                for (int y = cam.ScreenRect.Top - overdraw; y < cam.ScreenRect.Bottom + overdraw; y += map.TileHeight)
                {
                    var index = x / map.TileWidth * map.Width + y / map.TileHeight;
                    if (index >= map.TileArray[l].Length || index < 0)
                        continue;

                    var tile = map.TileArray[l][index];
                    if (tile == null)
                        continue;
                    if (string.IsNullOrEmpty(tile.TextureName))
                        continue;
                    counter++;
                    sb.Draw(AssetManager.Textures[map.Tilesets[0].Name], new Vector2(x, y), tile.SrcRect, Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0);
                }
            }
            return counter;
        }
    }
}