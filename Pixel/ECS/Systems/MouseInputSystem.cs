using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared.ECS;
using Shared.IO;

namespace Pixel.ECS.Systems
{
    public class MouseInputSystem : PixelSystem
    {
        public override string Name { get; set; } = "Mouse Input System";
        public MouseInputSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<MouseComponent>())
                base.AddEntity(entityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                ref var inp = ref entity.Get<MouseComponent>();

                var mouse = Mouse.GetState();

                inp.OldScroll = inp.Scroll;
                inp.Scroll = mouse.ScrollWheelValue;
                
                ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
                var worldPos = cam.ScreenToWorld(mouse.Position.ToVector2());
                inp.X = worldPos.X;
                inp.Y = worldPos.Y;
            }
        }
    }
}