using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.ECS
{
    public static class ReflectionHelper
    {
        private static readonly List<Action<int>> RemoveMethodCache;
        private static readonly List<Type> ComponentTypes;
        private static readonly Dictionary<Type, Action<int>> Cache = new Dictionary<Type, Action<int>>();
        static ReflectionHelper()
        {
            var types = from a in AppDomain.CurrentDomain.GetAssemblies()
                        from t in a.GetTypes()
                        let aList = t.GetCustomAttributes(typeof(ComponentAttribute), true)
                        where aList?.Length > 0
                        select t;

            var methods = types.Select(ct => (Action<int>)typeof(ComponentList<>).MakeGenericType(ct).GetMethod("Remove").CreateDelegate(typeof(Action<int>)));

            RemoveMethodCache = new List<Action<int>>(methods);
            ComponentTypes = new List<Type>(types);

            for (int i = 0; i < ComponentTypes.Count; i++)
            {
                var type = ComponentTypes[i];
                var method = RemoveMethodCache[i];
                Cache.Add(type, method);
            }
        }
        public static void Remove<T>(int entityId)
        {
            if (Cache.TryGetValue(typeof(T), out var method))
            {
                method.Invoke(entityId);
                World.Register(entityId);
            }
        }
        public static void RecycleComponents(int entityId)
        {
            for (int i = 0; i < RemoveMethodCache.Count; i++)
                RemoveMethodCache[i].Invoke(entityId);
        }
    }
}