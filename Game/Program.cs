using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Helpers;
using PixelGlueCore.Scenes;
using System;
using System.Threading;

namespace PixelGlueCore
{
    public static class Program
    {
        public static void Main()
        {
            FConsole.WriteLine("Current Root: " + Environment.CurrentDirectory);
            FConsole.LogToFile = false;
            FConsole.WriteLine("Setting Thread Priority to highest...");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            FConsole.WriteLine("Initializing the engine...");
            
            var engine = new Engine(false)
            {
                IsFixedTimeStep = false
            };

            PixelGlue.Profiling = true;
            FConsole.WriteLine("Initializing the scene...");

            var testScene = new TestingScene
            {
                Id = 1
            };

            var globalScene = new GameScene
            {
                Id = 0
            };
            globalScene.Systems.Add(new InputSystem());
            globalScene.Systems.Add(new NetworkSystem());
            globalScene.Systems.Add(new DialogSystem());
            globalScene.Systems.Add(new GCMonitor());

            SceneManager.ActivateScene(new TestingUIScene());
            SceneManager.Initialize(engine.Content);
            SceneManager.ActivateScene(globalScene);
            SceneManager.ActivateScene(testScene);

            FConsole.WriteLine("Initializing scene systems...");
            FConsole.WriteLine("Running game...");
            engine.Run();
        }
    }
}
