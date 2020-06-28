using System.Collections.Generic;
using Pixel.ECS;
using Pixel.Entities;
using Pixel.Enums;

namespace Pixel.Helpers
{
    public static class GameComponents<T> where T : struct
    {
        public static T[] Items = new T[200000];
    }
    public static class UIComponentList<T> where T : struct
    {
        public static T[] Items = new T[200000];
    }
    public static class CompIter
    {        
        public static List<Entity> Get<T>(GameScene scene) 
        where T : struct, IEntityComponent
        {
            var l = new List<Entity>();
            foreach (var kvp in scene.Entities)
                if (kvp.Value.Has<T>())
                    l.Add(kvp.Value);
            return l;
        }
        public static List<Entity> Get<T, T2>(GameScene scene) 
        where T : struct, IEntityComponent where T2 : struct, IEntityComponent
        {
            var l = new List<Entity>();
            foreach (var kvp in scene.Entities)
                if (kvp.Value.Has<T>() && kvp.Value.Has<T2>())
                    l.Add(kvp.Value);
            return l;
        }
        public static List<Entity> Get<T, T2, T3>(GameScene scene) 
        where T : struct, IEntityComponent where T2 : struct, IEntityComponent 
        where T3 : struct, IEntityComponent
        {
            var l = new List<Entity>();
            foreach (var kvp in scene.Entities)
                if (kvp.Value.Has<T>() && kvp.Value.Has<T2>() && kvp.Value.Has<T3>())
                    l.Add(kvp.Value);
            return l;
        }
        public static List<Entity> Get<T, T2, T3,T4>(GameScene scene) 
        where T : struct, IEntityComponent where T2 : struct, IEntityComponent 
        where T3 : struct, IEntityComponent where T4 : struct, IEntityComponent
        {
            var l = new List<Entity>();
            foreach (var kvp in scene.Entities)
                if (kvp.Value.Has<T>() && kvp.Value.Has<T2>() && kvp.Value.Has<T3>()&& kvp.Value.Has<T4>())
                    l.Add(kvp.Value);
            return l;
        }
    }
}