using System;
using System.Collections.Generic;
using PixelGlueCore.ECS;

namespace PixelGlueCore.Scenes
{
    public static class SceneFactory
    {
        public static Dictionary<Type,Scene> Scenes;
        public static Scene Create<T>(T scene) where T : Scene
        {
            return scene;
        }
    }
}
