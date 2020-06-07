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
using System;

namespace PixelGlueCore.ECS
{
    public class ComponentCollection
    {
        public int DrawablesCount, PositionsCount, MovesCount,DbgBoundingBoxCount,InputComponentsCount,NetworkedsCount,CameraFollowTagsCount;
        public DrawableComponent[] DrawableComponents;
        public PositionComponent[] PositionComponents;
        public InputComponent[] InputComponents;
        public Networked[] Networkeds;
        public CameraFollowTagComponent[] CameraFollowTags;
        public MoveComponent[] MoveComponents;
        public DbgBoundingBoxComponent[] DbgBoundingBoxComponents;
        public List<IEntityComponent> Components;

        public ComponentCollection()
        {
            DrawableComponents = new DrawableComponent[0];
            PositionComponents = new PositionComponent[0];
            DbgBoundingBoxComponents = new DbgBoundingBoxComponent[0];
            MoveComponents = new MoveComponent[0];
            Components = new List<IEntityComponent>();
            Networkeds = new Networked[0];
            InputComponents = new InputComponent[0];
            CameraFollowTags = new CameraFollowTagComponent[0];
        }

    }
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public ConcurrentDictionary<int, PixelEntity> Entities;
        public List<IEntitySystem> Systems;
        public List<IEntitySystem> UISystems;
        public Dictionary<int, ComponentCollection> Components;
        public Camera Camera;
        public TmxMap Map;

        public Scene()
        {
            Entities = new ConcurrentDictionary<int, PixelEntity>();
            Systems = new List<IEntitySystem>();
            UISystems = new List<IEntitySystem>();
            Components = new Dictionary<int, ComponentCollection>();
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
            var entity = new T();
            entity.UniqueId = uniqueId;
            entity.Scene=this;
            Entities.TryAdd(entity.UniqueId, entity);
            Components.TryAdd(entity.UniqueId,new ComponentCollection());
            AddDbgBoundingBox(new DbgBoundingBoxComponent(uniqueId));
            return entity;
        }

        internal void Destroy(PixelEntity entity)
        {
            Entities.TryRemove(entity.UniqueId, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void AddDrawable(DrawableComponent component)
        {
            if (!Components.TryGetValue(component.UniqueId, out var components))
            {
                components = new ComponentCollection();
                Components.TryAdd(component.UniqueId, components);
            }
            if(components.DrawableComponents.Length==0)
                components.DrawableComponents = new DrawableComponent[1];
            components.DrawableComponents[components.DrawablesCount] = component;
            components.DrawablesCount++;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void AddMovable(MoveComponent component)
        {
            if (!Components.TryGetValue(component.UniqueId, out var list))
            {
                list = new ComponentCollection();
                Components.TryAdd(component.UniqueId, list);
            }
            if(list.MoveComponents.Length==0)
                list.MoveComponents = new MoveComponent[1];
            list.MoveComponents[list.MovesCount] = component;
            list.MovesCount++;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void AddPosition(PositionComponent component)
        {
            if (!Components.TryGetValue(component.UniqueId, out var list))
            {
                list = new ComponentCollection();
                Components.TryAdd(component.UniqueId, list);
            }
            if(list.PositionComponents.Length==0)
                list.PositionComponents = new PositionComponent[1];
            list.PositionComponents[list.PositionsCount] = component;
            list.PositionsCount++;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void AddDbgBoundingBox(DbgBoundingBoxComponent component)
        {
            if (!Components.TryGetValue(component.PixelOwnerId, out var list))
            {
                list = new ComponentCollection();
                Components.TryAdd(component.PixelOwnerId, list);
            }
            if(list.DbgBoundingBoxComponents.Length==0)
                list.DbgBoundingBoxComponents = new DbgBoundingBoxComponent[1];
            list.DbgBoundingBoxComponents[list.PositionsCount] = component;
            list.DbgBoundingBoxCount++;
        }
        internal void AddInput(InputComponent component)
        {
            if (!Components.TryGetValue(component.PixelOwnerId, out var list))
            {
                list = new ComponentCollection();
                Components.TryAdd(component.PixelOwnerId, list);
            }
            if(list.InputComponents.Length==0)
                list.InputComponents = new InputComponent[1];
            list.InputComponents[list.InputComponentsCount] = component;
            list.InputComponentsCount++;
        }

        internal void AddCameraFollowTag(CameraFollowTagComponent component)
        {
            if (!Components.TryGetValue(component.PixelOwnerId, out var list))
            {
                list = new ComponentCollection();
                Components.TryAdd(component.PixelOwnerId, list);
            }
            if(list.CameraFollowTags.Length==0)
                list.CameraFollowTags = new CameraFollowTagComponent[1];
            list.CameraFollowTags[list.CameraFollowTagsCount] = component;
            list.CameraFollowTagsCount++;
        }

        internal void AddNetworked(Networked component)
        {
            if (!Components.TryGetValue(component.PixelOwnerId, out var list))
            {
                list = new ComponentCollection();
                Components.TryAdd(component.PixelOwnerId, list);
            }
            if(list.Networkeds.Length==0)
                list.Networkeds = new Networked[1];
            list.Networkeds[list.NetworkedsCount] = component;
            list.NetworkedsCount++;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool TryGetComponent<T>(int ownerId, out T component) where T : IEntityComponent
        {
            component = default;

            if (!Components.TryGetValue(ownerId, out var owned))
                return false;

            foreach (var comp in owned.Components)
            {
                if (comp is T t)
                {
                    component = t;
                    return true;
                }
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref DrawableComponent GetDrawableComponentRef(int ownerId)
        {
            if (!Components.ContainsKey(ownerId))
                Components.Add(ownerId, new ComponentCollection());
            return ref Components[ownerId].DrawableComponents[0];
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref InputComponent GetInputComponentRef(int ownerId)
        {
            if (!Components.ContainsKey(ownerId))
                Components.Add(ownerId, new ComponentCollection());
            return ref Components[ownerId].InputComponents[0];
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref PositionComponent GetPositionComponentRef(int ownerId)
        {
            if (!Components.ContainsKey(ownerId))
                Components.Add(ownerId, new ComponentCollection());
            return ref Components[ownerId].PositionComponents[0];
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref MoveComponent GetMoveComponentRef(int ownerId)
        {
            if (!Components.ContainsKey(ownerId))
                Components.Add(ownerId, new ComponentCollection());

            return ref Components[ownerId].MoveComponents[0];
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        internal ref CameraFollowTagComponent GetCamreaFollowRef(int ownerId)
        {
            if (!Components.ContainsKey(ownerId))
                Components.Add(ownerId, new ComponentCollection());

            return ref Components[ownerId].CameraFollowTags[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        internal T GetSystem<T>()
        {
            foreach (var sys in Systems)
                if (sys is T)
                    return (T)sys;
            return default(T);
        }
        internal T GetUISystem<T>()
        {
            foreach (var sys in UISystems)
                if (sys is T)
                    return (T)sys;
            return default(T);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public T Find<T>() where T : PixelEntity
        {
            foreach (var kvp in Entities)
            {
                if (kvp.Value is T)
                    return (T)kvp.Value;
            }
            return null;
        }       
        public override int GetHashCode() => Id;
        public override bool Equals(object obj) => (obj as Scene)?.Id == Id;
    }
}