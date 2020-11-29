using Shared.IO;
using System;

namespace Shared.ECS.Systems
{
    public class GCMonitor : PixelSystem
    {
        public override string Name { get; set; } = "GC Monitoring System";
        public int[] GenCollections = new int[3];
        public GCMonitor(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void FixedUpdate(float dt)
        {
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