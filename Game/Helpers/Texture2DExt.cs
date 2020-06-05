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
    public static class Vector2Ext
    {
        public static Vector2 DrawablePosition(this Vector2 vector)
        {
            var sx = PixelGlue.ScreenWidth / PixelGlue.VirtualScreenWidth;
            var sy = PixelGlue.ScreenHeight / PixelGlue.VirtualScreenHeight;
            var tpx = 1f/sx;
            var tpy = 1f/sy;
            var floor = Vector2.Floor(vector);
            var delta = ((vector-floor) / tpx) * tpx;
            //delta.X = (delta.X / tpx) * tpx;
            //delta.Y = (delta.Y / tpy) * tpy;
            return floor+delta;
        }
    }
}