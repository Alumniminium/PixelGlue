using System.Runtime.CompilerServices;
using Pixel.ECS.Components;

using Shared.ECS;
using Pixel.Scenes;

namespace Pixel.ECS.Systems
{
    public class CursorMoveSystem : PixelSystem
    {
        public CursorMoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Cursor Move System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                ref readonly var mos = ref entity.Get<MouseComponent>();
                ref var pos = ref entity.Get<PositionComponent>();

                pos.Value.X = mos.X;
                pos.Value.Y = mos.Y;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<PositionComponent, MouseComponent>() && !entity.Has<VelocityComponent, SpeedComponent>())
                base.AddEntity(entityId);
        }
    }
}