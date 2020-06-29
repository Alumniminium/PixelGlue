using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Enums;
using Pixel.Entities;
using PixelShared;

namespace Pixel.ECS.Systems
{
    public class SmartFramerate : IEntitySystem
    {
        public string Name { get; set; } = "Update Rate Monitoring System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        private double currentFrametimes;
        private int counter;
        public double updateRate;
        private readonly double weight;
        private readonly int numerator;
        public Scene Scene;
        public SmartFramerate(Scene scene,int oldFrameWeight)
        {
            Scene=scene;
            numerator = oldFrameWeight;
            weight = (double)oldFrameWeight / ((double)oldFrameWeight - 1d);
        }
        public void FixedUpdate(float _) { }
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
            AssetManager.Fonts["profont"].Draw($"PixelGlue Engine (Entities: {Scene.Entities.Count}, Draw calls: {Global.DrawCalls})", new Vector2(16, 16), sb);
            AssetManager.Fonts["profont"].Draw("FPS: " + updateRate.ToString("##0.00"), new Vector2(16, 64), sb);
            AssetManager.Fonts["profont"].Draw($"Draw: {Global.DrawProfiler.Time:##0.00}ms, Update: {Global.UpdateProfiler.Time:##0.00}ms", new Vector2(16, 96), sb);
        }
    }
}