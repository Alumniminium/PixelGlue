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
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<CameraFollowTagComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public override void Update(float dt)
        {
            var scene = SceneManager.ActiveScene;
            var entity = World.CreateEntity();
            scene.ApplyArchetype(ref entity, EntityType.Npc);
            var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
            ref var pos = ref entity.Get<PositionComponent>();
            ref var drw = ref entity.Get<DrawableComponent>();
            ref var dst = ref entity.Get<DestinationComponent>();
            ref var playerpos = ref Scene.Player.Get<PositionComponent>();
            pos.Value = playerpos.Value;
            
            drw.SrcRect = srcEntity.SrcRect;
            drw.TextureName = srcEntity.TextureName;
            dst.Value = new Vector2(1024, 2048);
            entity.Register();
        }
    }
}