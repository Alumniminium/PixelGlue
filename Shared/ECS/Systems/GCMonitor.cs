using Shared.IO;
using System;
using System.Collections.Generic;

namespace Shared.ECS.Systems
{
    public class GCMonitor : PixelSystem
    {
        public int[] GenCollections = new int[3];
        public DateTime LastUpdate = DateTime.UtcNow;
        public GCMonitor(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads)
        {
            Name = "GC Monitoring System";
        }

        public override void Update(float dt, GCNeutralList<Entity> entities)
        {
            if(DateTime.UtcNow >= LastUpdate.AddSeconds(1))
            {
                LastUpdate = DateTime.UtcNow;
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