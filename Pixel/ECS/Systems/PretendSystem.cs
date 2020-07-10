using System.Net.NetworkInformation;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Scenes;
using Pixel.World;
using Shared;
using Pixel.Enums;
using System.Threading;

namespace Pixel.ECS.Systems
{
    public class PretendSystem : PixelSystem
    {
        public override string Name { get; set; } = "Camera System";

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
                var entity = scene.CreateEntity(EntityType.Npc);
                var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                entity.Get<PositionComponent>().Value = Scene.Player.Get<PositionComponent>().Value;
                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                entity.Get<DestinationComponent>().Value = new Vector2(1024, 2048);
        }
    }
}