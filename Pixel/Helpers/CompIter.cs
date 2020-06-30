using System;
using System.Collections.Generic;
using Pixel.ECS;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;

namespace Pixel.Helpers
{
    public static class CompIter
    {
        public static Dictionary<Guid[], List<Entity>> Lists = new Dictionary<Guid[], List<Entity>>();
        internal static void Update()
        {
            //foreach (var kvp in Lists)
            //    kvp.Value.Clear();
        }
        public static List<Entity> Get<T>()
        where T : struct, IEntityComponent
        {
            var typeArray = new [] {typeof(T).GUID};
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
            var typeArray = new [] {typeof(T).GUID, typeof(T2).GUID};
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
            var typeArray = new [] {typeof(T).GUID, typeof(T2).GUID, typeof(T3).GUID};
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
            var typeArray = new [] {typeof(T).GUID, typeof(T2).GUID, typeof(T3).GUID, typeof(T4).GUID};
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