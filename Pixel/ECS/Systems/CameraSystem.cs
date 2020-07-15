using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Pixel.Scenes;
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
            if (entity.Has<CameraFollowTagComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
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

                Scene.Camera.Z = fol.Zoom;
                Scene.Camera.XY = pos.Value + fol.PositionOffset;
            }
        }
    }
}