using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public static class ComponentArray<T> where T : struct
    {
        public const int AMOUNT = 1000000;
        private readonly static T[] array = new T[AMOUNT];
        private readonly static ConcurrentStack<int> AvailableIndicies = new ConcurrentStack<int>(Enumerable.Range(0, AMOUNT));
        private readonly static ConcurrentDictionary<int, int> EntityIdToArrayOffset = new ConcurrentDictionary<int, int>();

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddFor(int owner) => ref AddFor(owner, default);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddFor(int owner, T component)
        {
            if(!EntityIdToArrayOffset.TryGetValue(owner,out var offset))
                if (AvailableIndicies.TryPop(out offset))
                    EntityIdToArrayOffset.TryAdd(owner, offset);
                else
                    throw new System.Exception("AvailableIndicies.TryPop(out offset)");

            array[offset] = component;
            return ref array[offset];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFor(int owner) => EntityIdToArrayOffset.ContainsKey(owner);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get(int owner)
        {
            if (EntityIdToArrayOffset.TryGetValue(owner, out var index))
                return ref array[index];
            throw new KeyNotFoundException($"Fucking index not found. ({nameof(array)} Len: {array.Length}, index for entity {owner} not found.)");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove(int owner)
        {
            if (EntityIdToArrayOffset.TryRemove(owner, out int offset))
                AvailableIndicies.Push(offset);
        }
    }
}