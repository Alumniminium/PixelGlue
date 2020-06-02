using PixelGlueCore.Helpers;
using System;

namespace PixelGlueCore.ECS.Systems
{
    public class GCMonitor : IEntitySystem
    {
        public string Name { get; set; } = "GC Monitoring System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public int[] GenCollections;

        public void Initialize()
        {
            GenCollections = new int[3];
            IsReady=true;
        }
        public void Update(double gameTime)
        {
            for (int i = 0; i < GenCollections.Length; i++)
            {
                var newVal = GC.CollectionCount(i);
                var oldVal = GenCollections[i];

                if (newVal != oldVal)
                {
                    FConsole.WriteLine($"GC: Gen0: {GenCollections[0].ToString("000")}, Gen1: {GenCollections[1].ToString("000")}, Gen2: {GenCollections[2].ToString("000")}");
                    GenCollections[i] = newVal;
                }
            }
        }
    }
}