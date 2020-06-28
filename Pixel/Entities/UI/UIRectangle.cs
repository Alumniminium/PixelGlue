using Microsoft.Xna.Framework;
using Pixel.ECS;
using Pixel.ECS.Components;
using Pixel.Helpers;

namespace Pixel.Entities.UI
{
    public class UIRectangle : Entity
    {
        public void Setup(int x, int y, int w, int h, Color color)
        {
            var texture = TextureGen.Blank(Global.Device,1,1,color);
            AssetManager.LoadTexture(texture,color.PackedValue.ToString());
            Add(new DrawableComponent(EntityId,color.PackedValue.ToString(),new Rectangle(0,0,1,1),new Rectangle(x,y,w,h)));
        }
    }    
    public class Textblock : Entity
    {
        public void Setup(int x, int y, int w, int h, Color color)
        {
            var texture = TextureGen.Blank(Global.Device,1,1,color);
            AssetManager.LoadTexture(texture,color.PackedValue.ToString());
            Add(new DrawableComponent(EntityId,color.PackedValue.ToString(),new Rectangle(0,0,1,1),new Rectangle(x,y,w,h)));
            Add(new TextComponent(EntityId,"Hello World","profont"));
        }
    }
}