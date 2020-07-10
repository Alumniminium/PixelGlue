using System.Threading;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Scenes;
using Shared;
using Shared.IO;
using Shared.Diagnostics;
using System.Runtime.CompilerServices;

namespace Pixel.ECS.Systems
{
    public class SmartFramerate : PixelSystem
    {
        public override string Name { get; set; } = "Metrics UI Overlay System";
        public Thread Worker;
        public AutoResetEvent Block = new AutoResetEvent(true);
        private double currentFrametimes;
        private int counter;
        public double updateRate;
        private double weight;
        private int numerator;

        private readonly string[] lines = new string[64];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Initialize()
        {
            numerator = 3;
            weight = 3 / (3 - 1d);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = string.Empty;
            lines[4] = "Legend:  last, 30s avg, 30s max";

            Worker = new Thread(Step)
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };
            Worker.Start();
            IsActive = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Step()
        {
            while (true)
            {
                Block.WaitOne();

                int lastLine = 5;
                for (int i = 0; i < SceneManager.ActiveScene.Systems.Count; i++)
                {
                    var system = Scene.Systems[i];
                    if (!system.IsActive)
                        continue;
                    lines[lastLine++] = string.Empty;
                    lines[lastLine++] = $"{system.Name}";
                    if (Profiler.SystemUpdateTimes.ContainsKey(system.Name))
                        lines[lastLine++] = $"Update: {Profiler.SystemUpdateTimes[system.Name].Values[^1].ToString("00.00")}, {Profiler.SystemUpdateTimes[system.Name].Avg:00.00}, {Profiler.SystemUpdateTimes[system.Name].Max:00.00}";
                    if (Profiler.SystemDrawTimes.ContainsKey(system.Name))
                        lines[lastLine++] = $"Draw:   {Profiler.SystemDrawTimes[system.Name].Values[^1].ToString("00.00")}, {Profiler.SystemDrawTimes[system.Name].Avg:00.00}, {Profiler.SystemDrawTimes[system.Name].Max:00.00}";

                }
                for (int i = lastLine; i < lines.Length; i++)
                    lines[i] = string.Empty;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update(float timeSinceLastFrame)
        {
            lines[0] = $"| Pixel Engine | Entities: {Scene.Entities.Count}, Primitives: {Global.Metrics.PrimitiveCount}, Textures: {Global.Metrics.TextureCount}, Targets: {Global.Metrics.TargetCount}";
            lines[1] = $"|   v {Global.Major}.{Global.Minor:00}     | Draw calls: {Global.Metrics.DrawCount} (Pre Batch: {Global.Metrics.SpriteCount})";
            lines[2] = $"|  {DateTime.Now.Day:00}/{DateTime.Now.Month:00}/{DateTime.Now.Year:0000}  | FPS: {updateRate:##0} (Total: {Global.DrawTime + Global.UpdateTime:##0.00}ms, Draw: {Global.DrawTime:##0.00}ms, Update: {Global.UpdateTime:##0.00}ms)";

            counter++;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            if (counter == 150)
            {
                updateRate = numerator / currentFrametimes;
                counter = 0;
                Block.Set();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < lines.Length; i++)
                AssetManager.Fonts["profont"].DrawText(sb, 16, 8 + 18 * i, lines[i], Color.IndianRed, 0.72f);
        }
    }
}