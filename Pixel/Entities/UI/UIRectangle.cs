using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.ECS;

namespace Pixel.Entities.UI
{
    public class UIRectangle : Entity
    {
        public void Setup(int x, int y, int w, int h, Color color)
        {
            Add(new PositionComponent(x,y,0));
            Add(new DrawableComponent(color, new Rectangle(x, y, w, h)));
        }
    }
}