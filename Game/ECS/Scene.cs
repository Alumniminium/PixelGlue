using PixelGlueCore.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Systems;
using System.Collections.Generic;
using TiledSharp;
using System;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.ECS
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public Dictionary<int, PixelEntity> Entities;
        public Dictionary<int,List<IEntityComponent>> Components;
        public List<IEntitySystem> Systems;
        public Camera Camera;
        public TmxMap Map;

        public Scene()
        {
            Components = new Dictionary<int, List<IEntityComponent>>();
            Entities = new Dictionary<int, PixelEntity>();
            Systems = new List<IEntitySystem>();
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].Initialize();
            IsReady = true;
        }

        internal T CreateEntity<T>(int uniqueId, params IEntityComponent[] components) where T : PixelEntity, new()
        {
            var entity = new T();
            entity.UniqueId=uniqueId;
            Entities.TryAdd(entity.UniqueId,entity);
            foreach(var component in components)
                AddComponent(uniqueId,component);
            return entity;
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
        }
        public virtual void FixedUpdate(double deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].FixedUpdate(deltaTime);
            }
        }
        public virtual void Draw(SpriteBatch sb)
        {
        }


        public void AddComponent(int ownerId, IEntityComponent component)
        {
            if(!Components.TryGetValue(ownerId, out var owned))
                {
                    owned = new List<IEntityComponent>();
                    Components.TryAdd(ownerId,owned);
                }
            owned.Add(component);
        }
        public bool TryGetComponent<T>(int ownerId, out T component) where T : IEntityComponent
        {
            component = default;

            if(!Components.TryGetValue(ownerId,out var owned))
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


        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}