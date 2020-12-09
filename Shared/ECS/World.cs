using System.Linq;
using System;
using System.Collections.Generic;
using Shared.IO;

namespace Shared.ECS
{
    public static class World
    {
        public static int EntityCount => 1_000_000 - AvailableArrayIndicies.Count;
        private static int LastEntityId = 1;
        private static readonly Entity[] Entities;
        private static readonly List<int> ChangedEntities;
        private static readonly Stack<int> AvailableArrayIndicies;
        private static readonly Dictionary<int, int> EntityToArrayOffset, UniqueIdToEntityId, EntityIdToUniqueId;
        private static readonly Dictionary<int, List<int>> Children;
        private static readonly List<int> ToBeRemoved;
        public static List<PixelSystem> Systems;
        static World()
        {
            Entities = new Entity[1_000_001];
            ChangedEntities = new List<int>();
            Children = new Dictionary<int, List<int>>();
            ToBeRemoved = new List<int>();
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

        public static List<int> GetChangedEntities() => ChangedEntities;

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
        public static bool EntityExists(int entityId) => EntityToArrayOffset.ContainsKey(entityId);

        internal static void Register(int entityId)
        {
            if (ChangedEntities.Contains(entityId))
                return;

            ChangedEntities.Add(entityId);
            ref var entity = ref GetEntity(entityId);

            if (entity.Children != null)
                foreach (var childId in entity.Children)
                    ChangedEntities.Add(childId);
        }
        public static bool UidExists(int uid) => UniqueIdToEntityId.ContainsKey(uid);
        public static void Destroy(int id)
        {
            if (ToBeRemoved.Contains(id))
                return;
            ToBeRemoved.Add(id);
        }
        private static void DestroyInternal(int id)
        {
            if(Global.Verbose)
            FConsole.WriteLine("World.DestroyInternal");
            if (EntityToArrayOffset.TryGetValue(id, out var arrayOffset))
            {
                ref var actualEntity = ref Entities[arrayOffset];
                if (actualEntity.Children != null)
                {
                    foreach (var childId in actualEntity.Children)
                    {
                        ref var child = ref GetEntity(childId);
                        DestroyInternal(child.EntityId);
                    }
                }
                actualEntity.Recycle();
                AvailableArrayIndicies.Push(arrayOffset);
            }
            EntityIdToUniqueId.Remove(id, out var uid);
            UniqueIdToEntityId.Remove(uid, out _);
        }
        public static void Update()
        {
            foreach (var id in ToBeRemoved)
                DestroyInternal(id);

            foreach (var entityId in ChangedEntities)
            {
                ref var entity = ref GetEntity(entityId);
                for (int i = 0; i < Systems.Count; i++)
                    Systems[i].EntityChanged(ref entity);
            }

            ChangedEntities.Clear();
            ToBeRemoved.Clear();
        }
    }
}