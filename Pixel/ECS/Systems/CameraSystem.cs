using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class CameraSystem : PixelSystem
    {
        public override string Name { get; set; } = "Camera System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<CameraFollowTagComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                var entity = Entities[i];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref var fol = ref entity.Get<CameraFollowTagComponent>();

                Scene.Camera.XY = pos.Value + fol.PositionOffset;
            }
        }
    }
}