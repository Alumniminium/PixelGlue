using System.Threading;
using Shared.IO;
using System;
using System.Runtime.CompilerServices;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class GCMonitor : PixelSystem
    {
        public override string Name { get; set; } = "GC Monitoring System";
        public int[] GenCollections;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Initialize()
        {
            GenCollections = new int[3];
            StartWorkerThreads(1, true, ThreadPriority.Lowest);
            IsActive = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void FixedUpdate(float gameTime) => UnblockThreads();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AsyncUpdate(int threadId)
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