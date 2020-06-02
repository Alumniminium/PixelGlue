using PixelGlueCore.ECS;
using PixelGlueCore.Helpers;
using System;
using System.Collections.Generic;

namespace PixelGlueCore
{
    public class PixelGlue
    {
        public static Random Random { get; set; } = new Random(Environment.TickCount);
        public static float FixedUpdateHz { get; set; } = 15;
        public static int ScreenWidth { get; set; } = 1280;
        public static int ScreenHeight { get; set; } = 720;
        public static int VirtualScreenWidth { get; set; } = 320;
        public static int VirtualScreenHeight { get; set; } = 180;
        public static bool Profiling { get; set; }
        public static Profiler UpdateProfiler { get; set; } = new Profiler();
        public static Profiler DrawProfiler { get; set; } = new Profiler();
    }
}
