using System.Runtime.CompilerServices;
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
        private double currentFrametimes;
        private int counter;
        public double updateRate;
        private readonly double weight;
        private readonly int numerator;
        public Scene Scene => SceneManager.ActiveScene;
        private readonly string[] lines = new string[3];
        public SmartFramerate(int oldFrameWeight)
        {
            numerator = oldFrameWeight;
            weight = oldFrameWeight / (oldFrameWeight - 1d);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = string.Empty;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FixedUpdate(float _)
        {
            lines[0] = $" Pixel Engine | Entities: {Scene.Entities.Count}, Sprites: {Global.Metrics.SpriteCount}, Primitives: {Global.Metrics.PrimitiveCount}";
            lines[1] = $"    v 0.1     | Draw calls: {Global.Metrics.DrawCount}, Textures: {Global.Metrics.TextureCount}, Targets: {Global.Metrics.TargetCount}";
            lines[2] = $"  7/20/2020   | FPS: {updateRate:##0} (Total: {Global.DrawProfiler.Time + Global.UpdateProfiler.Time:##0.00}ms, Draw: {Global.DrawProfiler.Time:##0.00}ms, Update: {Global.UpdateProfiler.Time:##0.00}ms)";
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < lines.Length; i++)
                AssetManager.Fonts["profont"].DrawText(sb, 16, 8 + 32 * i, lines[i]);
        }
    }
}