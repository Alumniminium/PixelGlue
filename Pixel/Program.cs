using System.Diagnostics;
using Pixel.Scenes;
using Shared.IO;
using System;
using System.Threading;

namespace Pixel
{
    public static class Program
    {
        public static void Main()
        {
            FConsole.LogToFile = false;
            FConsole.WriteLine("PID: "+Process.GetCurrentProcess().Id);
            FConsole.WriteLine("Current Root: " + Environment.CurrentDirectory);
            FConsole.WriteLine("Setting Thread Priority to highest...");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            FConsole.WriteLine("Initializing the engine...");

            var engine = new Engine(false)
            {
                IsFixedTimeStep = false
            };

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
