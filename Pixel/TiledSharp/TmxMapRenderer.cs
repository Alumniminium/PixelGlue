using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;
using Microsoft.Xna.Framework.Content;

namespace Pixel.TiledSharp
{
    //public static class TmxMapRenderer
    //{
    //    public static TmxMap Load(string path)
    //    {
    //        var map = new TmxMap(path);
    //        var tileSize = map.Tilesets[0].TileWidth;
    //        for (int i = 0; i < map.Tilesets.Count; i++)
    //            AssetManager.LoadTexture(map.Tilesets[i].Name);
//
    //        map.TileArray = new DrawableComponent[map.TileLayers.Count][];
    //        for (int i = 0; i < map.TileArray.Length; i++)
    //            map.TileArray[i] = new DrawableComponent[map.Width * map.Height];
//
    //        var tilesetTilesWide = AssetManager.Textures[map.Tilesets[0].Name].Width / tileSize;
    //        //var tilesetTilesHigh = AssetManager.Textures[map.Tilesets[0].Name].Height / tileSize;
//
    //        for (int c = 0; c < map.TileLayers.Count; c++)
    //        {
    //            for (var i = 0; i < map.TileLayers[c].Tiles.Count; i++)
    //            {
    //                int gid = map.TileLayers[c].Tiles[i].Gid;
    //                if (gid == 0)
    //                    continue;
//
    //                int tileFrame = gid - 1;
    //                int column = tileFrame % tilesetTilesWide;
    //                int row = tileFrame/ tilesetTilesWide;
//
    //                int x = i % map.Width;
    //                int y = i / map.Width;
//
    //                Rectangle tilesetRec = new Rectangle(tileSize * column, tileSize * row, tileSize, tileSize);
    //                map.TileArray[c][(y * map.Width) + x] = new DrawableComponent(0,map.Tilesets[0].Name, tilesetRec, new Rectangle(x*tileSize,y*tileSize,tileSize,tileSize));
    //            }
    //        }
    //        map.ImageLayers=null;
    //        map.TileLayers=null;
    //        return map;
    //    }
    //    public static int Draw(SpriteBatch sb, TmxMap map, int l, Camera cam)
    //    {
    //        int counter = 0;
    //        var origin = new Vector2(0, map.TileHeight / 2);
    //        var overdraw = map.TileWidth * 2;
//
    //        for (int x = cam.ScreenRect.Left - overdraw; x < cam.ScreenRect.Right + overdraw; x += map.TileWidth)
    //        {
    //            for (int y = cam.ScreenRect.Top - overdraw; y < cam.ScreenRect.Bottom + overdraw; y += map.TileHeight)
    //            {
    //                var index = (y / map.TileWidth * map.Width) + (x / map.TileHeight);
    //                if (index >= map.TileArray[l].Length || index < 0)
    //                    continue;
//
    //                var tile = map.TileArray[l][index];
    //                if (string.IsNullOrEmpty(tile.TextureName))
    //                    continue;
    //                counter++;
    //                sb.Draw(AssetManager.GetTexture(map.Tilesets[0].Name), tile.DestRect, tile.SrcRect, Color.White, 0f,origin, SpriteEffects.None, 0);
    //            }
    //        }
    //        return counter;
    //    }
    //}
}