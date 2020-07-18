using Microsoft.Xna.Framework;
using Pixel.Loaders.BmFont.Models;

namespace Pixel.Loaders
{
    public class BmpFontChar
    {
        public FontChar FontChar { get; }
        public Rectangle SrcRect { get; }

        public BmpFontChar(FontChar fontCharacter)
        {
            FontChar = fontCharacter;
            SrcRect = new Rectangle(FontChar.X, FontChar.Y, FontChar.Width, FontChar.Height);
        }
    }
}