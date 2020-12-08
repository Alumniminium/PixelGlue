using Shared.IO;
using System;
using System.Collections.Generic;

namespace Shared.ECS.Systems
{
    public class GCMonitor : PixelSystem
    {
        public int[] GenCollections = new int[3];
        public GCMonitor(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads)
        {
            Name = "GC Monitoring System";
        }

        public override void Update(float dt, List<Entity> entities)
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