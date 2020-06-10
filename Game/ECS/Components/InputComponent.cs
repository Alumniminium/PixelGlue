using Microsoft.Xna.Framework.Input;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public struct InputComponent
    {
        public GamePadState GamePad{get;set;}
        public MouseState Mouse { get; set; }
        public KeyboardState Keyboard { get; set; }
        public Keys[] OldKeys {get;set;}
        public float Scroll { get; set; }
    }
}