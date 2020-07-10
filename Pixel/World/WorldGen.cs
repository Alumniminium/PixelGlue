using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared;
using Shared.Enums;
using Shared.Extensions;
using Shared.Noise;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Pixel.World
{
    public static class WorldGen
    {
        public static ConcurrentDictionary<(int x, int y), bool> TilesLoading = new ConcurrentDictionary<(int x, int y), bool>();
        public static Thread[] Prefetcher = new Thread[Environment.ProcessorCount * 2];
        public static ConcurrentStack<(int x, int y)>[] Queue = new ConcurrentStack<(int x, int y)>[2];
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerZero = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerOne = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerTwo = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static FastNoise BiomeNoise, PlainNoise, DesertNoise, SwampNoise, MountainNoise, RiverNoise;
        public static Rectangle srcRect = new Rectangle(0, 0, Global.TileSize, Global.TileSize);
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
            PlainNoise.SetFrequency(0.002f);

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

            for (int i = 0; i < Queue.Length; i++)
                Queue[i] = new ConcurrentStack<(int x, int y)>();

            for (int i = 0; i < Prefetcher.Length; i++)
            {
                Prefetcher[i] = new Thread(new ParameterizedThreadStart(Load))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                };
                Prefetcher[i].Start(i % 2);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                        LayerTwo.TryAdd((x, y), river);
                    else if (tree.HasValue)
                        LayerOne.TryAdd((x, y), tree);

                    TilesLoading.TryRemove((x, y), out _);
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (DrawableComponent? terrain, DrawableComponent? river, DrawableComponent? decor) Generate(int x, int y)
        {
            var dstRect = new Rectangle(x, y, Global.TileSize, Global.TileSize);
            float x2, y2;
            x2 = x;
            y2 = y;
            RiverNoise.GradientPerturbFractal(ref x2, ref y2);
            var (terrain, decor) = GenerateBiome(x2, y2, dstRect);
            var river = GenerateRiver(x2, y2, dstRect);
            return (terrain, decor, river);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DrawableComponent? GenerateRiver(float x, float y, Rectangle dstRect)
        {
            var val = RiverNoise.GetNoise(x, y);

            if (val >= 0.98f)
                return new DrawableComponent("0099DB".ToColor(), dstRect);
            else if (val > 0.96f)
                return new DrawableComponent("4CB7E5".ToColor(), dstRect);
            else if (val > 0.945f)
                return new DrawableComponent("C28569".ToColor(), dstRect);
            else
                return null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (DrawableComponent? ground, DrawableComponent? decor) GenerateBiome(float x, float y, Rectangle dstRect)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (DrawableComponent? ground, DrawableComponent? decor) GenerateMountains(float x, float y, Rectangle dstRect)
        {
            DrawableComponent? decor = null;
            var val = MountainNoise.GetNoise(x, y);

            DrawableComponent? ground;
            if (val > 0.9f)
                ground = new DrawableComponent("C0CBDC".ToColor(), dstRect);
            else if (val > 0.7f)
                ground = new DrawableComponent("3e2731".ToColor(), dstRect);
            else if (val > 0.6f)
                ground = new DrawableComponent("733e39".ToColor(), dstRect);
            else if (val > 0.5f)
                ground = new DrawableComponent("B86F50".ToColor(), dstRect);
            else if (val > 0.4f)
                ground = new DrawableComponent("C28569".ToColor(), dstRect);
            else if (val > 0.3f)
                ground = new DrawableComponent("E4A672".ToColor(), dstRect);
            else if (val > -0.3)
                ground = new DrawableComponent("E8B796".ToColor(), dstRect);
            else
                ground = new DrawableComponent("EAD4AA".ToColor(), dstRect);

            return (ground, decor);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (DrawableComponent?, DrawableComponent?) GenerateDesert(float x, float y, Rectangle dstRect)
        {
            DrawableComponent? decor = null;
            var val = DesertNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * DesertNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * DesertNoise.GetNoise(x / 8, y / 8);
            DrawableComponent? ground;
            //val += 0.75f * RiverNoise.GetNoise(x, y);

            if (val > 0.7f)
                ground = new DrawableComponent("B86F50".ToColor(), dstRect);
            else if (val > 0.4f)
                ground = new DrawableComponent("E4A672".ToColor(), dstRect);
            else if (val > 0f)
                ground = new DrawableComponent("E8B796".ToColor(), dstRect);
            else if (val > -0.6)
                ground = new DrawableComponent("E4A672".ToColor(), dstRect);
            else
                ground = new DrawableComponent("C28569".ToColor(), dstRect);

            return (ground, decor);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (DrawableComponent?, DrawableComponent?) GenerateSwamp(float x, float y, Rectangle dstRect)
        {
            DrawableComponent? decor = null;

            var val = SwampNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * SwampNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * SwampNoise.GetNoise(x / 8, y / 8);
            DrawableComponent? ground;
            //val += 0.75f * RiverNoise.GetNoise(x, y);

            if (val > 0.6f)
                ground = new DrawableComponent("193c3e".ToColor(), dstRect);
            else if (val > 0.4f)
                ground = new DrawableComponent("265c42".ToColor(), dstRect);
            else if (val > -0.2f)
                ground = new DrawableComponent("3E8948".ToColor(), dstRect);
            else if (val > -0.4)
                ground = new DrawableComponent("4CB7E5".ToColor(), dstRect);
            else
                ground = new DrawableComponent("0099DB".ToColor(), srcRect);
            return (ground, decor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (DrawableComponent?, DrawableComponent?) GeneratePlains(float x, float y, Rectangle dstRect)
        {
            Dictionary<float, (DrawableComponent, DrawableComponent?)> heights = new Dictionary<float, (DrawableComponent, DrawableComponent?)>()
            {
                [0.85f] = (new DrawableComponent("C0CBDC".ToColor(), dstRect), null),
                [0.80f] = (new DrawableComponent("733e39".ToColor(), dstRect), null),
                [0.75f] = (new DrawableComponent("dawn", new Rectangle(480, 352, 16, 16), dstRect), null),
                [0.70f] = (new DrawableComponent("dawn", new Rectangle(96, 272, 16, 16), dstRect), null),
                [0.62f] = (new DrawableComponent("dawn", new Rectangle(16 * 4, 0, 16, 16), dstRect), null),
                [0.60f] = (new DrawableComponent("dawn", new Rectangle(16 * 5, 0, 16, 16), dstRect), null),
                [0.42f] = (new DrawableComponent("dawn", new Rectangle(32, 0, 16, 16), dstRect), new DrawableComponent("dawn", new Rectangle(96 + 16, 0, 16, 16), dstRect)),
                [0.40f] = (new DrawableComponent("dawn", new Rectangle(16, 0, 16, 16), dstRect), new DrawableComponent("dawn", new Rectangle(96 + 32, 0, 16, 16), dstRect)),
                [0.35f] = (new DrawableComponent("dawn", new Rectangle(96, 0, 16, 16), dstRect), null),
                [0.30f] = (new DrawableComponent("dawn", new Rectangle(16 * 3, 16, 16, 16), dstRect), null),
                [0.20f] = (new DrawableComponent("dawn", new Rectangle(16 * 2, 16, 16, 16), dstRect), null),
                [-0.5f] = (new DrawableComponent("dawn", new Rectangle(16, 0, 16, 16), dstRect), null),
                [-0.6f] = (new DrawableComponent("dawn", new Rectangle(144, 496, 16, 16), dstRect), null),
                [-1f] = (new DrawableComponent("dawn", new Rectangle(128, 496, 16, 16), dstRect), null),
            };

            var val = PlainNoise.GetNoise(x / 32, y / 32);
            val += 0.75f * PlainNoise.GetNoise(x / 26, y / 26);
            val += 0.25f * PlainNoise.GetNoise(x / 8, y / 8);

            foreach (var f in heights)
                if (val >= f.Key)
                    return f.Value;

            return (null, null);
        }
        public static int last;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static (DrawableComponent?, DrawableComponent?) GetTiles(int x, int y)
        {
            if (!LayerZero.TryGetValue((x, y), out var terrainTile) && !TilesLoading.TryGetValue((x, y), out _))
            {
                TilesLoading.TryAdd((x, y), false);
                Queue[last].Push((x, y));
                last++;
                if (last == Queue.Length)
                    last = 0;
            }
            LayerOne.TryGetValue((x, y), out var decorTile);
            LayerTwo.TryGetValue((x, y), out decorTile);
            return (terrainTile, decorTile);
        }
    }
}