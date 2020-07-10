using Microsoft.Xna.Framework.Input;
using Shared.Enums;
using System.Collections.Generic;

namespace Pixel.Configuration
{
    public static class UserKeybinds
    {
        public static Dictionary<PixelGlueButtons, (Keys defaultBind, Keys userBind)> GenericToKeybinds = new Dictionary<PixelGlueButtons, (Keys defaultBind, Keys userBind)>
        {
            [PixelGlueButtons.Up] = (Keys.W, Keys.W),
            [PixelGlueButtons.Down] = (Keys.S, Keys.S),
            [PixelGlueButtons.Left] = (Keys.A, Keys.A),
            [PixelGlueButtons.Right] = (Keys.D, Keys.D),
            [PixelGlueButtons.Activate] = (Keys.E, Keys.E),
            [PixelGlueButtons.DbgSwitchScene] = (Keys.T, Keys.T),
            [PixelGlueButtons.DbgOpenDialog] = (Keys.O, Keys.O),
            [PixelGlueButtons.EscapeMenu] = (Keys.Escape, Keys.Escape),
            [PixelGlueButtons.ConsoleToggle] = (Keys.OemTilde, Keys.OemTilde),
            [PixelGlueButtons.DbgBoundingBoxes] = (Keys.B, Keys.Back),
            [PixelGlueButtons.Sprint] = (Keys.LeftShift, Keys.LeftShift),
            [PixelGlueButtons.ScalePlus] = (Keys.PageUp, Keys.PageUp),
            [PixelGlueButtons.ScaleMinus] = (Keys.PageDown, Keys.PageDown),
        };
        public static Dictionary<Keys, PixelGlueButtons> KeybindsToGeneric = new Dictionary<Keys, PixelGlueButtons>
        {
            [Keys.W] = PixelGlueButtons.Up,
            [Keys.S] = PixelGlueButtons.Down,
            [Keys.A] = PixelGlueButtons.Left,
            [Keys.D] = PixelGlueButtons.Right,
            [Keys.E] = PixelGlueButtons.Activate,
            [Keys.T] = PixelGlueButtons.DbgSwitchScene,
            [Keys.O] = PixelGlueButtons.DbgOpenDialog,
            [Keys.Escape] = PixelGlueButtons.EscapeMenu,
            [Keys.OemTilde] = PixelGlueButtons.ConsoleToggle,
            [Keys.B] = PixelGlueButtons.DbgBoundingBoxes,
        };
        public static Dictionary<PixelGlueButtons, Buttons[]> GamepadMap = new Dictionary<PixelGlueButtons, Buttons[]>()
        {
            [PixelGlueButtons.Up] = new[] { Buttons.DPadUp },
            [PixelGlueButtons.Down] = new[] { Buttons.DPadDown },
            [PixelGlueButtons.Left] = new[] { Buttons.DPadLeft },
            [PixelGlueButtons.Right] = new[] { Buttons.DPadRight },
            [PixelGlueButtons.Activate] = new[] { Buttons.A },
            [PixelGlueButtons.DbgSwitchScene] = new[] { Buttons.Y },
            [PixelGlueButtons.DbgOpenDialog] = new[] { Buttons.B },
            [PixelGlueButtons.EscapeMenu] = new[] { Buttons.Start },
            [PixelGlueButtons.ConsoleToggle] = new[] { Buttons.Back },
        };
    }
}