using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Enums;
using Pixel.Scenes;
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
        public Scene Scene => SceneManager.ActiveScene;
        private readonly string[] lines = new string[2];
        public SmartFramerate(int oldFrameWeight)
        {
            numerator = oldFrameWeight;
            weight = (double)oldFrameWeight / ((double)oldFrameWeight - 1d);
            lines[0] = string.Empty;
            lines[1] = string.Empty;
        }
        public void FixedUpdate(float _)
        {
            updateStringsCounter++;
            if (updateStringsCounter > 10)
            {
                updateStringsCounter = 0;
                lines[0] = $"Pixel Engine (Entities: {Scene.Entities.Count}, Draw calls: {Global.DrawCalls})";
                lines[1] = $"FPS: {updateRate:##0} (Total: {Global.DrawProfiler.Time + Global.UpdateProfiler.Time:##0.00}ms, Draw: {Global.DrawProfiler.Time:##0.00}ms, Update: {Global.UpdateProfiler.Time:##0.00}ms)";
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

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < lines.Length; i++)
                AssetManager.Fonts["profont"].DrawText(sb, 16, 8 + 32 * i, lines[i]);
        }
    }
}