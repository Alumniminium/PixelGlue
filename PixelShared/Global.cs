using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelShared.Diagnostics;
using System;

namespace PixelShared
{
    public static class Global
    {
        public static Random Random { get; set; } = new Random(1337);
        public static float FixedUpdateHz { get; set; } = 15;
        public static int ScreenWidth { get; set; } = 1440;//1600;
        public static int ScreenHeight { get; set; } = 960;//900;
        public const int VirtualScreenWidth = 320;//400;//320;
        public const int VirtualScreenHeight = 224;//225;//180;
        public const int HalfVirtualScreenWidth = VirtualScreenWidth / 2;
        public const int HalfVirtualScreenHeight = VirtualScreenHeight / 2;
        public static bool Profiling { get; set; }
        public static GraphicsDevice Device { get; set; }
        public static Profiler UpdateProfiler { get; set; } = new Profiler();
        public static Profiler DrawProfiler { get; set; } = new Profiler();
        public static int DrawCalls { get; set; }
        public static string[] Names { get; set; }
        public static ContentManager ContentManager { get; set; }
        public static int TileSize => 16;
    }
}
