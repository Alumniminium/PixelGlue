using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using PixelGlueCore.Scenes;
using PixelGlueCore.Helpers;

namespace PixelGlueCore.ECS.Systems
{
    public class CameraSystem : IEntitySystem
    {
        public string Name { get; set; } = "Camera System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void Update(double deltaTime)
        {
            foreach (var scene in SceneManager.ActiveScenes)
            foreach (var kvp in scene.Entities)
            {
                if (!scene.TryGetComponent<PositionComponent>(kvp.Key,out var loc))
                    continue;
                if (!scene.TryGetComponent<CameraFollowTagComponent>(kvp.Key,out var follow))
                    continue;

                var camera =scene.Camera;

                var camX = ((int)loc.IntegerPosition.X /scene.Map.TileWidth) *scene.Map.TileWidth;
                var camY = ((int)loc.IntegerPosition.Y /scene.Map.TileHeight) *scene.Map.TileHeight;
                
                var x = Math.Max(0, Math.Min(scene.Map.Width *scene.Map.TileWidth - PixelGlue.VirtualScreenWidth, (camX - PixelGlue.HalfVirtualScreenWidth)));
                var y = Math.Max(0, Math.Min(scene.Map.Width *scene.Map.TileWidth - PixelGlue.VirtualScreenHeight, (camY -  PixelGlue.HalfVirtualScreenHeight)));

                camera.ScreenRect = new Rectangle(x, y, PixelGlue.VirtualScreenWidth, PixelGlue.VirtualScreenHeight);

                var Limits = new Rectangle(PixelGlue.HalfVirtualScreenWidth, PixelGlue.HalfVirtualScreenHeight,scene.Map.Width *scene.Map.TileWidth,scene.Map.Height *scene.Map.TileHeight);
                var cameraSize = new Vector2(PixelGlue.VirtualScreenWidth, PixelGlue.VirtualScreenHeight);
                var limitWorldMin = new Vector2(Limits.Left, Limits.Top);
                var limitWorldMax = new Vector2(Limits.Right, Limits.Bottom);

                var cameraPos = Vector2.Clamp(loc.Position.DrawablePosition(), limitWorldMin, limitWorldMax - cameraSize);

                camera.Transform = Matrix.CreateTranslation(-cameraPos.X -scene.Map.TileWidth / 2, -cameraPos.Y, 0)
                                                     * Matrix.CreateScale(PixelGlue.ScreenWidth / PixelGlue.VirtualScreenWidth, PixelGlue.ScreenHeight / PixelGlue.VirtualScreenHeight, 2f)
                                                     * Matrix.CreateScale(follow.Zoom)
                                                     * Matrix.CreateTranslation(PixelGlue.ScreenWidth / 2, PixelGlue.ScreenHeight / 2, 0);

            }
        }
    }
}