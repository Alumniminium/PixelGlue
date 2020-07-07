using System;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Scenes;
using Shared;

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

        public override void Update(float timeSinceLastFrame)
        {
            lines[0] = $"| Pixel Engine | Entities: {Scene.Entities.Count}, Sprites: {Global.Metrics.SpriteCount}, Primitives: {Global.Metrics.PrimitiveCount}, Textures: {Global.Metrics.TextureCount}, Targets: {Global.Metrics.TargetCount}";
            lines[1] = $"|   v {Global.Major}.{Global.Minor:00}     | Draw calls: {Global.Metrics.DrawCount} (Pre Batch: {Global.DrawCalls})";
            lines[2] = $"|  {DateTime.Now.Day:00}/{DateTime.Now.Month:00}/{DateTime.Now.Year:0000}  | FPS: {updateRate:##0} (Total: {Global.DrawTime + Global.UpdateTime:##0.00}ms, Draw: {Global.DrawTime:##0.00}ms, Update: {Global.UpdateTime:##0.00}ms)";
            lines[4] =  "Legend:  last, 30s avg, 30s max";
            int lastLine = 5;
            for (int i = 0; i < SceneManager.ActiveScene.Systems.Count; i++)
            {
                var system = Scene.Systems[i];
                var draw =   "Draw:   00.00, 00.00, 00.00";
                var update = "Update: 00.00, 00.00, 00.00";
                if (Profiler.SystemDrawTimes.ContainsKey(system.Name) && Profiler.SystemDrawTimes[system.Name].Sum != 0)
                    draw = $"Draw:   {Profiler.SystemDrawTimes[system.Name].Values[^1].ToString("00.00")}, {Profiler.SystemDrawTimes[system.Name].Avg:00.00}, {Profiler.SystemDrawTimes[system.Name].Max:00.00}";
                if (Profiler.SystemUpdateTimes.ContainsKey(system.Name) && Profiler.SystemUpdateTimes[system.Name].Sum != 0)
                    update = $"Update: {Profiler.SystemUpdateTimes[system.Name].Values[^1].ToString("00.00")}, {Profiler.SystemUpdateTimes[system.Name].Avg:00.00}, {Profiler.SystemUpdateTimes[system.Name].Max:00.00}";
                lines[lastLine] = string.Empty;
                lines[lastLine+1] = $"{system.Name}";
                lines[lastLine+2] = $"{update}";
                lines[lastLine+3] = $"{draw}";
                lastLine += 4;
            }
            counter++;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            if (counter == 200)
            {
                updateRate = numerator / currentFrametimes;
                counter = 0;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < lines.Length; i++)
                AssetManager.Fonts["profont"].DrawText(sb, 16, 8 + 18 * i, lines[i], 0.72f);
        }

        public override void Initialize()
        {
            numerator = 3;
            weight = 3 / (3 - 1d);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = string.Empty;
            IsActive = true;
        }
    }
}