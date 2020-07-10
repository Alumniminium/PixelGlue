using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Scenes;
using Shared;

namespace Pixel.ECS.Systems
{
    public class CameraSystem : PixelSystem
    {
        public override string Name { get; set; } = "Camera System";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<CameraFollowTagComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            var scene = SceneManager.ActiveScene;
            for (int i = 0; i < Entities.Count; i++)
            {
                var entity = Entities[i];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref var fol = ref entity.Get<CameraFollowTagComponent>();

                if (entity.Has<InputComponent>())
                {
                    ref readonly var inp = ref entity.Get<InputComponent>();
                    if (inp.Scroll > inp.OldScroll)
                        fol.Zoom *= 2f;
                    else if (inp.Scroll < inp.OldScroll)
                        fol.Zoom /= 2f;
                }
                var scaleX = Global.ScreenWidth / Global.VirtualScreenWidth;
                var scaleY = Global.ScreenHeight / Global.VirtualScreenHeight;

                var camPos = pos.Value + fol.PositionOffset;
                var camX = (int)(camPos.X / Global.TileSize) * Global.TileSize;
                var camY = (int)(camPos.Y / Global.TileSize) * Global.TileSize;
            
                var simRectX = (int)Math.Floor(camX+fol.PositionOffset.X) - (Global.HalfVirtualScreenWidth + (Global.TileSize * 2));
                var simRectY = (int)Math.Floor(camY+fol.PositionOffset.Y) - (Global.HalfVirtualScreenHeight + (Global.TileSize * 2));
                var simRectW = Global.VirtualScreenWidth + (Global.TileSize * 4);
                var simRectH = Global.VirtualScreenHeight + (Global.TileSize * 4);

                var screenRectX = (int)Math.Floor(camX+fol.PositionOffset.X) - Global.HalfVirtualScreenWidth;
                var screenRectY = (int)Math.Floor(camY+fol.PositionOffset.Y) - Global.HalfVirtualScreenHeight;
                var screenRectW = Global.VirtualScreenWidth;
                var screenRectH = Global.VirtualScreenHeight;

                scene.Camera.DrawRect = new Rectangle(screenRectX, screenRectY, screenRectW, screenRectH);

                screenRectX = (int)(camX - (Global.HalfVirtualScreenWidth / fol.Zoom));
                screenRectY = (int)(camY - (Global.HalfVirtualScreenHeight / fol.Zoom));
                screenRectW = (int)(Global.VirtualScreenWidth / fol.Zoom);
                screenRectH = (int)(Global.VirtualScreenHeight / fol.Zoom);
                
                scene.Camera.DrawRectZoomed = new Rectangle(screenRectX, screenRectY, screenRectW, screenRectH);
                scene.Camera.SimulationRect = new Rectangle(simRectX, simRectY, simRectW, simRectH);

                scene.Camera.ViewMatrix = Matrix.CreateTranslation(-camPos.X, -camPos.Y, 0)
                                            * Matrix.CreateScale(scaleX, scaleY, 1f)
                                            * Matrix.CreateScale(fol.Zoom)
                                            * Matrix.CreateTranslation(Global.HalfScreenWidth, Global.HalfScreenHeight, 0);
            }
        }
    }
}