using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct InputComponent
    {
        public Vector2 Axis{get;set;}
        public PixelGlueButtons[] Buttons {get;set;}
        public PixelGlueButtons[] OldButtons { get; set; }
        public float Scroll { get; set; }
    }
}