using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared.ECS;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class NameTagRenderSystem : PixelSystem
    {
        public NameTagRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Name Tag Render System";
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Parent == 0)
                return;
            if (entity.Has<TextComponent, PositionComponent>())
                base.AddEntity(entityId);
        }
        public override void Draw(SpriteBatch sb)
        {
            foreach (var entityId in Entities)
            {
                var nameTag = World.Entities[entityId];
                if (nameTag.Parent != 0)
                {
                    var parent = World.Entities[nameTag.Parent];

                    ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);

                    ref readonly var parentPos = ref parent.Get<PositionComponent>();
                    ref readonly var txt = ref nameTag.Get<TextComponent>();
                    ref readonly var posOff = ref nameTag.Get<PositionComponent>();

                    //if (parentPos.Value.X + posOff.Value.X <= cam.ScreenRect.Left || parentPos.Value.X + posOff.Value.X >= cam.ScreenRect.Right)
                    //    continue;
                    //if (parentPos.Value.Y + posOff.Value.Y <= cam.ScreenRect.Top || parentPos.Value.Y + posOff.Value.Y >= cam.ScreenRect.Bottom)
                    //    continue;

                    if (!string.IsNullOrEmpty(txt.Value))
                    {
                        var p = parentPos.Value + posOff.Value;
                        AssetManager.Fonts[txt.FontName].DrawText(sb, p.X, p.Y, txt.Value, Color.Blue, 0.2f);
                    }
                }
            }
        }
    }
}