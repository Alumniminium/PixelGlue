using System.Threading;
using Shared.IO;
using System;

namespace Pixel.ECS.Systems
{
    public class GCMonitor : PixelSystem
    {
        public override string Name { get; set; } = "GC Monitoring System";
        public Thread Worker;
        public AutoResetEvent Block = new AutoResetEvent(true);
        public int[] GenCollections;

        public override void Initialize()
        {
            GenCollections = new int[3];
            Worker = new Thread(Step)
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            Worker.Start();
            IsActive = true;
        }
        public override void FixedUpdate(float gameTime) => Block.Set();

        public void Step()
        {
            while (true)
            {
                Block.WaitOne();
                for (int i = 0; i < GenCollections.Length; i++)
                {
                    var newVal = GC.CollectionCount(i);
                    var oldVal = GenCollections[i];

                    if (newVal != oldVal)
                    {
                        FConsole.WriteLine($"GC: Gen0: {GenCollections[0]:000}, Gen1: {GenCollections[1]:000}, Gen2: {GenCollections[2]:000}");
                        GenCollections[i] = newVal;
                    }
                }
            }
        }
    }
}