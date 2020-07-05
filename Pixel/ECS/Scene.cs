using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.World;
using PixelShared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Pixel.ECS
{
    public static class Profiler
    {
        private static readonly Dictionary<string, List<float>> SystemUpdateTimes = new Dictionary<string, List<float>>();
        private static readonly Dictionary<string, List<float>> SystemDrawTimes = new Dictionary<string, List<float>>();

        public static void AddUpdate(string system, float time)
        {
            time = (float)Math.Round(time,3);
            if (time < 1)
                return;
            if (!SystemUpdateTimes.TryGetValue(system, out var list))
            {
                list = new List<float>();
                SystemUpdateTimes.Add(system, list);
            }
            list.Add(time);
        }
        public static void AddDraw(string system, float time)
        {
            if (time == 0)
                return;
            if (!SystemUpdateTimes.TryGetValue(system, out var list))
            {
                list = new List<float>();
                SystemDrawTimes.Add(system, list);
            }
            list.Add(time);
        }
    }
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public int LastEntityId = 1;
        public ConcurrentDictionary<int, Entity> Entities;
        public ConcurrentDictionary<int, int> UniqueIdToEntityId, EntityIdToUniqueId;
        public List<IEntitySystem> Systems;
        public Camera Camera;
        public Player Player;

        public Scene()
        {
            Entities = new ConcurrentDictionary<int, Entity>();
            UniqueIdToEntityId = new ConcurrentDictionary<int, int>();
            EntityIdToUniqueId = new ConcurrentDictionary<int, int>();
            Systems = new List<IEntitySystem>();
        }

        public virtual void Initialize()
        {
            Camera = CreateEntity<Camera>();
            Player = CreateEntity<Player>();
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].Initialize();
            IsReady = true;
        }

        public virtual void LoadContent(ContentManager cm)
        {
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            Global.Names = File.ReadAllText("../Build/Content/RuntimeContent/Names.txt").Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Update(GameTime deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                {
                    var preUpdateTicks = DateTime.UtcNow.Ticks;
                    Systems[i].Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
                    var postUpdateTicks = DateTime.UtcNow.Ticks;
                    Profiler.AddUpdate(Systems[i].Name, (postUpdateTicks - preUpdateTicks) / 10000f);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].FixedUpdate(deltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Draw(SpriteBatch sb)
        {
            if (Camera == null)
                return;

            sb.Begin(SpriteSortMode.Deferred, transformMatrix: Camera.Transform.ViewMatrix, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Draw(sb);
            }
            sb.End();
        }

        public virtual void Destroy(Entity entity)
        {
            Entities.TryRemove(entity.EntityId, out _);
            EntityIdToUniqueId.TryRemove(entity.EntityId, out var uid);
            UniqueIdToEntityId.TryRemove(uid, out _);
        }
        public virtual void Destroy(int entity)
        {
            Entities.TryRemove(entity, out var actualEntity);
            if (actualEntity != null)
            {
                foreach (var child in actualEntity?.Children)
                    Destroy(child);
            }
            EntityIdToUniqueId.TryRemove(entity, out var uid);
            UniqueIdToEntityId.TryRemove(uid, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual T CreateEntity<T>(int uniqueId) where T : Entity, new()
        {
            var entity = new T
            {
                EntityId = LastEntityId++,
                Scene = this
            };
            ApplyArchetype(entity);
            entity.Add(new NetworkComponent(this, entity.EntityId, uniqueId));
            Entities.TryAdd(entity.EntityId, entity);
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual T CreateEntity<T>() where T : Entity, new()
        {
            var entity = new T
            {
                EntityId = LastEntityId++,
                Scene = this
            };
            ApplyArchetype(entity);
            Entities.TryAdd(entity.EntityId, entity);
            return entity;
        }

        public virtual T GetSystem<T>()
        {
            foreach (var sys in Systems)
                if (sys is T t)
                    return t;
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual T Find<T>() where T : Entity
        {
            foreach (var (_, entity) in Entities)
                if (entity is T t)
                    return t;
            return null;
        }

        private void ApplyArchetype<T>(T entity) where T : Entity, new()
        {
            switch (entity)
            {
                case Camera _:
                    ComponentArray<TransformComponent>.AddFor(entity);
                    break;
                case Player _:
                    ComponentArray<InputComponent>.AddFor(entity);
                    ComponentArray<CameraFollowTagComponent>.AddFor(entity, new CameraFollowTagComponent(1));
                    ComponentArray<DrawableComponent>.AddFor(entity, new DrawableComponent("character.png", new Rectangle(0, 2, 16, 16)));
                    ComponentArray<VelocityComponent>.AddFor(entity, new VelocityComponent(64));
                    ComponentArray<PositionComponent>.AddFor(entity, new PositionComponent(500, 500, 0));
                    ComponentArray<DbgBoundingBoxComponent>.AddFor(entity);
                    var nt = CreateEntity<NameTag>();
                    ComponentArray<TextComponent>.AddFor(nt, new TextComponent("Name: waiting..", "profont"));
                    ComponentArray<PositionComponent>.AddFor(nt, new PositionComponent(-48, -48, 0));
                    var nt2 = CreateEntity<NameTag>();
                    ComponentArray<TextComponent>.AddFor(nt2, new TextComponent($"Id:   {entity.EntityId}", "profont"));
                    ComponentArray<PositionComponent>.AddFor(nt2, new PositionComponent(-48, -32, 0));
                    nt.Parent = entity;
                    nt2.Parent = entity;
                    entity.Children.Add(nt);
                    entity.Children.Add(nt2);
                    break;
                case Npc _:
                    var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                    entity.Add(new DrawableComponent(srcEntity.TextureName, srcEntity.SrcRect));
                    entity.Add<PositionComponent>();
                    entity.Add(new VelocityComponent(32));
                    entity.Add<DbgBoundingBoxComponent>();
                    var name = Global.Names[Global.Random.Next(0, Global.Names.Length)];
                    nt = CreateEntity<NameTag>();
                    nt.Add(new TextComponent($"Name: {name}", "profont_12"));
                    nt.Add(new PositionComponent(-48, -48, 0));
                    nt2 = CreateEntity<NameTag>();
                    nt2.Add(new TextComponent($"Id:  {entity.EntityId}", "profont_12"));
                    nt2.Add(new PositionComponent(-48, -32, 0));
                    nt.Parent = entity;
                    nt2.Parent = entity;
                    entity.Children.Add(nt);
                    entity.Children.Add(nt2);
                    break;
            }
        }

        public override int GetHashCode() => Id;
        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;
    }
}