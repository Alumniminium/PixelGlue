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

                        var camX = (int)camLoc.X / scene.Map.TileWidth * scene.Map.TileWidth;
                        var camY = (int)camLoc.Y / scene.Map.TileHeight * scene.Map.TileHeight;

                        var x = Math.Max(0, Math.Min((scene.Map.Width * scene.Map.TileWidth) - PixelGlue.VirtualScreenWidth, camX - PixelGlue.HalfVirtualScreenWidth));
                        var y = Math.Max(0, Math.Min((scene.Map.Width * scene.Map.TileWidth) - PixelGlue.VirtualScreenHeight, camY - PixelGlue.HalfVirtualScreenHeight));

                        camera.ScreenRect = new Rectangle(x, y, PixelGlue.VirtualScreenWidth, PixelGlue.VirtualScreenHeight);

                        var Limits = new Rectangle(PixelGlue.HalfVirtualScreenWidth, PixelGlue.HalfVirtualScreenHeight, (scene.Map.Width * scene.Map.TileWidth) + PixelGlue.HalfVirtualScreenWidth, (scene.Map.Height * scene.Map.TileHeight) + PixelGlue.HalfVirtualScreenHeight);
                        var cameraSize = new Vector2(PixelGlue.VirtualScreenWidth, PixelGlue.VirtualScreenHeight);
                        var limitWorldMin = new Vector2(Limits.Left, Limits.Top);
                        var limitWorldMax = new Vector2(Limits.Right, Limits.Bottom);
                        var cameraPos = Vector2.Clamp(camLoc, limitWorldMin, limitWorldMax - cameraSize);

                        camera.Transform = Matrix.CreateTranslation(-cameraPos.X, -cameraPos.Y, 0)
                                                             * Matrix.CreateScale(PixelGlue.ScreenWidth / PixelGlue.VirtualScreenWidth, PixelGlue.ScreenHeight / PixelGlue.VirtualScreenHeight, 1f)
                                                             * Matrix.CreateScale(follow.Zoom)
                                                             * Matrix.CreateTranslation(PixelGlue.ScreenWidth / 2, PixelGlue.ScreenHeight / 2, 0);
                    }
                }
            }
        }
    }
}