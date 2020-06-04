using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Helpers;

namespace PixelGlueCore.Scenes
{
    public class UIRectangle
    {
        public Texture2D Texture;
        public int X,Y;
        public Rectangle RenderRect;
        public Rectangle SourceRect;

        public UIRectangle(int x, int y, int w, int h, Color color)
        {
            X=x;
            Y=y;
            RenderRect = new Rectangle(X,Y,w,h);
            SourceRect = new Rectangle(0,0,1,1);
            Texture = Texture2DExt.Blank(1,1,color);
        }
    }
}