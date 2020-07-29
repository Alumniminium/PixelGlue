using Microsoft.Xna.Framework;
using Shared;
using Shared.Enums;
using Shared.Extensions;
using Shared.Noise;
using System;

namespace Pixel.Helpers
{
    public static class WorldGen
    {
        private const float Frequency = 0.00004f;
        public static FastNoise BiomeNoise, PlainNoise, DesertNoise, SwampNoise, MountainNoise, RiverNoise;
        static WorldGen()
        {
            BiomeNoise = new FastNoise(203414084);
            BiomeNoise.SetNoiseType(NoiseType.Cellular);
            BiomeNoise.SetInterp(Interp.Linear);
            BiomeNoise.SetFrequency(Frequency);
            BiomeNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            BiomeNoise.SetCellularReturnType(CellularReturnType.CellValue);
            BiomeNoise.SetCellularJitter(0.3f);
            BiomeNoise.SetGradientPerturbAmp(1000);
            BiomeNoise.SetGradientFrequency(0.0004f);

            RiverNoise = new FastNoise(203414084);
            RiverNoise.SetNoiseType(NoiseType.Cellular);
            RiverNoise.SetInterp(Interp.Linear);
            RiverNoise.SetFrequency(Frequency);
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
            MountainNoise.SetFrequency(Frequency);
            MountainNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            MountainNoise.SetCellularReturnType(CellularReturnType.Distance2Div);
            MountainNoise.SetCellularJitter(0.3f);
            MountainNoise.SetGradientPerturbAmp(1000);
            MountainNoise.SetGradientFrequency(0.0004f);
        }

        public static float GenerateRiver(float x, float y)
        {
            return RiverNoise.GetNoise(x, y);
            //if (val >= 0.98f)
            //    return new DrawableComponent("0099DB".ToColor();
            //else if (val > 0.96f)
            //    return new DrawableComponent("4CB7E5".ToColor();
            //else if (val > 0.945f)
            //    return new DrawableComponent("C28569".ToColor();
            //else
            //    return null;
        }
        public enum BiomeType
        {
            Plains,
            Desert,
            Swamp,
            Mountains,
        }
        public static BiomeType GenerateBiome(float x, float y)
        {
            var biome = BiomeNoise.GetNoise(x, y);

            if (biome > 0.7)
                return BiomeType.Plains;
            else if (biome > 0.5)
                return BiomeType.Desert;
            else if (biome < 0.2)
                return BiomeType.Swamp;
            else
                return BiomeType.Mountains;
        }
        private static float GenerateMountains(float x, float y)
        {
            return (float)MountainNoise.GetNoise(x, y);
        }
        private static float GenerateDesert(float x, float y)
        {
            var val = DesertNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * DesertNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * DesertNoise.GetNoise(x / 8, y / 8);
            //val += 0.75f * RiverNoise.GetNoise(x, y);
            return val;
        }
        private static float GenerateSwamp(float x, float y)
        {
            var val = SwampNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * SwampNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * SwampNoise.GetNoise(x / 8, y / 8);
            //val += 0.75f * RiverNoise.GetNoise(x, y);
            return val;
        }

        private static float GeneratePlains(float x, float y)
        {
            var val = PlainNoise.GetNoise(x / 32, y / 32);
            val += 0.75f * PlainNoise.GetNoise(x / 26, y / 26);
            val += 0.25f * PlainNoise.GetNoise(x / 8, y / 8);
            return val;
        }

        public static Tile GetTile(int x, int y)
        {
            float x2, y2;
            x2 = x;
            y2 = y;
            RiverNoise.GradientPerturbFractal(ref x2, ref y2);
            var river = GenerateRiver(x2, y2);
            var tile = new Tile(x, y, Color.Purple);
            if (river > 0.945f)
            {
                tile.Color = Color.Blue;
            }
            else
            {
                float val;
                switch (GenerateBiome(x2, y2))
                {
                    case BiomeType.Plains:
                        val = GeneratePlains(x2, y2);
                        if (val > 0.7f)
                            tile.Color = "B86F50".ToColor();//wrong colors
                        break;
                    case BiomeType.Desert:
                        val = GenerateDesert(x2, y2);
                        if (val > 0.7f)
                            tile.Color = "B86F50".ToColor();
                        else if (val > 0.4f)
                            tile.Color = "E4A672".ToColor();
                        else if (val > 0f)
                            tile.Color = "E8B796".ToColor();
                        else if (val > -0.6)
                            tile.Color = "E4A672".ToColor();
                        else
                            tile.Color = "C28569".ToColor();
                        break;
                    case BiomeType.Swamp:
                        val = GenerateSwamp(x2, y2);
                        if (val > 0.6f)
                            tile.Color = "193c3e".ToColor();
                        else if (val > 0.4f)
                            tile.Color = "265c42".ToColor();
                        else if (val > -0.2f)
                            tile.Color = "3E8948".ToColor();
                        else if (val > -0.4)
                            tile.Color = "4CB7E5".ToColor();
                        else
                            tile.Color = "0099DB".ToColor();
                        break;
                    case BiomeType.Mountains:
                        val = GenerateMountains(x2, y2);
                        if (val > 0.9f)
                            tile.Color = "C0CBDC".ToColor();
                        else if (val > 0.7f)
                            tile.Color = "3e2731".ToColor();
                        else if (val > 0.6f)
                            tile.Color = "733e39".ToColor();
                        else if (val > 0.5f)
                            tile.Color = "B86F50".ToColor();
                        else if (val > 0.4f)
                            tile.Color = "C28569".ToColor();
                        else if (val > 0.3f)
                            tile.Color = "E4A672".ToColor();
                        else if (val > -0.3)
                            tile.Color = "E8B796".ToColor();
                        else
                            tile.Color = "EAD4AA".ToColor();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("What the fuck");
                }
            }
            return tile;
        }
    }
}