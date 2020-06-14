using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;

namespace PixelGlueCore.ECS.Systems
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
        public Player player;

        public SmartFramerate(int oldFrameWeight)
        {
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
        public void Draw(Scene scene, SpriteBatch sb)
        {
            AssetManager.Fonts["profont"].Draw($"PixelGlue Engine (Entities: {scene.Entities.Count}, Rendered: {PixelGlue.RenderedObjects})", new Vector2(16, 16), sb);
            if(player==null)
            player = scene.Find<Player>();
            if (player != null)
            {
                ref var pos = ref player.Get<PositionComponent>();
                AssetManager.Fonts["profont"].Draw($"Position: {pos.Position.X},{pos.Position.Y}", new Vector2(16, 164), sb);
            }
            AssetManager.Fonts["profont"].Draw("FPS: " + updateRate.ToString("##0.00"), new Vector2(16, 64), sb);
            AssetManager.Fonts["profont"].Draw($"Draw: {PixelGlue.DrawProfiler.Time:##0.00}ms, Update: {PixelGlue.UpdateProfiler.Time:##0.00}ms", new Vector2(16, 96), sb);
        }
    }
}