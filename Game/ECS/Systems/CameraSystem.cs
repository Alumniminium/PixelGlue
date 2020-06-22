using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using PixelGlueCore.Scenes;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS.Systems
{
    public class CameraSystem : IEntitySystem
    {
        public string Name { get; set; } = "Camera System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            foreach (var s in SceneManager.ActiveGameScenes)
            {
                if (s is GameScene scene)
                {
                    foreach (var (_, entity) in scene.Entities)
                    {
                        if (!entity.Has<CameraFollowTagComponent>() || !entity.Has<PositionComponent>())
                            continue;
                        ref var follow = ref entity.Get<CameraFollowTagComponent>();
                        ref var loc = ref entity.Get<PositionComponent>();
                        var camera = scene.Camera;
                        var camLoc = loc.Position + new Vector2(8, 8);
                        follow.Zoom = (float)Math.Max(0.01f,follow.Zoom);
                        var camX = (int)camLoc.X / scene.Map.TileWidth * scene.Map.TileWidth;
                        var camY = (int)camLoc.Y / scene.Map.TileHeight * scene.Map.TileHeight;
                        camera.ScreenRect = new Rectangle((int)(camX - (PixelGlue.HalfVirtualScreenWidth/follow.Zoom)),(int)(camY- (PixelGlue.HalfVirtualScreenHeight/follow.Zoom)), (int)(PixelGlue.VirtualScreenWidth/follow.Zoom), (int)(PixelGlue.VirtualScreenHeight/follow.Zoom));
                        camera.Transform = Matrix.CreateTranslation(-camLoc.X, -camLoc.Y, 0)
                                                             * Matrix.CreateScale(PixelGlue.ScreenWidth / PixelGlue.VirtualScreenWidth, PixelGlue.ScreenHeight / PixelGlue.VirtualScreenHeight, 1f)
                                                             * Matrix.CreateScale(follow.Zoom)
                                                             * Matrix.CreateTranslation(PixelGlue.ScreenWidth / 2, PixelGlue.ScreenHeight / 2, 0);
                    }
                }
            }
        }
    }
}