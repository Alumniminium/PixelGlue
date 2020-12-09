using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.Maths;

namespace Pixel.ECS.Systems.Rendering
{
    public class CameraSystem : PixelSystem<PositionComponent,DrawableComponent,CameraComponent>
    {
        public CameraSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { 
            Name = "Camera System";
        }
        public override void Update(float deltaTime, GCNeutralList<Entity> Entities)
        {
            for(int i =0; i< Entities.Count; i++)
            {
                ref var entity = ref Entities[i];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drw = ref entity.Get<DrawableComponent>();
                ref var cam = ref entity.Get<CameraComponent>();
                
                if (entity.Has<MouseComponent>())
                {
                    ref readonly var mos = ref entity.Get<MouseComponent>();

                    if (mos.CurrentState.ScrollWheelValue > mos.OldState.ScrollWheelValue)
                        cam.Zoom *= 2f;
                    else if (mos.CurrentState.ScrollWheelValue < mos.OldState.ScrollWheelValue)
                        cam.Zoom /= 2f;
                }
                
                var entityCenter = pos.Value + (drw.SrcRect.Size.ToVector2() /2);
                var camCenter = pos.Value - cam.PositionOffset;

                var camGridPos = PixelMath.ToGridPosition(pos.Value);

                cam.ScreenRect = new Rectangle(camGridPos.X, camGridPos.Y, Global.VirtualScreenWidth, Global.VirtualScreenHeight);
                cam.Transform = Matrix.CreateTranslation(-entityCenter.X, -entityCenter.Y, 0)
                                 * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f)
                                 * Matrix.CreateScale(Math.Max(0.01f, cam.Zoom))
                                 * Matrix.CreateTranslation(Global.ScreenWidth / 2, Global.ScreenHeight / 2, 0);
            }
        }

        private static void UpdateZoom(ref Entity entity, ref CameraComponent cam)
        {
            
        }
    }
}