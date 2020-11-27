using System.Threading;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;
using Shared.ECS;
using Shared.Diagnostics;
using Pixel.Helpers;
using Shared.IO;

namespace Pixel.ECS.Systems
{
    public class SmartFramerate : PixelSystem
    {
        public override string Name { get; set; } = "Metrics UI Overlay System";
        private double currentFrametimes;
        private int counter;
        public double updateRate;
        private double weight;
        private int numerator;
        private readonly string[] lines = new string[64];

        public SmartFramerate(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void Initialize()
        {
            numerator = 3;
            weight = 3 / (3 - 1d);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = string.Empty;
            lines[7] = "Legend: last, 30s avg, 30s max";
            IsActive = true;
        }

        public override void Update(float timeSinceLastFrame)
        {
            counter++;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            if (counter == 150)
            {
                updateRate = numerator / currentFrametimes;
                counter = 0;
                lines[0] = $"| Pixel Engine | Entities: {World.EntityCount}, Textures: {Global.Metrics.TextureCount}";
                lines[0] = $"| Entities: {World.EntityCount}, Textures: {Global.Metrics.TextureCount}";
                lines[1] = $"| Primitives: {Global.Metrics.PrimitiveCount}, Targets: {Global.Metrics.TargetCount}";
                lines[2] = $"| Draw calls: {Global.Metrics.DrawCount} (Pre Batch: {Global.Metrics.SpriteCount})";
                lines[3] = $"| FPS: {updateRate:##0} Frametime: {Global.DrawTime + Global.UpdateTime:##0.00}ms";
                lines[4] = $"| Draw: {Global.DrawTime:##0.00}ms, Update: {Global.UpdateTime:##0.00}ms";
                lines[5] = $"| v {Global.Major}.{Global.Minor:00}, {DateTime.Now.Day:00}/{DateTime.Now.Month:00}/{DateTime.Now.Year:00}";

                int lastLine = 8;
                for (int i = 0; i < World.Systems.Count; i++)
                {
                    var system = World.Systems[i];
                    if (!system.IsActive)
                        continue;
                    lines[lastLine++] = string.Empty;
                    lines[lastLine++] = $"{system.Name}";
                    if (Profiler.SystemUpdateTimes.ContainsKey(system.Name))
                        lines[lastLine++] = $"Update: {Profiler.SystemUpdateTimes[system.Name].Values[^1].ToString("#0.00")}, {Profiler.SystemUpdateTimes[system.Name].Avg:#0.00}, {Profiler.SystemUpdateTimes[system.Name].Max:#0.00}";
                    if (Profiler.SystemDrawTimes.ContainsKey(system.Name))
                        lines[lastLine++] = $"Draw:   {Profiler.SystemDrawTimes[system.Name].Values[^1].ToString("#0.00")}, {Profiler.SystemDrawTimes[system.Name].Avg:#0.00}, {Profiler.SystemDrawTimes[system.Name].Max:#0.00}";

                }
                for (int i = lastLine; i < lines.Length; i++)
                    lines[i] = string.Empty;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < lines.Length; i++)
                AssetManager.Fonts["profont"].DrawText(sb, 16, 8 + 18 * i, lines[i], Color.Magenta, 0.72f);
        }
    }
}