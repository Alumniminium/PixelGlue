using Microsoft.Xna.Framework.Content;
using Pixel.ECS;
using System;
using System.Collections.Generic;

namespace Pixel.Scenes
{
    public static class SceneManager
    {
        public static Queue<Action> QueuedTasks = new Queue<Action>();
        public static ContentManager _content;
        public static Scene ActiveScene;
        public static List<Scene> LoadedScenes;
        public static void Initialize(ContentManager content)
        {
            _content = content;
            LoadedScenes = new List<Scene>();
        }

        public static void ActivateScene(Scene scene)
        {
            QueuedTasks.Enqueue(() =>
            {
                ActiveScene = scene;
                if (!LoadedScenes.Contains(scene))
                {
                    scene.LoadContent(_content);
                    scene.Initialize();
                    LoadedScenes.Add(scene);
                }
                scene.IsActive = true;
            });
        }
    }
}
