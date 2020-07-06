using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct InputComponent
    {
        public Vector2 Axis;
        public List<PixelGlueButtons> Buttons;
        public List<PixelGlueButtons> OldButtons;
        public float Scroll;
        public float OldScroll;
    }
}