using System.Collections.Generic;

namespace Pixel.Helpers
{
    public static class ComponentArray<T> where T : struct
    {
        //                      EntityId, (ArrayIndex)
        public static Dictionary<int,int> EntityIdToArrayOffset = new Dictionary<int,int>();
        public static T[] array = new T[1000];
        public static ref T Get(int owner)
        {
            if(EntityIdToArrayOffset.TryGetValue(owner,out var index))
                return ref array[index];
            throw new System.IndexOutOfRangeException($"{nameof(array)} is {array.Length} long, index requested: {owner}");
        }
        public static bool HasFrom(int owner) => EntityIdToArrayOffset.ContainsKey(owner);

        internal static void Add(int owner,T component)
        {
            EntityIdToArrayOffset.Add(owner,EntityIdToArrayOffset.Count);
            array[EntityIdToArrayOffset.Count-1]=component;
        }
        internal static void Remove(int owner)
        {
            EntityIdToArrayOffset.Remove(owner);
        }
    }
}