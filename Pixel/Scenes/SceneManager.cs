using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Pixel.ECS;

namespace Pixel.Scenes
{
    public static class SceneManager
    {
        public static Queue<Action> QueuedTasks = new Queue<Action>();
        public static ContentManager _content;
        public static List<GameScene> ActiveGameScenes;
        public static List<UIScene> ActiveUIScenes;
        public static List<Scene> LoadedScenes;
        public static void Initialize(ContentManager content)
        {
            _content = content;
            LoadedScenes = new List<Scene>();
            ActiveGameScenes = new List<GameScene>();
            ActiveUIScenes = new List<UIScene>();
        }

        public static void ActivateScene(Scene scene)
        {
            QueuedTasks.Enqueue(() =>
            {
                if (!LoadedScenes.Contains(scene))
                {
                    scene.LoadContent(_content);
                    scene.Initialize();
                    LoadedScenes.Add(scene);
                }
                if (scene is GameScene gameScene)
                    ActiveGameScenes.Add(gameScene);
                if (scene is UIScene uiScene)
                    ActiveUIScenes.Add(uiScene);
                scene.IsActive = true;
            });
        }
        public static void DeactivateScene<T>() where T : Scene
        {
            QueuedTasks.Enqueue(() =>
            {
                foreach (var loadedScene in LoadedScenes)
                {
                    if (loadedScene.IsActive)
                    {
                        loadedScene.IsActive = false;
                        if (loadedScene is GameScene gameScene)
                            ActiveGameScenes.Remove(gameScene);
                        if (loadedScene is UIScene uiScene)
                            ActiveUIScenes.Remove(uiScene);
                    }
                }
            });
        }
    }
}
