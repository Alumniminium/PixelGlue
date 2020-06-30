using Pixel.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using Pixel.Scenes;
using Pixel.Enums;
using System.Collections.Generic;
using Pixel.Helpers;
using PixelShared;

namespace Pixel.ECS.Systems
{
    public class CameraSystem : IEntitySystem
    {
        public string Name { get; set; } = "Camera System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            var scene = SceneManager.ActiveScene;
            foreach (var entity in CompIter.Get<CameraFollowTagComponent, PositionComponent>())
            {
                ref readonly var loc = ref entity.Get<PositionComponent>();
                ref readonly var follow = ref entity.Get<CameraFollowTagComponent>();

                var camLoc = loc.Position + new Vector2(8);
                var camX = (int)camLoc.X / PixelShared.Pixel.TileSize * PixelShared.Pixel.TileSize;
                var camY = (int)camLoc.Y / PixelShared.Pixel.TileSize * PixelShared.Pixel.TileSize;

                scene.Camera.ScreenRect = new Rectangle((int)(camX - (Global.HalfVirtualScreenWidth / follow.Zoom)), (int)(camY - (Global.HalfVirtualScreenHeight / follow.Zoom)), (int)(Global.VirtualScreenWidth / follow.Zoom), (int)(Global.VirtualScreenHeight / follow.Zoom));
                scene.Camera.Transform = Matrix.CreateTranslation(-camLoc.X, -camLoc.Y, 0)
                                                     * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f)
                                                     * Matrix.CreateScale(follow.Zoom)
                                                     * Matrix.CreateTranslation(Global.ScreenWidth / 2, Global.ScreenHeight / 2, 0);
            }
        }
    }
}