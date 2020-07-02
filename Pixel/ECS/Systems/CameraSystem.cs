using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
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
            foreach (var entity in CompIter<CameraFollowTagComponent, PositionComponent>.Get())
            {
                ref readonly var pos = ref ComponentArray<PositionComponent>.Get(entity);
                ref readonly var fol = ref ComponentArray<CameraFollowTagComponent>.Get(entity);

                var camLoc = pos.Position + fol.PositionOffset;
                var camX = (int)(camLoc.X / Global.TileSize) * Global.TileSize;
                var camY = (int)(camLoc.Y / Global.TileSize) * Global.TileSize;

                scene.Camera.ScreenRect = new Rectangle((int)(camX - (Global.HalfVirtualScreenWidth / fol.Zoom)), (int)(camY - (Global.HalfVirtualScreenHeight / fol.Zoom)), (int)(Global.VirtualScreenWidth / fol.Zoom), (int)(Global.VirtualScreenHeight / fol.Zoom));
                scene.Camera.ServerScreenRect = new Rectangle((int)camX - Global.VirtualScreenWidth, (int)camY - Global.VirtualScreenHeight, (int)Global.VirtualScreenWidth*2, (int)Global.VirtualScreenHeight*2);
                scene.Camera.Transform.ViewMatrix = Matrix.CreateTranslation(-camLoc.X, -camLoc.Y, 0)
                                                     * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f)
                                                     * Matrix.CreateScale(fol.Zoom)
                                                     * Matrix.CreateTranslation(Global.ScreenWidth / 2, Global.ScreenHeight / 2, 0);
            }
        }
    }
}