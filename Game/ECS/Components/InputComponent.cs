using Microsoft.Xna.Framework.Input;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct InputComponent: IEntityComponent
    {
        public int UniqueId {get;set;}
        public GamePadState GamePad{get;set;}
        public MouseState Mouse { get; set; }
        public KeyboardState Keyboard { get; set; }
        public Keys[] OldKeys {get;set;}
        public float Scroll { get; set; }

        public InputComponent(int ownerId)
        {
            UniqueId=ownerId;
            GamePad= new GamePadState();
            Mouse =new MouseState();
            Keyboard=new KeyboardState();
            OldKeys = new Keys[0];
            Scroll=0;
        }
    }
}