using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using PixelGlueCore.Helpers;

namespace PixelGlueCore.Entities.UI
{
    public class UIRectangle : PixelEntity
    {
        public void Setup(int x, int y, int w, int h, Color color)
        {
            var texture = Texture2DExt.Blank(1,1,color);
            AssetManager.LoadTexture(texture,color.PackedValue.ToString());
            Add(new DrawableComponent(EntityId,color.PackedValue.ToString(),new Rectangle(0,0,1,1),new Rectangle(x,y,w,h)));
        }
    }    
    public class Textblock : PixelEntity
    {
        public void Setup(int x, int y, int w, int h, Color color)
        {
            var texture = Texture2DExt.Blank(1,1,color);
            AssetManager.LoadTexture(texture,color.PackedValue.ToString());
            Add(new DrawableComponent(EntityId,color.PackedValue.ToString(),new Rectangle(0,0,1,1),new Rectangle(x,y,w,h)));
            Add(new TextComponent(EntityId,"Hello World","profont"));
        }
    }
}