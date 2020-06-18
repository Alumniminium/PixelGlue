using PixelGlueCore.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TiledSharp;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.IO;
using System;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS
{
    public class GameScene :Scene
    {
        public ConcurrentDictionary<int, int> UniqueIdToEntityId;
        public Camera Camera;
        public TmxMap Map;

        public GameScene()
        {
            Entities = new ConcurrentDictionary<int, PixelEntity>();
            UniqueIdToEntityId = new ConcurrentDictionary<int, int>();
            Systems = new List<IEntitySystem>();
        }

        public override void Initialize()
        {            
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].Initialize();
            IsReady = true;
        }

        public override void LoadContent(ContentManager cm)
        {
            PixelGlue.Names = File.ReadAllText("../Build/Content/RuntimeContent/Names.txt").Split(',',StringSplitOptions.RemoveEmptyEntries);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Update(GameTime deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].FixedUpdate(deltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Draw(SpriteBatch sb)
        {
            if (Camera == null)
                return;
            
            sb.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Draw(sb);
            }
            sb.End();
        }
        
        public override void Destroy(PixelEntity entity)
        {
            Entities.TryRemove(entity.EntityId, out _);
            UniqueIdToEntityId.TryRemove(entity.UniqueId,out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override T CreateEntity<T>(int uniqueId)
        {
            var entity = new T
            {
                EntityId = LastEntityId,
                Scene = this
            };
            UniqueIdToEntityId.TryAdd(uniqueId,entity.EntityId);
            Entities.TryAdd(entity.EntityId, entity);
            LastEntityId++;
            return entity;
        } 

        public override T GetSystem<T>()
        {
            foreach (var sys in Systems)
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
        public override bool Equals(object obj) => (obj as GameScene)?.Id == Id;
        public override T CreateUIEntity<T>() => throw new NotImplementedException();
    }
}