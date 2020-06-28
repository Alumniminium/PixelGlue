using Microsoft.Xna.Framework.Input;
using Pixel.Enums;
using Pixel.ECS.Systems;

namespace Pixel.ECS.Components
{
    public struct InputComponent: IEntityComponent
    {
        public int EntityId {get;set;}
        public GamePadState GamePad{get;set;}
        public MouseState Mouse { get; set; }
        public KeyboardState Keyboard { get; set; }
        public Keys[] OldKeys {get;set;}
        public float Scroll { get; set; }

        public InputComponent(int ownerId)
        {
            EntityId=ownerId;
            GamePad= new GamePadState();
            Mouse =new MouseState();
            Keyboard=new KeyboardState();
            OldKeys = new Keys[0];
            Scroll=0;
        }
    }
}