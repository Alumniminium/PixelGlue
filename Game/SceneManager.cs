using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using PixelGlueCore.ECS;

namespace PixelGlueCore
{
    public static class SceneManager
    {
        public static Queue<Action> QueuedTasks = new Queue<Action>();
        public static ContentManager _content;
        public static List<Scene> ActiveScenes;
        public static List<Scene> LoadedScenes;
        public static void Initialize(ContentManager content)
        {
            _content = content;
            LoadedScenes = new List<Scene>();
            ActiveScenes = new List<Scene>();
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
                ActiveScenes.Add(scene);
                scene.IsActive = true;
            });
        }        
        public static void DeactivateScene<T>() where T:Scene
        {
            QueuedTasks.Enqueue(() =>
            {
                foreach(var loadedScene in LoadedScenes)
                {
                    if(loadedScene.IsActive)
                    {
                        var t1 = loadedScene.GetType();
                        var t2 = typeof(T);

                        if(t1 == t2)
                        {
                            loadedScene.IsActive=false;
                            ActiveScenes.Remove(loadedScene);
                        }
                    }
                }
            });
        }

        public static T Find<T>() where T : GameObject
        {
            foreach (var scene in ActiveScenes)
            {
                foreach (var kvp in scene.GameObjects)
                    if (kvp.Value is T)
                        return (T)kvp.Value;
            }
            return null;
        }

        public static GameObject Find(uint uniqueId)
        {
            foreach (var scene in ActiveScenes)
            {
                foreach (var kvp in scene.GameObjects)
                    if (kvp.Key == uniqueId)
                        return kvp.Value;
            }
            return null;
        }
    }
}
