using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.World;
using Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Pixel.Enums;
using Shared.Diagnostics;

namespace Pixel.ECS
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public int LastEntityId = 1;
        public ConcurrentDictionary<int, Entity> Entities;
        public ConcurrentDictionary<int, int> UniqueIdToEntityId, EntityIdToUniqueId;
        public List<PixelSystem> Systems;
        public Camera Camera;
        public Entity Player;

        public Queue<Action> PostUpdateQueue = new Queue<Action>(8);

        public Scene()
        {
            Entities = new ConcurrentDictionary<int, Entity>();
            UniqueIdToEntityId = new ConcurrentDictionary<int, int>();
            EntityIdToUniqueId = new ConcurrentDictionary<int, int>();
            Systems = new List<PixelSystem>();
        }

        public virtual void Initialize()
        {
            Camera = new Camera();
            Player = CreateEntity(EntityType.Player);
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].Initialize();
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
            for (int i = 0; i < Systems.Count; i++)
            {
                var preUpdateTicks = DateTime.UtcNow.Ticks;
                if (Systems[i].IsActive)
                    Systems[i].Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
                var postUpdateTicks = DateTime.UtcNow.Ticks;
                Profiler.AddUpdate(Systems[i].Name, (postUpdateTicks - preUpdateTicks) / 10000f);
            }
            while (PostUpdateQueue.Count > 0)
                PostUpdateQueue.Dequeue().Invoke();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
                if (Systems[i].IsActive)
                    Systems[i].FixedUpdate(deltaTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, transformMatrix: Camera.ViewMatrix, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < Systems.Count; i++)
            {
                var preDrawTicks = DateTime.UtcNow.Ticks;
                if (Systems[i].IsActive)
                    Systems[i].Draw(sb);
                var postDrawTicks = DateTime.UtcNow.Ticks;
                Profiler.AddDraw(Systems[i].Name, (postDrawTicks - preDrawTicks) / 10000f);
            }
            sb.End();
        }

        public virtual void Destroy(Entity entity) => Destroy(entity.EntityId);
        public virtual void Destroy(int entity)
        {
            Entities.TryRemove(entity, out var actualEntity);
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].RemoveEntity(actualEntity);

            if (actualEntity.EntityId != -1)
            {
                actualEntity.DestroyComponents();
                if (actualEntity.Children != null)
                {
                    foreach (var id in actualEntity.Children)
                    {
                        var child = Entities[id];
                        child.DestroyComponents();
                        Destroy(child.EntityId);
                    }
                }
            }
            EntityIdToUniqueId.TryRemove(entity, out var uid);
            UniqueIdToEntityId.TryRemove(uid, out _);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Entity CreateEntity(int uniqueId, EntityType et = EntityType.Default)
        {
            var entity = new Entity
            {
                EntityId = LastEntityId++,
            };
            ApplyArchetype(ref entity, et);
            entity.Add(new NetworkComponent(uniqueId));
            UniqueIdToEntityId.TryAdd(uniqueId, entity.EntityId);
            EntityIdToUniqueId.TryAdd(entity.EntityId, uniqueId);
            Entities.TryAdd(entity.EntityId, entity);

            for (int i = 0; i < Systems.Count; i++)
                Systems[i].AddEntity(entity);
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Entity CreateEntity(EntityType et)
        {
            var entity = new Entity
            {
                EntityId = LastEntityId++,
                Valid = true
            };
            ApplyArchetype(ref entity, et);
            Entities.TryAdd(entity.EntityId, entity);
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].AddEntity(entity);
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual T GetSystem<T>()
        {
            foreach (var sys in Systems)
                if (sys is T t)
                    return t;
            return default;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyArchetype(ref Entity entity, EntityType et)
        {
            switch (et)
            {
                case EntityType.Camera:
                    entity.Add<TransformComponent>();
                    entity.Add<DbgBoundingBoxComponent>();
                    break;
                case EntityType.Player:
                    entity.Add<InputComponent>();
                    entity.Add<PositionComponent>();
                    entity.Add<DestinationComponent>();
                    entity.Add<DbgBoundingBoxComponent>();
                    entity.Add(new CameraFollowTagComponent(1));
                    entity.Add<VelocityComponent>();

                    entity.Add(new DrawableComponent("character.png", new Rectangle(0, 2, 16, 16)));
                    entity.Add(new SpeedComponent(64));

                    var nt = CreateEntity(EntityType.Default);
                    nt.Add(new TextComponent($"{entity.EntityId}: {entity}", "profont"));
                    nt.Add(new PositionComponent(-16, -16, 0));
                    nt.Parent = entity.EntityId;
                    entity.Children = new List<int> { nt.EntityId };
                    break;
                case EntityType.Npc:
                    var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                    entity.Add(new DrawableComponent(srcEntity.TextureName, srcEntity.SrcRect));
                    entity.Add<PositionComponent>();
                    entity.Add<DestinationComponent>();
                    entity.Add<VelocityComponent>();
                    entity.Add<DbgBoundingBoxComponent>();
                    entity.Add(new SpeedComponent(32));
                    var name = Global.Names[Global.Random.Next(0, Global.Names.Length)];
                    nt = CreateEntity(EntityType.Default);
                    nt.Add(new TextComponent($"{entity.EntityId}: {name}", "profont"));
                    nt.Add(new PositionComponent(-16, -16, 0));
                    nt.Parent = entity.EntityId;
                    entity.Children = new List<int> { nt.EntityId };
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Id;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;
    }
}