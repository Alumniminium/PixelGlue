using PixelGlueCore.Helpers;
using System;
using PixelGlueCore.Enums;

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
        public void FixedUpdate(float gameTime)
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

        public void Update(float deltaTime){}
    }
}