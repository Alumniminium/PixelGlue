using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.Enums;
using PixelGlueCore.Loaders;
using PixelGlueCore.Helpers;
using System;
using System.Collections.Generic;
using static PixelGlueCore.Loaders.FastNoise;
using System.Threading;
using System.Collections.Concurrent;

namespace PixelGlueCore.ECS.Systems
{
    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene { get; set; }

        public EntityRenderSystem(GameScene scene)
        {
            Scene = scene;
        }
        public void Update(float timeSinceLastFrame)
        {
        }
        public void FixedUpdate(float timeSinceLastFrame)
        {
        }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;

            var overdraw = Scene.Map.TileWidth * 2;
            var origin = new Vector2(8, 8);
            PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 0, Scene.Camera);
            PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 1, Scene.Camera);
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;
            }
            if (Scene.Map?.Layers?.Count >= 2)
                PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 2, Scene.Camera);
        }
    }
    public class ProceduralEntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene { get; set; }
        public ConcurrentDictionary<(int x, int y), DrawableComponent> Tiles = new ConcurrentDictionary<(int x, int y), DrawableComponent>();
        public Dictionary<(int x, int y), DrawableComponent?> Tiles2 = new Dictionary<(int x, int y), DrawableComponent?>();
        public Thread[] Prefetcher = new Thread[12];
        public ConcurrentQueue<(int x, int y)>[] Queue = new ConcurrentQueue<(int x, int y)>[12];
        public ProceduralEntityRenderSystem(GameScene scene)
        {
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(18, 78, 137, 255)), "deep_water");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(0, 153, 219, 255)), "water");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(76, 183, 229, 255)), "shallow_water");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(234, 212, 170, 255)), "sand");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(232, 183, 150, 255)), "sand2");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(228, 166, 114, 255)), "sand3");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(194, 133, 105, 255)), "dirt");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(99, 199, 77, 255)), "plains");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(62, 137, 72, 255)), "grass");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(38, 92, 66, 255)), "trees");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(184, 111, 80, 255)), "rock");
            AssetManager.LoadTexture(Texture2DExt.Blank(1, 1, Color.FromNonPremultiplied(192, 203, 220, 255)), "snow");
            Scene = scene;
            for (int i = 0; i < Prefetcher.Length; i++)
            {
                Queue[i]=new ConcurrentQueue<(int x, int y)>();
                Prefetcher[i] = new Thread(new ParameterizedThreadStart(Load));
                Prefetcher[i].IsBackground = true;
                Prefetcher[i].Start(i);
            }
        }
        public void Load(object idobj)
        {
            int id = (int)idobj;
            while (true)
            {
                while (Queue[id].TryDequeue(out var point))
                {
                    var x = point.x;
                    var y = point.y;
                    if(Tiles.ContainsKey((x,y)))
                        continue;
                    DrawableComponent d;
                    var srcRect = new Rectangle(0, 0, 1, 1);
                    var dstRect = new Rectangle(x, y, 16, 16);
                    PixelGlue.Noise.SetSeed(1337);
                PixelGlue.Noise.SetNoiseType(NoiseType.Simplex);
                PixelGlue.Noise.SetInterp(Interp.Linear);

                var val = PixelGlue.Noise.GetNoise(x / 32, y / 32, PixelGlue.Z / 32);
                val += 0.5f * PixelGlue.Noise.GetNoise(x / 16, y / 16, PixelGlue.Z / 16);
                val += 0.15f * PixelGlue.Noise.GetNoise(x / 2, y / 2, PixelGlue.Z / 2);

                if (val > 0.94)
                    d = new DrawableComponent(0, "snow", srcRect, dstRect);
                else if (val > 0.70)
                    d = new DrawableComponent(0, "rock", srcRect, dstRect);
                else if (val > 0.4)
                    d = new DrawableComponent(0, "trees", srcRect, dstRect);
                else if (val > 0.30)
                    d = new DrawableComponent(0, "grass", srcRect, dstRect);
                else if (val > -0.20)
                    d = new DrawableComponent(0, "plains", srcRect, dstRect);
                else if (val > -0.4f)
                    d = new DrawableComponent(0, "dirt", srcRect, dstRect);
                else if (val > -0.41 && val < 0.42)
                    d = new DrawableComponent(0, "sand", srcRect, dstRect);
                else if (val > -0.5f)
                    d = new DrawableComponent(0, "shallow_water", srcRect, dstRect);
                else if (val > -0.7)
                    d = new DrawableComponent(0, "water", srcRect, dstRect);
                else
                    d = new DrawableComponent(0, "deep_water", srcRect, dstRect);
                    Tiles.TryAdd((x, y), d);
                    //Thread.Sleep(10);
                }
                Thread.Sleep(1);
            }
        }
        public void Update(float timeSinceLastFrame) { }
        public void FixedUpdate(float timeSinceLastFrame) { }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;

            var overdraw = Scene.Map.TileWidth * 2;
            int last=0;
            for (int x = Scene.Camera.ScreenRect.Left - overdraw; x < Scene.Camera.ScreenRect.Right + overdraw; x += 16)//Scene.Map.TileWidth)
                for (int y = Scene.Camera.ScreenRect.Top - overdraw; y < Scene.Camera.ScreenRect.Bottom + overdraw; y += 16)//Scene.Map.TileHeight)
                {
                    if (!Tiles.TryGetValue((x, y), out var terrainTile))
                    {
                        Queue[last].Enqueue((x, y));
                        last++;
                        if(last==Queue.Length)
                        last=0;
                        continue;
                    }
                    //  var  terrainTile = Terrain(x, y);
                    sb.Draw(AssetManager.GetTexture(terrainTile.TextureName), terrainTile.DestRect, terrainTile.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    PixelGlue.DrawCalls++;

                    //if (!Tiles2.TryGetValue((x, y), out var riverTile))
                    //    riverTile = River(x, y);
                    //if (!riverTile.HasValue)
                    //    continue;

                    //sb.Draw(AssetManager.GetTexture(riverTile.Value.TextureName), riverTile.Value.DestRect, riverTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    //PixelGlue.DrawCalls++;
                }
            RenderEntities(sb, overdraw);
        }

        private void RenderEntities(SpriteBatch sb, int overdraw)
        {
            var origin = new Vector2(8, 8);
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;
            }
        }

        public DrawableComponent? River(int x, int y)
        {
            float x2 = x;
            float y2 = y;
            PixelGlue.Noise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            PixelGlue.Noise.SetCellularReturnType(CellularReturnType.Distance2Div);
            PixelGlue.Noise.SetCellularJitter(0.4f);
            PixelGlue.Noise.SetSeed(965678457);
            PixelGlue.Noise.SetGradientPerturbAmp(30);
            PixelGlue.Noise.GradientPerturbFractal(ref x2, ref y2);
            var val = PixelGlue.Noise.GetCellular(x2 * 0.002f, y2 * 0.002f);
            DrawableComponent? d;
            var srcRect = new Rectangle(0, 0, 1, 1);
            var dstRect = new Rectangle(x, y, 16, 16);
            if (val > 0.999)
                d = new DrawableComponent(0, "deep_water", srcRect, dstRect);
            else if (val > 0.998)
                d = new DrawableComponent(0, "water", srcRect, dstRect);
            else if (val > 0.997)
                d = new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else
                d = null;
            Tiles2.Add((x, y), d);
            return d;
        }
        public float Biome(int x, int y)
        {
            float x2 = x;
            float y2 = y;
            PixelGlue.Noise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            PixelGlue.Noise.SetCellularReturnType(CellularReturnType.CellValue);
            PixelGlue.Noise.SetCellularJitter(0.4f);
            PixelGlue.Noise.SetSeed(965678457);
            PixelGlue.Noise.SetGradientPerturbAmp(22);
            PixelGlue.Noise.SetFrequency(0.02f);
            PixelGlue.Noise.GradientPerturbFractal(ref x2, ref y2);
            var val = PixelGlue.Noise.GetCellular(x2 * 0.004f, y2 * 0.004f);

            if (val > 0.5)
                return 1;
            else if (val > 0)
                return 2;
            else if (val > -0.5)
                return 4;
            else
                return 5;
        }
        private DrawableComponent Terrain(int x, int y)
        {
            DrawableComponent d;
            var srcRect = new Rectangle(0, 0, 1, 1);
            var dstRect = new Rectangle(x, y, 16, 16);

            var biome = 1;//Biome(x, y, scale*10);

            if (biome == 1)
            {
                PixelGlue.Noise.SetSeed(1337);
                PixelGlue.Noise.SetNoiseType(NoiseType.Simplex);
                PixelGlue.Noise.SetInterp(Interp.Linear);

                var val = PixelGlue.Noise.GetNoise(x / 32, y / 32);
                //val += 0.25f * PixelGlue.Noise.GetNoise(x / 16, y / 16,PixelGlue.Z / 16);
                //val += 0.15f * PixelGlue.Noise.GetNoise(x / 16, y / 16,PixelGlue.Z / 32);

                if (val > 0.94)
                    d = new DrawableComponent(0, "rock", srcRect, dstRect);
                else if (val > 0.6f)
                    d = new DrawableComponent(0, "sand3", srcRect, dstRect);
                else if (val > -0.4)
                    d = new DrawableComponent(0, "sand2", srcRect, dstRect);
                else
                    d = new DrawableComponent(0, "sand", srcRect, dstRect);
            }
            else if (biome == 2)
            {
                PixelGlue.Noise.SetSeed(1337);
                PixelGlue.Noise.SetNoiseType(NoiseType.Simplex);
                PixelGlue.Noise.SetInterp(Interp.Linear);

                var val = PixelGlue.Noise.GetNoise(x / 32, y / 32);
                //val += 0.25f * PixelGlue.Noise.GetNoise(x / 16, y / 16,PixelGlue.Z / 16);
                //val += 0.15f * PixelGlue.Noise.GetNoise(x / 16, y / 16,PixelGlue.Z / 32);

                if (val > 0.94)
                    d = new DrawableComponent(0, "rock", srcRect, dstRect);
                else if (val > 0.6f)
                    d = new DrawableComponent(0, "sand3", srcRect, dstRect);
                else if (val > -0.4)
                    d = new DrawableComponent(0, "sand2", srcRect, dstRect);
                else
                    d = new DrawableComponent(0, "sand", srcRect, dstRect);
            }
            else if (biome == 3)
            {
                PixelGlue.Noise.SetSeed(1337);
                PixelGlue.Noise.SetNoiseType(NoiseType.Simplex);
                PixelGlue.Noise.SetInterp(Interp.Linear);

                var val = PixelGlue.Noise.GetNoise(x / 32, y / 32, PixelGlue.Z / 32);
                val += 0.5f * PixelGlue.Noise.GetNoise(x / 16, y / 16, PixelGlue.Z / 16);
                val += 0.15f * PixelGlue.Noise.GetNoise(x / 2, y / 2, PixelGlue.Z / 2);

                if (val > 0.94)
                    d = new DrawableComponent(0, "snow", srcRect, dstRect);
                else if (val > 0.70)
                    d = new DrawableComponent(0, "rock", srcRect, dstRect);
                else if (val > 0.4)
                    d = new DrawableComponent(0, "trees", srcRect, dstRect);
                else if (val > 0.30)
                    d = new DrawableComponent(0, "grass", srcRect, dstRect);
                else if (val > -0.20)
                    d = new DrawableComponent(0, "plains", srcRect, dstRect);
                else if (val > -0.4f)
                    d = new DrawableComponent(0, "dirt", srcRect, dstRect);
                else if (val > -0.41 && val < 0.42)
                    d = new DrawableComponent(0, "sand", srcRect, dstRect);
                else if (val > -0.5f)
                    d = new DrawableComponent(0, "shallow_water", srcRect, dstRect);
                else if (val > -0.7)
                    d = new DrawableComponent(0, "water", srcRect, dstRect);
                else
                    d = new DrawableComponent(0, "deep_water", srcRect, dstRect);
            }
            else
            {
                PixelGlue.Noise.SetSeed(1337);
                PixelGlue.Noise.SetNoiseType(NoiseType.Cellular);
                PixelGlue.Noise.SetInterp(Interp.Linear);

                var val = PixelGlue.Noise.GetNoise(x / 32, y / 32, PixelGlue.Z / 32);
                val += 0.5f * PixelGlue.Noise.GetNoise(x / 16, y / 16, PixelGlue.Z / 16);
                val += 0.15f * PixelGlue.Noise.GetNoise(x / 2, y / 2, PixelGlue.Z / 2);
                if (val > 0.94)
                    d = new DrawableComponent(0, "rock", srcRect, dstRect);
                else if (val > 0.6f)
                    d = new DrawableComponent(0, "sand3", srcRect, dstRect);
                else if (val > -0.4)
                    d = new DrawableComponent(0, "sand2", srcRect, dstRect);
                else
                    d = new DrawableComponent(0, "sand", srcRect, dstRect);
            }
            // Tiles.Add((x, y), d);
            return d;
        }
    }
}