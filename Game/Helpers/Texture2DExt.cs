using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelGlueCore.Helpers
{
    public static class Texture2DExt
    {
        public static Texture2D Blank(int w, int h,Color color)
        {
            var _blankTexture = new Texture2D(PixelGlue.Device,w,h);
            _blankTexture.SetData(new[] {color});
            return _blankTexture;
        }
    }
}