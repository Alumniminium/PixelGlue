using Pixel.Enums;
using PixelShared;
using System;
using System.Collections.Generic;

namespace Pixel.Helpers
{
    public static class CompIter<T, T2, T3, T4> where T : struct, IEntityComponent where T2 : struct, IEntityComponent where T3 : struct, IEntityComponent where T4 : struct, IEntityComponent
    {
        private static readonly List<int> _items = new List<int>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<int> Get()
        {
            if (_itemsFrame != Global.FrameCounter)
                Update();
            _itemsFrame = Global.FrameCounter;
            for (int i = 0; i < Scenes.SceneManager.ActiveScene.Entities.Count; i++)
            {
                if (!ComponentArray<T>.HasFrom(i) || !ComponentArray<T2>.HasFrom(i)|| !ComponentArray<T3>.HasFrom(i)|| !ComponentArray<T4>.HasFrom(i))
                    continue;
                _items.Add(i);
            }
            return _items;
        }
    }
    public static class CompIter<T, T2, T3> where T : struct, IEntityComponent where T2 : struct, IEntityComponent where T3 : struct, IEntityComponent
    {
        private static readonly List<int> _items = new List<int>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<int> Get()
        {
            if (_itemsFrame != Global.FrameCounter)
                Update();
            _itemsFrame = Global.FrameCounter;
            for (int i = 0; i < Scenes.SceneManager.ActiveScene.Entities.Count; i++)
            {
                if (!ComponentArray<T>.HasFrom(i) || !ComponentArray<T2>.HasFrom(i)|| !ComponentArray<T3>.HasFrom(i))
                    continue;
                _items.Add(i);
            }
            return _items;
        }
    }
    public static class CompIter<T, T2> where T : struct, IEntityComponent where T2 : struct, IEntityComponent
    {
        private static readonly List<int> _items = new List<int>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<int> Get()
        {
            if (_itemsFrame != Global.FrameCounter)
                Update();
            _itemsFrame = Global.FrameCounter;
            for (int i = 0; i < Scenes.SceneManager.ActiveScene.Entities.Count; i++)
            {
                if (!ComponentArray<T>.HasFrom(i) || !ComponentArray<T2>.HasFrom(i) )
                    continue;
                _items.Add(i);
            }
            return _items;
        }
    }
    public static class CompIter<T> where T : struct, IEntityComponent
    {
        private static readonly List<int> _items = new List<int>();
        internal static void Update() => _items.Clear();
        private static float _itemsFrame;
        public static List<int> Get()
        {
            if (_itemsFrame != Global.FrameCounter)
                Update();
            _itemsFrame = Global.FrameCounter;
            for (int i = 0; i < Scenes.SceneManager.ActiveScene.Entities.Count; i++)
            {
                if (!ComponentArray<T>.HasFrom(i))
                    continue;
                _items.Add(i);
            }
            return _items;
        }
    }
}