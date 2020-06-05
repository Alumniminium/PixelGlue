using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelGlueCore.ECS.Systems
{
    public class SmartFramerate : IEntitySystem
    {
        public string Name { get; set; } = "Update Rate Monitoring System";
        public bool IsActive  { get; set; } 
        public bool IsReady { get; set; } 

        double currentFrametimes;
        double weight;
        int numerator;
        int counter;

        public double updateRate;

        public SmartFramerate(int oldFrameWeight)
        {
            numerator = oldFrameWeight;
            weight = (double)oldFrameWeight / ((double)oldFrameWeight - 1d);
        }

        public void Update(double timeSinceLastFrame)
        {
            counter++;
            currentFrametimes = currentFrametimes / weight;
            currentFrametimes += timeSinceLastFrame;
            if (counter == 200)
            {
                updateRate = (numerator / currentFrametimes);
                counter = 0;
            }
        }
        public void Draw(Scene scene, SpriteBatch sb)
        { 
            AssetManager.Fonts["profont"].Draw($"PixelGlue Engine (Objects: {(scene.Map.TileArray[0].Length * scene.Map.TileArray.Length) + scene.Entities.Count + scene.Components.Values.Sum(p=>p.Count)})", new Vector2(16, 16), sb);
            AssetManager.Fonts["profont"].Draw($"Position: {scene.Camera.ScreenRect.X},{scene.Camera.ScreenRect.Y}", new Vector2(16, 164), sb);
            AssetManager.Fonts["profont"].Draw("FPS: " + updateRate.ToString("##0.00"), new Vector2(16, 64), sb);
            AssetManager.Fonts["profont"].Draw($"Draw: {PixelGlue.DrawProfiler.Time:##0.00}ms, Update: {PixelGlue.UpdateProfiler.Time:##0.00}ms", new Vector2(16, 96), sb);
        }
    }
}