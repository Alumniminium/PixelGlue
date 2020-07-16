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
        public const int CHUNK_SIZE = 512;
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
                while (PointsToGenerate.TryPop(out var cxy))
                {
                    if(Chunks.TryGetValue(cxy,out var chunk))
                    {
                    for (int tx = 0; tx < CHUNK_SIZE; tx++)
                        for (int ty = 0; ty < CHUNK_SIZE; ty++)
                        {
                            if (chunk.Tiles[tx] == null)
                                chunk.Tiles[tx] = new Tile[CHUNK_SIZE];

                            var wx = chunk.X + (tx * Global.TileSize);
                            var wy = chunk.Y + (ty * Global.TileSize);

                            chunk.Tiles[tx][ty] = WorldGen.GetTile(wx, wy);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public static Chunk GetChunk(int x, int y)
        {
            var cx = x / CHUNK_SIZE;
            var cy = y / CHUNK_SIZE;
            var cxy = new Point(cx,cy);
            
            if (!Chunks.TryGetValue(cxy, out var chunk))
            {
                chunk = new Chunk(x, y, CHUNK_SIZE);
                Chunks.Add(cxy, chunk);
                QueueChunk(cxy);
            }
            return chunk;
        }

        public static Tile GetTile(int x, int y)
        {
            var chunk = GetChunk(x, y);
            var tx = Math.Abs(x % CHUNK_SIZE);
            var ty = Math.Abs(y % CHUNK_SIZE);
            var tile = chunk.Tiles[tx]?[ty];
            return tile;
        }
    }
}