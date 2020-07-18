using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class CameraSystem : PixelSystem
    {
        public CameraSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Camera System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<PositionComponent, CameraComponent>())
                base.AddEntity(entityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref var cam = ref entity.Get<CameraComponent>();

                if (entity.Has<KeyboardComponent>())
                {
                    ref readonly var inp = ref entity.Get<MouseComponent>();
                    if (inp.Scroll > inp.OldScroll)
                        cam.Zoom *= 2f;
                    else if (inp.Scroll < inp.OldScroll)
                        cam.Zoom /= 2f;
                }
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
    }
}