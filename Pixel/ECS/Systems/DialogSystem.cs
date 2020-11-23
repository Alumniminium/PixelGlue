using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class DialogSystem : PixelSystem
    {
        public override string Name { get; set; } = "Dialog System";
        public DialogSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override bool MatchesFilter(Entity entity) => entity.Has<PositionComponent, DrawableComponent>() && entity.Children != null;
        
        public override void Draw(SpriteBatch sb)
        {
        }
    }
}