using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Enums;
using PixelShared;

namespace Pixel.ECS.Systems
{
    public class SmartFramerate : IEntitySystem
    {
        public string Name { get; set; } = "Update Rate Monitoring System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        private int updateStringsCounter = 0;
        private double currentFrametimes;
        private int counter;
        public double updateRate;
        private readonly double weight;
        private readonly int numerator;
        public Scene Scene;
        public SmartFramerate(Scene scene, int oldFrameWeight)
        {
            Scene = scene;
            numerator = oldFrameWeight;
            weight = (double)oldFrameWeight / ((double)oldFrameWeight - 1d);
        }
        public void FixedUpdate(float _)
        {
            updateStringsCounter++;
            if (updateStringsCounter > 10)
            {
                updateStringsCounter = 0;
                   lines = new[]
                {
                    $"PixelGlue Engine (Entities: {Scene.Entities.Count}, Draw calls: {Global.DrawCalls})",
                    "FPS: " + updateRate.ToString("##0.00"),
                    $"Draw: {Global.DrawProfiler.Time:##0.00}ms, Update: {Global.UpdateProfiler.Time:##0.00}ms"
                };
            }
        }
        public void Update(float timeSinceLastFrame)
        {
            counter++;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            if (counter == 200)
            {
                updateRate = numerator / currentFrametimes;
                counter = 0;
            }
        }
        string[] lines = new[]
        {
            $"PixelGlue Engine (Entities: 0, Draw calls: {Global.DrawCalls})",
            "FPS:",
            $"Draw: {Global.DrawProfiler.Time:##0.00}ms, Update: {Global.UpdateProfiler.Time:##0.00}ms"
        };
        public void Draw(SpriteBatch sb)
        {
            AssetManager.Fonts["profont"].Draw(lines[0], new Vector2(16, 16), sb);
            AssetManager.Fonts["profont"].Draw(lines[1], new Vector2(16, 64), sb);
            AssetManager.Fonts["profont"].Draw(lines[2], new Vector2(16, 96), sb);
        }
    }
}