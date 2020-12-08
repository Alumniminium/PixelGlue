using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared;
using Shared.ECS;
using Shared.Diagnostics;
using Pixel.Helpers;
using System.Collections.Generic;

namespace Pixel.ECS.Systems
{
    public class SmartFramerate : PixelSystem
    {
        private double currentFrametimes;
        private int counter;
        public double updateRate;
        private double weight;
        private int numerator;
        private readonly string[] lines = new string[64];

        public SmartFramerate(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name  = "Metrics UI Overlay System";}
        public override void Initialize()
        {
            numerator = 3;
            weight = 3 / (3 - 1d);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = string.Empty;
            lines[7] = "Legend: last, 30s avg, 30s max";
            IsActive = true;
        }

        public override void Update(float deltaTime, List<Entity> Entities)
        {
            counter++;
            currentFrametimes /= weight;
            currentFrametimes += deltaTime;
            if (counter == 60)
            {
                updateRate = numerator / currentFrametimes;
                counter = 0;
                lines[0] = $"| Pixel Engine v {Global.Major}.{Global.Minor:00}, {DateTime.Now.Day:00}/{DateTime.Now.Month:00}/{DateTime.Now.Year:00}";
                lines[1] = $"| Entities: {World.EntityCount}, Textures: {Global.Metrics.TextureCount}";
                lines[2] = $"| Primitives: {Global.Metrics.PrimitiveCount}, Targets: {Global.Metrics.TargetCount}";
                lines[3] = $"| Draw calls: {Global.Metrics.DrawCount} (Pre Batch: {Global.Metrics.SpriteCount})";
                lines[4] = $"| FPS: {updateRate:########0} Frametime: {Global.DrawTime + Global.UpdateTime:##0.00}ms";
                lines[5] = $"| Draw: {Global.DrawTime:##0.00}ms, Update: {Global.UpdateTime:##0.00}ms";

                int lastLine = 7;
                for (int i = 0; i < World.Systems.Count; i++)
                {
                    var system = World.Systems[i];
                    if (!system.IsActive)
                        continue;

                    var updateCur = 0f;
                    var updateAvg = 0f;
                    var updateMax = 0f;
                    var drawCur = 0f;
                    var drawAvg = 0f;
                    var drawMax = 0f;

                    if (Profiler.SystemUpdateTimes.ContainsKey(system.Name))
                    {
                        updateCur = Profiler.SystemUpdateTimes[system.Name].Values[^1];
                        updateAvg = Profiler.SystemUpdateTimes[system.Name].Avg;
                        updateMax = Profiler.SystemUpdateTimes[system.Name].Max;
                    }
                    if (Profiler.SystemDrawTimes.ContainsKey(system.Name))
                    {
                        drawCur = Profiler.SystemDrawTimes[system.Name].Values[^1];
                        drawAvg = Profiler.SystemDrawTimes[system.Name].Avg;
                        drawMax = Profiler.SystemDrawTimes[system.Name].Max;
                    }
                    
                    //if(updateMax >0.5f || drawMax >0.5f)
                    //{
                        lines[lastLine++] = string.Empty;
                        lines[lastLine++] = $"{system.Name}";
                        
                    //    if(updateMax > 0.5f)
                                lines[lastLine++] = $"Update: {updateCur.ToString("#0.00")}, {updateAvg:#0.00}, {updateMax:#0.00}";
                    //    if(drawMax > 0.5f)
                                lines[lastLine++] = $"Draw: {drawCur.ToString("#0.00")}, {drawAvg:#0.00}, {drawMax:#0.00}";
                    //}
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