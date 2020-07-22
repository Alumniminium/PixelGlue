using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.ECS
{
    public class PixelSystem
    {
        public bool IsActive { get; set; } = true;
        public virtual string Name { get; set; } = "Unnamed System";
        public HashSet<int> Entities { get; set; } = new HashSet<int>();
        public bool WantsUpdate, WantsDraw;

        public PixelSystem(bool doUpdate, bool doDraw)
        {
            WantsUpdate = doUpdate;
            WantsDraw = doDraw;
        }
        public virtual void Initialize() { }
        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float deltaTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void AddEntity(int entity)
        {
            if (!Entities.Contains(entity))
                Entities.Add(entity);
        }
        public void RemoveEntity(int entity) => Entities.Remove(entity);
    }
}