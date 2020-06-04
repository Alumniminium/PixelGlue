using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelGlueCore.Configuration
{
    public class UserKeybinds
    {
        public Keys Up = Keys.W;
        public Keys Down = Keys.Down;
        public Keys Left=Keys.Left;
        public Keys Right=Keys.Right;
        public Keys Exit=Keys.Escape;
        public Keys Console=Keys.OemTilde;
    }
    public class UserGamepadBinds
    {
        public Buttons Up = Buttons.DPadUp;
        public Buttons Down =Buttons.DPadDown;
        public Buttons Left=Buttons.DPadLeft;
        public Buttons Right=Buttons.DPadRight;
        public Buttons Exit=Buttons.Start;
        public Buttons Console=Buttons.Back;
    }
}