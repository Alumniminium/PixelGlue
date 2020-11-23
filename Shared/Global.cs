using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shared.ECS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Shared
{
    public static class Global
    {
        public static Random Random { get; set; } = new Random(1337);
        public static float FixedUpdateHz { get; set; } = 10;
        public static int ScreenWidth { get; set; } = 1600;//1440;//1600;
        public static int ScreenHeight { get; set; } = 900;//960;//900;
        public const int VirtualScreenWidth = 400;//400;//320;
        public const int VirtualScreenHeight = 225;//225;//180;
        public const int HalfVirtualScreenWidth = VirtualScreenWidth / 2;
        public const int HalfVirtualScreenHeight = VirtualScreenHeight / 2;
        public static int HalfScreenWidth => ScreenWidth / 2;
        public static int HalfScreenHeight =>ScreenHeight / 2;
        public static GraphicsDevice Device { get; set; }
        public static string[] Names { get; set; }
        public static ContentManager ContentManager { get; set; }
        public static int TileSize => 16;
        public static long FrameCounter { get; set; }
        public static GraphicsMetrics Metrics { get; set; }
        public static bool Verbose { get; set; } = true;
        public static float DrawTime { get; set; }
        public static float UpdateTime { get; set; }
        public static int Major { get; set; } = 0;
        public static int Minor { get; set; } = 6;
        public static Vector2 HalfTileSizeVector { get; set; } =  new Vector2(TileSize/2,TileSize/2);
        public static ConcurrentDictionary<Entity,Prefab> Prefabs = new ConcurrentDictionary<Entity, Prefab>();

        //public static Queue<Action> PostUpdateQueue = new Queue<Action>(128);
    }
}
