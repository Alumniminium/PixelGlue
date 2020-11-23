using System.Linq;
using System;
using System.Collections.Generic;
using Shared.IO;

namespace Shared.ECS
{
    public static partial class World
    {
        public static int EntityCount => 1_000_000 - AvailableArrayIndicies.Count;
        private static int LastEntityId = 1;
        private static readonly Entity[] Entities;
        private static readonly Stack<int> AvailableArrayIndicies;
        private static readonly Dictionary<int, int> EntityToArrayOffset, UniqueIdToEntityId, EntityIdToUniqueId;
        private static readonly Dictionary<int, List<int>> Children;
        public static List<PixelSystem> Systems;
        static World()
        {
            Entities = new Entity[1_000_001];
            Children = new Dictionary<int, List<int>>();
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
            Register(entity.EntityId);
            return ref Entities[arrayIndex];
        }

        public static Entity[] GetEntities() => Entities;

        public static unsafe T CreateEntity<T>(int uniqueId) where T : unmanaged
        {
            ref var d = ref CreateEntity(uniqueId);
            var entity = stackalloc T[1];
            *(Entity*)(entity + 0) = d;
            return *entity;
        }
        public static ref Entity CreateEntity(int uniqueId)
        {
            ref var entity = ref CreateEntity();
            RegisterUniqueIdFor(uniqueId, entity.EntityId);
            Register(entity.EntityId);
            return ref entity;
        }

        public static void RegisterUniqueIdFor(int uniqueId, int entityId)
        {
            UniqueIdToEntityId.TryAdd(uniqueId, entityId);
            EntityIdToUniqueId.TryAdd(entityId, uniqueId);
        }
        public static List<int> GetChildren(ref Entity entity)
        {
            if (!Children.ContainsKey(entity.EntityId))
                Children.Add(entity.EntityId, new List<int>());
            return Children[entity.EntityId];
        }
        internal static void AddChildFor(ref Entity entity, ref Entity child)
        {
            child.Parent = entity.EntityId;
            if (!Children.TryGetValue(entity.EntityId, out var children))
                Children.Add(entity.EntityId, new List<int>());
            else
                Children[entity.EntityId].Add(child.EntityId);
        }
        public static ref Entity GetEntity(int entityId) => ref Entities[EntityToArrayOffset[entityId]];
        public static ref Entity GetEntityByUniqueId(int uniqueId) => ref Entities[EntityToArrayOffset[UniqueIdToEntityId[uniqueId]]];

        internal static void Register(int entityId)
        {
            ref readonly var entity = ref GetEntity(entityId);

            if(entity.Children != null)
                foreach(var childId in entity.Children)
                    Queue(childId);

            Queue(entityId);

            static void Queue(int id)
            {
                ref readonly var entity = ref GetEntity(id);
                for (int i = 0; i < Systems.Count; i++)
                    Systems[i].EntityChanged(entity);
            }
        }

        public static bool IdExists(int id) => EntityToArrayOffset.ContainsKey(id);
        public static bool UidExists(int uid) => UniqueIdToEntityId.ContainsKey(uid);
        public static void Destroy(int id)
        {
            FConsole.WriteLine("Destroying Entity "+id);

            if (EntityToArrayOffset.TryGetValue(id, out var arrayOffset))
            {
                ref var actualEntity = ref Entities[arrayOffset];
                actualEntity.Recycle();
                if (actualEntity.Children != null)
                {
                    foreach (var childId in actualEntity.Children)
                    {
                        ref var child = ref GetEntity(childId);
                        Destroy(child.EntityId);
                    }
                }
                
                for (int i = 0; i < Systems.Count; i++)
                    Systems[i].EntityChanged(actualEntity);

                EntityIdToUniqueId.Remove(id, out var uid);
                UniqueIdToEntityId.Remove(uid, out _);
                AvailableArrayIndicies.Push(arrayOffset);
            }
        }
    }
}