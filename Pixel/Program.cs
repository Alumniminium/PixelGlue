using PixelShared.IO;
using Pixel.ECS;
using Pixel.ECS.Systems;
using Pixel.Scenes;
using System;
using System.Threading;
using PixelShared;

namespace Pixel
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

            Global.Profiling = true;
            FConsole.WriteLine("Initializing the scene...");

            var testScene = new TestingScene
            {
                Id = 1
            };
            SceneManager.Initialize(engine.Content);
            SceneManager.ActivateScene(testScene);

            FConsole.WriteLine("Initializing scene systems...");
            FConsole.WriteLine("Running game...");
            engine.Run();
        }
    }
}
