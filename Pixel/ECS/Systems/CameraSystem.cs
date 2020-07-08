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
        
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<CameraFollowTagComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entity);
        }
        public override void Update(float deltaTime)
        {
            var scene = SceneManager.ActiveScene;
            foreach (var entity in Entities)
            {
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref var fol = ref entity.Get<CameraFollowTagComponent>();

                if(entity.Has<InputComponent>())
                {
                    ref readonly var inp = ref entity.Get<InputComponent>();
                    if (inp.Scroll > inp.OldScroll)
                        fol.Zoom *= 2f;
                    else if (inp.Scroll < inp.OldScroll)
                        fol.Zoom /= 2f;
                }

                var camLoc = pos.Value + fol.PositionOffset;
                var camX = (int)(camLoc.X / Global.TileSize) * Global.TileSize;
                var camY = (int)(camLoc.Y / Global.TileSize) * Global.TileSize;

                scene.Camera.ScreenRect = new Rectangle((int)(camX - (Global.HalfVirtualScreenWidth / fol.Zoom)), (int)(camY - (Global.HalfVirtualScreenHeight / fol.Zoom)), (int)(Global.VirtualScreenWidth / fol.Zoom), (int)(Global.VirtualScreenHeight / fol.Zoom));
                scene.Camera.ServerScreenRect = new Rectangle((int)camLoc.X - (Global.HalfVirtualScreenWidth+ (Global.TileSize * 4)), (int)camLoc.Y - (Global.HalfVirtualScreenHeight + (Global.TileSize * 4)), Global.VirtualScreenWidth+ (Global.TileSize * 4), Global.VirtualScreenHeight+ (Global.TileSize * 4));

                scene.Camera.ViewMatrix = Matrix.CreateTranslation(-camLoc.X, -camLoc.Y, 0)
                                                     * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f)
                                                     * Matrix.CreateScale(fol.Zoom)
                                                     * Matrix.CreateTranslation(Global.HalfScreenWidth, Global.HalfScreenHeight, 0);
            }
        }
    }
}