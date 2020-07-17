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
        public Scene Scene => SceneManager.ActiveScene;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (!entity.Has<PositionComponent>())
                return;
            if(entity.Children == null)
                return;
            foreach (var id in entity.Children)
            {
                var child = World.Entities[id];
                if (child.Has<TextComponent>() || child.Has<PositionComponent>())
                {
                    base.AddEntity(entityId);
                    break;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                foreach (var id in entity.Children)
                {
                    var child = World.Entities[id];
                    if (!child.Has<TextComponent>() || !child.Has<PositionComponent>())
                        continue;

                    ref readonly var pos = ref entity.Get<PositionComponent>();
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);

                    if (pos.Value.X <= cam.ScreenRect.Left || pos.Value.X >=cam.ScreenRect.Right)
                        continue;
                    if (pos.Value.Y <= cam.ScreenRect.Top || pos.Value.Y >= cam.ScreenRect.Bottom)
                        continue;

                    //if (pos.Value.X <= Scene.Camera.SimulationRect.Left || pos.Value.X >= Scene.Camera.SimulationRect.Right)
                    //    continue;
                    //if (pos.Value.Y <= Scene.Camera.SimulationRect.Top || pos.Value.Y >= Scene.Camera.SimulationRect.Bottom)
                    //    continue;

                    ref readonly var offset = ref child.Get<PositionComponent>();
                    ref readonly var text = ref child.Get<TextComponent>();

                    if (!string.IsNullOrEmpty(text.Value))
                    {
                        var p = pos.Value + offset.Value;
                        AssetManager.Fonts[text.FontName].DrawText(sb, p.X, p.Y, text.Value, Color.Blue, 0.2f);
                    }
                }
            }
        }
    }
}