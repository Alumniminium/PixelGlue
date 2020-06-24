using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static PixelGlueCore.Loaders.FastNoise;

namespace PixelGlueCore.Helpers
{
    public enum NoisePattern
    {
        None,
        Flowers,
        Rough,
        Waves
    }
    public static class NoisePatterns
    {
        public static Loaders.FastNoise WhiteNoise = new Loaders.FastNoise();
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
            var pixels = None(w,h,color);
            WhiteNoise.SetFrequency(0.25f);
            WhiteNoise.SetFractalOctaves(2);
            WhiteNoise.SetFractalType(FractalType.Billow);
            for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                {
                    var val = WhiteNoise.GetSimplexFractal(x,y);
                    var multi = PixelMath.Map(val,-1,1,0,1);
                    pixels[(y * w) + x].A = (byte)(pixels[(y * w) + x].A * multi);
                }
            return pixels;
        }
        public static Color[] Flowers(int w, int h, Color color)
        {
            var pixels = None(w,h,color);

            for (int i = 0; i < PixelGlue.Random.Next(6, 12); i++)
            {
                var x = PixelGlue.Random.Next(0, w);
                var y = PixelGlue.Random.Next(0, h);
                
                if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                    continue;

                if (pixels[((y - 1) * w) + x] != color
                || pixels[((y + 1) * w) + x] != color
                || pixels[(y * w) + x - 1] != color
                || pixels[(y * w) + x + 1] != color
                || pixels[(y * w) + x] != color)
                    continue;

                var flower = PixelGlue.Random.Next(0, 101);
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
            var pixels = None(w,h,color);

            for (int i = 0; i < PixelGlue.Random.Next(6, 12); i++)
            {
                var x = PixelGlue.Random.Next(0, w);
                var y = PixelGlue.Random.Next(0, h);
                
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
    public static class TextureGen
    {
        public static Texture2D Blank(int w, int h, Color color)
        {
            var _blankTexture = new Texture2D(PixelGlue.Device, w, h);
            var pixels = new Color[w * h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    pixels[(y * w) + x - 1] = color;
            _blankTexture.SetData(pixels);
            var pixels2 = new Color[w * h];
            _blankTexture.GetData(pixels2);
            return _blankTexture;
        }
        public static Texture2D Noise(int w, int h, string hexcolor, NoisePattern pattern)
        {
            var color = hexcolor.ToColor();
            var texture = new Texture2D(PixelGlue.Device, w, h);
            var pixels = NoisePatterns.Patterns[pattern].Invoke(w, h, color);
            texture.SetData(pixels);
            return texture;
        }
        public static Texture2D Pixel(string color) => Blank(1, 1, color.ToColor());
    }
}