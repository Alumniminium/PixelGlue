using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class DialogSystem : PixelSystem<PositionComponent, DrawableComponent>
    {
        public DialogSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name= "Dialog System"; }
    }
}