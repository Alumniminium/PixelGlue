using System;

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
}