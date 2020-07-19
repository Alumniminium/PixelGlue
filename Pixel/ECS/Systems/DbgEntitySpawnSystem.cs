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
        public override string Name { get; set; } = "Entity Spawner System";

        public DbgEntitySpawnSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, CameraComponent, DrawableComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float dt)
        {
            for (int i = 0; i < 10; i++)
            {
                ref var entity = ref World.CreateEntity();
                SceneManager.ActiveScene.ApplyArchetype(ref entity, EntityType.Npc);

                var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                ref var pos = ref entity.Get<PositionComponent>();
                ref var drw = ref entity.Get<DrawableComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();

                ref readonly var playerPos = ref SceneManager.ActiveScene.Player.Get<PositionComponent>();
                pos.Value = playerPos.Value;

                drw.SrcRect = srcEntity.SrcRect;
                drw.TextureName = srcEntity.TextureName;
                dst.Value = new Vector2(Global.Random.Next(int.MinValue, int.MaxValue), Global.Random.Next(int.MinValue, int.MaxValue));
                entity.Register();
            }
        }
    }
}