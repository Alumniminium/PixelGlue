using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public static class World
    {
        public static int EntityCount => 1_000_000 - AvailableArrayIndicies.Count;
        private static int LastEntityId = 1;
        private static readonly Entity[] Entities;
        private static readonly Queue<int> AvailableArrayIndicies;
        private static readonly Dictionary<int, int> EntityToArrayOffset, UniqueIdToEntityId, EntityIdToUniqueId;
        public static List<PixelSystem> Systems;
        static World()
        {
            Entities = new Entity[1_000_001];
            AvailableArrayIndicies = new Queue<int>(Enumerable.Range(1,1_000_000));
            EntityToArrayOffset = new Dictionary<int, int>();
            UniqueIdToEntityId = new Dictionary<int, int>();
            EntityIdToUniqueId = new Dictionary<int, int>();
            Systems = new List<PixelSystem>();
        }
        public static ref Entity CreateEntity()
        {
            var entity = new Entity
            {
                EntityId = LastEntityId++
            };
            var arrayIndex=  AvailableArrayIndicies.Dequeue();
            EntityToArrayOffset.TryAdd(entity.EntityId, arrayIndex);
            Entities[arrayIndex] = entity;
            return ref Entities[arrayIndex];
        }
        public static ref Entity CreateEntity(int uniqueId)
        {
            ref var entity = ref CreateEntity();
            RegisterUniqueIdFor(entity.EntityId,uniqueId);
            return ref entity;
        }
        public static void RegisterUniqueIdFor(int entityId, int uniqueId)
        {
            UniqueIdToEntityId.TryAdd(uniqueId,entityId);
            EntityIdToUniqueId.TryAdd(entityId, uniqueId);
        }
        public static ref Entity GetEntity(int entityId) => ref Entities[EntityToArrayOffset[entityId]];
        public static ref Entity GetEntityByUniqueId(int uniqueId) => ref Entities[EntityToArrayOffset[UniqueIdToEntityId[uniqueId]]];
        public static bool IdExists(int id) => EntityToArrayOffset.ContainsKey(id);
        public static bool UidExists(int uid) => UniqueIdToEntityId.ContainsKey(uid);
        public static void Destroy(int entity) => Global.PostUpdateQueue.Enqueue(() => DestroyNow(entity));
        internal static void DestroyNow(int id)
        {
            EntityToArrayOffset.Remove(id, out var arrayOffset);
            ref var actualEntity = ref Entities[arrayOffset];

            for (int i = 0; i < Systems.Count; i++)
                Systems[i].RemoveEntity(actualEntity.EntityId);

            EntityIdToUniqueId.Remove(id, out var uid);
            UniqueIdToEntityId.Remove(uid, out _);
            AvailableArrayIndicies.Enqueue(arrayOffset);

            if (actualEntity.Children == null)
                return;

            foreach (var childId in actualEntity.Children)
            {
                ref var child = ref GetEntity(childId);
                Destroy(child.EntityId);
            }
        }
        public static T GetSystem<T>() where T : PixelSystem
        {
            foreach (var sys in Systems)
                if (sys is T type)
                    return type;
            throw new ArgumentException("No system of requested type");
        }
    }
}