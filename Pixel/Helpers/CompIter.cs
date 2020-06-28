using System.Collections.Generic;
using Pixel.ECS;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;

namespace Pixel.Helpers
{
    public static class CompIter
    {
        public static Dictionary<string, List<Entity>> Lists = new Dictionary<string, List<Entity>>();
        internal static void Update()
        {
            //foreach (var kvp in Lists)
            //    kvp.Value.Clear();
        }
        public static List<Entity> Get<T>()
        where T : struct, IEntityComponent
        {
            var typeArray = typeof(T).Name;
            if (!Lists.TryGetValue(typeArray, out var l))
            {
                if (l == null)
                    l = new List<Entity>();
                foreach (var kvp in SceneManager.ActiveScene.Entities)
                    if (kvp.Value.Has<T>())
                        l.Add(kvp.Value);
                if (l.Count > 0)
                    Lists.Add(typeArray, l);
            }
            return l;
        }
        public static List<Entity> Get<T, T2>()
        where T : struct, IEntityComponent where T2 : struct, IEntityComponent
        {
            var typeArray = typeof(T).Name + typeof(T2).Name;
            if (!Lists.TryGetValue(typeArray, out var l))
            {
                if (l == null)
                    l = new List<Entity>();
                foreach (var kvp in SceneManager.ActiveScene.Entities)
                    if (kvp.Value.Has<T>() && kvp.Value.Has<T2>())
                        l.Add(kvp.Value);
                if (l.Count > 0)
                    Lists.Add(typeArray, l);
            }
            return l;
        }
        public static List<Entity> Get<T, T2, T3>()
        where T : struct, IEntityComponent where T2 : struct, IEntityComponent
        where T3 : struct, IEntityComponent
        {
            var typeArray = typeof(T).Name + typeof(T2).Name + typeof(T3).Name;
            if (!Lists.TryGetValue(typeArray, out var l))
            {
                if (l == null)
                    l = new List<Entity>();
                foreach (var kvp in SceneManager.ActiveScene.Entities)
                    if (kvp.Value.Has<T>() && kvp.Value.Has<T2>() && kvp.Value.Has<T3>())
                        l.Add(kvp.Value);
                if (l.Count > 0)
                    Lists.Add(typeArray, l);
            }
            return l;
        }
        public static List<Entity> Get<T, T2, T3, T4>()
        where T : struct, IEntityComponent where T2 : struct, IEntityComponent
        where T3 : struct, IEntityComponent where T4 : struct, IEntityComponent
        {
            var typeArray = typeof(T).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name;
            if (!Lists.TryGetValue(typeArray, out var l))
            {
                if (l == null)
                    l = new List<Entity>();
                foreach (var kvp in SceneManager.ActiveScene.Entities)
                    if (kvp.Value.Has<T>() && kvp.Value.Has<T2>() && kvp.Value.Has<T3>() && kvp.Value.Has<T4>())
                        l.Add(kvp.Value);
                if (l.Count > 0)
                    Lists.Add(typeArray, l);
            }
            return l;
        }
    }
}