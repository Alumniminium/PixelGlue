using System;
using System.Collections.Generic;
using PixelGlueCore.ECS;

namespace PixelGlueCore.Scenes
{
    public static class SceneFactory
    {
        public static Dictionary<Type,GameScene> Scenes;
        public static GameScene Create<T>(T scene) where T : GameScene
        {
            return scene;
        }
    }
}
