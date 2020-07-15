using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public static class ComponentArray<T> where T : struct
    {
        public const int AMOUNT = 1000000;
        public static ConcurrentStack<int> AvailableIndicies = new ConcurrentStack<int>(Enumerable.Range(0, AMOUNT));
        public static ConcurrentDictionary<int, int> EntityIdToArrayOffset = new ConcurrentDictionary<int, int>();
        private readonly static T[] array = new T[AMOUNT];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get(int owner)
        {
            if (EntityIdToArrayOffset.TryGetValue(owner, out var index))
                return ref array[index];
            throw new KeyNotFoundException($"{nameof(array)} is {array.Length} long, index for entity#{owner} not found.");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFor(int owner) => EntityIdToArrayOffset.ContainsKey(owner);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddFor(int owner, T component)
        {
            if (AvailableIndicies.TryPop(out int offset))
            {
                EntityIdToArrayOffset.TryAdd(owner, offset);
                array[offset] = component;
            }
            else
                throw new System.Exception("AHHHHH");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddFor(int owner)
        {
            if (AvailableIndicies.TryPop(out int offset))
            {
                EntityIdToArrayOffset.TryAdd(owner, offset);
                array[offset] = default;
            }
            else
                throw new System.Exception("AHHHHH");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove(int owner)
        {
            if (EntityIdToArrayOffset.TryRemove(owner, out int offset))
                AvailableIndicies.Push(offset);
        }
    }
}