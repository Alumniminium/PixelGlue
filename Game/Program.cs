using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Helpers;
using PixelGlueCore.Scenes;
using System;
using System.Threading;

namespace PixelGlueCore
{
    class Program
    {
        static void Main(string[] args)
        {
            FConsole.WriteLine("Current Root: " + Environment.CurrentDirectory);
            FConsole.LogToFile = false;
            FConsole.WriteLine("Setting Thread Priority to highest...");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            FConsole.WriteLine("Initializing the engine...");
            var engine = new Engine(false);
            engine.IsFixedTimeStep = false;
            PixelGlue.Profiling = true;
            FConsole.WriteLine("Initializing the scene...");

            var testScene = new TestingScene();
            testScene.Id = 1;
            testScene.Systems.Add(new MoveSystem());
            testScene.Systems.Add(new CameraSystem());


            var globalScene = new Scene();
            globalScene.Id=0;
            globalScene.Systems.Add(new InputSystem());
            globalScene.Systems.Add(new GCMonitor());
            globalScene.Systems.Add(new SmartFramerate(4));
            globalScene.Systems.Add(new NetworkSystem());
            globalScene.Systems.Add(new DialogSystem());


            SceneManager.Initialize(engine.Content);
            SceneManager.ActivateScene(globalScene);
            SceneManager.ActivateScene(testScene);

            FConsole.WriteLine("Initializing scene systems...");
            FConsole.WriteLine("Running game...");
            engine.Run();
        }
    }
}
