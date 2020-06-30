using Microsoft.Xna.Framework;
using PixelShared;
using PixelShared.Enums;
using PixelShared.Extensions;
using PixelShared.Maths;
using PixelShared.Noise;
using System;
using System.Collections.Generic;

namespace Pixel.Helpers
{
    public static class NoiseGen
    {
        public static FastNoise WhiteNoise = new FastNoise();
        public static Dictionary<NoisePattern, Func<int, int, Color, Color[]>> Patterns = new Dictionary<NoisePattern, Func<int, int, Color, Color[]>>
        {
            [NoisePattern.None] = None,
            [NoisePattern.Flowers] = Flowers,
            [NoisePattern.Waves] = Waves,
            [NoisePattern.Rough] = Rough,
        };

        public static Color[] None(int w, int h, Color color)
        {
            var pixels = new Color[w * h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    pixels[(y * w) + x] = color;
            return pixels;
        }
        public static Color[] Rough(int w, int h, Color color)
        {
            var pixels = None(w, h, color);
            WhiteNoise.SetFrequency(0.25f);
            WhiteNoise.SetFractalOctaves(2);
            WhiteNoise.SetFractalType(FractalType.Billow);
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var val = WhiteNoise.GetSimplexFractal(x, y);
                    var multi = PixelMath.Map(val, -1, 1, 0, 1);
                    pixels[(y * w) + x].A = (byte)(pixels[(y * w) + x].A * multi);
                }
            return pixels;
        }
        public static Color[] Flowers(int w, int h, Color color)
        {
            var pixels = None(w, h, color);

            for (int i = 0; i < Global.Random.Next(6, 12); i++)
            {
                var x = Global.Random.Next(0, w);
                var y = Global.Random.Next(0, h);

                if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                    continue;

                if (pixels[((y - 1) * w) + x] != color
                || pixels[((y + 1) * w) + x] != color
                || pixels[(y * w) + x - 1] != color
                || pixels[(y * w) + x + 1] != color
                || pixels[(y * w) + x] != color)
                    continue;

                var flower = Global.Random.Next(0, 101);
                Color flowerColor;
                if (flower >= 75)
                    flowerColor = "fee761".ToColor();
                else if (flower >= 50)
                    flowerColor = "ff0044".ToColor();
                else if (flower >= 25)
                    flowerColor = "2ce8f5".ToColor();
                else if (flower >= 12)
                    flowerColor = "f77622".ToColor();
                else
                    flowerColor = "3e8948".ToColor();

                pixels[((y - 1) * w) + x] = flowerColor;
                pixels[((y + 1) * w) + x] = flowerColor;
                pixels[(y * w) + x - 1] = flowerColor;
                pixels[(y * w) + x + 1] = flowerColor;
            }
            return pixels;
        }
        public static Color[] Waves(int w, int h, Color color)
        {
            var pixels = None(w, h, color);

            for (int i = 0; i < Global.Random.Next(6, 12); i++)
            {
                var x = Global.Random.Next(0, w);
                var y = Global.Random.Next(0, h);

                if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                    continue;

                if (pixels[((y - 1) * w) + x] != color
                || pixels[((y + 1) * w) + x] != color
                || pixels[(y * w) + x - 1] != color
                || pixels[(y * w) + x + 1] != color
                || pixels[(y * w) + x] != color)
                    continue;

                Color flowerColor = "2ce8f5".ToColor();

                pixels[((y - 1) * w) + x] = flowerColor;
                pixels[(y * w) + x - 1] = flowerColor;
                pixels[(y * w) + x + 1] = flowerColor;
            }
            return pixels;
        }
    }
}