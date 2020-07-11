using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public static class World
    {
        public static int LastEntityId = 1;
        public static ConcurrentDictionary<int, Entity> Entities;
        public static ConcurrentDictionary<int, int> UniqueIdToEntityId, EntityIdToUniqueId;
        public static List<PixelSystem> Systems;
        static World()
        {
            Entities = new ConcurrentDictionary<int, Entity>();
            UniqueIdToEntityId = new ConcurrentDictionary<int, int>();
            EntityIdToUniqueId = new ConcurrentDictionary<int, int>();
            Systems = new List<PixelSystem>();
        }

        public static void Destroy(int entity)
        {
            Global.PostUpdateQueue.Enqueue(() =>
           {
               Entities.TryRemove(entity, out var actualEntity);
               for (int i = 0; i < Systems.Count; i++)
                   Systems[i].RemoveEntity(actualEntity);

               if (actualEntity.EntityId == 0)
                   return;
               if (actualEntity.Children == null)
                   return;

               foreach (var id in actualEntity.Children)
               {
                   var child = Entities[id];
                   Destroy(child.EntityId);
               }

               EntityIdToUniqueId.TryRemove(entity, out var uid);
               UniqueIdToEntityId.TryRemove(uid, out _);
           });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity CreateEntity()
        {
            var entity = new Entity
            {
                EntityId = LastEntityId++
            };

            Entities.TryAdd(entity.EntityId, entity);

            for (int i = 0; i < Systems.Count; i++)
                Systems[i].AddEntity(entity);

            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetSystem<T>() where T : PixelSystem
        {
            foreach (var sys in Systems)
                if (sys is T type)
                    return type;
            throw new ArgumentException("No system of requested type");
        }
    }
}