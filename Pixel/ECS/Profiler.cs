using System.Globalization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PixelShared;

namespace Pixel.ECS
{
    public static class Profiler
    {
        public static readonly Dictionary<string, Metrics> SystemUpdateTimes = new Dictionary<string, Metrics>();
        public static readonly Dictionary<string, Metrics> SystemDrawTimes = new Dictionary<string, Metrics>();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void AddUpdate(string system, float time)
        {
            time = (float)Math.Round(time, 2);
            Global.UpdateTime+=time;
            if (time == 0)
                return;
            if (!SystemUpdateTimes.TryGetValue(system, out var list))
            {
                list = new Metrics();
                SystemUpdateTimes.Add(system, list);
            }
            if (list.Created.AddSeconds(30) < DateTime.UtcNow)
            {
                list = new Metrics();
                SystemUpdateTimes[system] = list;
            }
            list.AddValue(time);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void AddDraw(string system, float time)
        {
            time = (float)Math.Round(time, 2);
            Global.DrawTime+=time;
            if (time == 0)
                return;
            if (!SystemDrawTimes.TryGetValue(system, out var list))
            {
                list = new Metrics();
                SystemDrawTimes.Add(system, list);
            }
            if (list.Created.AddSeconds(30) < DateTime.UtcNow)
            {
                list = new Metrics();
                SystemDrawTimes[system] = list;
            }
            list.AddValue(time);
        }
    }
}