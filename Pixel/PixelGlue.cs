using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using PixelShared.Diagnostics;

namespace Pixel
{
    public static class Global
    {
        public static Random Random { get; set; } = new Random(1337);
        public static float FixedUpdateHz { get; set; } = 15;
        public static int ScreenWidth { get; set; } = 1600;
        public static int ScreenHeight { get; set; } = 900;
        public const int VirtualScreenWidth = 400;//320;
        public const int VirtualScreenHeight = 225;//180;
        public const int HalfVirtualScreenWidth =VirtualScreenWidth/2;
        public const int HalfVirtualScreenHeight = VirtualScreenHeight/2;
        public static bool Profiling { get; set; }
        public static GraphicsDevice Device { get; set; }
        public static Profiler UpdateProfiler { get; set; } = new Profiler();
        public static Profiler DrawProfiler { get; set; } = new Profiler();
        public static int DrawCalls { get; internal set; }
        public static string[] Names { get; internal set; }
        public static ContentManager ContentManager { get; set; }
    }
}