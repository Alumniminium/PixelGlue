using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using PixelShared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Pixel.ECS
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public int LastEntityId = 1;
        public ConcurrentDictionary<int, Entity> Entities;
        public List<IEntitySystem> Systems;
        public ConcurrentDictionary<int, int> UniqueIdToEntityId, EntityIdToUniqueId;
        public Camera Camera;

        public Scene()
        {
            Entities = new ConcurrentDictionary<int, Entity>();
            UniqueIdToEntityId = new ConcurrentDictionary<int, int>();
            EntityIdToUniqueId = new ConcurrentDictionary<int, int>();
            Systems = new List<IEntitySystem>();
        }

        public virtual void Initialize()
        {
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
                    Systems[i].Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
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

            sb.Begin(transformMatrix: Camera.Transform.ViewMatrix, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Draw(sb);
            }
            sb.End();
        }

        public virtual void Destroy(Entity entity)
        {
            foreach(var child in entity.Children)
                Destroy(child);
            Entities.TryRemove(entity.EntityId, out _);
            EntityIdToUniqueId.TryRemove(entity.EntityId, out var uid);
            UniqueIdToEntityId.TryRemove(uid, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual T CreateEntity<T>(int uniqueId) where T : Entity, new()
        {
            var entity = new T
            {
                EntityId = LastEntityId,
                Scene = this
            };
            entity.Add(new NetworkComponent(this,entity.EntityId,uniqueId));
            Entities.TryAdd(entity.EntityId, entity);
            LastEntityId++;
            return entity;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual T CreateEntity<T>() where T : Entity, new()
        {
            var entity = new T
            {
                EntityId = LastEntityId,
                Scene = this
            };
            Entities.TryAdd(entity.EntityId, entity);
            LastEntityId++;
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
        public override int GetHashCode() => Id;
        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;
    }
}