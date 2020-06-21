using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelGlueCore.Helpers
{
    public static class TextureGen
    {
        public static Texture2D Blank(int w, int h,Color color)
        {
            var _blankTexture = new Texture2D(PixelGlue.Device,w,h);
            _blankTexture.SetData(new[] {color});
            return _blankTexture;
        }
        public static Texture2D Pixel(string color)
        {   
            var (r,g,b,a) = Convert(color);
            return Blank(1,1,Color.FromNonPremultiplied(r,g,b,a));
        }
        public static (byte r,byte g,byte b, byte a) Convert(string colorCode)
        {
            if(colorCode.IndexOf("#") !=-1)
            {
                colorCode = colorCode.Replace("#", "");
            }
            byte r,g,b,a;
            r = byte.Parse(colorCode.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            g = byte.Parse(colorCode.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            b = byte.Parse(colorCode.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            a = 255;
            return (r,g,b,a);
        } 
    }
}