using PixelGlueCore.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Systems;
using System.Collections.Generic;
using TiledSharp;
using System;
using PixelGlueCore.ECS.Components;
using System.Collections.Concurrent;

namespace PixelGlueCore.ECS
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public ConcurrentDictionary<int, PixelEntity> Entities;
        public ConcurrentDictionary<int, List<IEntityComponent>> Components;
        public List<IEntitySystem> Systems;
        public List<IEntitySystem> UISystems;
        public Camera Camera;
        public TmxMap Map;

        public Scene()
        {
            Components = new ConcurrentDictionary<int, List<IEntityComponent>>();
            Entities = new ConcurrentDictionary<int, PixelEntity>();
            Systems = new List<IEntitySystem>();
            UISystems = new List<IEntitySystem>();
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].Initialize();
            for (int i = 0; i < UISystems.Count; i++)
                UISystems[i].Initialize();
            IsReady = true;
        }

        public virtual void LoadContent(ContentManager cm)
        {
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont", cm);
            AssetManager.LoadFont("../Build/Content/RuntimeContent/emoji.fnt", "emoji", cm);
        }
        public virtual void Update(GameTime deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Update(deltaTime.ElapsedGameTime.TotalSeconds);
            }
            for (int i = 0; i < UISystems.Count; i++)
            {
                if (UISystems[i].IsActive && UISystems[i].IsReady)
                    UISystems[i].Update(deltaTime.ElapsedGameTime.TotalSeconds);
            }
        }
        public virtual void FixedUpdate(double deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].FixedUpdate(deltaTime);
            }
            for (int i = 0; i < UISystems.Count; i++)
            {
                if (UISystems[i].IsActive && UISystems[i].IsReady)
                    UISystems[i].FixedUpdate(deltaTime);
            }
        }
        public virtual void Draw(Scene scene, SpriteBatch sb)
        {
            if (Camera == null)
                return;
            sb.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Draw(scene, sb);
            }
            sb.End();
            sb.Begin(samplerState: SamplerState.PointClamp);
            for (int i = 0; i < UISystems.Count; i++)
            {
                if (UISystems[i].IsActive && UISystems[i].IsReady)
                    UISystems[i].Draw(scene, sb);
            }
            sb.End();
        }

        internal T CreateEntity<T>(int uniqueId, params IEntityComponent[] components) where T : PixelEntity, new()
        {
            var entity = new T();
            entity.UniqueId = uniqueId;
            Entities.TryAdd(entity.UniqueId, entity);
            foreach (var component in components)
                AddComponent(uniqueId, component);
            AddComponent(uniqueId,new DbgBoundingBoxComponent(uniqueId));
            return entity;
        }

        internal void Destroy(PixelEntity entity)
        {
            Entities.TryRemove(entity.UniqueId, out _);
            Components.TryRemove(entity.UniqueId,out _);
        }

        public void AddComponent(int ownerId, IEntityComponent component)
        {
            if (!Components.TryGetValue(ownerId, out var owned))
            {
                owned = new List<IEntityComponent>();
                Components.TryAdd(ownerId, owned);
            }
            owned.Add(component);
        }
        public bool TryGetComponent<T>(int ownerId, out T component) where T : IEntityComponent
        {
            component = default;

            if (!Components.TryGetValue(ownerId, out var owned))
                return false;

            foreach (var comp in owned)
            {
                if (comp is T)
                {
                    component = (T)comp;
                    return true;
                }
            }
            return false;
        }
        public bool TryGetComponent<T>(out T component) where T : IEntityComponent
        {
            component = default;

            foreach (var entityComponentPair in Components)
            {
                foreach (var comp in entityComponentPair.Value)
                {
                    if (comp is T)
                    {
                        component = (T)comp;
                        return true;
                    }
                }
            }
            return false;
        }

        internal T GetSystem<T>()
        {
            foreach(var sys in Systems)
                if(sys is T)
                    return (T)sys;
            return default(T);
        }
        internal T GetUISystem<T>()
        {
            foreach(var sys in UISystems)
                if(sys is T)
                    return (T)sys;
            return default(T);
        }
        public T Find<T>() where T : PixelEntity, new()
        {
            foreach (var kvp in Entities)
            {
                if (kvp.Value is T)
                    return (T)kvp.Value;
            }
            return null;
        }
        public T GetEntity<T>(int uniqueId) where T : PixelEntity, new()
        {
            if (Entities.TryGetValue(uniqueId, out var entity))
                return (T)entity;
            return null;
        }
        public PixelEntity GetEntity(int uniqueId)
        {
            if (Entities.TryGetValue(uniqueId, out var entity))
                return entity;
            return null;
        }

        public override int GetHashCode() =>Id;
        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;
    }
}