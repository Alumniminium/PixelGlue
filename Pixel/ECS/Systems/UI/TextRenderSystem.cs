using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class TextRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Name Tag Render System";

        public TextRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override bool MatchesFilter(Entity entity) => entity.Has<TextComponent, PositionComponent>() && entity.Parent != 0;
        
        public override void Draw(SpriteBatch sb)
        {
            foreach (var id in Entities)
            {
                ref readonly var entity = ref World.GetEntity(id);
                ref readonly var parent = ref World.GetEntity(entity.Parent);
                
                if(!parent.Has<PositionComponent,TextComponent>())
                    continue;
                    
                ref readonly var parentPos = ref parent.Get<PositionComponent>();
                ref readonly var txt = ref entity.Get<TextComponent>();
                ref readonly var posOff = ref entity.Get<PositionComponent>();

                var textPos = parentPos.Value + posOff.Value;
                AssetManager.Fonts[txt.FontName].DrawText(sb, textPos.X, textPos.Y, txt.Value, Color.Blue);
            }
        }
    }
}