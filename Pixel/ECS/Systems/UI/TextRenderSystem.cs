using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class TextRenderSystem : PixelSystem<TextComponent,PositionComponent>
    {
        public TextRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) {Name= "Name Tag Render System"; }
        
        public override void Draw(SpriteBatch sb)
        {   
            foreach (var entityList in Entities)
            for(int i =0; i< entityList.Count; i++)
            {
                ref var entity = ref entityList[i];
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