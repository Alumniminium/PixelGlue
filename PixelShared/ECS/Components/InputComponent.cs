using PixelShared.Enums;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pixel.ECS.Components
{
    public struct InputComponent
    {
        public Vector2 Axis;
        public List<PixelGlueButtons> Buttons;
        public List<PixelGlueButtons> OldButtons;
        public float Scroll;
        public float OldScroll;

        public bool IsPressed(PixelGlueButtons btn) => Buttons.Contains(btn) && !OldButtons.Contains(btn);
        public bool IsDown(PixelGlueButtons btn) => Buttons.Contains(btn);
        public bool IsUp(PixelGlueButtons btn) => !Buttons.Contains(btn);
    }
}