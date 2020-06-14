using System;

namespace PixelGlueCore.Helpers
{
    public static class ComponentList<T> where T: struct 
    { 
        public static T[] Items = new T[10000]; 
    }
}