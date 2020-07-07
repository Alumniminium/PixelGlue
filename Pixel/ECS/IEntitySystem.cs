using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS;
using Pixel.Entities;
using Pixel.Scenes;

namespace Pixel.ECS
{
    public class PixelSystem
    {
        public virtual string Name { get; set; } = "Unnamed System";
        public bool IsActive { get; set; } = true;
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Initialize() { }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Update(float deltaTime) { }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void FixedUpdate(float deltaTime) { }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Draw(SpriteBatch spriteBatch) { }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void AddEntity(Entity entity)
        {
            if (Scene != null)
                Scene.PostUpdateQueue.Enqueue(() => Entities.Add(entity));
            else
                Entities.Add(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void RemoveEntity(Entity entity)
        {
            if (Scene != null)
                Scene.PostUpdateQueue.Enqueue(() => 
                {
                    Entities.Remove(entity);
                    entity.DestroyComponents();
                });
            else
                Entities.Remove(entity);
        }
    }
}