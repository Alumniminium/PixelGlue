using Microsoft.Xna.Framework;
using Pixel.ECS;
using Pixel.ECS.Components;
using Pixel.Helpers;
using PixelShared;

namespace Pixel.Entities.UI
{
    public class UIRectangle : Entity
    {
        public void Setup(int x, int y, int w, int h, Color color)
        {
            var texture = TextureGen.Blank(Global.Device, 1, 1, color);
            AssetManager.LoadTexture(color.PackedValue.ToString(),texture);
            Add(new DrawableComponent(color.PackedValue.ToString(), new Rectangle(0, 0, 1, 1), new Rectangle(x, y, w, h)));
        }
    }
}