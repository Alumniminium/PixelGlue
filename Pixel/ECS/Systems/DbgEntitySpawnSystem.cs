using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared;
using Pixel.Enums;
using Shared.ECS;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class DbgEntitySpawnSystem : PixelSystem
    {
        public override string Name { get; set; } = "Camera System";
        public Scene Scene => SceneManager.ActiveScene;

        public DbgEntitySpawnSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<PositionComponent, CameraComponent>())
                base.AddEntity(entityId);
        }
        public override void FixedUpdate(float dt)
        {
            var entity = World.CreateEntity();
            Scene.ApplyArchetype(ref entity, EntityType.Npc);
            var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
            ref var pos = ref entity.Get<PositionComponent>();
            ref var drw = ref entity.Get<DrawableComponent>();
            ref var dst = ref entity.Get<DestinationComponent>();
            ref var playerpos = ref Scene.Player.Get<PositionComponent>();
            pos.Value = playerpos.Value;

            drw.SrcRect = srcEntity.SrcRect;
            drw.TextureName = srcEntity.TextureName;
            dst.Value = new Vector2(int.MaxValue, int.MaxValue);
            entity.Register();
        }
    }
}