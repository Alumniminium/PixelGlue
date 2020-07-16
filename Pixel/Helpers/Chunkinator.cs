using System.Threading;
using Microsoft.Xna.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Pixel.Helpers
{
    public static class Chunkinator
    {
        public const int PRELOAD_DISTANCE = 3;
        public const int CHUNK_SIZE = 32;
        public static Dictionary<Point, Chunk> Chunks = new Dictionary<Point, Chunk>();

        public static Thread ChunkLoader = new Thread(LoadLoop);
        public static ConcurrentStack<Point> PointsToGenerate = new ConcurrentStack<Point>();
        public static AutoResetEvent Block = new AutoResetEvent(false);

        static Chunkinator()
        {
            ChunkLoader.Name = "Chunkloader";
            ChunkLoader.Priority = ThreadPriority.Highest;
            ChunkLoader.Start();
        }
        public static void QueueChunk(Point point)
        {
            PointsToGenerate.Push(point);
            Block.Set();
        }
        private static void LoadLoop()
        {
            while (true)
            {
                Block.WaitOne();
                while (PointsToGenerate.TryPop(out var xy))
                {
                    if(Chunks.TryGetValue(xy,out var chunk))
                    {
                    for (int tx = 0; tx < CHUNK_SIZE; tx++)
                        for (int ty = 0; ty < CHUNK_SIZE; ty++)
                        {
                            if (chunk.Tiles[tx] == null)
                                chunk.Tiles[tx] = new Tile[CHUNK_SIZE];

                            var wx = xy.X + (tx * Global.TileSize);
                            var wy = xy.Y + (ty * Global.TileSize);

                            chunk.Tiles[tx][ty] = WorldGen.GetTile(wx, wy);
                            Thread.Yield();
                        }
                    }
                }
            }
        }

        public static Chunk GetChunk(int x, int y)
        {
            var cx = x / (CHUNK_SIZE * Global.TileSize);
            var cy = y / (CHUNK_SIZE * Global.TileSize);
            var cxy = new Point(x,y);
            
            if (!Chunks.TryGetValue(cxy, out var chunk))
            {
                chunk = new Chunk(cx, cy, CHUNK_SIZE);
                QueueChunk(cxy);
                Chunks.Add(cxy, chunk);
            }
            return chunk;
        }

        public static Tile GetTile(int x, int y)
        {
            var chunk = GetChunk(x, y);
            var tx = Math.Abs(x % CHUNK_SIZE);
            var ty = Math.Abs(y % CHUNK_SIZE);
            var tile = chunk.Tiles[tx]?[ty];
            if (tile != null)
                tile.Dst = new Rectangle(x, y, Global.TileSize, Global.TileSize);
            return tile;
        }
    }
}