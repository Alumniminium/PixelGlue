using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.World;
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
            Camera=CreateEntity<Camera>();
            Player=CreateEntity<Player>();
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

            sb.Begin(SpriteSortMode.Deferred,transformMatrix: Camera.Transform.ViewMatrix, samplerState: SamplerState.PointClamp);
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
            if(actualEntity!=null)
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
                    entity.Add(new TransformComponent(entity.EntityId));
                    break;
                case Player _:
                    entity.Add(new InputComponent(entity.EntityId));
                    entity.Add(new CameraFollowTagComponent(entity.EntityId, 1));
                    entity.Add(new DrawableComponent(entity.EntityId, "character.png", new Rectangle(0, 2, 16, 16)));
                    entity.Add(new VelocityComponent(entity.EntityId, 64));
                    entity.Add(new PositionComponent(entity.EntityId, 0, 0, 0));
                    entity.Add(new DbgBoundingBoxComponent(entity.EntityId));
                    var nt = CreateEntity<NameTag>();
                    nt.Add(new TextComponent(nt.EntityId, "Name: waiting..", "profont_12"));
                    nt.Add(new PositionComponent(nt.EntityId, -48, -48, 0));
                    var nt2 = CreateEntity<NameTag>();
                    nt2.Add(new TextComponent(nt2.EntityId, $"Id:   {entity.EntityId}", "profont_12"));
                    nt2.Add(new PositionComponent(nt2.EntityId, -48, -32, 0));
                    nt.Parent = entity;
                    nt2.Parent = entity;
                    entity.Children.Add(nt);
                    entity.Children.Add(nt2);
                    break;
                case Npc _:
                    var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                    entity.Add(new DrawableComponent(entity.EntityId, srcEntity.TextureName, srcEntity.SrcRect));
                    entity.Add(new PositionComponent(entity.EntityId, 0, 0, 0));
                    entity.Add(new VelocityComponent(entity.EntityId, 32));
                    entity.Add(new DbgBoundingBoxComponent(entity.EntityId));
                    var name = Global.Names[Global.Random.Next(0, Global.Names.Length)];
                    nt = CreateEntity<NameTag>();
                    nt.Add(new TextComponent(nt.EntityId, $"Name: {name}", "profont_12"));
                    nt.Add(new PositionComponent(nt.EntityId, -48, -48, 0));
                    nt2 = CreateEntity<NameTag>();
                    nt2.Add(new TextComponent(nt2.EntityId, $"Id:  {entity.EntityId}", "profont_12"));
                    nt2.Add(new PositionComponent(nt2.EntityId, -48, -32, 0));
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