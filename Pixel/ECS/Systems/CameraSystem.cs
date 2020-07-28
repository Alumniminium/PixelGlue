using System;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class CameraSystem : PixelSystem
    {
        public CameraSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Camera System";
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, DrawableComponent,CameraComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref var entity = ref World.GetEntity(entityId);
                
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drw = ref entity.Get<DrawableComponent>();
                ref var cam = ref entity.Get<CameraComponent>();
                
                UpdateZoom(ref entity, ref cam);
                var entityCenter = pos.Value + (drw.SrcRect.Size.ToVector2() /2);
                var camCenter = pos.Value - cam.PositionOffset;
                var camX = (int)(camCenter.X / Global.TileSize) * Global.TileSize;
                var camY = (int)(camCenter.Y / Global.TileSize) * Global.TileSize;

                cam.ScreenRect = new Rectangle(camX, camY, Global.VirtualScreenWidth, Global.VirtualScreenHeight);
                cam.Transform = Matrix.CreateTranslation(-entityCenter.X, -entityCenter.Y, 0)
                                 * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f)
                                 * Matrix.CreateScale(Math.Max(0.01f, cam.Zoom))
                                 * Matrix.CreateTranslation(Global.ScreenWidth / 2, Global.ScreenHeight / 2, 0);
            }
        }

        private static void UpdateZoom(ref Entity entity, ref CameraComponent cam)
        {
            if (!entity.Has<MouseComponent>())
                return;

            ref readonly var mos = ref entity.Get<MouseComponent>();

            if (mos.Scroll > mos.OldScroll)
                cam.Zoom *= 2f;
            else if (mos.Scroll < mos.OldScroll)
                cam.Zoom /= 2f;
        }
    }
}