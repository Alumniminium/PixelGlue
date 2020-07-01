using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct InputComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public Vector2 Axis{get;set;}
        public PixelGlueButtons[] Buttons {get;set;}
        public PixelGlueButtons[] OldButtons { get; set; }
        public float Scroll { get; set; }

        public InputComponent(int ownerId)
        {
            EntityId = ownerId;
            Axis = Vector2.Zero;
            Buttons = new PixelGlueButtons[0];
            OldButtons = new PixelGlueButtons[0];
            Scroll = 0;
        }
    }
}