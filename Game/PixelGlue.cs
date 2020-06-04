using PixelGlueCore.Helpers;
using System;

namespace PixelGlueCore
{
    public class PixelGlue
    {
        public static Random Random { get; set; } = new Random(Environment.TickCount);
        public static float FixedUpdateHz { get; set; } = 15;
        public static int ScreenWidth { get; set; } = 1280;
        public static int ScreenHeight { get; set; } = 720;
        public const int VirtualScreenWidth = 320;
        public const int VirtualScreenHeight = 180;
        public const int HalfVirtualScreenWidth =VirtualScreenWidth/2;
        public const int HalfVirtualScreenHeight = VirtualScreenHeight/2;
        public static bool Profiling { get; set; }
        public static Profiler UpdateProfiler { get; set; } = new Profiler();
        public static Profiler DrawProfiler { get; set; } = new Profiler();
    }
}
