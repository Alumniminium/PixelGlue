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
            if (entity.Has<PositionComponent, CameraComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref var entity = ref World.GetEntity(entityId);
                
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref var cam = ref entity.Get<CameraComponent>();
                
                UpdateZoom(ref entity, ref cam);

                var camLoc = pos.Value - cam.PositionOffset;
                var camX = (int)camLoc.X / Global.TileSize * Global.TileSize;
                var camY = (int)camLoc.Y / Global.TileSize * Global.TileSize;

                cam.ScreenRect = new Rectangle(camX, camY, Global.VirtualScreenWidth, Global.VirtualScreenHeight);
                cam.Transform = Matrix.CreateTranslation(-pos.Value.X, -pos.Value.Y, 0)
                                                     * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f)
                                                     * Matrix.CreateScale(Math.Max(0.5f, cam.Zoom))
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