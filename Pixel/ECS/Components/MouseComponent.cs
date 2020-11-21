using Microsoft.Xna.Framework.Input;
using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct MouseComponent
    {
        public MouseState CurrentState, OldState;
        public float WorldX,WorldY;
    }
}