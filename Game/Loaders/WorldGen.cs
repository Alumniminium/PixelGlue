using Microsoft.Xna.Framework;
using PixelGlueCore.ECS.Components;
using System.Threading;
using System.Collections.Concurrent;
using Pixel.Noise;
using Pixel.Enums;

namespace PixelGlueCore.ECS.Systems
{
    public static class WorldGen
    {
        public static ConcurrentDictionary<(int x, int y), bool> TilesLoading = new ConcurrentDictionary<(int x, int y), bool>();
        public static Thread[] Prefetcher = new Thread[2];
        public static ConcurrentStack<(int x, int y)>[] Queue = new ConcurrentStack<(int x, int y)>[2];
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerZero = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerOne = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static FastNoise BiomeNoise, PlainNoise, DesertNoise, SwampNoise, MountainNoise, RiverNoise;
        public static Rectangle srcRect = new Rectangle(0, 0, PixelGlue.TileSize, PixelGlue.TileSize);
        static WorldGen()
        {
            BiomeNoise = new FastNoise(203414084);
            BiomeNoise.SetNoiseType(NoiseType.Cellular);
            BiomeNoise.SetInterp(Interp.Linear);
            BiomeNoise.SetFrequency(0.00004f);
            BiomeNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            BiomeNoise.SetCellularReturnType(CellularReturnType.CellValue);
            BiomeNoise.SetCellularJitter(0.3f);
            BiomeNoise.SetGradientPerturbAmp(1000);
            BiomeNoise.SetGradientFrequency(0.0004f);

            RiverNoise = new FastNoise(203414084);
            RiverNoise.SetNoiseType(NoiseType.Cellular);
            RiverNoise.SetInterp(Interp.Linear);
            RiverNoise.SetFrequency(0.00004f);
            RiverNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            RiverNoise.SetCellularReturnType(CellularReturnType.Distance2Div);
            RiverNoise.SetCellularJitter(0.3f);
            RiverNoise.SetGradientPerturbAmp(1000);
            RiverNoise.SetGradientFrequency(0.0004f);

            PlainNoise = new FastNoise(1337);
            PlainNoise.SetNoiseType(NoiseType.SimplexFractal);
            PlainNoise.SetInterp(Interp.Quintic);
            PlainNoise.SetFrequency(0.001f);

            DesertNoise = new FastNoise(30);
            DesertNoise.SetNoiseType(NoiseType.SimplexFractal);
            DesertNoise.SetInterp(Interp.Quintic);
            DesertNoise.SetFrequency(0.0015f);

            SwampNoise = new FastNoise(330);
            SwampNoise.SetNoiseType(NoiseType.SimplexFractal);
            SwampNoise.SetInterp(Interp.Quintic);
            SwampNoise.SetFrequency(0.0015f);

            MountainNoise = new FastNoise(330);
            MountainNoise.SetNoiseType(NoiseType.Cellular);
            MountainNoise.SetInterp(Interp.Linear);
            MountainNoise.SetFrequency(0.00004f);
            MountainNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            MountainNoise.SetCellularReturnType(CellularReturnType.Distance2Div);
            MountainNoise.SetCellularJitter(0.3f);
            MountainNoise.SetGradientPerturbAmp(1000);
            MountainNoise.SetGradientFrequency(0.0004f);

            for (int i = 0; i < Prefetcher.Length; i++)
            {
                Queue[i] = new ConcurrentStack<(int x, int y)>();
                Prefetcher[i] = new Thread(new ParameterizedThreadStart(Load))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                };
                Prefetcher[i].Start(i);
            }
        }
        public static void Load(object idobj)
        {
            int id = (int)idobj;
            while (true)
            {
                while (Queue[id].TryPop(out var t))
                {
                    var (x, y) = t;
                    if (LayerZero.ContainsKey((x, y)))
                        continue;
                    var (terrain, river, tree) = Generate(x, y);
                    LayerZero.TryAdd((x, y), terrain);
                    if (river.HasValue)
                        LayerOne.TryAdd((x, y), river);
                    else if (tree.HasValue)
                        LayerOne.TryAdd((x, y), tree);

                    TilesLoading.TryRemove((x, y), out _);
                    //Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }
        public static (DrawableComponent? terrain, DrawableComponent? river, DrawableComponent? tree) Generate(int x, int y)
        {
            var dstRect = new Rectangle(x, y, PixelGlue.TileSize, PixelGlue.TileSize);
            float x2, y2;
            x2 = x;
            y2 = y;
            RiverNoise.GradientPerturbFractal(ref x2, ref y2);
            var terrain = GenerateBiome(x2, y2, dstRect);
            var river = GenerateRiver(x2, y2, dstRect);
            var tree = GenerateTrees(x2, y2, dstRect);
            return (terrain, river, tree);
        }

        public static DrawableComponent? GenerateTrees(float x, float y, Rectangle dstRect)
        {
            var ground = GeneratePlains(x,y,dstRect);
            if(ground.HasValue&&ground.Value.TextureName =="trees")
                    return new DrawableComponent(0, "tree", srcRect, dstRect);
                    return null;
        }
        public static DrawableComponent? GenerateRiver(float x, float y, Rectangle dstRect)
        {
            var val = RiverNoise.GetNoise(x, y);

            if (val >= 0.98f)
                return new DrawableComponent(0, "water", srcRect, dstRect);
            else if (val > 0.96f)
                return new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else if (val > 0.945f)
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
            else
                return null;
        }
        public static DrawableComponent? GenerateBiome(float x, float y, Rectangle dstRect)
        {
            var biome = BiomeNoise.GetNoise(x, y);

            if (biome > 0.7)
                return GeneratePlains(x, y, dstRect);
            else if (biome > 0.5)
                return GenerateDesert(x, y, dstRect);
            else if (biome < 0.2)
                return GenerateSwamp(x, y, dstRect);
            else
                return GenerateMountains(x, y, dstRect);
        }

        private static DrawableComponent? GenerateMountains(float x, float y, Rectangle dstRect)
        {
            var val = MountainNoise.GetNoise(x, y);

            if (val > 0.9f)
                return new DrawableComponent(0, "snow", srcRect, dstRect);
            else if (val > 0.7f)
                return new DrawableComponent(0, "rock3", srcRect, dstRect);
            else if (val > 0.6f)
                return new DrawableComponent(0, "rock2", srcRect, dstRect);
            else if (val > 0.5f)
                return new DrawableComponent(0, "rock", srcRect, dstRect);
            else if (val > 0.4f)
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
            else if (val > 0.3f)
                return new DrawableComponent(0, "sand3", srcRect, dstRect);
            else if (val > -0.3)
                return new DrawableComponent(0, "sand2", srcRect, dstRect);
            else
                return new DrawableComponent(0, "sand", srcRect, dstRect);
        }
        private static DrawableComponent? GenerateDesert(float x, float y, Rectangle dstRect)
        {
            var val = DesertNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * DesertNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * DesertNoise.GetNoise(x / 8, y / 8);
            //val += 0.75f * RiverNoise.GetNoise(x, y);

            if (val > 0.7f)
                return new DrawableComponent(0, "rock", srcRect, dstRect);
            else if (val > 0.4f)
                return new DrawableComponent(0, "sand3", srcRect, dstRect);
            else if (val > 0f)
                return new DrawableComponent(0, "sand2", srcRect, dstRect);
            else if (val > -0.6)
                return new DrawableComponent(0, "sand3", srcRect, dstRect);
            else
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
        }
        private static DrawableComponent? GenerateSwamp(float x, float y, Rectangle dstRect)
        {
            var val = SwampNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * SwampNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * SwampNoise.GetNoise(x / 8, y / 8);
            //val += 0.75f * RiverNoise.GetNoise(x, y);

            if (val > 0.6f)
                return new DrawableComponent(0, "grass3", srcRect, dstRect);
            else if (val > 0.4f)
                return new DrawableComponent(0, "grass2", srcRect, dstRect);
            else if (val > -0.2f)
                return new DrawableComponent(0, "grass", srcRect, dstRect);
            else if (val > -0.4)
                return new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else
                return new DrawableComponent(0, "water", srcRect, dstRect);
        }

        private static DrawableComponent? GeneratePlains(float x, float y, Rectangle dstRect)
        {
            var val = PlainNoise.GetNoise(x / 32, y / 32);
            val += 0.75f * PlainNoise.GetNoise(x / 26, y / 26);
            val += 0.25f * PlainNoise.GetNoise(x / 8, y / 8);

            if (val > 0.85)
                return new DrawableComponent(0, "snow", srcRect, dstRect);
            else if (val > 0.8)
                return new DrawableComponent(0, "rock2", srcRect, dstRect);
            else if (val > 0.75)
                return new DrawableComponent(0, "rock", srcRect, dstRect);
            else if (val > 0.7)
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
            else if (val > 0.6)
                return new DrawableComponent(0, "trees", srcRect, dstRect);
            else if (val > 0.4)
                return new DrawableComponent(0, "grass", srcRect, dstRect);
            else if (val > -0.5)
            {
                var idx = PixelGlue.Random.Next(1, 9);
                var idy = PixelGlue.Random.Next(0, 2);
                if(idy == 1 && idx > 6)
                    return new DrawableComponent(0, "dawn", new Rectangle(16*3,16,16,16), dstRect);
                if (idx > 5)
                    return new DrawableComponent(0, "dawn", new Rectangle(16,0,16,16), dstRect);
                else
                    return new DrawableComponent(0, "dawn", new Rectangle(16*idx,16*idy,16,16), dstRect);
            }
            else if (val > -0.55)
                return new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else
            {
                var idx = PixelGlue.Random.Next(-3, 6);
                if (idx <= 0)
                    return new DrawableComponent(0, "dawn", new Rectangle(256,464,16,16), dstRect);
                else
                    return new DrawableComponent(0, "water" + idx, srcRect, dstRect);
            }
        }
        public static int last;
        internal static DrawableComponent? GetTileLayerZero(int x, int y)
        {
            if (!LayerZero.TryGetValue((x, y), out var terrainTile))
            {
                if (TilesLoading.TryGetValue((x, y), out _))
                    return null;
                TilesLoading.TryAdd((x, y), false);
                Queue[last].Push((x, y));
                last++;
                if (last == Queue.Length)
                    last = 0;
            }
            return terrainTile;
        }
        internal static DrawableComponent? GetTileLayerOne(int x, int y)
        {
            if (!LayerOne.TryGetValue((x, y), out var riverTile))
            {
                if (TilesLoading.TryGetValue((x, y), out _))
                    return null;
                TilesLoading.TryAdd((x, y), false);
                Queue[last].Push((x, y));
                last++;
                if (last == Queue.Length)
                    last = 0;
            }
            return riverTile;
        }
    }
}