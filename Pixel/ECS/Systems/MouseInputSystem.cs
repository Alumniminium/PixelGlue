using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;

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
            var mouse = Mouse.GetState();
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                ref var mos = ref entity.Get<MouseComponent>();

                mos.OldScroll = mos.Scroll;
                mos.Scroll = mouse.ScrollWheelValue;
                
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    ref readonly var came = ref ComponentArray<CameraComponent>.Get(1);
                    var point = came.ScreenToWorld(mouse.Position.ToVector2());
                    point.X = (int)point.X / Global.TileSize;
                    point.Y = (int)point.Y / Global.TileSize;
                    point.X = (int)point.X * Global.TileSize;
                    point.Y = (int)point.Y * Global.TileSize;
                    Entity selected = default;
                    foreach (var kvp in World.Entities)
                    {
                        if (kvp.Value.Has<PositionComponent>() && kvp.Value.Has<DrawableComponent>())
                        {
                            ref readonly var pos = ref kvp.Value.Get<PositionComponent>();
                            if (pos.Value == point)
                            {
                                selected = kvp.Value;
                                break;
                            }
                        }
                    }
                    World.Destroy(selected.EntityId);
                }

                ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
                var worldPos = cam.ScreenToWorld(mouse.Position.ToVector2());
                mos.X = worldPos.X;
                mos.Y = worldPos.Y;
            }
        }
    }
}