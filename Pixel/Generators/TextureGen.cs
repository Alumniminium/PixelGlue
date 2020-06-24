using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Enums;
using Pixel.Extensions;

namespace PixelGlueCore.Helpers
{
    public static class TextureGen
    {
        public static Texture2D Blank(GraphicsDevice device, int w, int h, Color color)
        {
            var _blankTexture = new Texture2D(device, w, h);
            var pixels = new Color[w * h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    pixels[(y * w) + x - 1] = color;
            _blankTexture.SetData(pixels);
            var pixels2 = new Color[w * h];
            _blankTexture.GetData(pixels2);
            return _blankTexture;
        }
        public static Texture2D Noise(GraphicsDevice device,int w, int h, string hexcolor, NoisePattern pattern)
        {
            var color = hexcolor.ToColor();
            var texture = new Texture2D(device, w, h);
            var pixels = NoiseGen.Patterns[pattern].Invoke(w, h, color);
            texture.SetData(pixels);
            return texture;
        }
        public static Texture2D Pixel(GraphicsDevice device,string color) => Blank(device,1, 1, color.ToColor());
    }
}