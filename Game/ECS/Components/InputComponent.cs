using Microsoft.Xna.Framework.Input;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS.Components
{
    public class InputComponent: IEntityComponent
    {
        public GamePadState GamePad{get;set;}
        public MouseState Mouse { get; set; }
        public KeyboardState Keyboard { get; set; }
        public MouseState OldMouse { get; set; }
        public KeyboardState OldKeyboard { get; set; }
        public GamePadState OldGamePad{get;set;}
        public float ScrollWheelValue { get; set; }
        public int PixelOwnerId { get; set; }
    }
}