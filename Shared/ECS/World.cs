using System.Linq;
using System;
using System.Collections.Generic;

namespace Shared.ECS
{
    public static partial class World
    {
        public static int EntityCount => 1_000_000 - AvailableArrayIndicies.Count;
        private static int LastEntityId = 1;
        private static readonly Entity[] Entities;
        private static readonly Stack<int> AvailableArrayIndicies;
        private static readonly Dictionary<int, int> EntityToArrayOffset, UniqueIdToEntityId, EntityIdToUniqueId;
        public static List<PixelSystem> Systems;
        static World()
        {
            Entities = new Entity[1_000_001];
            AvailableArrayIndicies = new Stack<int>(Enumerable.Range(1, 1_000_000));
            EntityToArrayOffset = new Dictionary<int, int>();
            UniqueIdToEntityId = new Dictionary<int, int>();
            EntityIdToUniqueId = new Dictionary<int, int>();
            Systems = new List<PixelSystem>();
        }

        public static T GetSystem<T>() where T : PixelSystem
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                var sys = Systems[i];
                if (sys is T type)
                    return type;
            }
            throw new ArgumentException("No system of requested type");
        }

        public static ref Entity CreateEntity()
        {
            var entity = new Entity
            {
                EntityId = LastEntityId++
            };
            var arrayIndex = AvailableArrayIndicies.Pop();
            EntityToArrayOffset.TryAdd(entity.EntityId, arrayIndex);
            Entities[arrayIndex] = entity;
            return ref Entities[arrayIndex];
        }
        public static ref Entity CreateEntity(int uniqueId)
        {
            ref var entity = ref CreateEntity();
            RegisterUniqueIdFor(uniqueId, entity.EntityId);
            return ref entity;
        }

        public static void RegisterUniqueIdFor(int uniqueId, int entityId)
        {
            UniqueIdToEntityId.TryAdd(uniqueId, entityId);
            EntityIdToUniqueId.TryAdd(entityId, uniqueId);
        }

        public static ref Entity GetEntity(int entityId) => ref Entities[EntityToArrayOffset[entityId]];
        public static ref Entity GetEntityByUniqueId(int uniqueId) => ref Entities[EntityToArrayOffset[UniqueIdToEntityId[uniqueId]]];
        public static void Register(ref Entity entity) => Register(entity.EntityId);
        public static void Register(int entityId)
        {
            Global.PostUpdateQueue.Enqueue(() =>
            {
                for (int i = 0; i < Systems.Count; i++)
                {
                    var sys = Systems[i];
                    sys.RemoveEntity(entityId);
                }
                for (int i = 0; i < Systems.Count; i++)
                {
                    var sys = Systems[i];
                    sys.AddEntity(entityId);
                }
            });
        }

        public static bool IdExists(int id) => EntityToArrayOffset.ContainsKey(id);
        public static bool UidExists(int uid) => UniqueIdToEntityId.ContainsKey(uid);

        public static void DestroyAsap(int entity) => Global.PostUpdateQueue.Enqueue(() => DestroyNow(entity));
        private static void DestroyNow(int id)
        {
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].RemoveEntity(id);

            if (EntityToArrayOffset.TryGetValue(id, out var arrayOffset))
            {
                ref var actualEntity = ref Entities[arrayOffset];

                if (actualEntity.Children != null)
                {
                    foreach (var childId in actualEntity.Children)
                    {
                        ref var child = ref GetEntity(childId);
                        DestroyNow(child.EntityId);
                    }
                }
            }
            EntityIdToUniqueId.Remove(id, out var uid);
            UniqueIdToEntityId.Remove(uid, out _);
            AvailableArrayIndicies.Push(arrayOffset);
        }
    }
}