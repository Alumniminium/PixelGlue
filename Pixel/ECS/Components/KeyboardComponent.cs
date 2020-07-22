using Shared.Enums;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct KeyboardComponent
    {
        public Vector2 Axis;
        public List<PixelGlueButtons> Buttons;
        public List<PixelGlueButtons> OldButtons;

        public bool IsPressed(PixelGlueButtons btn) => Buttons.Contains(btn) && !OldButtons.Contains(btn);
        public bool IsDown(PixelGlueButtons btn) => Buttons.Contains(btn);
        public bool IsUp(PixelGlueButtons btn) => !Buttons.Contains(btn);
    }
}