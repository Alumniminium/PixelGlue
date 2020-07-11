using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Entities;
using Shared;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Pixel.Enums;
using Shared.Diagnostics;
using Shared.ECS;
using Pixel.ECS.Components;
using Pixel.Helpers;
using Pixel.ECS;

namespace Pixel.Scenes
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public Camera Camera;
        public Entity Player;

        public Scene()=>Camera = new Camera(Vector2.Zero, 0, Vector2.One, (Global.VirtualScreenWidth, Global.VirtualScreenHeight));

        public virtual void Initialize()
        {
            for (int i = 0; i < World.Systems.Count; i++)
                World.Systems[i].Initialize();
            Player = World.CreateEntity();
            ApplyArchetype(ref Player, EntityType.Player);
            IsReady = true;
        }

        public virtual void LoadContent(ContentManager cm)
        {
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            Global.Names = File.ReadAllText("../Build/Content/RuntimeContent/Names.txt").Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Update(GameTime deltaTime)
        {
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var preUpdateTicks = DateTime.UtcNow.Ticks;
                var system = World.Systems[i];
                if (!system.WantsUpdate)
                    continue;
                if (system.IsActive)
                    system.Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
                var postUpdateTicks = DateTime.UtcNow.Ticks;
                Profiler.AddUpdate(system.Name, (postUpdateTicks - preUpdateTicks) / 10000f);
            }
            while (Global.PostUpdateQueue.Count > 0)
                Global.PostUpdateQueue.Dequeue().Invoke();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < World.Systems.Count; i++)
                if (World.Systems[i].IsActive)
                    World.Systems[i].FixedUpdate(deltaTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, transformMatrix: Camera.View(), samplerState: SamplerState.PointClamp);
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var preDrawTicks = DateTime.UtcNow.Ticks;
                var system = World.Systems[i];
                if (!system.WantsDraw)
                    continue;
                if (system.IsActive)
                    system.Draw(sb);
                var postDrawTicks = DateTime.UtcNow.Ticks;
                Profiler.AddDraw(system.Name, (postDrawTicks - preDrawTicks) / 10000f);
            }
            sb.End();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyArchetype(ref Entity entity, EntityType et)
        {
            switch (et)
            {
                case EntityType.Camera:
                    entity.With<TransformComponent>()
                          .With<DbgBoundingBoxComponent>();
                    break;
                case EntityType.Player:
                    entity = entity.With<InputComponent>()
                                .With<PositionComponent>()
                                .With<DestinationComponent>()
                                .With<DbgBoundingBoxComponent>()
                                .With<VelocityComponent>()
                                .With(new SpeedComponent(32))
                                .With(new CameraFollowTagComponent(1))
                                .With(new DrawableComponent("character.png", new Rectangle(0, 2, 16, 16)));

                    var nameTag = World.CreateEntity()
                                .With(new TextComponent($"{entity.EntityId}: {entity}"))
                                .With(new PositionComponent(-16, -16, 0));

                    entity.AddChild(nameTag);
                    break;
                case EntityType.Npc:
                    var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                    var name = Global.Names[Global.Random.Next(0, Global.Names.Length)];

                    entity = entity.With(new DrawableComponent(srcEntity.TextureName, srcEntity.SrcRect))
                                .With<PositionComponent>()
                                .With<DestinationComponent>()
                                .With<VelocityComponent>()
                                .With<DbgBoundingBoxComponent>()
                                .With(new SpeedComponent(32));

                    nameTag = World.CreateEntity()
                                .With(new TextComponent($"{entity.EntityId}: {name}"))
                                .With(new PositionComponent(-16, -16, 0));

                    entity.AddChild(nameTag);
                    break;
            }
        }
    }
}