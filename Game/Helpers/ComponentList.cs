namespace PixelGlueCore.Helpers
{
    public static class GameComponentList<T> where T: struct 
    { 
        public static T[] Items = new T[20000]; 
    }    
    public static class UIComponentList<T> where T: struct 
    { 
        public static T[] Items = new T[20000]; 
    }
}