using PixelShared;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pixel.Helpers
{
    public static class CompIter<T> where T : struct
    {
        private static float _lastUpdateFrame;
        private static readonly List<int> _items = new List<int>(10000);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Update()
        {    
            _lastUpdateFrame = Global.FrameCounter;
             _items.Clear();
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static List<int> Get()
        {
            if (_lastUpdateFrame != Global.FrameCounter)
                Update();

            for (int entityId = 0; entityId < Scenes.SceneManager.ActiveScene.Entities.Count; entityId++)
                if (ComponentArray<T>.HasFor(entityId))
                    _items.Add(entityId);

            return _items;
        }
    }
    public static class CompIter<T,TT> where T : struct where TT:struct
    {
        private static float _lastUpdateFrame;
        private static readonly List<int> _items = new List<int>(10000);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Update()
        {    
            _lastUpdateFrame = Global.FrameCounter;
             _items.Clear();
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static List<int> Get()
        {
            if (_lastUpdateFrame != Global.FrameCounter)
                Update();

            for (int entityId = 0; entityId < Scenes.SceneManager.ActiveScene.Entities.Count; entityId++)
                if (ComponentArray<T>.HasFor(entityId)&&ComponentArray<TT>.HasFor(entityId))
                    _items.Add(entityId);

            return _items;
        }
    }
    public static class CompIter<T,TT,TTT> where T : struct where TT:struct where TTT:struct
    {
        private static float _lastUpdateFrame;
        private static readonly List<int> _items = new List<int>(10000);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Update()
        {    
            _lastUpdateFrame = Global.FrameCounter;
             _items.Clear();
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static List<int> Get()
        {
            if (_lastUpdateFrame != Global.FrameCounter)
                Update();

            for (int entityId = 0; entityId < Scenes.SceneManager.ActiveScene.Entities.Count; entityId++)
                if (ComponentArray<T>.HasFor(entityId)&&ComponentArray<TT>.HasFor(entityId)&&ComponentArray<TTT>.HasFor(entityId))
                    _items.Add(entityId);

            return _items;
        }
    }
    public static class CompIter<T,TT,TTT,TTTT> where T : struct where TT:struct where TTT:struct where TTTT : struct
    {
        private static float _lastUpdateFrame;
        private static readonly List<int> _items = new List<int>(10000);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Update()
        {    
            _lastUpdateFrame = Global.FrameCounter;
             _items.Clear();
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static List<int> Get()
        {
            if (_lastUpdateFrame != Global.FrameCounter)
                Update();

            for (int entityId = 0; entityId < Scenes.SceneManager.ActiveScene.Entities.Count; entityId++)
                if (ComponentArray<T>.HasFor(entityId)&&ComponentArray<TT>.HasFor(entityId)&&ComponentArray<TTT>.HasFor(entityId)&&ComponentArray<TTTT>.HasFor(entityId))
                    _items.Add(entityId);

            return _items;
        }
    }
}