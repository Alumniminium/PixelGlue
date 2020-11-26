using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Shared.IO;

namespace Shared.ECS
{
    public class PixelSystem
    {
        public bool IsActive { get; set; } = true;
        public virtual string Name { get; set; } = "Unnamed System";
        public List<int> Entities { get; set; } = new List<int>();
        internal List<int> addedEntities { get; set; } = new List<int>();
        internal List<int> removedEntities { get; set; } = new List<int>();
        public bool WantsUpdate, WantsDraw;

        public PixelSystem(bool doUpdate, bool doDraw)
        {
            WantsUpdate = doUpdate;
            WantsDraw = doDraw;
        }
        public virtual void Initialize() { }

        public void PreUpdate()
        {
            foreach (var id in removedEntities)
                Entities.Remove(id);
            Entities.AddRange(addedEntities);

            addedEntities.Clear();
            removedEntities.Clear();
        }

        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float deltaTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual bool MatchesFilter(Entity entityId) => false;
        
        public virtual void EntityChanged(ref Entity entity)
        {
            if (MatchesFilter(entity))
            {
                if(removedEntities.Contains(entity.EntityId))
                    removedEntities.Remove(entity.EntityId);
                if (Entities.Contains(entity.EntityId))
                    return;
                if (addedEntities.Contains(entity.EntityId))
                    return;
                addedEntities.Add(entity.EntityId);
            }
            else if (Entities.Contains(entity.EntityId) && !removedEntities.Contains(entity.EntityId))
                removedEntities.Add(entity.EntityId);
        }

        public virtual void EntityRemoved(ref Entity entity) => removedEntities.Add(entity.EntityId);
    }
}