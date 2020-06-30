using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;
using System.Collections.Generic;

namespace Pixel.Helpers
{
    public static class CompIter<T, T2, T3, T4> where T : struct, IEntityComponent where T2 : struct, IEntityComponent
        where T3 : struct, IEntityComponent where T4 : struct, IEntityComponent
    {
        private static readonly List<Entity> _items = new List<Entity>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<Entity> Get(float deltaTime)
        {
            if (_itemsFrame != deltaTime)
                Update();
            _itemsFrame = deltaTime;
            foreach (var kvp in SceneManager.ActiveScene.Entities)
                if (kvp.Value.Has<T>() && kvp.Value.Has<T2>() && kvp.Value.Has<T3>() && kvp.Value.Has<T4>())
                    _items.Add(kvp.Value);
            return _items;
        }
    }
    public static class CompIter<T, T2, T3> where T : struct, IEntityComponent where T2 : struct, IEntityComponent
        where T3 : struct, IEntityComponent
    {
        private static readonly List<Entity> _items = new List<Entity>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<Entity> Get(float deltaTime)
        {
            if (_itemsFrame != deltaTime)
                Update();
            _itemsFrame = deltaTime;
            foreach (var kvp in SceneManager.ActiveScene.Entities)
                if (kvp.Value.Has<T>() && kvp.Value.Has<T2>() && kvp.Value.Has<T3>())
                    _items.Add(kvp.Value);
            return _items;
        }
    }
    public static class CompIter<T, T2> where T : struct, IEntityComponent where T2 : struct, IEntityComponent
    {
        private static readonly List<Entity> _items = new List<Entity>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<Entity> Get(float deltaTime)
        {
            if (_itemsFrame != deltaTime)
                Update();
            _itemsFrame = deltaTime;
            foreach (var kvp in SceneManager.ActiveScene.Entities)
                if (kvp.Value.Has<T>() && kvp.Value.Has<T2>())
                    _items.Add(kvp.Value);
            return _items;
        }
    }
    public static class CompIter<T> where T : struct, IEntityComponent
    {
        private static readonly List<Entity> _items = new List<Entity>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<Entity> Get(float deltaTime)
        {
            if (_itemsFrame != deltaTime)
                Update();
            _itemsFrame = deltaTime;
            foreach (var kvp in SceneManager.ActiveScene.Entities)
                if (kvp.Value.Has<T>())
                    _items.Add(kvp.Value);
            return _items;
        }
    }
}