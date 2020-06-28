using Pixel.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Pixel.Enums;

namespace Pixel.ECS
{
    public abstract class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;
        public int LastEntityId = 1;
        public ConcurrentDictionary<int, Entity> Entities;
        public List<IEntitySystem> Systems;
        protected Scene()
        {
            Entities = new ConcurrentDictionary<int, Entity>();
            Systems = new List<IEntitySystem>();
        }   
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager cm);
        public abstract void Update(GameTime deltaTime);
        public abstract void FixedUpdate(float deltaTime);
        public abstract void Draw(SpriteBatch sb);
        public abstract T CreateEntity<T>(int uniqueId) where T : Entity, new();
        public abstract T CreateUIEntity<T>() where T : Entity, new();
        public abstract T GetSystem<T>();
        public abstract void Destroy(Entity entity);
    }
}