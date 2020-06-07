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
        public float ScrollWheelValue { get; set; }
        public int PixelOwnerId { get; set; }

        public InputComponent(int ownerId)
        {
            PixelOwnerId=ownerId;
            GamePad = new GamePadState();
            Mouse = new MouseState();
            Keyboard = new KeyboardState();
            OldKeys = new Keys[0];
            ScrollWheelValue=0;
        }
    }
}