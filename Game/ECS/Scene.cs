using PixelGlueCore.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Systems;
using System.Collections.Generic;
using TiledSharp;
using PixelGlueCore.ECS.Components;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace PixelGlueCore.ECS
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public ConcurrentDictionary<int, PixelEntity> Entities;
        public List<IEntitySystem> Systems;
        public List<IEntitySystem> UISystems;
        public Camera Camera;
        public TmxMap Map;

        public Scene()
        {
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        internal T CreateEntity<T>(int uniqueId) where T : PixelEntity, new()
        {
            var entity = new T
            {
                UniqueId = uniqueId,
                Scene = this
            };
            Entities.TryAdd(entity.UniqueId, entity);
            entity.AddDbgBoundingBox(new DbgBoundingBoxComponent(uniqueId));
            return entity;
        }

        internal void Destroy(PixelEntity entity)
        {
            Entities.TryRemove(entity.UniqueId, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        internal T GetSystem<T>()
        {
            foreach (var sys in Systems)
                if (sys is T t)
                    return t;
            return default;
        }
        internal T GetUISystem<T>()
        {
            foreach (var sys in UISystems)
                if (sys is T t)
                    return t;
            return default;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public T Find<T>() where T : PixelEntity
        {
            foreach (var (_,entity) in Entities)
                if (entity is T t)
                    return t;
            return null;
        }
        public override int GetHashCode() => Id;
        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;
    }
}