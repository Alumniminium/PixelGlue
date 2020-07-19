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
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<MouseComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            var mouse = Mouse.GetState();
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var mos = ref entity.Get<MouseComponent>();

                mos.OldScroll = mos.Scroll;
                mos.Scroll = mouse.ScrollWheelValue;
                
                //if (mouse.LeftButton == ButtonState.Pressed)
                //{
                //    ref readonly var came = ref ComponentArray<CameraComponent>.Get(1);
                //    var point = came.ScreenToWorld(mouse.Position.ToVector2());
                //    point.X = (int)point.X / Global.TileSize;
                //    point.Y = (int)point.Y / Global.TileSize;
                //    point.X = (int)point.X * Global.TileSize;
                //    point.Y = (int)point.Y * Global.TileSize;
                //    Entity selected = default;
//
                //    foreach (var kvp in World.EntityToArrayOffset)
                //    {
                //        ref readonly var found = ref World.GetEntity(kvp.Key);
                //        if (found.Has<PositionComponent>() && found.Has<DrawableComponent>())
                //        {
                //            ref readonly var pos = ref found.Get<PositionComponent>();
                //            if (pos.Value == point)
                //            {
                //                selected = World.GetEntity(kvp.Key);
                //                break;
                //            }
                //        }
                //    }
                //    World.Destroy(selected.EntityId);
                //}

                ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
                var worldPos = cam.ScreenToWorld(mouse.Position.ToVector2());
                mos.X = worldPos.X;
                mos.Y = worldPos.Y;
            }
        }
    }
}