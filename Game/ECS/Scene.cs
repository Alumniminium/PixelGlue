using PixelGlueCore.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Systems;
using System.Collections.Generic;
using System.Collections.Concurrent;
using PixelGlueCore.Enums;

namespace PixelGlueCore.ECS
{
    public abstract class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public int LastEntityId = 1;
        public ConcurrentDictionary<int, PixelEntity> Entities;
        public List<IEntitySystem> Systems;
        protected Scene()
        {
            Entities = new ConcurrentDictionary<int, PixelEntity>();
            Systems = new List<IEntitySystem>();
        }   
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager cm);
        public abstract void Update(GameTime deltaTime);
        public abstract void FixedUpdate(float deltaTime);
        public abstract void Draw(SpriteBatch sb);
        public abstract T CreateEntity<T>(int uniqueId) where T : PixelEntity, new();
        public abstract T CreateUIEntity<T>() where T : PixelEntity, new();
        public abstract T GetSystem<T>();
        public abstract void Destroy(PixelEntity entity);
    }
}