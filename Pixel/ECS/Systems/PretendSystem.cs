using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared;
using Pixel.Enums;
using Shared.ECS;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class PretendSystem : PixelSystem
    {
        public PretendSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Camera System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<CameraFollowTagComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public override void Update(float dt)
        {
            var scene = SceneManager.ActiveScene;
                var entity = World.CreateEntity();
                scene.ApplyArchetype(ref entity, EntityType.Npc);
                var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                entity.Get<PositionComponent>().Value = Scene.Player.Get<PositionComponent>().Value;
                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                entity.Get<DestinationComponent>().Value = new Vector2(1024, 2048);
        }
    }
}