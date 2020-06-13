using System;

namespace PixelGlueCore.Helpers
{
    public static class ComponentHolder<T> where T: struct 
    { 
        public static T[] data = new T[10000]; 
    }
    public class ComponentList<T>
    {
        private T[] _items;
        public int Count => _items.Length;
        private int _curIndex;

        public ComponentList(int size)
        {
            _items = new T[size];
        }

        public ref T this[int index]
        {
            get
            {
                if (_items.Length > index)
                    return ref _items[index];
                Grow();
                return ref _items[index];
            }
        }

        public void Add(T val)
        {
            if(_items.Length == _curIndex)
                Grow();
            
            _items[_curIndex] = val;
            _curIndex++;
        }
        public void Clear()
        {
            _items = new T[1];
            _curIndex=0;
        }
        private void Grow()
        {
            var newCount = Count+1;
            var newItems = new T[newCount];
            for (int i = 0; i < Count;i++)
                newItems[i] = _items[i];
            _items=newItems;
        }
    }
}