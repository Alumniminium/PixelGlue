using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework;
using System;

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
            foreach (var kvp in scene.GameObjects)
            {
                if (!kvp.Value.TryGetComponent<PositionComponent>(out var loc))
                    continue;
                if (!kvp.Value.TryGetComponent<CameraFollowTagComponent>(out var follow))
                    continue;

                var camera =scene.Camera;

                var camX = ((int)loc.Position.X /scene.Map.TileWidth) *scene.Map.TileWidth;
                var camY = ((int)loc.Position.Y /scene.Map.TileHeight) *scene.Map.TileHeight;
                var halfWidth = PixelGlue.VirtualScreenWidth / 2;
                var halfHeight = PixelGlue.VirtualScreenHeight / 2;

                var x = Math.Max(0, Math.Min(scene.Map.Width *scene.Map.TileWidth - PixelGlue.VirtualScreenWidth, (camX - halfWidth)));
                var y = Math.Max(0, Math.Min(scene.Map.Width *scene.Map.TileWidth - PixelGlue.VirtualScreenHeight, (camY - halfHeight)));

                camera.ScreenRect = new Rectangle(x, y, PixelGlue.VirtualScreenWidth, PixelGlue.VirtualScreenHeight);

                var Limits = new Rectangle(halfWidth, halfHeight,scene.Map.Width *scene.Map.TileWidth,scene.Map.Height *scene.Map.TileHeight);
                var cameraSize = new Vector2(PixelGlue.VirtualScreenWidth, PixelGlue.VirtualScreenHeight);
                var limitWorldMin = new Vector2(Limits.Left, Limits.Top);
                var limitWorldMax = new Vector2(Limits.Right, Limits.Bottom);

                var cameraPos = Vector2.Clamp(loc.Position, limitWorldMin, limitWorldMax - cameraSize);

                camera.Transform = Matrix.CreateTranslation(-cameraPos.X -scene.Map.TileWidth / 2, -cameraPos.Y, 0)
                                                     * Matrix.CreateScale(PixelGlue.ScreenWidth / PixelGlue.VirtualScreenWidth, PixelGlue.ScreenHeight / PixelGlue.VirtualScreenHeight, 2f)
                                                     * Matrix.CreateScale(follow.Zoom)
                                                     * Matrix.CreateTranslation(PixelGlue.ScreenWidth / 2, PixelGlue.ScreenHeight / 2, 0);

            }
        }
    }
}